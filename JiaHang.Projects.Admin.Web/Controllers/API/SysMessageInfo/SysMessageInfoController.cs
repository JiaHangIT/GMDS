using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysMessageInfoBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysMessageInfo;
using JiaHang.Projects.Admin.Model.SysMessageInfo.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysMessageInfo
{
    [Route("api/[controller]")]
    //[ApiController]
    public class SysMessageInfoController : ControllerBase
    {
        private readonly SysMessageInfoBLL storeService;
        private readonly IMemoryCache cache;
        public SysMessageInfoController(DataContext dataContext, IMemoryCache cache)
        {
            this.cache = cache;
            storeService = new SysMessageInfoBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysMessageInfo model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return storeService.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{code}")]
        public async Task<FuncResult> Select(string MessageId)
        {
            return await storeService.Details(MessageId);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public async Task<FuncResult> Add([FromBody]SysMessageInfoModel model, string currentUserId)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await storeService.Add(model, HttpContext.CurrentUser(cache).UserName);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{MessageId}")]
        public async Task<FuncResult> Update(string MessageId, [FromBody]SysMessageInfoModel model)
        {
            FuncResult data = await storeService.Update(MessageId, model, HttpContext.CurrentUser(cache).UserName);
            return data;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{MessageId}")]
        public async Task<FuncResult> Delete([FromRoute]string MessageId)
        {
            return await storeService.Delete(MessageId, HttpContext.CurrentUser(cache).UserName);
        }
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] MessageIds)
        {
            return await storeService.Delete(MessageIds, HttpContext.CurrentUser(cache).Id);

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
        public async Task<FuncResult> UpdateExamine([FromBody]SysMessageInfoModel model)
        {
            FuncResult data = await storeService.UpdateExamine(model, HttpContext.CurrentUser(cache).UserName);
            return data;
        }

        [Route("GetUser")]
        [HttpGet]
        public object GetUser()
        {
            var data = storeService.GetUser();
            return data;
        }
    }
}