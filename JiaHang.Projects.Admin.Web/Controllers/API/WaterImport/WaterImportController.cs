using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.ExcelFctWaterBLL;
using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.ApdFtcWater;
using JiaHang.Projects.Admin.Model.ExcelSearchMode.Gas;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace JiaHang.Projects.Admin.Web.Controllers.API.WaterImport
{
    [Route("api/[controller]")]
    //[ApiController]
    public class WaterImportController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IHostingEnvironment hosting;
        private readonly WaterBll waterBll;
        public WaterImportController(DataContext _context, IHostingEnvironment _hosting)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.waterBll = new WaterBll(_context);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [Route("GetListPagination")]
        [HttpPost]
        public async Task<FuncResult> GetListPagination([FromBody] SearchExcelModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }
            return waterBll.GetListPagination(model);
        }
        /// <summary>
        /// excel数据导入到数据库(apdfctcontaminants)
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
                        var prefilter = datalist.Where(f => !(f.S1 == "") && f.S1 != null).ToList();
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
                                var s_6 = current.S6;
                                if (s_6 == "")
                                {
                                    continue;
                                }
                                colname = "S6";
                                Convert.ToDecimal(s_6);

                                var S_7 = current.S7;
                                if (S_7 == "")
                                {
                                    continue;
                                }
                                colname = "S7";
                                Convert.ToDecimal(S_7);

                                var s_8 = current.S8;
                                if (s_8 == "")
                                {
                                    continue;
                                }
                                colname = "S8";
                                Convert.ToDecimal(s_8);


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
                        var filterdata = prefilter.Select(g => new ApdFctWaterDal
                        {
                            RecordId = new Random().Next(1, 99999),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.S3,
                            Water = g.S6 == "" || null ? 0 : Convert.ToDecimal(g.S6),
                            Other = g.S7 == "" || null ? 0 : Convert.ToDecimal(g.S7),
                            Remark = g.S8,
                            CreationDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now
                        });

                        result = waterBll.WriteData(filterdata, year);

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
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("delete/{key}")]
        public async Task<FuncResult> DeleteData(string key)
        {
            //try
            //{

            //    await gasBll.Delete(key);

            //    return fr;
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception("error", ex);
            //}
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    fr.IsSuccess = false;
                    fr.Message = "未接收到参数信息!";
                }
                var _key = Convert.ToDecimal(key);
                ApdFctWaterDal entity = context.ApdFctWater.FirstOrDefault(f => f.RecordId.Equals(_key));
                if (entity == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "异常参数，未找到数据!";
                }

                //删除
                context.ApdFctWater.Remove(entity);
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
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downtemplate")]
        public FileResult DownTemplate()
        {
            try
            {

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业用水情况取数表格式-佛山水业集团高明供水有限公司.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业用水情况取数表格式-佛山水业集团高明供水有限公司.xls");
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
                var summarydata = waterBll.GetList();
                var data = (List<ReturnWaterModel>)((dynamic)summarydata).Content;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业用水情况取数表格式-佛山水业集团高明供水有限公司.xls";
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
                    sheet1.GetRow(i).GetCell(6).SetCellValue(Convert.ToDouble(data[i - 5].Water));
                    sheet1.GetRow(i).GetCell(7).SetCellValue(Convert.ToDouble(data[i - 5].Other));
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
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(int id, [FromBody] ApdFtcWaterModel model)
        {
            FuncResult data = await waterBll.Update(id, model);
            return data;

        }
    }
}