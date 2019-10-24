using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    public class EchartReportParmsController:ControllerBase
    {
        private readonly EchartReportParmsBLL echartReportParmsInfo;


        public EchartReportParmsController(DataContext context)
        {
            echartReportParmsInfo = new EchartReportParmsBLL(context);
        }
        [Route("GetMuAdd")]
        [HttpGet]
        public FuncResult GetMuAdd()
        {
            return echartReportParmsInfo.GetMuAdd();
        }
        [Route("GetIndustryData")]
        [HttpGet]
        public FuncResult GetIndustryData(string name, int score1, int score2)
        {
            return echartReportParmsInfo.GetIndustryData(name,score1,score2);
        }
        [Route("GetTownData")]
        [HttpGet]
        public FuncResult GetTownData(string name, int score1, int score2)
        {
            return echartReportParmsInfo.GetTownData(name,score1,score2);
        }
        [Route("GetScorePercentage")]
        [HttpGet]
        public FuncResult GetScorePercentage() {
            return echartReportParmsInfo.GetScorePercentage();
        }
    }
}
