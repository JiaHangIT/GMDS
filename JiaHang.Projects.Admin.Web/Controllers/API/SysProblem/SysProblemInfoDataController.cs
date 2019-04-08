using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysProblemInfoBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysProblemInfo.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysProblem
{
    [Route("api/[controller]")]
    public class SysProblemInfoDataController : ControllerBase
    {
        private readonly SysProblemInfoBLL ProblemInfoService;
        private readonly IMemoryCache cache;
        public SysProblemInfoDataController(DataContext dataContext, IMemoryCache cache)
        {
            ProblemInfoService = new SysProblemInfoBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysProblemInfo model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return ProblemInfoService.Select(model);

        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await ProblemInfoService.Select(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[Route("add")]
        public async Task<FuncResult> Add([FromBody] SysProblemInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await ProblemInfoService.Add(model, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]SysProblemInfoModel model)
        {
            FuncResult data = await ProblemInfoService.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await ProblemInfoService.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await ProblemInfoService.Delete(ids, HttpContext.CurrentUser(cache).Id);

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
        public async Task<FuncResult> UpdateExamine([FromBody]SysProblemInfoModel model)
        {
            string ProblemId = model.ProblemId;
            FuncResult data = await ProblemInfoService.UpdateExamine(ProblemId, HttpContext.CurrentUser(cache).UserName);
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