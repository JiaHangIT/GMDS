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
    public class ApdFctOrgIndexVController:ControllerBase
    {
        private readonly ApdFctOrgIndexVBLL apdFctOrgIndexVInfo;


        public ApdFctOrgIndexVController(DataContext context)
        {
            apdFctOrgIndexVInfo = new ApdFctOrgIndexVBLL(context);
        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize, int currentPage,string OrgName)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.Select(pageSize, currentPage,OrgName);
        }
        [Route("SelectTownDdata")]
        [HttpGet]
        public FuncResult SelectTownDdata(int pageSize, int currentPage,  string Town)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.SelectTownDdata(pageSize, currentPage, Town);
        }
        [Route("SelectIndustryData")]
        [HttpGet]
        public FuncResult SelectIndustryData(int pageSize, int currentPage, string Industry)
        {
            currentPage--;
            return apdFctOrgIndexVInfo.SelectIndustryData(pageSize, currentPage, Industry);
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
        public FuncResult IndustryDetail(string code) {
            return apdFctOrgIndexVInfo.IndustryDetail(code);
        }
        [Route("SelectORGInfo")]
        [HttpGet]
        public FuncResult SelectORGInfo(int pageSize, int currentPage, string OrgCode) {
            currentPage--;
            return apdFctOrgIndexVInfo.SelectORGInfo(pageSize,currentPage, OrgCode);
        }
        [Route("BenefiteValuationInfo")]
        [HttpGet]
        public FuncResult BenefiteValuationInfo(string code) {
            return apdFctOrgIndexVInfo.BenefiteValuationInfo(code);
        }
    }
}
