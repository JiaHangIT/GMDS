﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.ExcelLandDistrictBLL;
using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace JiaHang.Projects.Admin.Web.Controllers.API.LandDistrictImportA
{
    [Route("api/[controller]")]
    //[ApiController]
    public class LandDistrictImportController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment hosting;
        private readonly LandDistrictBll LanBll;

        public LandDistrictImportController(DataContext _context, IHostingEnvironment _hosting, IMemoryCache _cache)
        {
           this.context = _context;
           this.hosting = _hosting; this.cache = _cache;
            this.LanBll = new LandDistrictBll(_context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public FuncResult GetList([FromBody] RequestDistric model = null)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };

            //条件查询情况下，需要重新考虑Count值的问题
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            var query = from t1 in context.ApdFctLandDistrict.Where(f => f.DeleteFlag == 0)                       
                        join o in context.ApdDimOrg on t1.OrgCode equals o.OrgCode
                        select new DistricModel
                        {
                            RecordId = t1.RecordId,
                            //Array = listnew,
                            OrgName = o.OrgName,
                            Town = o.Town,
                            OrgCode = o.OrgCode,
                            RegistrationType = o.RegistrationType,
                            Address = o.Address,
                            LegalRepresentative = o.LegalRepresentative,
                            Phone = o.Phone,
                            LinkMan = o.LinkMan,
                            Phone2 = o.Phone2,
                            LandNo = t1.LandNo,
                            Area = t1.Area,
                            ShareDesc = t1.ShareDesc,
                            RightType = t1.RightType,
                            Purpose= t1.Purpose,
                            BeginDate = t1.BeginDate,
                            End_Date=t1.EndDate,
                            Remark = t1.Remark
                        };
            query = query.Where(f => (
            (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
            (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
            (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
            )).OrderBy(o => o.Create);

            /*
             * 通过groupby data来分页
             * groupby 处理后，再根据groupby data自来query data
             * **/

            var querygroup = query.GroupBy(g => new { g.OrgCode, g.RegistrationType}).OrderBy(o => o.Key.OrgCode);
            int count = querygroup.Count();
            var l = querygroup.Skip(model.limit * model.page).Take(model.limit);


            var list = new List<int>();
            //重新定义query里count的值

            var queryr = new List<DistricModel>();
            foreach (var item in l)
            {
                //query.Where(f => f.Key == item.Key.Key).ToList().ForEach(p => p.Count = item.Count());

                var currentquery = query.Where(f => f.OrgCode == item.Key.OrgCode).ToList();
                foreach (var itemquery in currentquery)
                {
                    itemquery.Count = item.Count();
                    queryr.Add(itemquery);
                }
                //listResut.Where(w => w.CategoryID > 30 && w.CategoryID < 40).ToList().ForEach(p => p.CategoryName = p.CategoryName + "bb");
                int c = item.Count();
                list.Add(c);
            }

            var listnew = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    listnew.Add(0);
                }
                else if (i == 1)
                {
                    listnew.Add(list[0]);
                }
                else
                {
                    //listnew.Add(list[i - 1] + list[i - 2]);
                    listnew.Add(list.Take(i).Sum());
                }
            }
            result.Content = new { data = queryr, array = listnew, total = count };
            return result;
        }

        /// <summary>
        /// 获取编辑数据
        /// 连表查询
        /// </summary>
        /// <param name="recordid"></param>
        /// <returns></returns>
        [Route("edit/{recordid}")]
        [HttpGet]
        public async Task<FuncResult> GetEditData(int recordid)
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var query = from t1 in context.ApdFctLandDistrict.Where(f => f.DeleteFlag == 0)                            
                            join o in context.ApdDimOrg on t1.OrgCode equals o.OrgCode
                            select new
                            {
                                //Key = t2.RecordId,
                                RecordId = t1.RecordId,
                                OrgName = o.OrgName,
                                LandNo = t1.LandNo,
                                Area = t1.Area,
                                ShareDesc = t1.ShareDesc,
                                RightType = t1.RightType,
                                Purpose = t1.Purpose,
                                BeginDate = t1.BeginDate,
                                EndDate = t1.EndDate,
                                Remark = t1.Remark,
                                Create = t1.CreationDate
                            };
                query = query.OrderBy(o => o.Create);

                if (query == null || query.Count() <= 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = "异常，未查到相关数据!";
                    return fr;
                }
                fr.Content = query;
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// excel数据导入到数据库(apdfctlandtown、apdfctlandtown2表)
        /// 一个机构一年只有一批数据
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        //[HttpPost("upload")]
        [Route("upload")]
        [HttpGet("{year}")]
        public FuncResult Import(string year)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };
            try
            {
                var excelfile = Request.Form.Files[0];
                List<dynamic> datalist = new List<dynamic>();
                if (excelfile.Length > 0)
                {
                    DataTable dt = new DataTable();
                    string strMsg;
                    string filename = excelfile.FileName;//文件名即为区域名
                    //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                    dt = ExcelHelper.ExcelToDatatable(excelfile.OpenReadStream(), Path.GetExtension(excelfile.FileName), out strMsg);
                    if (!string.IsNullOrEmpty(strMsg))
                    {
                        result.IsSuccess = false;
                        result.Message = strMsg;
                        return result;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        var listorgan = context.ApdDimOrg.ToList();
                        
                        //需要导入到数据库的数据
                        datalist = JsonConvert.DeserializeObject<List<dynamic>>(JsonConvert.SerializeObject(dt));

                        var prefilter = datalist.Where(f => !(f.L1 == ""));
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }

                        List<dynamic> filterdata = datalist.Where(f => !(f.L1 == "" && f.L2 == "" && f.L3 == "" && f.L4 == "" && f.L5 == "" && f.L6 == "" && f.L7 == "" && f.L8 == "") && f.L1 != null).ToList();
                        //if (filterdata == null || filterdata.Count <= 0)
                        //{
                        //    result.IsSuccess = false;
                        //    result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                        //    return result;
                        //}
                        ////处理筛选过后的数据
                        //string g1 = string.Empty;
                        //string g3 = string.Empty;
                        //foreach (var item in filterdata)
                        //{
                        //    if (!string.IsNullOrEmpty(item.L1))
                        //    {
                        //        g1 = item.L1;
                        //    }
                        //    if (string.IsNullOrEmpty(item.L1))
                        //    {
                        //        //为上一个有值的g1
                        //        item.L1 = g1;
                        //    }

                        //    if (!string.IsNullOrEmpty(item.L3))
                        //    {
                        //        g3 = item.L3;
                        //    }
                        //    if (string.IsNullOrEmpty(item.L3))
                        //    {
                        //        //为上一个有值的g3
                        //        item.L3 = g3;
                        //    }
                        //}
                        var filterdataT = prefilter.Select(g => new ApdFctLandDistrict
                        {
                            RecordId = Guid.NewGuid().ToString(),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.L3,
                            LandNo = g.L10,
                            Area = g.L11 == ""||null ? 0 : Convert.ToDecimal(g.L11),
                            ShareDesc=g.L12,
                            RightType = g.L13,
                            Purpose = g.L14,
                            BeginDate=Convert.ToDateTime(g.L15),
                            EndDate=Convert.ToDateTime(g.L16),
                            Remark = g.L17,
                            CreationDate = DateTime.Now
                        });
                        result = LanBll.WriteData(filterdataT, year);
                       


                        //decimal currenttown2key = 0;//本次apd_fct_town2表的主键
                        //var town2context = context.ApdFctLandDistrict.OrderByDescending(o => o.RecordId).ToList();
                        //if (town2context == null || town2context.Count() <= 0)
                        //{
                        //    currenttown2key = 1;
                        //}
                        //else
                        //{
                        //    currenttown2key = town2context[0].RecordId + 1;
                        //}
                        

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "excel无数据！";
                    }
                }



                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"{ex.Message},{ex.InnerException}";
                return result;
            }

        }
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downtemplate")]
        public FileResult DownTemplate()
        {
            try
            {

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业自有土地情况取数表格式-高明区自然资源分局.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业自有土地情况取数表格式-高明区自然资源分局.xls");
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// 数据导出到excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        public FileResult Export()
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var summarydata = GetList(new RequestDistric() { orgname = "", orgcode = "" });
                var summarydataT = LanBll.GetList();
                var data = (List<DistricModelT>)((dynamic)summarydataT).Content;
                var groupdata = (List<int>)((dynamic)summarydata.Content).array;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业自有土地情况取数表格式-高明区自然资源分局.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //从row7开始，B到Q(1到16)
                //sheet1.GetRow(7).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                //sheet1.GetRow(8).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                //sheet1.GetRow(9).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                for (int i = 6; i < data.Count + 6; i++)
                {
                    sheet1.GetRow(i).GetCell(1).SetCellValue(data[i - 6].OrgName);
                    sheet1.GetRow(i).GetCell(2).SetCellValue(data[i - 6].Town);
                    sheet1.GetRow(i).GetCell(3).SetCellValue(data[i - 6].OrgCode);
                    sheet1.GetRow(i).GetCell(4).SetCellValue(data[i - 6].RegistrationType);
                    sheet1.GetRow(i).GetCell(5).SetCellValue(data[i - 6].Address);
                    sheet1.GetRow(i).GetCell(6).SetCellValue(data[i - 6].LegalRepresentative);
                    sheet1.GetRow(i).GetCell(7).SetCellValue(data[i - 6].Phone);
                    sheet1.GetRow(i).GetCell(8).SetCellValue(data[i - 6].LinkMan);
                    sheet1.GetRow(i).GetCell(9).SetCellValue(data[i - 6].Phone2);
                    sheet1.GetRow(i).GetCell(10).SetCellValue(data[i - 6].LandNo);
                    sheet1.GetRow(i).GetCell(11).SetCellValue(Convert.ToDouble(data[i - 6].Area));
                    sheet1.GetRow(i).GetCell(12).SetCellValue(data[i - 6].ShareDesc);
                    sheet1.GetRow(i).GetCell(13).SetCellValue(data[i - 6].RightType);
                    sheet1.GetRow(i).GetCell(14).SetCellValue(data[i - 6].Purpose);
                    sheet1.GetRow(i).GetCell(15).SetCellValue(Convert.ToDateTime(data[i - 6].BeginDate));
                    sheet1.GetRow(i).GetCell(16).SetCellValue(Convert.ToDateTime(data[i - 6].End_Date));
                    sheet1.GetRow(i).GetCell(17).SetCellValue(data[i - 6].Remark);
                }

                /*
                 *
                 * 处理部分行合并(测试B1到J9)
                 * CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                 * 合并的列是一样的，只需要处理好行的关系即可
                 * 模板是从第7行(即行坐标为6)开始写数据
                 * 10、11、12列不合并
                 * **/

                int currentIndex = 6;
                //groupdata.RemoveAt(groupdata.Count- 1);
                for (int i = 0; i < groupdata.Count; i++)
                {
                    if (i == groupdata.Count - 1)
                    {
                        for (int j = 1; j < 17; j++)
                        {
                            if (j == 10 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17)
                            {
                                continue;
                            }
                            sheet1.AddMergedRegion(new CellRangeAddress(currentIndex, currentIndex + data.Count - groupdata[i] - 1, j, j));
                        }
                    }
                    else
                    {
                        for (int j = 1; j < 18; j++)
                        {
                            if (j == 10 || j == 11 || j == 12 || j == 13 || j == 14 || j == 15 || j == 16 || j == 17)
                            {
                                continue;
                            }
                            sheet1.AddMergedRegion(new CellRangeAddress(currentIndex, currentIndex + groupdata[i + 1] - groupdata[i] - 1, j, j));
                        }
                    }
                    if (i < groupdata.Count - 1)
                    {
                        currentIndex += groupdata[i + 1] - groupdata[i];
                    }

                }
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 1, 1));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 2, 2));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 3, 3));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 4, 4));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 5, 5));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 6, 6));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 7, 7));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 8, 8));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 9, 9));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 13, 13));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 14, 14));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 15, 15));
                //sheet1.AddMergedRegion(new CellRangeAddress(6, 8, 16, 16));


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"{DateTime.Now.ToString("yyyy-MM-dd:hh:mm:ss")}.xls");


                #region 原有方法
                //byte[] result;

                //using (var package = new ExcelPackage())
                //{
                //    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                //    worksheet.Cells["A1:H1"].Merge = true;
                //    //worksheet.Cells[1, 1].Value = string.IsNullOrEmpty(model.searchcondition) ? "全国" : model.searchcondition;
                //    //worksheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //    //for (int i = 0; i < comlumHeadrs.Count(); i++)
                //    //{
                //    //    worksheet.Cells[2, i + 1].Value = comlumHeadrs[i];
                //    //    worksheet.Cells[2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //    //}
                //    //如果是空数据 直接返回
                //    if (data.Count() <= 1)
                //    {
                //        //return package.GetAsByteArray();
                //        //return File(package.GetAsByteArray(), "application/ms-excel", "test");
                //    }
                //    var j = 3;//数据行
                //    foreach (var item in data)
                //    {
                //        var rt = item.GetType();
                //        var rp = rt.GetProperties();

                //        //B到Q
                //        worksheet.Cells["B" + j].Value = item.OrgName;
                //        worksheet.Cells["C" + j].Value = item.Town;
                //        worksheet.Cells["D" + j].Value = item.OrgCode;
                //        worksheet.Cells["E" + j].Value = item.RegistrationType;
                //        worksheet.Cells["F" + j].Value = item.Address;
                //        worksheet.Cells["G" + j].Value = item.LegalRepresentative;
                //        worksheet.Cells["H" + j].Value = item.Phone;
                //        worksheet.Cells["I" + j].Value = item.LinkMan;
                //        worksheet.Cells["J" + j].Value = item.Phone2;
                //        worksheet.Cells["K" + j].Value = item.OwnershipLand;
                //        worksheet.Cells["L" + j].Value = item.ProtectionLand;
                //        worksheet.Cells["M" + j].Value = item.ReduceLand;
                //        worksheet.Cells["N" + j].Value = item.FactLand;
                //        worksheet.Cells["O" + j].Value = item.RentLand;
                //        worksheet.Cells["P" + j].Value = item.LeaseLand;
                //        worksheet.Cells["Q" + j].Value = item.Remark;

                //        j++;
                //    }

                //    result = package.GetAsByteArray();
                //}

                //return File(result, "application/ms-excel", $"{DateTime.Now.ToString("yyyy-MM-dd:hh:mm:ss")}.xls");
                #endregion



            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// 更新详细数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/{key}")]
        public FuncResult UpdateDetailData(string key, [FromBody] ApdFctDistrict model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                if (model == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未正常接收参数!";
                    return fr;
                }
                
                foreach (var item in model.detaillist)
                {
                    var cd = context.ApdFctLandDistrict.FirstOrDefault(f => f.RecordId.Equals(item.RecordId));
                    cd.LandNo = item.LandNo;
                    cd.Area = item.Area;
                    cd.ShareDesc = item.ShareDesc;
                    cd.RightType = item.RightType;
                    cd.Purpose = item.Purpose;
                    cd.BeginDate = item.BeginDate;
                    cd.EndDate = item.EndDate;
                    cd.Remark = item.Remark;
                    cd.LastUpdateDate = DateTime.Now;
                    context.ApdFctLandDistrict.Update(cd);
                }


                using (IDbContextTransaction trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.SaveChangesAsync();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        fr.IsSuccess = false;
                        fr.Message = $"{ex.InnerException},{ex.Message}";
                        return fr;
                        throw new Exception("error", ex);
                    }
                }
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] Ids)
        {
            return await LanBll.Delete(Ids, HttpContext.CurrentUser(cache).Id);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("delete/{key}")]
        public async Task<FuncResult> DeleteData(int key)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                //if (string.IsNullOrWhiteSpace(key))
                //{
                //    fr.IsSuccess = false;
                //    fr.Message = "未接收到参数信息!";
                //}
                var cd = context.ApdFctLandDistrict.FirstOrDefault(f => f.RecordId.Equals(key));
                //List<ApdFctLandDistrict> listtown = context.ApdFctLandDistrict.Where(f => f.RecordId.Equals(key)).ToList();

                //删除
                context.ApdFctLandDistrict.RemoveRange(cd);


                using (IDbContextTransaction trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        await context.SaveChangesAsync();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        fr.IsSuccess = false;
                        fr.Message = $"{ex.InnerException},{ex.Message}";
                        throw new Exception("error", ex);
                    }
                }
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// 处理某机构某年是否已导入数据
        /// </summary>
        /// <param name="orgcode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool isAlreadyExport(string orgcode, string year)
        {
            try
            {
                var formatyear = Convert.ToDecimal(year);
                var town2 = context.ApdFctLandTown2.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                return town2 != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }
    public class DistricModel
    {
        public string RecordId { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal Count { get; set; }
        public decimal Key { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string LegalRepresentative { get; set; }
        public string Phone { get; set; }
        public string LinkMan { get; set; }
        public string Phone2 { get; set; }

        public string LandNo { get; set; }
        public decimal? Area { get; set; }
        public string ShareDesc { get; set; }
        public string RightType { get; set; }
        public string Purpose { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? End_Date { get; set; }

        public decimal? LeaseLand { get; set; }
        public string Remark { get; set; }
        public DateTime? Create { get; set; }
    }
    public class DemoT
    {
        public string L1 { get; set; }

        public string L2 { get; set; }

        public string L3 { get; set; }

        public string L4 { get; set; }

        public string L5 { get; set; }

        public string L6 { get; set; }

        public string L7 { get; set; }

        public string L8 { get; set; }

        public string L9 { get; set; }

        public string L10 { get; set; }

        public int? L11 { get; set; }

        public string L12 { get; set; }

        public string L13 { get; set; }

        public string L14 { get; set; }

        public string L15 { get; set; }

        public string L16 { get; set; }

        public string L17 { get; set; }
    }

    /// <summary>
    /// APD_FCT_LAND_District
    /// </summary>
    public class DistrictModelT
    {
        public decimal Count { get; set; }
        public decimal Key { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string LegalRepresentative { get; set; }
        public string Phone { get; set; }
        public string LinkMan { get; set; }
        public string Phone2 { get; set; }
        /// <summary>
        /// L10
        /// </summary>
        public string LANDNO { get; set; }

        /// <summary>
        /// L11
        /// </summary>
        public int? AREA { get; set; }

        /// <summary>
        /// L12
        /// </summary>
        public string SHAREDESC { get; set; }

        /// <summary>
        /// L13
        /// </summary>
        public string RIGHTTYPE { get; set; }

        /// <summary>
        /// L14
        /// </summary>
        public string PURPOSE { get; set; }

        /// <summary>
        /// L15
        /// </summary>
        public DateTime BEGINDATE { get; set; }

        /// <summary>
        /// L16
        /// </summary>
        public DateTime ENDDATE { get; set; }


        /// <summary>
        /// L17
        /// </summary>
        public string REMARK { get; set; }

        
    }

    /// <summary>
    /// APD_FCT_LAND_District
    /// </summary>
    public class DistrictModel1
    {
        /// <summary>
        /// L10
        /// </summary>
        public string LANDNO { get; set; }

        /// <summary>
        /// L11
        /// </summary>
        public int? AREA { get; set; }

        /// <summary>
        /// L12
        /// </summary>
        public string SHAREDESC { get; set; }

        /// <summary>
        /// L13
        /// </summary>
        public string RIGHTTYPE { get; set; }

        /// <summary>
        /// L14
        /// </summary>
        public string PURPOSE { get; set; }

        /// <summary>
        /// L15
        /// </summary>
        public DateTime BEGINDATE { get; set; }

        /// <summary>
        /// L16
        /// </summary>
        public DateTime ENDDATE { get; set; }


        /// <summary>
        /// L17
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// L1
        /// </summary>
        public string ORGANIZATION { get; set; }

        /// <summary>
        /// L3
        /// </summary>
        public string ORGCODE { get; set; }

        ///// <summary>
        ///// L1
        ///// </summary>
        //public string CRGNAME { get; set; }
    }
    public class RequestDistric
    {
        public string year { get; set; }

        public string orgname { get; set; }

        public string orgcode { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }
    }
    /// <summary>
    /// 数据更新接收实体
    /// </summary>
    public class ApdFctDistrict
    {

        public List<ApdFctLandDistrict> detaillist { get; set; }
    }
}