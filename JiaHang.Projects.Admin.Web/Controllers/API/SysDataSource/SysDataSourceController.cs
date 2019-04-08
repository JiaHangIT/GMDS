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
        /// 关联查询
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
        /// <summary>
        /// 查询主表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SelectDataSourceInfo")]
        [HttpPost]
        public FuncResult SelectDataSourceInfo([FromBody]SearchDatasource model) {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return dataSourceServers.SelectDataSourceInfo(model);
        }
        /// <summary>
        /// 查询数据库连接名称
        /// </summary>
        /// <returns></returns>
        [Route("SelectConnection")]
        [HttpGet]
        public async Task<FuncResult> SelectConnection()
        {
            return await dataSourceServers.SelectConnection();

        }
        /// <summary>
        /// 添加(数据源信息)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> AddDataSourceInfo([FromBody] SysDataSourceModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await dataSourceServers.AddDataSourceInfo(model, HttpContext.CurrentUser(cache).Id);
        }
       
        /// <summary>
        /// 修改(数据源信息)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> UpdateDataSourceInfo(string id, [FromBody]SysDataSourceModel model)
        {
            FuncResult data = await dataSourceServers.UpdateDataSourceInfo(id, model, HttpContext.CurrentUser(cache).Id);
            return data;

        }
       
        /// <summary>
        /// 删除(单个数据源信息)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> DeleteDatasourceInfo([FromRoute]string id)
        {
            return await dataSourceServers.DeleteDataSourceInfo(id, HttpContext.CurrentUser(cache).Id);

        }
       
        /// <summary>
        /// 删除(多个数据源信息)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("DeleteDataSourceInfos")]
        [HttpDelete]
        public async Task<FuncResult> DeleteDataSourceInfos(string[] ids) {
            return await dataSourceServers.DeleteDataSourceInfos(ids, HttpContext.CurrentUser(cache).Id);
        }
       
    }
}
