using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class QiYeFileController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment hosting;

        public QiYeFileController(DataContext _context,IHostingEnvironment _hosting, IMemoryCache _cache) { this.context = _context; this.hosting = _hosting; this.cache = _cache; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public FuncResult GetList([FromBody] RequestLandTown model = null)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };

            //条件查询情况下，需要重新考虑Count值的问题
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            var query = from t1 in context.ApdFctLandTown.Where(f=>f.DeleteFlag == 0)
                        join t2 in context.ApdFctLandTown2.Where(f => f.DeleteFlag == 0) on t1.T2Id equals t2.RecordId
                        join o in context.ApdDimOrg on t2.OrgCode equals o.OrgCode
                        select new ReturnModel
                        {
                            //Array = listnew,
                            PeriodYear = t2.PeriodYear,
                            Count = t2.Count,
                            Key = t2.RecordId,
                            OrgName = o.OrgName,
                            Town = o.Town,
                            OrgCode = o.OrgCode,
                            RegistrationType = o.RegistrationType,
                            Address = o.Address,
                            LegalRepresentative = o.LegalRepresentative,
                            Phone = o.Phone,
                            LinkMan = o.LinkMan,
                            Phone2 = o.Phone2,
                            OwnershipLand = t1.OwnershipLand,
                            ProtectionLand = t1.ProtectionLand,
                            ReduceLand = t1.ReduceLand,
                            FactLand = t2.FactLand,
                            RentLand = t2.RentLand,
                            LeaseLand = t2.LeaseLand,
                            Remark = t2.Remark,
                            Create = t2.CreationDate
                        };
            query = query.Where(f=> (
            (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode))&&
            (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
            (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
            )).OrderBy(o => o.Create);

            /*
             * 通过groupby data来分页
             * groupby 处理后，再根据groupby data自来query data
             * **/

            var querygroup = query.GroupBy(g => new { g.OrgCode, g.RegistrationType, g.FactLand, g.RentLand, g.LeaseLand, g.Key }).OrderBy(o => o.Key.Key);
            int count = querygroup.Count();

            if (model.limit * model.page >= count)
            {
                model.page = 0;
            }
            var l = querygroup.Skip(model.limit * model.page).Take(model.limit);
            

            var list = new List<int>();
            //重新定义query里count的值

            var queryr = new List<ReturnModel>();
            foreach (var item in l)
            {
                //query.Where(f => f.Key == item.Key.Key).ToList().ForEach(p => p.Count = item.Count());

                var currentquery = query.Where(f => f.Key == item.Key.Key).ToList();
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
            result.Content = new { data = queryr, array = listnew,total = count };
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
        public async Task<FuncResult>  GetEditData(string recordid)
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var query = from t1 in context.ApdFctLandTown.Where(f => f.DeleteFlag == 0)
                            join t2 in context.ApdFctLandTown2.Where(f => f.DeleteFlag == 0 && f.RecordId.Equals(recordid)) on t1.T2Id equals t2.RecordId
                            join o in context.ApdDimOrg on t1.OrgCode equals o.OrgCode
                            select new
                            {
                                Key = t2.RecordId,
                                RecordId = t1.RecordId,
                                OrgName = o.OrgName,
                                OwnershipLand = t1.OwnershipLand,
                                ProtectionLand = t1.ProtectionLand,
                                ReduceLand = t1.ReduceLand,
                                FactLand = t2.FactLand,
                                RentLand = t2.RentLand,
                                LeaseLand = t2.LeaseLand,
                                Remark = t2.Remark,
                                Create = t2.CreationDate
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

                throw new Exception("error",ex);
            }
        }

        /// <summary>
        /// 更新详细数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/{key}")]
        public FuncResult UpdateDetailData(string key,[FromBody] ApdFctLandTowns model)
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
                var town2 = context.ApdFctLandTown2.FirstOrDefault(f => f.RecordId.Equals(model.Key));
                if (town2 == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未正常接收参数!";
                    return fr;
                }
                town2.FactLand = model.FactLand;
                town2.RentLand = model.RentLand;
                town2.LeaseLand = model.LeaseLand;
                town2.Remark = model.Remark;
                town2.LastUpdateDate = DateTime.Now;
                town2.LastUpdatedBy = Convert.ToDecimal(HttpContext.CurrentUser(cache).Id);
                context.ApdFctLandTown2.Update(town2);

                foreach (var item in model.detaillist)
                {
                    var cd = context.ApdFctLandTown.FirstOrDefault(f => f.RecordId.Equals(item.RecordId));
                    cd.OwnershipLand = item.OwnershipLand;
                    cd.ProtectionLand = item.ProtectionLand;
                    cd.ReduceLand = item.ReduceLand;
                    context.ApdFctLandTown.Update(cd);
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
                        throw new Exception("error",ex);
                    }
                }
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("delete/{key}")]
        public async Task<FuncResult> DeleteData(string key)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    fr.IsSuccess = false;
                    fr.Message = "未接收到参数信息!";
                }
                var _key = Convert.ToDecimal(key);
                ApdFctLandTown2 town2 = context.ApdFctLandTown2.FirstOrDefault(f => f.RecordId.Equals(_key));
                if (town2 == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "异常参数，未找到数据!";
                }
                List<ApdFctLandTown> listtown = context.ApdFctLandTown.Where(f => f.T2Id.Equals(key)).ToList();

                //删除
                context.ApdFctLandTown2.Remove(town2);
                context.ApdFctLandTown.RemoveRange(listtown);


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
                        throw new Exception("error",ex);
                    }
                }
                return fr;
            }
            catch (Exception ex)
            {
                
                throw new Exception("error",ex) ;
            }
        }

        /// <summary>
        /// 数据导出到excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("export/{pagesize}/{pagenum}")]
        public FileResult Export(int pagesize,int pagenum)
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var summarydata = GetList(new RequestLandTown() { orgname = "", orgcode = "",limit=pagesize,page=pagenum });
                var data = (List<ReturnModel>)((dynamic)summarydata.Content).data;
                var groupdata = (List<int>)((dynamic)summarydata.Content).array;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业土地使用情况取数表格式（六类土地）-高明街镇、西江产业新城.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //从row7开始，B到Q(1到16)
                //sheet1.GetRow(7).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                //sheet1.GetRow(8).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                //sheet1.GetRow(9).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                for (int i = 6; i < data.Count + 6; i++)
                {
                    sheet1.GetRow(i).GetCell(1).SetCellValue(data[i-6].OrgName);
                    sheet1.GetRow(i).GetCell(2).SetCellValue(data[i - 6].Town);
                    sheet1.GetRow(i).GetCell(3).SetCellValue(data[i - 6].OrgCode);
                    sheet1.GetRow(i).GetCell(4).SetCellValue(data[i - 6].RegistrationType);
                    sheet1.GetRow(i).GetCell(5).SetCellValue(data[i - 6].Address);
                    sheet1.GetRow(i).GetCell(6).SetCellValue(data[i - 6].LegalRepresentative);
                    sheet1.GetRow(i).GetCell(7).SetCellValue(data[i - 6].Phone);
                    sheet1.GetRow(i).GetCell(8).SetCellValue(data[i - 6].LinkMan);
                    sheet1.GetRow(i).GetCell(9).SetCellValue(data[i - 6].Phone2);
                    sheet1.GetRow(i).GetCell(10).SetCellValue(Convert.ToDouble(data[i - 6].OwnershipLand));
                    sheet1.GetRow(i).GetCell(11).SetCellValue(Convert.ToDouble(data[i - 6].ProtectionLand));
                    sheet1.GetRow(i).GetCell(12).SetCellValue(Convert.ToDouble(data[i - 6].ReduceLand));
                    sheet1.GetRow(i).GetCell(13).SetCellValue(Convert.ToDouble(data[i - 6].FactLand));
                    sheet1.GetRow(i).GetCell(14).SetCellValue(Convert.ToDouble(data[i - 6].RentLand));
                    sheet1.GetRow(i).GetCell(15).SetCellValue(Convert.ToDouble(data[i - 6].LeaseLand));
                    sheet1.GetRow(i).GetCell(16).SetCellValue(data[i - 6].Remark);
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
                            if (j == 10 || j == 11 || j == 12 )
                            {
                                continue;
                            }
                            sheet1.AddMergedRegion(new CellRangeAddress(currentIndex, currentIndex + data.Count - groupdata[i] - 1, j, j));
                        }
                    }
                    else
                    {
                        for (int j = 1; j < 17; j++)
                        {
                            if (j == 10 || j == 11 || j == 12)
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

                throw new Exception("error",ex);
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

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业土地使用情况取数表格式（六类土地）-高明街镇、西江产业新城.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作

                
                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业土地使用情况取数表格式（六类土地）-高明街镇、西江产业新城.xls");
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
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
                List<Demo> datalist = new List<Demo>();
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
                        datalist = JsonConvert.DeserializeObject<List<Demo>>(JsonConvert.SerializeObject(dt));
                        List<Demo> filterdata = datalist.Where(f =>  f.G10 != null && f.G11 != null && f.G12 != null).ToList();
                        if (filterdata == null || filterdata.Count <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }
                        //处理筛选过后的数据
                        string g1 = string.Empty;
                        string g3 = string.Empty;
                        foreach (var item in filterdata)
                        {
                            if (!string.IsNullOrEmpty(item.G1))
                            {
                                g1 = item.G1;
                            }
                            if (string.IsNullOrEmpty(item.G1))
                            {
                                //为上一个有值的g1
                                item.G1 = g1;
                            }

                            if (!string.IsNullOrEmpty(item.G3))
                            {
                                g3 = item.G3;
                            }
                            if (string.IsNullOrEmpty(item.G3))
                            {
                                //为上一个有值的g3
                                item.G3 = g3;
                            }
                        }

                        var groupdata_1 = filterdata.GroupBy(g => new { g.G1, g.G3, g.G10, g.G11, g.G12 }).Select(s => new ReflctModel1
                        {
                            OWNERSHIPLAND = s.Key.G10,
                            PROTECTIONLAN = s.Key.G11,
                            REDUCELAND = s.Key.G12,
                            ORGANIZATION = s.Key.G1,
                            ORGCODE = s.Key.G3
                        });

                        var groupdata_2 = filterdata.GroupBy(g => new { g.G1, g.G3, g.G13, g.G14, g.G15, g.G16 }).Select(s => new ReflctModel2
                        {
                            FACTLAND = s.Key.G13,
                            RENTLAND = s.Key.G14,
                            LEASELAND = s.Key.G15,
                            REMARK = s.Key.G16,
                            ORGANIZATION = s.Key.G1,
                            ORGCODE = s.Key.G3
                        }).Where(f => (f.FACTLAND != null && f.RENTLAND != null && f.LEASELAND != null) == true);

                        string currenttown2key = "";//本次apd_fct_town2表的主键
                        //var town2context = context.ApdFctLandTown2.OrderByDescending(o => o.RecordId).ToList();
                        //if (town2context == null || town2context.Count() <= 0)
                        //{
                        //    currenttown2key = 1;
                        //}
                        //else
                        //{
                        //    currenttown2key = town2context[0].RecordId + 1;
                        //}
                        //存在orgcode不存在的情况就整个都不写入
                        //t2作为t1的主表
                        foreach (var item in groupdata_2)
                        {
                            var currentorganization = listorgan.FirstOrDefault(f => f.OrgCode.Equals(item.ORGCODE));
                            if (currentorganization == null)
                            {
                                result.IsSuccess = false;
                                result.Message = $"此机构号:{item.ORGCODE}找不到对应机构，导入失败！";
                                return result;
                            }
                            bool isalreadyexport = isAlreadyExport(item.ORGCODE, year);
                            if (isalreadyexport)
                            {
                                //删除(添加删除标记字段)
                                //物理删除
                                var formatyear = Convert.ToDecimal(year);
                                var alreadytown2 = context.ApdFctLandTown2.Where(f => f.OrgCode.Equals(item.ORGCODE) && f.PeriodYear.Equals(formatyear));
                                var alreadytown = context.ApdFctLandTown.Where(f => alreadytown2.Select(g => g.RecordId).Contains(f.T2Id));
                                context.ApdFctLandTown2.RemoveRange(alreadytown2);
                                context.ApdFctLandTown.RemoveRange(alreadytown);
                            }
                            ApdFctLandTown2 t2 = new ApdFctLandTown2()
                            {
                                OrgCode = item.ORGCODE,
                                FactLand = item.FACTLAND,
                                RentLand = item.RENTLAND,
                                LeaseLand = item.LEASELAND,
                                Remark = item.REMARK,
                                PeriodYear = Convert.ToDecimal(year),
                                RecordId = Guid.NewGuid().ToString(),
                                CreatedBy = Convert.ToDecimal(HttpContext.CurrentUser(cache).Id),
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                LastUpdatedBy = Convert.ToDecimal(HttpContext.CurrentUser(cache).Id),
                                Count = groupdata_1.Where(f=>f.ORGCODE.Equals(item.ORGCODE)).Count()
                            };
                            context.ApdFctLandTown2.Add(t2);

                            foreach (var itemdetail in groupdata_1.Where(f => f.ORGCODE.Equals(item.ORGCODE)))
                            {

                                ApdFctLandTown t1 = new ApdFctLandTown()
                                {
                                    OrgCode = itemdetail.ORGCODE,
                                    OwnershipLand = itemdetail.OWNERSHIPLAND,
                                    ProtectionLand = itemdetail.PROTECTIONLAN,
                                    ReduceLand = itemdetail.REDUCELAND,
                                    CreatedBy = Convert.ToDecimal(HttpContext.CurrentUser(cache).Id),
                                    CreationDate = DateTime.Now,
                                    LastUpdateDate = DateTime.Now,
                                    LastUpdatedBy = Convert.ToDecimal(HttpContext.CurrentUser(cache).Id),
                                    PeriodYear = DateTime.Now.Year,
                                    RecordId = new Random().Next(1, 999),
                                    T2Id = t2.RecordId
                                };
                                context.ApdFctLandTown.Add(t1);
                            }
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
                                throw new Exception("error", ex);
                            }
                        }

                        //将groupdata_1、groupdata_2写入到两张表

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
                var town2 = context.ApdFctLandTown2.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(Convert.ToDecimal(formatyear))).FirstOrDefault();
                return town2 != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
            }
        }
    }

    public class Demo
    {
        public string G1 { get; set; }

        public string G2 { get; set; }

        public string G3 { get; set; }

        public string G4 { get; set; }

        public string G5 { get; set; }

        public string G6 { get; set; }

        public string G7 { get; set; }

        public string G8 { get; set; }

        public string G9 { get; set; }

        public decimal? G10 { get; set; }

        public decimal? G11 { get; set; }

        public decimal? G12 { get; set; }

        public decimal? G13 { get; set; }

        public decimal? G14 { get; set; }

        public decimal? G15 { get; set; }

        public string G16 { get; set; }
    }

    public class ReturnModel
    {
        public decimal PeriodYear { get; set; }
        public decimal Count { get; set; }
        public string Key { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string LegalRepresentative { get; set; }
        public string Phone { get; set; }
        public string LinkMan { get; set; }
        public string Phone2 { get; set; }
        public decimal? OwnershipLand { get; set; }
        public decimal? ProtectionLand { get; set; }
        public decimal? ReduceLand { get; set; }
        public decimal? FactLand { get; set; }
        public decimal? RentLand { get; set; }
        public decimal? LeaseLand { get; set; }
        public string Remark { get; set; }
        public DateTime? Create { get; set; }
    }

    /// <summary>
    /// APD_FCT_LAND_TOWN
    /// </summary>
    public class ReflctModel1
    {
        /// <summary>
        /// G10
        /// </summary>
        public decimal? OWNERSHIPLAND { get; set; }

        /// <summary>
        /// G11
        /// </summary>
        public decimal? PROTECTIONLAN { get; set; }

        /// <summary>
        /// G12
        /// </summary>
        public decimal? REDUCELAND { get; set; }

        /// <summary>
        /// G1
        /// </summary>
        public string ORGANIZATION { get; set; }

        /// <summary>
        /// G3
        /// </summary>
        public string ORGCODE { get; set; }
    }

    /// <summary>
    /// APD_FCT_LAND_TOWN2
    /// </summary>
    public class ReflctModel2
    {
        /// <summary>
        /// G13
        /// </summary>
        public decimal? FACTLAND { get; set; }

        /// <summary>
        /// G14
        /// </summary>
        public decimal? RENTLAND { get; set; }

        /// <summary>
        /// G15
        /// </summary>
        public decimal? LEASELAND { get; set; }

        /// <summary>
        /// G16
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// G1
        /// </summary>
        public string ORGANIZATION { get; set; }

        /// <summary>
        /// G3
        /// </summary>
        public string ORGCODE { get; set; }
    }

    public class RequestLandTown
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
    public class ApdFctLandTowns
    {
        public string Key { get; set; }
        public decimal? FactLand { get; set; }
        public decimal? RentLand { get; set; }
        public decimal? LeaseLand { get; set; }
        public string Remark { get; set; }
        public List<ApdFctDetailTown> detaillist { get; set; }
    }

    public class ApdFctDetailTown
    {
        public decimal RecordId { get; set; }
        public decimal? OwnershipLand { get; set; }
        public decimal? ProtectionLand { get; set; }
        public decimal? ReduceLand { get; set; }
    }
}