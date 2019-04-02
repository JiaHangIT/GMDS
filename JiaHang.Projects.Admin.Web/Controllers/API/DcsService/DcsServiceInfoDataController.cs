using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.DcsService;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceInfo.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsService
{
    [Route("api/[controller]")]
    public class DcsServiceInfoDataController : Controller
    {
        private readonly DcsServiceInfoBLL DcsServiceInfo;
        private readonly IMemoryCache cache;

        public DcsServiceInfoDataController(DataContext datacontext, IMemoryCache cache)
        {
            DcsServiceInfo = new DcsServiceInfoBLL(datacontext);
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchDcsServiceInfo model)
        {
            model.page--;if (model.page < 0)
                model.page = 0;

            return DcsServiceInfo.Select(model);
        }
    }
}
