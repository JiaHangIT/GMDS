﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace JiaHang.Projects.Admin.Web.Controllers.SysModel
{
    public class SysModelGroupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SysModelGroupInfo() {
            return View();
        }
    }
}