using JiaHang.Projects.Admin.BLL.DashboardBLL;
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
    public class DashboardDataController : ControllerBase
    {
        private readonly DashboardBLL dashboardBLL;
        

        public DashboardDataController(DataContext context)
        {
            dashboardBLL = new DashboardBLL(context);
        }
        [Route("Statistics")]
        [HttpGet]
        public FuncResult Statistics() {
            var data = dashboardBLL.Statistics();
            return data;
        }
    }
    
}
