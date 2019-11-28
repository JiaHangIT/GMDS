using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace JiaHang.Projects.Admin.Web.Controllers.ImportResult
{
    public class ImportEXController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
              
    }
}