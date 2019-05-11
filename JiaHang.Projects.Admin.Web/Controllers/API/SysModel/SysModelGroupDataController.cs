using JiaHang.Projects.Admin.BLL.SysModelBLL;
using JiaHang.Projects.Admin.BLL.SysModelGroupBLL;
using JiaHang.Projects.Admin.BLL.SysModuleBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysModule.RequestModel;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysModule
{
    [Route("api/[controller]")]
    public class SysModelGroupDataController : ControllerBase
    {

        private readonly SysModelGroupBLL   sysModelGroupBLL;
        private readonly IMemoryCache cache;
        public SysModelGroupDataController(DataContext dataContext,IMemoryCache cache)
        {
            this.cache = cache;
            sysModelGroupBLL = new SysModelGroupBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize,int currentPage, string modelName, string parentModelName)
        {
            currentPage--;
            return sysModelGroupBLL.Select(pageSize,currentPage,modelName,parentModelName);

        }

       
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysModelGroup model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await sysModelGroupBLL.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<FuncResult> Update([FromBody]SysModelGroup model)
        {
            FuncResult data = await sysModelGroupBLL.Update(model, HttpContext.CurrentUser(cache).Id);
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
            return await sysModelGroupBLL.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [HttpGet("{id}")]
        public FuncResult Select(string id)
        {
           
            return sysModelGroupBLL.GetParentModule(id);

        }

        
    }
}

