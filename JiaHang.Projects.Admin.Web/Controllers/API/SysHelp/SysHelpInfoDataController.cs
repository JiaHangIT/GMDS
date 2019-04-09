using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysHelpInfoBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysHelpInfo.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysHelp
{
    [Route("api/[controller]")]
    public class SysHelpInfoDataController : ControllerBase
    {
        private readonly SysHelpInfoBLL HelpInfoService;
        private readonly IMemoryCache cache;
        public SysHelpInfoDataController(DataContext dataContext, IMemoryCache cache)
        {
            HelpInfoService = new SysHelpInfoBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysHelpInfo model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return HelpInfoService.Select(model);

        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await HelpInfoService.Select(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("add")]
        public async Task<FuncResult> Add([FromBody] SysHelpInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await HelpInfoService.Add(model, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]SysHelpInfoModel model)
        {
            FuncResult data = await HelpInfoService.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await HelpInfoService.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await HelpInfoService.Delete(ids, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [Route("UpdateExamine")]
        [HttpPost]
        public async Task<FuncResult> UpdateExamine([FromBody]SysHelpInfoModel model)
        {
            
            FuncResult data = await HelpInfoService.UpdateExamine(model, HttpContext.CurrentUser(cache).UserName);
            return data;
        }
        //[HttpGet]
        //[Route("Export")]
        //public async Task<IActionResult> Export()
        //{
        //    var result = await HelpInfoService.GetUserListBytes();
        //    return File(result, "application/ms-excel", $"系统帮助信息.xlsx");

        //}
    }
}