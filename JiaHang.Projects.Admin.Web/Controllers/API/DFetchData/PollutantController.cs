﻿using System;
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
using JiaHang.Projects.Admin.Model.DFetchData.Pollutant;
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
    public class PollutantController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IHostingEnvironment hosting;
        private readonly PollutantBLL pollutantBll;
        private readonly IMemoryCache cache;

        public PollutantController(DataContext _context, IHostingEnvironment _hosting, IMemoryCache _cache)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.cache = _cache;
            this.pollutantBll = new PollutantBLL(_context);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [Route("GetListPagination")]
        [HttpPost]
        public FuncResult GetListPagination([FromBody] SearchModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }
            return pollutantBll.GetListPagination(model);
        }

        /// <summary>
        /// 更新详细数据
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/{recordid}")]
        public FuncResult Update(string recordid,[FromBody] PostPolluantModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {

                return pollutantBll.Update(recordid,model,HttpContext.CurrentUser(cache).Id);
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
                ApdFctContaminants entity = context.ApdFctContaminants.FirstOrDefault(f => f.RecordId.Equals(_key));
                if (entity == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "异常参数，未找到数据!";
                }

                //删除
                context.ApdFctContaminants.Remove(entity);
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
        /// excel数据导入到数据库(apdfctcontaminants)
        /// 一个机构一年只有一批数据
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [Route("upload")]
        [HttpGet("{year}")]
        public FuncResult Import(string year)
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
                        var prefilter = datalist.Where(f => !(f.H1 == "") && f.H1 != null);
                        if (prefilter == null || prefilter.Count() <= 0)
                        {
                            result.IsSuccess = false;
                            result.Message = "未选择正确的Excel文件或选择的Excel文件无可导入数据！";
                            return result;
                        }
                        var filterdata = prefilter.Select(g => new ApdFctContaminants
                        {
                            RecordId = new Random().Next(1,99999),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.H3,
                            IsInSystem = g.H6,
                            Oxygen = g.H7 == "" ? null : Convert.ToDecimal(g.H7),
                            AmmoniaNitrogen = g.H8 == "" ? null : Convert.ToDecimal(g.H8),
                            SulfurDioxide = g.H9 == "" ? null : Convert.ToDecimal(g.H9),
                            NitrogenOxide = g.H10 == "" ? null : Convert.ToDecimal(g.H10),
                            Coal = g.H11 == "" ? null : Convert.ToDecimal(g.H11),
                            FuelOil = g.H12 == "" ? null : Convert.ToDecimal(g.H12),
                            Hydrogen = g.H13 == "" ? null : Convert.ToDecimal(g.H13),
                            Firewood = g.H14 == "" ? null : Convert.ToDecimal(g.H14),
                            Remark = g.H15
                        });

                        result = pollutantBll.WriteData(filterdata,year,HttpContext.CurrentUser(cache).Id);
                        
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
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        [HttpGet("downtemplate")]
        public FileResult DownTemplate()
        {
            try
            {

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业污染物排放取数表格式-佛山市生态环境局高明分局.xls";
                FileStream file = new FileStream(TempletFileName, FileMode.Open, FileAccess.Read);

                var xssfworkbook = new HSSFWorkbook(file);
                ISheet sheet1 = xssfworkbook.GetSheet("Sheet1");
                //可操作


                //转为字节数组
                var stream = new MemoryStream();
                xssfworkbook.Write(stream);
                var buf = stream.ToArray();
                return File(buf, "application/ms-excel", $"企业污染物排放取数表格式-佛山市生态环境局高明分局.xls");
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
                //var summarydata = pollutantBll.GetList();
                var summarydata = pollutantBll.GetList(new SearchModel() { orgname = orgname, orgcode = orgcode, year = year, limit = pagesize, page = pagenum });
                var data = (List<ReturnPollutantModel>)((dynamic)summarydata).Content;

                string TempletFileName = $"{hosting.WebRootPath}\\template\\企业污染物排放取数表格式-佛山市生态环境局高明分局.xls";
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
                    sheet1.GetRow(i).GetCell(6).SetCellValue(data[i - 5].IsInSystem);
                    sheet1.GetRow(i).GetCell(7).SetCellValue(Convert.ToDouble(data[i - 5].Oxygen));
                    sheet1.GetRow(i).GetCell(8).SetCellValue(Convert.ToDouble(data[i - 5].AmmoniaNitrogen));
                    sheet1.GetRow(i).GetCell(9).SetCellValue(Convert.ToDouble(data[i - 5].SulfurDioxide));
                    sheet1.GetRow(i).GetCell(10).SetCellValue(Convert.ToDouble(data[i - 5].NitrogenOxide));
                    sheet1.GetRow(i).GetCell(11).SetCellValue(Convert.ToDouble(data[i - 5].Coal));
                    sheet1.GetRow(i).GetCell(12).SetCellValue(Convert.ToDouble(data[i - 5].FuelOil));
                    sheet1.GetRow(i).GetCell(13).SetCellValue(Convert.ToDouble(data[i - 5].Hydrogen));
                    sheet1.GetRow(i).GetCell(14).SetCellValue(Convert.ToDouble(data[i - 5].Firewood));
                    sheet1.GetRow(i).GetCell(15).SetCellValue(data[i - 5].Remark);
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
    }
}