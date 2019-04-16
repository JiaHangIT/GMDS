using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.Projects.Admin.Web.Controllers.SysDataSource
{
    public class SysDataSourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ElemeIndex() {
            return View();
        }
    }
}