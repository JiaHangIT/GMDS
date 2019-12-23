using JiaHang.Projects.Admin.BLL;
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
                                PeriodYear=t1.PERIOD_YEAR,
                                Create = t1.CREATION_DATE
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
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
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
        public FileResult Export(int pagesize, int pagenum, string orgname, string orgcode, string year)
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var summarydata = GetListNoPagination(new adpFctax() { orgname = orgname, orgcode = orgcode, year = year });
                var data = (List<ReturnModel>)((dynamic)summarydata.Content).data;
                var groupdata = (List<int>)((dynamic)summarydata.Content).array;

                int Years = DateTime.Now.Year;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\高明区2019年年主营业务收入2000万元及以上或年纳税额100万元及以上工业企业2018年度有关数据情况表-1.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");

                ICellStyle Style = xssfworkbook.CreateCellStyle();

                Style.Alignment = HorizontalAlignment.Center;
                Style.VerticalAlignment = VerticalAlignment.Center;
                Style.BorderTop = BorderStyle.Thin;
                Style.BorderRight = BorderStyle.Thin;
                Style.BorderLeft = BorderStyle.Thin;
                Style.BorderBottom = BorderStyle.Thin;
                Style.DataFormat = 0;

                var obj = new Object();
                for (int i = 6; i < data.Count + 6; i++)
                {
                    var row = sheet1.CreateRow(i);
                    row.Height = 35 * 20;


                    row.CreateCell(0).SetCellValue(i - 5);
                    row.Cells[0].CellStyle = Style;
                    row.CreateCell(1).SetCellValue(data[i - 6].OrgName);
                    row.Cells[1].CellStyle = Style;
                    row.CreateCell(2).SetCellValue(data[i - 6].Town);
                    row.Cells[2].CellStyle = Style;
                    row.CreateCell(3).SetCellValue(data[i - 6].OrgCode);
                    row.Cells[3].CellStyle = Style;
                    row.CreateCell(4).SetCellValue(data[i - 6].RegistrationType);
                    row.Cells[4].CellStyle = Style;
                    row.CreateCell(5).SetCellValue(data[i - 6].Address);
                    row.Cells[5].CellStyle = Style;
                    row.CreateCell(6).SetCellValue(data[i - 6].LegalRepresentative);
                    row.Cells[6].CellStyle = Style;
                    row.CreateCell(7).SetCellValue(data[i - 6].Phone);
                    row.Cells[7].CellStyle = Style;
                    row.CreateCell(8).SetCellValue(data[i - 6].LinkMan);
                    row.Cells[8].CellStyle = Style;
                    row.CreateCell(9).SetCellValue(data[i - 6].Phone2);
                    row.Cells[9].CellStyle = Style;
                    row.CreateCell(10).SetCellValue(Convert.ToDouble(data[i - 6].EntPaidTax ?? 0));
                    row.Cells[10].CellStyle = Style;
                    row.CreateCell(11).SetCellValue(Convert.ToDouble(data[i - 6].EmployeeRemunerationON ?? 0));
                    row.Cells[11].CellStyle = Style;
                    row.CreateCell(12).SetCellValue(Convert.ToDouble(data[i - 6].Depreciation ?? 0));
                    row.Cells[12].CellStyle = Style;
                    row.CreateCell(13).SetCellValue(Convert.ToDouble(data[i - 6].PROFIT ?? 0));
                    row.Cells[13].CellStyle = Style;
                    row.CreateCell(14).SetCellValue(Convert.ToDouble(data[i - 6].MainBusinessIncome ?? 0));
                    row.Cells[14].CellStyle = Style;
                    row.CreateCell(15).SetCellValue(Convert.ToDouble(data[i - 6].RadEexpenses ?? 0));
                    row.Cells[15].CellStyle = Style;
                    row.CreateCell(16).SetCellValue(Convert.ToDouble(data[i - 6].NumberOfEmployees ?? 0));
                    row.Cells[16].CellStyle = Style;
                    row.CreateCell(17).SetCellValue(Convert.ToDouble(data[i - 6].OwnerEquity ?? 0));
                    row.Cells[17].CellStyle = Style;
                    row.CreateCell(18).SetCellValue(Convert.ToDouble(data[i - 6].TotalProfit ?? 0));
                    row.Cells[18].CellStyle = Style;
                    row.CreateCell(19).SetCellValue(data[i - 6].Remark);
                    row.Cells[19].CellStyle = Style;
                }


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