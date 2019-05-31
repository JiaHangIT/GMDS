﻿using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysUser
{
    [Route("api/[controller]")]
    public class SysUserGroupDataController:ControllerBase
    {
        private readonly SysUserGroupBLL userGroupService;
        private readonly IMemoryCache cache;
        public SysUserGroupDataController(DataContext dataContext,IMemoryCache cache)
        {
            this.cache = cache;
            userGroupService = new SysUserGroupBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysUserGroupModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return userGroupService.Select(model);

        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult ElementSelect(int pageSize, int currentPage, int level, string name)
        {
            currentPage--;
            return userGroupService.ElementSelect(pageSize, currentPage, level,name);
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await userGroupService.Select(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] UserGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await userGroupService.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]UserGroupModel model)
        {
            FuncResult data = await userGroupService.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await userGroupService.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

       

        [Route("AcquisitionModule")]
        [HttpGet]
        public async Task<object> AcquisitionModule()
        {
            return await userGroupService.AcquisitionModule();
        }
    }
}
