using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysHelpTypeBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysHelpInfo.RequestModel;
using JiaHang.Projects.Admin.Model.SysHelpType.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysHelpType
{
    [Route("api/[controller]")]

    public class SysHelpTypeDataController : ControllerBase
    {
        private readonly SysHelpTypeBLL helpTypeService;
        private readonly IMemoryCache cache;
        public SysHelpTypeDataController(DataContext dataContext, IMemoryCache cache)
        {
            helpTypeService = new SysHelpTypeBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysHelpTypeModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return helpTypeService.Select(model);

        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await helpTypeService.Select(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysHelpTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await helpTypeService.Add(model, HttpContext.CurrentUser(cache).Id);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]SysHelpTypeModel model)
        {
            FuncResult data = await helpTypeService.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await helpTypeService.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await helpTypeService.Delete(ids, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("Export")]
        //public async Task<IActionResult> Export()
        //{
        //    var result = await HelpTypeService.GetUserListBytes();
        //    return File(result, "application/ms-excel", $"系统帮助类型.xlsx");

        //}

    }
}