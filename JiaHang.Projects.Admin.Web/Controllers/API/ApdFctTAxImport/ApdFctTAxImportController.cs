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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Oracle.ManagedDataAccess.Client;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ApdFctTAxImportController : ControllerBase
    {
        public readonly DataContext context;

        public ApdFctTAxImportController(DataContext _context)
        {
            this.context = _context;
        }
        private readonly ExcelTaxImportBLL storeService;
        private readonly IMemoryCache cache;
        public ApdFctTAxImportController(DataContext dataContext, IMemoryCache cache)
        {
            storeService = new ExcelTaxImportBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("GetList")]
        public FuncResult GetList(int pageSize, int pageNum, string OrgName)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };

            var query = from t1 in context.ApdFctTAx
                        join o in
                        context.ApdDimOrg on t1.ORG_CODE equals o.OrgCode
                        where (
                              (string.IsNullOrWhiteSpace(OrgName) || o.OrgName.Contains(OrgName))
                             )
                        select new
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
                            DEPRECIATION = t1.DEPRECIATION,
                            PROFIT = t1.PROFIT,
                            ENT_PAID_TAX = t1.ENT_PAID_TAX,
                            MAIN_BUSINESS_INCOME = t1.MAIN_BUSINESS_INCOME,
                            RAD_EXPENSES = t1.RAD_EXPENSES,
                            NUMBER_OF_EMPLOYEES = t1.NUMBER_OF_EMPLOYEES,
                            OWNER_EQUITY = t1.OWNER_EQUITY,
                            TOTAL_PROFIT = t1.TOTAL_PROFIT,

                        };
            int total = query.Count();
            var data = query.Skip(pageSize * pageNum).Take(pageSize).ToList();
            result.Content = new { data = query, array = total };
            return result;

        }
        
        /// <summary>
        /// excel数据导入到数据库(mapcoderela表)
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>       
        [HttpPost("importtax")]
        public FuncResult importtax()
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

                        var data_one = filterdata.GroupBy(g => new { g.W3, g.W11, g.W12, g.W13, g.W14, g.W15, g.W16, g.W17, g.W18, g.W10 }).Select(s => new ApdFctTAxModel
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
    }
}