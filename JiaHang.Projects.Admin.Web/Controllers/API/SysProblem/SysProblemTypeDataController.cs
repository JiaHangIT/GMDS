using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.SysProblemTypeBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysProblemType.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysProblemType
{
    [Route("api/[controller]")]

    public class SysProblemTypeDataController : ControllerBase
    {
        private readonly SysProblemTypeBLL problemTypeService;
        private readonly IMemoryCache cache;
        public SysProblemTypeDataController(DataContext dataContext, IMemoryCache cache)
        {
            problemTypeService = new SysProblemTypeBLL(dataContext);
            this.cache = cache;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysProblemTypeModel model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return problemTypeService.Select(model);

        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult ElemeSelect(int pageSize, int currentPage, string problemTypeName)
        {
            currentPage--;
            return problemTypeService.ElemeSelect(pageSize, currentPage, problemTypeName);
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await problemTypeService.Select(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysProblemTypeModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await problemTypeService.Add(model, HttpContext.CurrentUser(cache).Id);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]SysProblemTypeModel model)
        {
            FuncResult data = await problemTypeService.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await problemTypeService.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await problemTypeService.Delete(ids, HttpContext.CurrentUser(cache).Id);

        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Route("Export")]
        //public async Task<IActionResult> Export()
        //{
        //    var result = await ProblemTypeService.GetUserListBytes();
        //    return File(result, "application/ms-excel", $"系统帮助类型.xlsx");

        //}

    }
}