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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ApdFctTAxImportController : ControllerBase
    {
        private readonly ExcelTaxImportBLL cExcelTaxImportBLL;
        private readonly IMemoryCache cache;
        public ApdFctTAxImportController(DataContext datacontext, IMemoryCache cache)
        {
            this.cache = cache;
            cExcelTaxImportBLL = new ExcelTaxImportBLL(datacontext);
        }

        //private readonly DAL.EntityFramework.DataContext _context;
        //public ApdFctTAxImportController(DAL.EntityFramework.DataContext context)
        //{
        //    _context = context;
        //}
        /// <summary>
        /// excel数据导入到数据库(mapcoderela表)
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [Route("importtax")]
        public FuncResult importtax()
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };
            try
            {
                var excelfile = Request.Form.Files[0];
                List<ApdFctTAx> mapcoderelalist = new List<ApdFctTAx>();
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
                        
                    }
                   
                    if (dt.Rows.Count > 0)
                    {
                       
                        for (var i = 0; i < dt.Rows.Count; i++)
                        {                            
                                //var obj = dt.Rows[i].ItemArray[j];
                                
                                ApdFctTAx aftax = new ApdFctTAx()
                                {
                                    RECORD_ID = i,
                                    ORG_CODE = "HT",
                                    CREATION_DATE = DateTime.Now,
                                    CREATED_BY = 1,
                                    LAST_UPDATE_DATE = DateTime.Now,
                                    LAST_UPDATED_BY = 1,
                                    PERIOD_YEAR = DateTime.Now.Year,
                                    //EMPLOYEE_REMUNERATION =int.Parse(dt.Rows[i].ItemArray[11].ToString()),//职工薪酬
                                    //DEPRECIATION = int.Parse(dt.Rows[i].ItemArray[12].ToString()),//固定资产折旧
                                    //PROFIT = int.Parse(dt.Rows[i].ItemArray[13].ToString()),//营业利润
                                    //MAIN_BUSINESS_INCOME = int.Parse(dt.Rows[i].ItemArray[14].ToString()),//主营业务收入（万元）
                                    //TOTAL_PROFIT = int.Parse(dt.Rows[i].ItemArray[18].ToString()),//利润总额
                                    //OWNER_EQUITY = int.Parse(dt.Rows[i].ItemArray[17].ToString()),//所有者权益
                                    //NUMBER_OF_EMPLOYEES= int.Parse(dt.Rows[i].ItemArray[16].ToString()),//平均从业人数
                                    //RAD_EXPENSES = int.Parse(dt.Rows[i].ItemArray[15].ToString()),//允许扣除的研发费用
                                };
                                mapcoderelalist.Add(aftax);
                            //var data = db.Insertable<ApdFctTAx>(mapcoderelalist).ExecuteCommand();
                                                       
                        }
                        //需要导入到数据库的数据
                        ApdFctTAx aft = new ApdFctTAx();
                        //aft.RECORD_ID=
                        mapcoderelalist = JsonConvert.DeserializeObject<List<ApdFctTAx>>(JsonConvert.SerializeObject(dt));

                        ////查看当前区域是否有数据，若有区域，则先新原来数据写入mapcoderelahistory表，再清除掉写入新的数据
                        //var handelResult = mapCodelRelaBll.WriteData(mapcoderelalist, region.Split('.')[0]);

                        //if (!handelResult.IsSuccess)
                        //{
                        //    return handelResult;
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
                result.Message = ex.Message;
                return result;
            }

        }
    }
}