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
    public class ApdFctOrgIndexVController : ControllerBase
    {
        private readonly ApdFctOrgIndexVBLL apdFctOrgIndexVInfo;


        public ApdFctOrgIndexVController(DataContext context)
        {
            apdFctOrgIndexVInfo = new ApdFctOrgIndexVBLL(context);
        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize, int currentPage, string OrgName, string year, string field, string desc)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.Select(pageSize, currentPage, OrgName, year, field, desc);
        }
        //[HttpGet("SelectByFieldOrderby")]
        //public FuncResult SelectByFieldOrderby(int pageSize, int currentPage, string OrgName, string year, string field, string desc) {
        //    currentPage--;
        //    return apdFctOrgIndexVInfo.SelectByFieldOrderby(pageSize, currentPage, OrgName, year, field,desc);
        //}
        [Route("SelectTownDdata")]
        [HttpGet]
        public FuncResult SelectTownDdata(int pageSize, int currentPage, string Town, string OrgName, string year, string Industy)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.SelectTownDdata(pageSize, currentPage, Town, OrgName, year, Industy);
        }
        [Route("SelectIndustryData")]
        [HttpGet]
        public FuncResult SelectIndustryData(int pageSize, int currentPage, string Industry, string OrgName, string year, string Town)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.SelectIndustryData(pageSize, currentPage, Industry, OrgName, year, Town);
        }
        [Route("GetAvarageScore")]
        [HttpGet]
        public FuncResult GetAvarageScore(string year)
        {
            return apdFctOrgIndexVInfo.GetAvarageScore(year);
        }
        [Route("GetTown")]
        [HttpGet]
        public FuncResult GetTown()
        {
            return apdFctOrgIndexVInfo.GetTown();
        }
        [Route("GetIndustry")]
        [HttpGet]
        public FuncResult GetIndustry()
        {
            return apdFctOrgIndexVInfo.GetIndustry();
        }
        [Route("IndustryDetail")]
        [HttpGet]
        public FuncResult IndustryDetail(string code)
        {
            return apdFctOrgIndexVInfo.IndustryDetail(code);
        }
        [Route("SelectORGInfo")]
        [HttpGet]
        public FuncResult SelectORGInfo(int pageSize, int currentPage, string OrgCode)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.SelectORGInfo(pageSize, currentPage, OrgCode);
        }
        [Route("BenefiteValuationInfo")]
        [HttpGet]
        public FuncResult BenefiteValuationInfo(string code)
        {
            return apdFctOrgIndexVInfo.BenefiteValuationInfo(code);
        }
        [HttpGet]
        [Route("ExportAll")]
        public async Task<IActionResult> ExportAll(string orgname, string year, string industy, string town, string field, string desc)
        {
            var result = await apdFctOrgIndexVInfo.ExportAll(orgname, year, industy, town, field, desc);
            return File(result, "application/ms-excel", $"企业评分数据.xlsx");

        }
    }
}
