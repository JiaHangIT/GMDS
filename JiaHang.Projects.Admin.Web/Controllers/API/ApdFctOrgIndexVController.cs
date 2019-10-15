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
    }
}
