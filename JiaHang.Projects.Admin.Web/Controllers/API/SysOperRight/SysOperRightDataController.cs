using JiaHang.Projects.Admin.BLL.DcsCustomerBLL;
using JiaHang.Projects.Admin.BLL.SysOperRightBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsCustomerServiceParams.RequestModel;
using JiaHang.Projects.Admin.Model.SysOperRightParamsModel.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{

    [Route("api/[controller]")]
    public class SysOperRightDataController : ControllerBase
    {

        private readonly SysOperRightBLL sysOperRightBLL;
        private readonly IMemoryCache cache;
        public SysOperRightDataController(DAL.EntityFramework.DataContext dataContext, IMemoryCache cache)
        {
            sysOperRightBLL = new SysOperRightBLL(dataContext);
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        [HttpGet("{modelGroupId}")]
        public FuncResult Select(string modelGroupId)
        {

          
            return sysOperRightBLL.Select(modelGroupId);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        [HttpGet]
        [Route("NotBind/{modelId}")]
        public FuncResult NotBind(string modelId)
        {
            return sysOperRightBLL.NotBind(modelId);

        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysOperRightParamsModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await sysOperRightBLL.Add(HttpContext.CurrentUser(cache).Id, model);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<FuncResult> Delete([FromBody] SysOperRightDeleteParamsModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await sysOperRightBLL.Delete(HttpContext.CurrentUser(cache).Id, model);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        [HttpGet]
        [Route("CurrentUserRoutes")]
        public object CurrentUserRoutes()
        {
            return sysOperRightBLL.CurrentUserRoutes(HttpContext.CurrentUser(cache).Id);
        }
        
    }
}
