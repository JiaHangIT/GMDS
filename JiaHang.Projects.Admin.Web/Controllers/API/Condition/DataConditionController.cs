using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysRoute;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.Condition
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataConditionController : ControllerBase
    {
        private readonly DataConditionBLL storeService;
        private readonly IMemoryCache cache;
        public DataConditionController(DAL.EntityFramework.DataContext dataContext, IMemoryCache cache)
        {
            this.cache = cache;
            storeService = new DataConditionBLL(dataContext);
        }
        [Route("Add")]
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DataConditionModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await storeService.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        [Route("Update")]
        [HttpPut]
        public  FuncResult Update([FromBody]DataConditionModel model)
        {
            FuncResult data =  storeService.Update(model, HttpContext.CurrentUser(cache).Id);
            return data;
        }
        [Route("Delete/{id}")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string id)
        {
            FuncResult data = await storeService.Delete(id, HttpContext.CurrentUser(cache).Id);
            return data;
        }
        [Route("Get")]
        [HttpGet]
        public FuncResult Get()
        {
            FuncResult data = storeService.Select();
            return data;
        }

        [Route("GetNotBindConditionValues/{userid}/{controllerid}")]
        [HttpGet]
        public FuncResult GetNotBindConditionValues(string userid, string controllerid)
        {
            var data = storeService.GetNotBindConditionValues(userid, controllerid);
            return new FuncResult() { IsSuccess = true, Content = data };
        }

        [Route("GetUserGroupNotBindConditionValues/{userGroupId}/{controllerid}")]
        [HttpGet]
        public FuncResult GetUserGroupNotBindConditionValues(string userGroupId, string controllerid)
        {
            var data = storeService.GetUserGroupNotBindConditionValues(userGroupId, controllerid);
            return new FuncResult() { IsSuccess = true, Content = data };
        }

        [Route("GetUserRoutes")]
        [HttpGet]
        public FuncResult GetUserRoutes(string userName, string userGroupName)
        {
            var data = storeService.GetUserRoutes(userName, userGroupName);
            return new FuncResult() { IsSuccess = true, Content = data };
        }


        [Route("Bind")]
        [HttpPost]
        public FuncResult Bind([FromBody]List<SysUserDataCondition> model)
        {
            var data = storeService.Bind(model, HttpContext.CurrentUser(cache).Id);
            return new FuncResult() { IsSuccess = true, Content = data };
        }

        [Route("Unbind/{id}")]
        [HttpDelete]
        public FuncResult Unbind(string id)
        {
            var data = storeService.Unbind(id, HttpContext.CurrentUser(cache).Id);
            return new FuncResult() { IsSuccess = true, Content = data };
        }

        ///// <summary>
        ///// 以树型结构返回当前用户在当前路径下所拥有的数据查询权限
        ///// 用于前端页面构建查询条件的下拉框
        ///// </summary>
        ///// <param name="conditionId"></param>
        ///// <returns></returns>
        //[Route("GetCurrentUserConditionTree")]
        //[HttpGet]
        //public object GetCurrentUserConditionTree(string conditionId)
        //{
        //    var data = storeService.GetCurrentUserConditionTree(HttpContext.CurrentUser(cache).Id, HttpContext.CurrentPathId(cache), conditionId);
        //    return data;
        //}
        //[Route("GetControllerIdTest")]
        //[HttpGet]
        //public string GetControllerIdTest()
        //{
        //    return HttpContext.CurrentPathId(cache);
        //}


        ///// <summary>
        ///// 拿到数据查询权限联动信息
        ///// </summary>
        ///// <returns></returns>
        //[Route("GetDataConditionLinkage")]
        //[HttpGet]
        //public object GetDataConditionLinkage()
        //{
        //    var data = storeService.GetDataConditionLinkage();
        //    return data;
        //}

        ///// <summary>
        ///// 用于验证 当前用户是否存在改code
        ///// </summary>
        ///// <param name="conditionId"></param>
        ///// <returns></returns>
        //[Route("GetCurrentUserConditionList")]
        //[HttpGet]
        //public bool DataConditionMathch(string conditionId,string conditionValue)
        //{
        //    var data = storeService.DataConditionMathch(conditionId,conditionValue, HttpContext.CurrentPathId(), HttpContext.CurrentUser(cache).Id);
        //    return data;
        //}
    }
}