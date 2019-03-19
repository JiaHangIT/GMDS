using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysRoute;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysRoute
{
    [Route("api/[controller]")]
    public class SysRouteDataController : ControllerBase
    {
        private readonly SysRouteBLL sysRouteBLL;
        private readonly IMemoryCache cache;
        public SysRouteDataController(DataContext dataContext,IMemoryCache cache)
        {
            this.cache = cache;
            sysRouteBLL = new SysRouteBLL(dataContext);
        }

        [HttpGet]
        public async Task<FuncResult> Get()
        {
            return await sysRouteBLL.Select();
        }

        //[Route("AddOrUpdateArea")]
        //[HttpPost]
        //public async Task<FuncResult> AddOrUpdateArea([FromBody]AreaRouteModel model)
        //{
        //    return await sysRouteBLL.AddOrUpdateArea(model, HttpContext.CurrentUser(cache).Id);
        //}


        [Route("AddOrUpdateController")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdateController([FromBody]ControllerRouteModel model)
        {
            return await sysRouteBLL.AddOrUpdateController(model, HttpContext.CurrentUser(cache).Id);
        }


        [Route("AddOrUpdateMethod")]
        [HttpPost]
        public async Task<FuncResult> AddOrUpdateMethod([FromBody]MethodRouteModel model)
        {
            return await sysRouteBLL.AddOrUpdateMethod(model, HttpContext.CurrentUser(cache).Id);
        }

        //[HttpDelete]
        //[Route("DeleteAreaRoute/{id}")]
        //public async Task<FuncResult> DeleteAreaRoute(string id) {
        //    return await sysRouteBLL.DeleteAreaRoute(id, HttpContext.CurrentUser(cache).Id);
        //}

        [HttpDelete]
        [Route("DeleteControllerRoute/{id}")]
        public async Task<FuncResult> DeleteControllerRoute(string id)
        {
            return await sysRouteBLL.DeleteControllerRoute(id, HttpContext.CurrentUser(cache).Id);
        }

        [HttpDelete]
        [Route("DeleteMethodRoute/{id}")]
        public async Task<FuncResult> DeleteMethodRoute(string id)
        {
            return await sysRouteBLL.DeleteMethodRoute(id, HttpContext.CurrentUser(cache).Id);
        }
    }
}
