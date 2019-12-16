using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.OrgBLL;
using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Org;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace JiaHang.Projects.Admin.Web.Controllers.API.Org
{
    [Route("api/[controller]")]
    //[ApiController]
    public class DimOrgController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IHostingEnvironment hosting;
        private readonly DimOrgBll orgBll;
        private readonly IMemoryCache cache;

        public DimOrgController(DataContext _context,IHostingEnvironment _hosting, IMemoryCache _cache)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.cache = _cache;
            this.orgBll = new DimOrgBll(_context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetListPagination")]
        public FuncResult GetListPagination([FromBody] SearchOrgModel model)
        {
            try
            {
                model.page--; if (model.page < 0)
                {
                    model.page = 0;
                }
                return orgBll.GetListPagination(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public FuncResult Add([FromBody]PostOrgModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return orgBll.Add(model,HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update/{recordid}")]
        public FuncResult Update(string recordid,[FromBody]PostOrgModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return orgBll.Update(recordid,model, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <returns></returns>
        [HttpGet("delete/{recordid}")]
        public FuncResult Delete(string recordid)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return orgBll.Delete(recordid, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// excel数据导入到数据库(apdfctrd)
        /// 一个机构一年只有一批数据
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [Route("upload")]
        [HttpGet("{year}")]
        public FuncResult Import(string year)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "操作成功!" };
            try
            {
                //System.Threading.Thread.Sleep(6000);
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
                        var prefilter = datalist.Where(f => !(f.X1 == "") && f.X1 != null).ToList();
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }
                        /*
                         *1、筛选数据前，检查数据格式，只需要检测数值类型的列 
                         *2、如当前是企业的，导入时只需要判断列X12(可以为空的或者是数值)
                         * **/

                        int count = 1;//错误列号(对应实际列7)
                        string colname = "";
                        for (int i = 0; i < prefilter.Count(); i++)
                        {
                            try
                            {
                                var current = prefilter[i];
                                var x_12 = current.X12;
                                if (x_12 == "")
                                {
                                    continue;
                                }
                                colname = "X12";
                                Convert.ToDecimal(x_12);
                                //colname = "X13";
                                //Convert.ToDecimal(x_13);
                                count++;
                            }
                            catch (Exception ex)
                            {
                                LogService.WriteError(ex);
                                result.IsSuccess = false;
                                result.Message = $"第{count + 6}行，{colname}列数据异常！";
                                return result;
                                throw new Exception("err",ex);
                            }
                        }

                        var filterdata = prefilter.Select(g => new ApdDimOrg
                        {
                            OrgName = g.X1,
                            Town = g.X2,
                            OrgCode = g.X3,
                            RegistrationType = g.X4,
                            Address = g.X5,
                            LegalRepresentative = g.X6,
                            Phone = g.X7,
                            LinkMan = g.X8,
                            Phone2 = g.X9,
                            Industry = g.X10,
                            RegistrationStatus = g.X11,
                            RegistrationMoney = g.X12 == "" ? null : Convert.ToDecimal(g.X12),
                            //RegistrationDate = Convert.ToDateTime(g.X13),
                            PeriodYear = Convert.ToDecimal(year),
                            CreationDate = DateTime.Now,
                            CreatedBy = Convert.ToDecimal(HttpContext.CurrentUser(cache).Id),
                            LastUpdateDate = DateTime.Now
                        });

                        //result = orgBll.WriteData(filterdata, year, HttpContext.CurrentUser(cache).Id);

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
        /// 数据导出到excel
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        public FileResult Export(int pagesize, int pagenum, string orgname, string orgcode, string year)
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var summarydata = orgBll.GetList(new SearchOrgModel() { orgname = orgname, orgcode = orgcode, year = year, limit = pagesize, page = pagenum });
                var data = (List<ApdDimOrg>)((dynamic)summarydata).Content;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业信息.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");


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
                    sheet1.GetRow(i).GetCell(10).SetCellValue(data[i - 6].Industry);
                    sheet1.GetRow(i).GetCell(11).SetCellValue(data[i - 6].RegistrationStatus);
                    sheet1.GetRow(i).GetCell(12).SetCellValue(Convert.ToDouble(data[i - 6].RegistrationMoney));
                    sheet1.GetRow(i).GetCell(13).SetCellValue(Convert.ToString(data[i - 6].CreationDate));
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
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downtemplate")]
        public FileResult DownTemplate()
        {
            try
            {

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业信息.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业信息.xls");
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }
}