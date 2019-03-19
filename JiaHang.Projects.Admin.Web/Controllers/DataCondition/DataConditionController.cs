using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers
{

    /// <summary>
    /// 数据权限
    /// </summary>
    public class DataConditionController :Controller
    {
        /// <summary>
        /// 维护
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 绑定
        /// </summary>
        /// <returns></returns>
        public IActionResult Relation()
        {
            return View();

        }
    }
}
