using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.Projects.Admin.Web.Controllers.SysHelp
{
    public class SysHelpTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}