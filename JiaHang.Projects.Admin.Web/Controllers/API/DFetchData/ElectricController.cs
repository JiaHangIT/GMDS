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
using Newtonsoft.Json;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DFetchData
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ElectricController : ControllerBase
    {
        private readonly DataContext context;
        private readonly ElectricBLL eletricBll;
        private readonly IHostingEnvironment hosting;

        public ElectricController(DataContext _context,IHostingEnvironment _hosting)
        {
            this.context = _context;
            this.hosting = _hosting;
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
                return null;
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
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
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
                        var prefilter = datalist.Where(f => !(f.D1 == ""));
                        var filterdata = prefilter.Select(g => new ApdFctElectric
                        {
                            RecordId = new Random().Next(1, 99999),
                            PeriodYear = Convert.ToDecimal(year),
                            OrgCode = g.D3,
                            NetSupply = g.D6,
                            Spontaneous = g.D7 == "" ? null : Convert.ToDecimal(g.K7),
                            Remark = g.D8,
                            CreationDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now
                        });

                        result.IsSuccess = eletricBll.WriteData(filterdata, year);

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
    }
}