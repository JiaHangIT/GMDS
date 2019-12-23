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
using JiaHang.Projects.Admin.Model.DFetchData.Electric;
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
    public class ElectricController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ElectricBLL eletricBll;
        private readonly IHostingEnvironment hosting;
        private readonly IMemoryCache cache;

        public ElectricController(DataContext _context,IHostingEnvironment _hosting, IMemoryCache _cache)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.cache = _cache;
            eletricBll = new ElectricBLL(context);
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
                return eletricBll.GetList(new SearchElectricModel ());
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetListPagination")]
        public FuncResult GetListPagination([FromBody] SearchElectricModel model)
        {
            try
            {
                model.page--; if (model.page< 0)
                {
                    model.page = 0;
                }
                return eletricBll.GetListPagination(model);
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("update/{recordid}")]
        public FuncResult Update(string recordid, [FromBody] PostElectricModel model)
        {
            try
            {
                return eletricBll.Update(recordid, model,HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("Delete/{key}")]
        public async Task<FuncResult> Delete(string key)
        {
            try
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
                    ApdFctElectric entity = context.ApdFctElectric.FirstOrDefault(f => f.RecordId.Equals(_key));
                    if (entity == null)
                    {
                        fr.IsSuccess = false;
                        fr.Message = "异常参数，未找到数据!";
                    }

                    //删除
                    context.ApdFctElectric.Remove(entity);
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
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete("batchdelete")]
        public FuncResult Deletes(string[] ids)
        {
            try
            {
                return eletricBll.Deletes(ids);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// excel数据导入到数据库(apdfctelectric)
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
                        var prefilter = datalist.Where(f => !(f.D1 == "") && f.D1 != null).ToList();
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }

                        /*
                        *1、筛选数据前，检查数据格式，只需要检测数值类型的列 
                        * **/

                        int count = 1;//错误列号(对应实际列7)
                        string colname = "";
                        for (int i = 0; i < prefilter.Count(); i++)
                        {
                            try
                            {
                                var current = prefilter[i];
                                var d_6 = current.D6;
                                if (d_6 == "")
                                {
                                    continue;
                                }
                                colname = "D6";
                                Convert.ToDecimal(d_6);

                                var d_7 = current.D7;
                                if (d_7 == "")
                                {
                                    continue;
                                }
                                colname = "D7";
                                Convert.ToDecimal(d_7);
                                //colname = "X13";
                                //Convert.ToDecimal(x_13);
                                count++;
                            }
                            catch (Exception ex)
                            {
                                LogService.WriteError(ex);
                                result.IsSuccess = false;
                                result.Message = $"第{count + 5}行，{colname}列数据异常！";
                                return result;

                            }
                        }

                        var filterdata = prefilter.Select(g => new ApdFctElectric
                        {
                            RecordId = Guid.NewGuid().ToString(),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.D3,
                            NetSupply = g.D6 == "" ? null : Convert.ToDecimal(g.D6),
                            Spontaneous = g.D7 == "" ? null : Convert.ToDecimal(g.D7),
                            Remark = g.D8,
                            CreationDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now,
                            DeleteFlag = 0
                        });

                        result = eletricBll.WriteData(filterdata, year,HttpContext.CurrentUser(cache).Id);

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
                LogService.WriteError(ex);
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
                //var summarydata = eletricBll.GetList();
                pagenum--; if (pagenum < 0)
                {
                    pagenum = 0;
                }
                var summarydata = eletricBll.GetList(new SearchElectricModel() { orgname = orgname, orgcode = orgcode, year = year, limit = pagesize, page = pagenum });
                var data = (List<ReturnElectricModel>)((dynamic)summarydata).Content;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业用电情况取数表格式-高明供电局.xls";
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


                for (int i = 5; i < data.Count + 5; i++)
                {
                    var row = sheet1.CreateRow(i);
                    row.Height = 35 * 20;

                    row.CreateCell(0).SetCellValue(i - 4);
                    row.Cells[0].CellStyle = Style;
                    row.CreateCell(1).SetCellValue(data[i - 5].OrgName);
                    row.Cells[1].CellStyle = Style;
                    row.CreateCell(2).SetCellValue(data[i - 5].Town);
                    row.Cells[2].CellStyle = Style;
                    row.CreateCell(3).SetCellValue(data[i - 5].OrgCode);
                    row.Cells[3].CellStyle = Style;
                    row.CreateCell(4).SetCellValue(data[i - 5].RegistrationType);
                    row.Cells[4].CellStyle = Style;
                    row.CreateCell(5).SetCellValue(data[i - 5].Address);
                    row.Cells[5].CellStyle = Style;
                    row.CreateCell(6).SetCellValue(Convert.ToDouble(data[i - 5].NetSupply));
                    row.Cells[6].CellStyle = Style;
                    row.CreateCell(7).SetCellValue(Convert.ToDouble(data[i - 5].Spontaneous));
                    row.Cells[7].CellStyle = Style;
                    row.CreateCell(8).SetCellValue(data[i - 5].Remark);
                    row.Cells[8].CellStyle = Style;

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

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业用电情况取数表格式-高明供电局.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业用电情况取数表格式-高明供电局.xls");
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }
}