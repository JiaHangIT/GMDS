﻿using JiaHang.Projects.Admin.BLL.DcsCustomerBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsCustomer
{
  
    [Route("api/[controller]")]
    public class DcsCustomerDataController : ControllerBase
    {

        private readonly DcsCustomerBLL  dcsCustomerBLL;
        private readonly IMemoryCache cache;
        public DcsCustomerDataController(DataContext dataContext, IMemoryCache cache)
        {
            dcsCustomerBLL = new DcsCustomerBLL(dataContext);
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize, int currentPage,string customerName,string customerMobile)
        {
            currentPage--;

            return dcsCustomerBLL.Select(pageSize,currentPage, customerName, customerMobile);

        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DcsCustomerInfo model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await dcsCustomerBLL.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]DcsCustomerInfo model)
        {
            FuncResult data = await dcsCustomerBLL.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await dcsCustomerBLL.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await dcsCustomerBLL.Delete(ids, HttpContext.CurrentUser(cache).Id);

        }
    }
}