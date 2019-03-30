using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using JiaHang.Projects.Admin.BLL.SysConnectionBLL;
using JiaHang.Projects.Admin.Model.SysDataSource.RequestModel;
using JiaHang.Projects.Admin.BLL.SysDataSourceBLL;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysDataSource
{
    [Route("api/[controller]")]
    public class SysDataSourceController:ControllerBase
    {
        private readonly SysDataSourceBLL dataSourceServers;
        private readonly IMemoryCache cache;
        public SysDataSourceController(DataContext dataContext, IMemoryCache cache)
        {
            dataSourceServers = new SysDataSourceBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchDatasource model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return dataSourceServers.Select(model);

        }
    }
}
