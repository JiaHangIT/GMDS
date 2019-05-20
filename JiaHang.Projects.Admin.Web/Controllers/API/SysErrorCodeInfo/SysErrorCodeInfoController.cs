using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysErrorCodeInfoBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysErrorCodeInfo.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysErrorCodeInfo
{
    [Route("api/[controller]")]
    public class SysErrorCodeInfoController : ControllerBase
    {
        private readonly SysErrorCodeInfoBLL errorCode;
        private readonly IMemoryCache cache;
        public SysErrorCodeInfoController(DataContext dataContext, IMemoryCache cache)
        {
            this.cache = cache;
            errorCode = new SysErrorCodeInfoBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysErrorCodeInfo model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return errorCode.Select(model);

        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search1")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysErrorCodesInfo model)
        {
            model.PageNum--; if (model.PageNum < 0)
            {
                model.PageNum = 0;
            }

            return errorCode.Select(model);

        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{code}")]
        public async Task<FuncResult> Select(string MessageId)
        {
            return await errorCode.Details(MessageId);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public async Task<FuncResult> Add([FromBody]SysErrorCodeInfoModel model, string currentUserId)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await errorCode.Add(model, HttpContext.CurrentUser(cache).UserName);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="code"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{ErrorCodeId}")]
        public async Task<FuncResult> Update(string ErrorCodeId, [FromBody]SysErrorCodeInfoModel model)
        {
            FuncResult data = await errorCode.Update(ErrorCodeId, model, HttpContext.CurrentUser(cache).UserName);
            return data;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{ErrorCodeId}")]
        public async Task<FuncResult> Delete([FromRoute]string ErrorCodeId)
        {
            return await errorCode.Delete(ErrorCodeId, HttpContext.CurrentUser(cache).UserName);
        }
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ErrorCodeId)
        {
            return await errorCode.Delete(ErrorCodeId, HttpContext.CurrentUser(cache).Id);

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
        public async Task<FuncResult> UpdateExamine([FromBody]SysErrorCodeInfoModel model)
        {

            FuncResult data = await errorCode.UpdateExamine(model, HttpContext.CurrentUser(cache).UserName);
            return data;
        }
    }
}