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
                        var prefilter = datalist.Where(f => !(f.X1 == "") && f.X1 != null);
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }
                        var filterdata = prefilter.Select(g => new ApdDimOrg
                        {
                            RecordId = new Random().Next(1, 99999),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.K3,
                            CreationDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now
                        });

                        result = orgBll.WriteData(filterdata, year, HttpContext.CurrentUser(cache).Id);

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
        public FileResult Export()
        {
            try
            {
                FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
                var summarydata = orgBll.GetList();
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
                    //sheet1.GetRow(i).GetCell(6).SetCellValue(data[i - 6].IsHighTech);
                    //sheet1.GetRow(i).GetCell(7).SetCellValue(Convert.ToDouble(data[i - 6].RDExpenditure));
                    //sheet1.GetRow(i).GetCell(8).SetCellValue(data[i - 6].Remark);
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