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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DFetchData
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollutantController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IHostingEnvironment hosting;
        private readonly PollutantBLL pollutantBll;

        public PollutantController(DataContext _context, IHostingEnvironment _hosting)
        {
            this.context = _context;
            this.hosting = _hosting;
            this.pollutantBll = new PollutantBLL(_context);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [Route("GetListPagination")]
        public FuncResult GetListPagination()
        {
            return null;
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
                        var prefilter = datalist.Where(f => !(f.H1 == ""));
                        var filterdata = prefilter.Select(g => new ApdFctContaminants
                        {
                            OrgCode = g.H1,
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

                        result.IsSuccess = pollutantBll.WriteData(filterdata);
                        
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