using JiaHang.Projects.Admin.BLL.SysModelBLL;
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
    public class SysModelDataController : ControllerBase
    {

        private readonly SysModelBLL  sysModelBLL;
        private readonly IMemoryCache cache;
        public SysModelDataController(DataContext dataContext,IMemoryCache cache)
        {
            this.cache = cache;
            sysModelBLL = new SysModelBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize,int currentPage,string modelName,string parentModelName)
        {
            currentPage--;
            return sysModelBLL.Select(pageSize,currentPage, modelName, parentModelName);

        }

       
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysModelInfo model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await sysModelBLL.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<FuncResult> Update([FromBody]SysModelInfo model)
        {
            FuncResult data = await sysModelBLL.Update(model, HttpContext.CurrentUser(cache).Id);
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
            return await sysModelBLL.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        /// <summary>
        /// 获取二级模块组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetParentModule")]
        [HttpGet]
        public FuncResult GetParentModule()
        {
            return sysModelBLL.GetParentModule();

        }

    }
}

