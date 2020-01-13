using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace JiaHang.Projects.Admin.Web.Controllers.API
{
    [Route("api/[controller]")]
    public class ApdDimRatioController:ControllerBase
    {
        private readonly ApdDimRatioBLL apdDimRatioBLL;
        public ApdDimRatioController(DataContext context) {
            apdDimRatioBLL = new ApdDimRatioBLL(context);
        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize, int currentPage, string OrgName, int year)
        {
            currentPage--;
            return apdDimRatioBLL.Select(pageSize, currentPage, year);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody]Params model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await apdDimRatioBLL.Add(model);
        }
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(int id, [FromBody]Param model)
        {
            FuncResult data = await apdDimRatioBLL.Update(id, model);
            return data;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]int id)
        {
            return await apdDimRatioBLL.Delete(id);
        }
        [Route("Deletes")]
        [HttpDelete]
        public async Task<FuncResult> Deletes(string[] years)
        {
            return await apdDimRatioBLL.Deletes(years);

        }
    }
}
