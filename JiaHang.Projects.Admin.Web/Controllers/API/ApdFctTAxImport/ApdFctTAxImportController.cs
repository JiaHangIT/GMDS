﻿using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.ApdFctTax;
using JiaHang.Projects.Admin.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.ExcelTaxBLL;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ApdFctTAxImportController : ControllerBase
    {
        public readonly DataContext context;
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment hosting;
        private readonly TaxBll taxBlla;

        public ApdFctTAxImportController(DataContext _context, IHostingEnvironment _hosting, IMemoryCache _cache)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.cache = _cache;
            this.taxBlla = new TaxBll(_context);
        }
      
        //private readonly ExcelTaxImportBLL storeService;
        //private readonly IMemoryCache cache;
        //public ApdFctTAxImportController(DataContext dataContext, IMemoryCache cache)
        //{
        //    storeService = new ExcelTaxImportBLL(dataContext);
        //    this.cache = cache;
        //}
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetList")]
        public FuncResult GetList([FromBody] adpFctax model = null)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };
            try
            {


                var query = from t1 in context.ApdFctTAx
                            join o in
                            context.ApdDimOrg on t1.ORG_CODE equals o.OrgCode
                            where (
                                  (string.IsNullOrWhiteSpace(model.orgname) || o.OrgName.Contains(model.orgname))&&
                                  (string.IsNullOrWhiteSpace(model.orgcode) || o.OrgCode.Contains(model.orgcode))
                                 )
                            select new ReturnModel
                            {
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
                                recordId=t1.RECORD_ID,
                                Depreciation = t1.DEPRECIATION,
                                EmployeeRemunerationON = t1.EMPLOYEE_REMUNERATION,
                                PROFIT = t1.PROFIT,
                                EntPaidTax = t1.ENT_PAID_TAX,
                                MainBusinessIncome = t1.MAIN_BUSINESS_INCOME,
                                RadEexpenses = t1.RAD_EXPENSES,
                                NumberOfEmployees = t1.NUMBER_OF_EMPLOYEES,
                                OwnerEquity = t1.OWNER_EQUITY,
                                TotalProfit = t1.TOTAL_PROFIT,
                                PeriodYear=t1.PERIOD_YEAR
                            };
                query = query.Where(f => (
            (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Contains(model.orgcode)) &&
            (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Contains(model.orgname)) &&
            (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
            )).OrderBy(o => o.Create);
                //var query = _context.ApdFctTAx.
                //        Where(a =>
                //        (
                //         (string.IsNullOrWhiteSpace(ORG_CODE) || a.ORG_CODE.Contains(ORG_CODE))
                //        //&& (string.IsNullOrWhiteSpace(BUSINESS_TYPE_STATUS.ToString()) || a.BUSINESS_TYPE_STATUS == (BUSINESS_TYPE_STATUS))
                //        //&& (a.DELETE_FLAG != 1)
                //        )
                //        ).ToList().OrderByDescending(e => e.LAST_UPDATE_DATE);
                var l = query.GroupBy(g => new { g.OrgCode, g.RegistrationType}).OrderBy(o => o.Key.OrgCode);
                int count = query.Count();
                if (model.limit * model.page >= count)
                {
                    model.page = 0;
                }
                query = query.Skip(model.limit * model.page).Take(model.limit);
                //var l= itemList.Skip(model.limit * model.page).Take(model.limit);
                var list = new List<int>();
                //重新定义query里count的值

                //var queryr = new List<ReturnModel>();
                //foreach (var item in l)
                //{
                //    //query.Where(f => f.Key == item.Key.Key).ToList().ForEach(p => p.Count = item.Count());

                //    var currentquery = query.Where(f => f.OrgCode == item.Key.OrgCode).ToList();
                //    foreach (var itemquery in currentquery)
                //    {
                //        itemquery.Count = item.Count();
                //        queryr.Add(itemquery);
                //    }
                //    //listResut.Where(w => w.CategoryID > 30 && w.CategoryID < 40).ToList().ForEach(p => p.CategoryName = p.CategoryName + "bb");
                //    int c = item.Count();
                //    list.Add(c);
                //}

                var listnew = new List<int>();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (i == 0)
                //    {
                //        listnew.Add(0);
                //    }
                //    else if (i == 1)
                //    {
                //        listnew.Add(list[0]);
                //    }
                //    else
                //    {
                //        //listnew.Add(list[i - 1] + list[i - 2]);
                //        listnew.Add(list.Take(i).Sum());
                //    }
                //}

                //int total = listnew;
                //var data = query.ToList();
                result.Content = new { data = query, array = listnew , total =count};
                //return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Content = null, Message = ex.Message };
            }
           
            return result;

        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetListNoPagination")]
        public FuncResult GetListNoPagination([FromBody] adpFctax model = null)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };
            try
            {


                var query = from t1 in context.ApdFctTAx
                            join o in
                            context.ApdDimOrg on t1.ORG_CODE equals o.OrgCode
                            select new ReturnModel
                            {
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
                                recordId = t1.RECORD_ID,
                                Depreciation = t1.DEPRECIATION,
                                EmployeeRemunerationON = t1.EMPLOYEE_REMUNERATION,
                                PROFIT = t1.PROFIT,
                                EntPaidTax = t1.ENT_PAID_TAX,
                                MainBusinessIncome = t1.MAIN_BUSINESS_INCOME,
                                RadEexpenses = t1.RAD_EXPENSES,
                                NumberOfEmployees = t1.NUMBER_OF_EMPLOYEES,
                                OwnerEquity = t1.OWNER_EQUITY,
                                TotalProfit = t1.TOTAL_PROFIT,
                                PeriodYear = t1.PERIOD_YEAR
                            };
                query = query.Where(f => (
            (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Contains(model.orgcode)) &&
            (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Contains(model.orgname)) &&
            (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
            )).OrderBy(o => o.Create);
                var l = query.GroupBy(g => new { g.OrgCode, g.RegistrationType }).OrderBy(o => o.Key.OrgCode);
                int count = query.Count();
                var querylist = query.ToList();
                if (model.limit * model.page >= count)
                {
                    model.page = 0;
                }
                //var l= itemList.Skip(model.limit * model.page).Take(model.limit);
                var list = new List<int>();
                //重新定义query里count的值

                //var queryr = new List<ReturnModel>();
                //foreach (var item in l)
                //{
                //    //query.Where(f => f.Key == item.Key.Key).ToList().ForEach(p => p.Count = item.Count());

                //    var currentquery = query.Where(f => f.OrgCode == item.Key.OrgCode).ToList();
                //    foreach (var itemquery in currentquery)
                //    {
                //        itemquery.Count = item.Count();
                //        queryr.Add(itemquery);
                //    }
                //    //listResut.Where(w => w.CategoryID > 30 && w.CategoryID < 40).ToList().ForEach(p => p.CategoryName = p.CategoryName + "bb");
                //    int c = item.Count();
                //    list.Add(c);
                //}

                var listnew = new List<int>();
                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (i == 0)
                //    {
                //        listnew.Add(0);
                //    }
                //    else if (i == 1)
                //    {
                //        listnew.Add(list[0]);
                //    }
                //    else
                //    {
                //        //listnew.Add(list[i - 1] + list[i - 2]);
                //        listnew.Add(list.Take(i).Sum());
                //    }
                //}

                //int total = listnew;
                //var data = query.ToList();
                result.Content = new { data = querylist, array = listnew, total = count };
                //return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Content = null, Message = ex.Message };
            }

            return result;

        }
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("apxfcttaximportlate")]
        public FileResult apxfcttaximportlate()
        {
            try
            {
                var YearS = DateTime.Now.Year;
                string TempletFileName = $"{hosting.WebRootPath}\\template\\高明区2019年年主营业务收入2000万元及以上或年纳税额100万元及以上工业企业2018年度有关数据情况表-1.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"高明区2019年年主营业务收入2000万元及以上或年纳税额100万元及以上工业企业2018年度有关数据情况表-1.xls");
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
            return await taxBlla.Delete(Ids, HttpContext.CurrentUser(cache).Id);
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
                //if (key.GetType() == typeof(string))
                //{

                //}
                if (string.IsNullOrWhiteSpace(key))
                {
                    fr.IsSuccess = false;
                    fr.Message = "未接收到参数信息!";
                }
                var _key = Convert.ToDecimal(key);
                ApdFctTAx entity = context.ApdFctTAx.FirstOrDefault(f => f.RECORD_ID.Equals(_key));
                if (entity == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "异常参数，未找到数据!";
                }
                //List<ApdFctLandDistrict> listtown = context.ApdFctLandDistrict.Where(f => f.RecordId.Equals(key)).ToList();

                //删除
                context.ApdFctTAx.RemoveRange(entity);


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
        /// 数据导出到excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        public FileResult Export()
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var summarydata = GetListNoPagination(new adpFctax() { orgname = "", orgcode = "" });
                var data = (List<ReturnModel>)((dynamic)summarydata.Content).data;
                var groupdata = (List<int>)((dynamic)summarydata.Content).array;

                int Years = DateTime.Now.Year;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\高明区2019年年主营业务收入2000万元及以上或年纳税额100万元及以上工业企业2018年度有关数据情况表-1.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //从row7开始，B到Q(1到16)
                //sheet1.GetRow(7).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                //sheet1.GetRow(8).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                //sheet1.GetRow(9).GetCell(2).SetCellValue("佛山市高明盈夏纺织有限公司");
                var obj = new Object();
                for (int i = 6; i < data.Count + 6; i++)
                {
                    try
                    {
                        var cur = data[i-6];
                        if (i == 392 || i == 397)
                        {
                            string p = string.Empty;
                        }
                        sheet1.GetRow(i).GetCell(1).SetCellValue(data[i - 6].OrgName);
                        sheet1.GetRow(i).GetCell(2).SetCellValue(data[i - 6].Town);
                        sheet1.GetRow(i).GetCell(3).SetCellValue(data[i - 6].OrgCode);
                        sheet1.GetRow(i).GetCell(4).SetCellValue(data[i - 6].RegistrationType);
                        sheet1.GetRow(i).GetCell(5).SetCellValue(data[i - 6].Address);
                        sheet1.GetRow(i).GetCell(6).SetCellValue(data[i - 6].LegalRepresentative);
                        sheet1.GetRow(i).GetCell(7).SetCellValue(data[i - 6].Phone);
                        sheet1.GetRow(i).GetCell(8).SetCellValue(data[i - 6].LinkMan);
                        sheet1.GetRow(i).GetCell(9).SetCellValue(data[i - 6].Phone2);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    sheet1.GetRow(i).GetCell(10).SetCellValue(Convert.ToDouble(data[i - 6].EntPaidTax ?? 0));
                    sheet1.GetRow(i).GetCell(11).SetCellValue(Convert.ToDouble(data[i - 6].EmployeeRemunerationON ?? 0));
                    sheet1.GetRow(i).GetCell(12).SetCellValue(Convert.ToDouble(data[i - 6].Depreciation ?? 0));
                    sheet1.GetRow(i).GetCell(13).SetCellValue(Convert.ToDouble(data[i - 6].PROFIT ?? 0));
                    sheet1.GetRow(i).GetCell(14).SetCellValue(Convert.ToDouble(data[i - 6].MainBusinessIncome ?? 0));
                    sheet1.GetRow(i).GetCell(15).SetCellValue(Convert.ToDouble(data[i - 6].RadEexpenses ?? 0));
                    sheet1.GetRow(i).GetCell(16).SetCellValue(Convert.ToDouble(data[i - 6].NumberOfEmployees ?? 0));
                    sheet1.GetRow(i).GetCell(17).SetCellValue(Convert.ToDouble(data[i - 6].OwnerEquity ?? 0));
                    sheet1.GetRow(i).GetCell(18).SetCellValue(Convert.ToDouble(data[i - 6].TotalProfit ?? 0));
                    sheet1.GetRow(i).GetCell(19).SetCellValue(data[i - 6].Remark);
                }

                /*
                 *
                 * 处理部分行合并(测试B1到J9)
                 * CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                 * 合并的列是一样的，只需要处理好行的关系即可
                 * 模板是从第7行(即行坐标为6)开始写数据
                 * 10、11、12列不合并
                 * **/

                //int currentIndex = 6;
                //groupdata.RemoveAt(groupdata.Count- 1);
               
                   
                        //for (int j = 1; j < 17; j++)
                        //{
                        //    if (j == 10 || j == 11 || j == 12)
                        //    {
                        //        continue;
                        //    }
                        //    sheet1.AddMergedRegion(new CellRangeAddress(currentIndex, currentIndex + data.Count, j, j));
                        //}
                  
                   

                
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
               
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }

        ///// <summary>
        ///// excel数据导入到数据库(mapcoderela表)
        ///// </summary>
        ///// <param name="excelfile"></param>
        ///// <returns></returns> 
        //[Route("upload")]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        //[HttpGet("{year}")]
        //public FuncResult importtax(string year)
        //{
            

        //    FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };
        //    try
        //    {
        //        var excelfile = Request.Form.Files[0];
        //        List<Demo> datalist = new List<Demo>();
        //        if (excelfile.Length > 0)
        //        {
        //            DataTable dt = new DataTable();
        //            string strMsg;
        //            string region = excelfile.FileName;//文件名即为区域名
        //            //利用IFormFile里面的OpenReadStream()方法直接读取文件流
        //            dt = Common.ExcelHelper.ExcelToDatatable(excelfile.OpenReadStream(), Path.GetExtension(excelfile.FileName), out strMsg);
        //            var filenameliu = excelfile.OpenReadStream();
        //            if (!string.IsNullOrEmpty(strMsg))
        //            {
        //                result.IsSuccess = false;
        //                result.Message = strMsg;
        //                return result;
        //            }
                   
        //            if (dt.Rows.Count > 0)
        //            {
        //                var listorgan = context.ApdDimOrg.ToList();
        //                //需要导入到数据库的数据
        //                var lista = JsonConvert.SerializeObject(dt);
        //                //反序列化
        //                datalist = JsonConvert.DeserializeObject<List<Demo>>(JsonConvert.SerializeObject(dt));
        //                List<Demo> filterdata = datalist.Where(f => !(f.W1 == "" ) && f.W1 != null).ToList();
                       
        //                LogService.WriteInfo($"{DateTime.Now},有{datalist.Count}条数据");
        //                LogService.WriteInfo($"{DateTime.Now},有{filterdata.Count}条数据");
        //                LogService.WriteInfo(JsonConvert.SerializeObject(filterdata));

        //                //处理筛选过后的数据
        //                string g1 = string.Empty;
        //                string g3 = string.Empty;
        //                foreach (var item in filterdata)
        //                {
        //                    if (!string.IsNullOrEmpty(item.W1))
        //                    {
        //                        g1 = item.W1;
        //                    }
        //                    if (string.IsNullOrEmpty(item.W1))
        //                    {
        //                        //为上一个有值的g1
        //                        item.W1 = g1;
        //                    }


        //                }

        //                //var data = filterdata.Select(s)

        //                    //.GroupBy(g => new {g.W1, g.W3, g.W11, g.W12, g.W13, g.W14, g.W15, g.W16, g.W17, g.W18, g.W10 })
        //                var data_one = filterdata.Select(s => new ApdFctTAxModel
        //                {

        //                    ORG_CODE = s.W3,
        //                    ENT_PAID_TAX = s.W10 == null ? 0 : Convert.ToDecimal(s.W10),
        //                    EMPLOYEE_REMUNERATION = s.W11 == null ? 0 : Convert.ToDecimal(s.W11) ,
        //                    DEPRECIATION = s.W12 == null ? 0 : Convert.ToDecimal(s.W12),
        //                    PROFIT = s.W13 == null ? 0 : Convert.ToDecimal(s.W13),
        //                    MAIN_BUSINESS_INCOME = s.W14 == null ? 0 : Convert.ToDecimal(s.W14),
        //                    TOTAL_PROFIT = s.W14 == null ? 0 : Convert.ToDecimal(s.W14),
        //                    OWNER_EQUITY = s.W17 == null ? 0 : Convert.ToDecimal(s.W17),
        //                    NUMBER_OF_EMPLOYEES = s.W16 == null ? 0 : Convert.ToDecimal(s.W16),
        //                    RAD_EXPENSES = s.W15 == null ? 0 : Convert.ToDecimal(s.W15)
        //                });

        //                //存在orgcode不存在的情况就整个都不写入
        //                LogService.WriteInfo($"{DateTime.Now},有{data_one.Count()}条数据");
        //                foreach (var item in data_one)
        //                {
        //                    var currentorganization = listorgan.FirstOrDefault(f => f.OrgCode.Equals(item.ORG_CODE));
        //                    if (currentorganization == null)
        //                    {
        //                        LogService.WriteInfo($"{DateTime.Now},有{11111111111111}条数据");
        //                        result.IsSuccess = false;
        //                        result.Message = $"此机构号:{item.ORG_CODE}找不到对应机构，导入失败！";
        //                        return result;
        //                    }
        //                    ApdFctTAx Datatb = new ApdFctTAx()
        //                    {
        //                        EMPLOYEE_REMUNERATION = item.EMPLOYEE_REMUNERATION,
        //                        DEPRECIATION = item.DEPRECIATION,
        //                        PROFIT = item.PROFIT,
        //                        MAIN_BUSINESS_INCOME = item.MAIN_BUSINESS_INCOME,
        //                        TOTAL_PROFIT = item.TOTAL_PROFIT,
        //                        OWNER_EQUITY = item.OWNER_EQUITY,
        //                        ENT_PAID_TAX=item.ENT_PAID_TAX,
        //                        NUMBER_OF_EMPLOYEES = item.NUMBER_OF_EMPLOYEES,
        //                        RAD_EXPENSES = item.RAD_EXPENSES,
        //                        ORG_CODE = item.ORG_CODE,
        //                        CREATION_DATE = DateTime.Now,
        //                        LAST_UPDATE_DATE = DateTime.Now,
        //                        PERIOD_YEAR = Convert.ToDecimal(year),
        //                        RECORD_ID = Guid.NewGuid().ToString()
        //                    };
        //                     context.ApdFctTAx.Add(Datatb);
        //                }



        //                //}
        //                using (IDbContextTransaction trans = context.Database.BeginTransaction())
        //                {
        //                    try
        //                    {
        //                        context.SaveChanges();
        //                        trans.Commit();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        LogService.WriteInfo(ex);
        //                        throw new Exception("error", ex);
        //                    }
        //                }
                        

        //            }
        //            else
        //            {
        //                result.IsSuccess = false;
        //                result.Message = "excel无数据！";
        //            }
        //        }



        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsSuccess = false;
        //        result.Message = ex.Message;
        //        return result;
        //    }

        //}

        /// <summary>
        /// excel数据导入到数据库(apdfcttax)
        /// 一个机构一年只有一批数据
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [Route("upload")]
        [HttpGet("{year}")]
        public FuncResult Importa(string year)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "操作成功" };
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
                    dt = Common.ExcelHelper.ExcelToDatatablePollutant(excelfile.OpenReadStream(), Path.GetExtension(excelfile.FileName), out strMsg);
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
                        var prefilter = datalist.Where(f => !(f.W1 == "") && f.W1 != null).ToList();
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }

                        /*
                     *1、筛选数据前，检查数据格式，只需要检测数值类型的列 
                     * **/

                        int count = 1;//错误列号(对应实际列6)
                        string colname = "";
                        for (int i = 0; i < prefilter.Count(); i++)
                        {
                            try
                            {
                                var current = prefilter[i];
                                var w_10 = current.W10;
                                if (w_10 == "")
                                {
                                    continue;
                                }
                                colname = "W10";
                                Convert.ToDecimal(w_10);

                                var w_11 = current.W11;
                                if (w_11 == "")
                                {
                                    continue;
                                }
                                colname = "W11";
                                Convert.ToDecimal(w_11);

                                var w_12 = current.W12;
                                if (w_12 == "")
                                {
                                    continue;
                                }
                                colname = "W12";
                                Convert.ToDecimal(w_12);

                                var w_13 = current.W13;
                                if (w_13 == "")
                                {
                                    continue;
                                }
                                colname = "W13";
                                Convert.ToDecimal(w_13);

                                var w_14 = current.W14;
                                if (w_14 == "")
                                {
                                    continue;
                                }
                                colname = "W14";
                                Convert.ToDecimal(w_14);

                                var w_15 = current.W15;
                                if (w_15 == "")
                                {
                                    continue;
                                }
                                colname = "W15";
                                Convert.ToDecimal(w_15);

                                var w_16 = current.W16;
                                if (w_16 == "")
                                {
                                    continue;
                                }
                                colname = "W16";
                                Convert.ToDecimal(w_16);

                                var w_17 = current.W17;
                                if (w_17 == "")
                                {
                                    continue;
                                }
                                colname = "W17";
                                Convert.ToDecimal(w_17);

                                var w_18 = current.W18;
                                if (w_18 == "")
                                {
                                    continue;
                                }
                                colname = "W18";
                                Convert.ToDecimal(w_18);

                                var w_19 = current.W19;
                                if (w_19 == "")
                                {
                                    continue;
                                }
                                colname = "W19";
                                Convert.ToDecimal(w_19);

                                count++;
                            }
                            catch (Exception ex)
                            {
                                LogService.WriteError(ex);
                                result.IsSuccess = false;
                                result.Message = $"第{count + 9}行，{colname}列数据异常！";
                                return result;

                            }
                        }

                        var filterdata = prefilter.Select(s => new ApdFctTAx
                        {
                            RECORD_ID = Guid.NewGuid().ToString(),
                            PERIOD_YEAR = Convert.ToDecimal(year),
                            ORG_CODE = s.W3,
                            ENT_PAID_TAX = s.W10 == "" ? 0 : Convert.ToDecimal(s.W10),
                            EMPLOYEE_REMUNERATION = s.W11 == ""  ? 0 : Convert.ToDecimal(s.W11),
                            DEPRECIATION = s.W12 == ""  ? 0 : Convert.ToDecimal(s.W12),
                            PROFIT = s.W13 == "" ? 0 : Convert.ToDecimal(s.W13),
                            MAIN_BUSINESS_INCOME = s.W14 == ""  ? 0 : Convert.ToDecimal(s.W14),
                            RAD_EXPENSES = s.W15 == ""  ? 0 : Convert.ToDecimal(s.W15),
                            NUMBER_OF_EMPLOYEES = s.W16 == ""  ? 0 : Convert.ToDecimal(s.W16),
                            OWNER_EQUITY = s.W17 == ""  ? 0 : Convert.ToDecimal(s.W17),
                            TOTAL_PROFIT = s.W18 == ""  ? 0 : Convert.ToDecimal(s.W18),
                            Remark = s.W19
                        });

                        result = taxBlla.WriteData(filterdata, year, HttpContext.CurrentUser(cache).Id);

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
                var town2 = context.ApdFctTAx.Where(f => f.ORG_CODE.Equals(orgcode) && f.PERIOD_YEAR.Equals(formatyear));
                return town2 != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="model"></param>
        ///// <param name="currentUserId"></param>
        ///// <returns></returns>
        //public async Task<FuncResult> Update(string id, ApdFctTAxModel model, string currentUserId)
        //{
        //    ApdFctTAx entity = await context.ApdFctTAx.FindAsync(id);
        //    if (entity == null)
        //    {
        //        return new FuncResult() { IsSuccess = false, Message = "ID错误!" };
        //    }
        //    entity.ORG_CODE = model.ORG_CODE;
        //    entity.ENT_PAID_TAX = model.ENT_PAID_TAX;
        //    entity.EMPLOYEE_REMUNERATION = model.EMPLOYEE_REMUNERATION;
        //    entity.DEPRECIATION = model.DEPRECIATION;
        //    entity.PROFIT = model.PROFIT;
        //    entity.MAIN_BUSINESS_INCOME = model.MAIN_BUSINESS_INCOME;
        //    entity.RAD_EXPENSES = model.RAD_EXPENSES;
        //    entity.NUMBER_OF_EMPLOYEES = model.NUMBER_OF_EMPLOYEES;
        //    entity.OWNER_EQUITY = model.OWNER_EQUITY;
        //    entity.TOTAL_PROFIT = model.TOTAL_PROFIT;

        //    entity.LAST_UPDATE_DATE = model.LAST_UPDATE_DATE;
        //    //entity.BUSINESS_ID = model.BUSINESS_ID;
        //    //entity.MATERIAL_ID = model.MATERIAL_ID;

        //    //entity.LAST_UPDATED_BY = currentUserId;
        //    //entity.LAST_UPDATE_DATE = DateTime.Now;


        //    //_context.BusBusinessMaterialInfo.Update(entity);
        //    //await _context.SaveChangesAsync();
        //    return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        //}
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]ApdFctTAxModel model)
        {
            ApdFctTAx entity;
            try
            {
                 entity = await context.ApdFctTAx.FirstOrDefaultAsync(m => m.RECORD_ID == model.RECORD_ID);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
            }
            entity.ORG_CODE = model.ORG_CODE;
            entity.ENT_PAID_TAX = model.ENT_PAID_TAX;
            entity.EMPLOYEE_REMUNERATION = model.EMPLOYEE_REMUNERATION;
            entity.DEPRECIATION = model.DEPRECIATION;
            entity.PROFIT = model.PROFIT;
            entity.MAIN_BUSINESS_INCOME = model.MAIN_BUSINESS_INCOME;
            entity.RAD_EXPENSES = model.RAD_EXPENSES;
            entity.NUMBER_OF_EMPLOYEES = model.NUMBER_OF_EMPLOYEES;
            entity.OWNER_EQUITY = model.OWNER_EQUITY;
            entity.TOTAL_PROFIT = model.TOTAL_PROFIT;
            //entity.LAST_UPDATED_BY = HttpContext.CurrentUser(cache).Id;
            entity.LAST_UPDATE_DATE = DateTime.Now;
           
                context.ApdFctTAx.Update(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {               
                LogService.WriteError(ex);
                return new FuncResult() { IsSuccess = false, Message = "修改时发生了意料之外的错误" };
            }
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };

        }

        public class Demo
        {
            public string W1 { get; set; }

            public string W2 { get; set; }

            public string W3 { get; set; }

            public string W4 { get; set; }

            public string W5 { get; set; }

            public string W6 { get; set; }

            public string W7 { get; set; }

            public string W8 { get; set; }

            public string W9 { get; set; }

            public double? W10 { get; set; }

            public double? W11 { get; set; }

            public double? W12 { get; set; }

            public double? W13 { get; set; }

            public double? W14 { get; set; }
            public double? W15 { get; set; }
            public double? W16 { get; set; }
            public double? W17 { get; set; }
            public double? W18 { get; set; }
            public string W19 { get; set; }

        }
        public class adpFctax
        {
            public string orgname { get; set; }

            public string orgcode { get; set; }

            public string year { get; set; }
            /// <summary>
            /// 页大小
            /// </summary>
            public int limit { get; set; }

            /// <summary>
            /// 页码
            /// </summary>
            public int page { get; set; }
        }

        public class ReturnModel
        {
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
            public decimal? PROFIT { get; set; }
            public decimal? Depreciation { get; set; }
            public decimal? Pforit { get; set; }
            public string recordId { get; set; }
            public decimal? EntPaidTax { get; set; }
            public decimal? MainBusinessIncome { get; set; }
            public decimal? RadEexpenses { get; set; }
            public decimal? EmployeeRemunerationON { get; set; }
            public decimal? OwnerEquity { get; set; }
            public decimal? TotalProfit { get; set; }
            public decimal? NumberOfEmployees { get; set; }
            public string Remark { get; set; }
            public DateTime? Create { get; set; }
        }
    }
}