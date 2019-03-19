using JiaHang.Projects.Admin.BLL.Relation;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Relation;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    public class SysModuleRouteRelationDataController : ControllerBase
    {
        private readonly SysModuleRouteRelationBLL  sysModuleRouteRelationBLL;
        private readonly IMemoryCache cache;
        public SysModuleRouteRelationDataController(DataContext datacontext,IMemoryCache cache)
        {
            this.cache = cache;
            sysModuleRouteRelationBLL = new SysModuleRouteRelationBLL(datacontext);
        }

        [HttpGet]
        public async Task<FuncResult<List<ModuleRouteRelationResponseModel>>>  Get()
        {
            var data = await sysModuleRouteRelationBLL.Select();
            return data;
        }

        [Route("AddOrUpdate")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdate([FromBody] List<ModuleRouteRelationRequestModel> data)
        {
            return await sysModuleRouteRelationBLL.AddOrUpdate(data, HttpContext.CurrentUser(cache).Id);

        }


        /// <summary>
        /// 获取未绑定用户/用户组
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("NotBindRoute")]
        public FuncResult NotBindRoute()
        {

            return sysModuleRouteRelationBLL.NotBindRoute();
        }


        [HttpDelete("{id}")]
        public async Task<FuncResult> Delte(string Id)
        {
            return await sysModuleRouteRelationBLL.Delete(Id, HttpContext.CurrentUser(cache).Id);
        }
    }
}
