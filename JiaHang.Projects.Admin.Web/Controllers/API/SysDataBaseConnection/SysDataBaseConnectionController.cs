using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using JiaHang.Projects.Admin.BLL.SysConnectionBLL;
using JiaHang.Projects.Admin.Model.SysConnection.RequestModel;
using JiaHang.Projects.Admin.Model.SysDataBaseConnection.RequestModel;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysConnection
{
    [Route("api/[controller]")]
    public class SysDataBaseConnectionController : ControllerBase
    {
        
        private readonly SysDataBaseConnectionBLL connectionServers;
        private readonly IMemoryCache cache;
        public SysDataBaseConnectionController(DataContext dataContext, IMemoryCache cache)
        {
            connectionServers = new SysDataBaseConnectionBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysConnection model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return connectionServers.Select(model);

        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult ElementSelect(int pageSize, int currentPage, string connectionName, string databaseType)
        {
            currentPage--;
            return connectionServers.ElementSelect(pageSize, currentPage, connectionName, databaseType);
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await connectionServers.Select(id);
        }
        /// <summary>
        ///查询数据库连接类型
        /// </summary>
        /// <returns></returns>
        [Route("SelectDatabaseType")]
        [HttpGet]
        public async Task<FuncResult> SelectDatabaseType()
        {
            return await connectionServers.SelectDatabaseType();

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysConnectionModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await connectionServers.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]SearchContentConnection model)
        {
            FuncResult data = await connectionServers.Update(id, model, HttpContext.CurrentUser(cache).Id);
            return data;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]string id)
        {
            return await connectionServers.Delete(id, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await connectionServers.Delete(ids, HttpContext.CurrentUser(cache).Id);

        }
    }
}