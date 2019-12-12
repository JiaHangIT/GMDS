using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.BLL.ExcelImportTax;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.ApdFctTax;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Oracle.ManagedDataAccess.Client;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ApdFctTAxImportController : ControllerBase
    {
        public readonly DataContext context;
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment hosting;

        public ApdFctTAxImportController(DataContext _context, IHostingEnvironment _hosting, IMemoryCache _cache) { this.context = _context; this.hosting = _hosting; this.cache = _cache; }
      
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
            (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
            (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
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
                
                //var l= itemList.Skip(model.limit * model.page).Take(model.limit);
                var list = new List<int>();
                //重新定义query里count的值

                var queryr = new List<ReturnModel>();
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

                //int total = listnew;
                //var data = query.ToList();
                result.Content = new { data = queryr, array = listnew , total =count};
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
                var cd = context.ApdFctTAx.FirstOrDefault(f => f.RECORD_ID.Equals(key));
                //List<ApdFctLandDistrict> listtown = context.ApdFctLandDistrict.Where(f => f.RecordId.Equals(key)).ToList();

                //删除
                context.ApdFctTAx.RemoveRange(cd);


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
                var summarydata = GetList(new adpFctax() { orgname = "", orgcode = "" });
                var data = (List<ReturnModel>)((dynamic)summarydata.Content).data;
                var groupdata = (List<int>)((dynamic)summarydata.Content).array;

                int Years = DateTime.Now.Year;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\高明区"+ Years + "年年主营业务收入2000万元及以上或年纳税额100万元及以上工业企业" + Years + "年度有关数据情况表-1.xls";
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
                    sheet1.GetRow(i).GetCell(10).SetCellValue(Convert.ToDouble(data[i - 6].EntPaidTax));
                    sheet1.GetRow(i).GetCell(11).SetCellValue(Convert.ToDouble(data[i - 6].EmployeeRemunerationON));
                    sheet1.GetRow(i).GetCell(12).SetCellValue(Convert.ToDouble(data[i - 6].Depreciation));
                    sheet1.GetRow(i).GetCell(13).SetCellValue(Convert.ToDouble(data[i - 6].PROFIT));
                    sheet1.GetRow(i).GetCell(14).SetCellValue(Convert.ToDouble(data[i - 6].MainBusinessIncome));
                    sheet1.GetRow(i).GetCell(15).SetCellValue(Convert.ToDouble(data[i - 6].RadEexpenses));
                    sheet1.GetRow(i).GetCell(16).SetCellValue(Convert.ToDouble(data[i - 6].NumberOfEmployees));
                    sheet1.GetRow(i).GetCell(17).SetCellValue(Convert.ToDouble(data[i - 6].OwnerEquity));
                    sheet1.GetRow(i).GetCell(18).SetCellValue(Convert.ToDouble(data[i - 6].TotalProfit));
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

        /// <summary>
        /// excel数据导入到数据库(mapcoderela表)
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns> 
        [Route("upload")]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
        [HttpGet("{year}")]
        public FuncResult importtax(string year)
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
                    string region = excelfile.FileName;//文件名即为区域名
                    //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                    dt = ExcelHelper.ExcelToDatatable(excelfile.OpenReadStream(), Path.GetExtension(excelfile.FileName), out strMsg);
                    var filenameliu = excelfile.OpenReadStream();
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
                        var lista = JsonConvert.SerializeObject(dt);
                        //反序列化
                        datalist = JsonConvert.DeserializeObject<List<Demo>>(JsonConvert.SerializeObject(dt));
                        List<Demo> filterdata = datalist.Where(f => !(f.W1 == "" && f.W2 == "" && f.W3 == "" && f.W4 == "" && f.W5 == "" && f.W6 == "" && f.W7 == "" && f.W8 == "" && f.W9 == "")).ToList();

                        //处理筛选过后的数据
                        string g1 = string.Empty;
                        string g3 = string.Empty;
                        foreach (var item in filterdata)
                        {
                            if (!string.IsNullOrEmpty(item.W1))
                            {
                                g1 = item.W1;
                            }
                            if (string.IsNullOrEmpty(item.W1))
                            {
                                //为上一个有值的g1
                                item.W1 = g1;
                            }


                        }

                        var data_one = filterdata.GroupBy(g => new {g.W1, g.W3, g.W11, g.W12, g.W13, g.W14, g.W15, g.W16, g.W17, g.W18, g.W10 }).Select(s => new ApdFctTAxModel
                        {

                            ORG_CODE = s.Key.W3,
                            EMPLOYEE_REMUNERATION = s.Key.W11,
                            DEPRECIATION = s.Key.W12,
                            PROFIT = s.Key.W13,
                            MAIN_BUSINESS_INCOME = s.Key.W14,
                            TOTAL_PROFIT = s.Key.W18,
                            OWNER_EQUITY = s.Key.W17,
                            NUMBER_OF_EMPLOYEES = s.Key.W16,
                            RAD_EXPENSES = s.Key.W15
                        });

                        //存在orgcode不存在的情况就整个都不写入
                        foreach (var item in data_one)
                        {
                            var currentorganization = listorgan.FirstOrDefault(f => f.OrgCode.Equals(item.ORG_CODE));
                            if (currentorganization == null)
                            {
                                result.IsSuccess = false;
                                result.Message = $"此机构号:{item.ORG_CODE}找不到对应机构，导入失败！";
                                return result;
                            }
                            bool isalreadyexport = isAlreadyExport(item.ORG_CODE, year);
                            ApdFctTAx Datatb = new ApdFctTAx()
                            {
                                EMPLOYEE_REMUNERATION = item.EMPLOYEE_REMUNERATION,
                                DEPRECIATION = item.DEPRECIATION,
                                PROFIT = item.PROFIT,
                                MAIN_BUSINESS_INCOME = item.MAIN_BUSINESS_INCOME,
                                TOTAL_PROFIT = item.TOTAL_PROFIT,
                                OWNER_EQUITY = item.OWNER_EQUITY,
                                NUMBER_OF_EMPLOYEES = item.NUMBER_OF_EMPLOYEES,
                                RAD_EXPENSES = item.RAD_EXPENSES,
                                ORG_CODE = item.ORG_CODE,
                                CREATION_DATE = DateTime.Now,
                                LAST_UPDATE_DATE = DateTime.Now,
                                PERIOD_YEAR = DateTime.Now.Year,
                                RECORD_ID = new Random().Next(1, 999)
                            };
                             context.ApdFctTAx.Add(Datatb);
                        }



                        //}
                        using (IDbContextTransaction trans = context.Database.BeginTransaction())
                        {
                            try
                            {
                                context.SaveChangesAsync();
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {

                                throw new Exception("error", ex);
                            }
                        }
                        

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
                result.Message = ex.Message;
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

            public int? W10 { get; set; }

            public int? W11 { get; set; }

            public int? W12 { get; set; }

            public int? W13 { get; set; }

            public int? W14 { get; set; }
            public int? W15 { get; set; }
            public int? W16 { get; set; }
            public int? W17 { get; set; }
            public int? W18 { get; set; }
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
            public decimal? recordId { get; set; }
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