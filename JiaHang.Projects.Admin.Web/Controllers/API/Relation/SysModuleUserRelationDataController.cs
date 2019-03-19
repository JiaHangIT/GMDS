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
    public class SysModuleUserRelationDataController : ControllerBase
    {
        private readonly CurrentUserRouteBLL currentUserRouteBLL;
        private readonly SysModuleUserRelationBLL sysModuleUserRelationBLL;
        private readonly IMemoryCache cache;
        public SysModuleUserRelationDataController(DataContext datacontext,IMemoryCache cache)
        {
            this.cache = cache;
            sysModuleUserRelationBLL = new SysModuleUserRelationBLL(datacontext);
            currentUserRouteBLL = new CurrentUserRouteBLL(datacontext);
        }

        [HttpGet]
        public async Task<FuncResult> Get()
        {
            var data = await sysModuleUserRelationBLL.Select();
            return data;
        }

        [Route("AddOrUpdate")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdate([FromBody] List<SysModuleUserRelationModel> data)
        {
            return await sysModuleUserRelationBLL.AddOrUpdate(data, HttpContext.CurrentUser(cache).Id);

        }


        /// <summary>
        /// 获取未绑定用户/用户组
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("NotBindUsers/{moduleId}")]
        public FuncResult NotBindUsers(string moduleId)
        {

            return sysModuleUserRelationBLL.NotBindUsers(moduleId);
        }


        [HttpDelete("{id}")]
        public async Task<FuncResult> Delte(string Id)
        {
            return await sysModuleUserRelationBLL.Delte(Id, HttpContext.CurrentUser(cache).Id);
        }


        [Route("GetRoutes")]
        [HttpGet]
        public object GetRoutes()
        {
            return currentUserRouteBLL.GetRoutes(HttpContext.CurrentUser(cache).Id);
        }

        //[Route("GetCondition/{id}")]
        //[HttpGet]
        //public FuncResult GetConditionPropertyByModuleId([FromRoute]string id)
        //{
        //    return sysModuleUserRelationBLL.ModuleConditionAndProperty(id);
        //}
    }
}
