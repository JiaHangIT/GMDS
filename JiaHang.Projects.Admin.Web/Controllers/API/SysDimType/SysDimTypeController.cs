using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysDimTypeBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Sys_Dim_Type.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysDimType
{
    [Route("api/[controller]")]
    //[ApiController]
    public class SysDimTypeController : ControllerBase
    {
        private readonly Sys_Dim_TypeBLL storeService;
        private readonly IMemoryCache cache;
        public SysDimTypeController(DataContext dataContext, IMemoryCache cache)
        {
            this.cache = cache;
            storeService = new Sys_Dim_TypeBLL(dataContext);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysDimType model)
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
        public async Task<FuncResult> Select(string DimTypeCode)
        {
            return await storeService.Details(DimTypeCode);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public async Task<FuncResult> Add([FromBody]SysDimTypeModel model, string currentUserId)
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
        [HttpPut("{DimTypeCode}")]
        public async Task<FuncResult> Update(string DimTypeCode, [FromBody]SysDimTypeModel model)
        {
            FuncResult data = await storeService.Update(DimTypeCode, model, HttpContext.CurrentUser(cache).UserName);
            return data;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{DimTypeCode}")]
        public async Task<FuncResult> Delete([FromRoute]string DimTypeCode)
        {
            return await storeService.Delete(DimTypeCode, HttpContext.CurrentUser(cache).UserName);
        }
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] DimTypeCodes)
        {
            return await storeService.Delete(DimTypeCodes, HttpContext.CurrentUser(cache).Id);

        }
    }
}