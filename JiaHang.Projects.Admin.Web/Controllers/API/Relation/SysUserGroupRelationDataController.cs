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
    public class SysUserGroupRelationDataController : ControllerBase
    {
        private readonly SysUserGroupRelationBLL sysUserGroupRelationBLL;
        private readonly IMemoryCache cache;
        public SysUserGroupRelationDataController(DataContext datacontext,IMemoryCache cache)
        {
            this.cache = cache;
            sysUserGroupRelationBLL = new SysUserGroupRelationBLL(datacontext);
        }

        [HttpGet]
        public FuncResult Get()
        {
            return sysUserGroupRelationBLL.Select();
        }
        [HttpGet]
        [Route("NotBindUser")]
        public FuncResult NotBindUser(string groupId)
        {
            return sysUserGroupRelationBLL.NotBindUser(groupId);
        }

        [HttpPost]
        public async Task<FuncResult> Post([FromBody]List<SysUserGroupRelationModel> model)
        {
            return await sysUserGroupRelationBLL.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">RelationId 绑定记录id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete(string id)
        {
            return await sysUserGroupRelationBLL.Delete(id, HttpContext.CurrentUser(cache).Id);
        }

    }
}
