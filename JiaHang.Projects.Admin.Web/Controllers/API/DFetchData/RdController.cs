using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.DFetchDataBLL;
using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DFetchData.Rd;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DFetchData
{
    [Route("api/[controller]")]
    //[ApiController]
    public class RdController : ControllerBase
    {
        private readonly DataContext context;
        private readonly RdBLL rdBll;
        private readonly IHostingEnvironment hosting;
        private readonly IMemoryCache cache;

        public RdController(DataContext _context,IHostingEnvironment _hosting, IMemoryCache _cache)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.cache = _cache;
            rdBll = new RdBLL(_context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public FuncResult GetList()
        {
            try
            {
                return rdBll.GetList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetListPagination")]
        public FuncResult GetListPagination([FromBody] SearchRdModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }
            return rdBll.GetListPagination(model);
        }

        /// <summary>
        /// 更新详细数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/{recordid}")]
        public FuncResult Update(string recordid,[FromBody] PostRdModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {

                return rdBll.Update(recordid,model,HttpContext.CurrentUser(cache).Id);
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
                ApdFctRD entity = context.ApdFctRD.FirstOrDefault(f => f.RecordId.Equals(_key));
                if (entity == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "异常参数，未找到数据!";
                }

                //删除
                context.ApdFctRD.Remove(entity);
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
                var excelfile = Request.Form.Files[0];
                List<dynamic> datalist = new List<dynamic>();
                if (excelfile.Length > 0)
                {
                    DataTable dt = new DataTable();
                    string strMsg;
                    string filename = excelfile.FileName;//文件名即为区域名
                    //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                    dt = ExcelHelper.ExcelToDatatablePollutant(excelfile.OpenReadStream(), Path.GetExtension(excelfile.FileName), out strMsg);
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
                        var prefilter = datalist.Where(f => !(f.K1 == "") && f.K1 != null);
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }
                        var filterdata = prefilter.Select(g => new ApdFctRD
                        {
                            RecordId = new Random().Next(1, 99999),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.K3,
                            IsHighTech = g.K6,
                            RDExpenditure = g.K7 == "" ? null : Convert.ToDecimal(g.K7),
                            Remark = g.K8,
                            CreationDate=DateTime.Now,
                            LastUpdateDate=DateTime.Now
                        });

                        result = rdBll.WriteData(filterdata, year,HttpContext.CurrentUser(cache).Id);

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
                var summarydata = rdBll.GetList(new SearchRdModel() { orgname = orgname, orgcode = orgcode, year = year, limit = pagesize, page = pagenum });
                var data = (List<ReturnRDModel>)((dynamic)summarydata).Content;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业研发经费支出情况取数表格式-局高新合作交流科.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");


                for (int i = 5; i < data.Count + 5; i++)
                {
                    sheet1.GetRow(i).GetCell(1).SetCellValue(data[i - 5].OrgName);
                    sheet1.GetRow(i).GetCell(2).SetCellValue(data[i - 5].Town);
                    sheet1.GetRow(i).GetCell(3).SetCellValue(data[i - 5].OrgCode);
                    sheet1.GetRow(i).GetCell(4).SetCellValue(data[i - 5].RegistrationType);
                    sheet1.GetRow(i).GetCell(5).SetCellValue(data[i - 5].Address);
                    sheet1.GetRow(i).GetCell(6).SetCellValue(data[i - 5].IsHighTech);
                    sheet1.GetRow(i).GetCell(7).SetCellValue(Convert.ToDouble(data[i - 5].RDExpenditure));
                    sheet1.GetRow(i).GetCell(8).SetCellValue(data[i - 5].Remark);
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

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业研发经费支出情况取数表格式-局高新合作交流科.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业研发经费支出情况取数表格式-局高新合作交流科.xls");
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }
}