using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using JiaHang.Projects.Admin.BLL.SysConnectionBLL;
using JiaHang.Projects.Admin.Model.SysDataSource.RequestModel;
using JiaHang.Projects.Admin.BLL.SysDataSourceBLL;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysDataSourceField
{
    [Route("api/[controller]")]
    public class SysDataSourceFieldController:ControllerBase
    {
        private readonly SysDataSourceBLL dataSourceServers;
        private readonly IMemoryCache cache;
        public SysDataSourceFieldController(DataContext dataContext, IMemoryCache cache)
        {
            dataSourceServers = new SysDataSourceBLL(dataContext);
            this.cache = cache;
        }
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] AddFieldInfoParm model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await dataSourceServers.Add(model, HttpContext.CurrentUser(cache).Id);
        }
        /// <summary>
        /// 修改(数据源字段信息)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> UpdateDataSourceField(string id, [FromBody]SysDataSourceFieldModel model)
        {
            FuncResult data = await dataSourceServers.UpdateDataSourceField(id, model, HttpContext.CurrentUser(cache).Id);
            return data;

        }
        /// <summary>
        /// 删除(单个数据源字段信息)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]string id)
        {
            return await dataSourceServers.DeleteDataSourceField(id, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 删除(多个数据源字段信息)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("DeleteDatasourceFields")]
        [HttpDelete]
        public async Task<FuncResult> DeleteDataSourceFields(string[] ids)
        {
            return await dataSourceServers.DeleteDataSourceFields(ids, HttpContext.CurrentUser(cache).Id);
        }
        /// <summary>
        ///查询数据源字段类型
        /// </summary>
        /// <returns></returns>
        [Route("SelectFieldInfo")]
        [HttpGet]
        public async Task<FuncResult> SelectFieldInfo()
        {
            return await dataSourceServers.SelectFieldInfo();

        }
        
        /// <summary>
        ///查询数据源所有字段
        /// </summary>
        /// <returns></returns>
        [Route("SelectDataSourceFiled")]
        [HttpPost]
        public  FuncResult SelectDataSourceFiled([FromBody]SerchByDatasourceId model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }
            return dataSourceServers.SelectDataSourceFiled(model); ;

        }

    }
}
