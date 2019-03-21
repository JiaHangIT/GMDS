using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.DcsServiceGroupBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceGroup.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsServiceGroup
{
    [Route("api/[controller]")]
    public class DcsServiceGroupDataController : Controller
    {
        private readonly DcsServiceGroupBLL DcsserviceBll;
        private readonly IMemoryCache cache;

        public DcsServiceGroupDataController(DataContext datacontext, IMemoryCache cache)
        {
            DcsserviceBll = new DcsServiceGroupBLL(datacontext);
            this.cache = cache;
        }

        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchDesServiceGroup model)
        {
            model.page--;if (model.page < 0)
                model.page = 0;

            return DcsserviceBll.Select(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]string [] id)
        {
            return await DcsserviceBll.Delete(id, HttpContext.CurrentUser(cache).Id);

        }
    }
}
