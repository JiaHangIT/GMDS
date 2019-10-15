using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using JiaHang.Projects.Admin.BLL.SysUserInfoervice;

namespace JiaHang.Projects.Admin.Web.Controllers.API.SysUser
{
    [Route("api/[controller]")]
    public class SysUserInfoDataController : ControllerBase
    {

        private readonly SysUserInfoBLL userInfoService;
        private readonly IMemoryCache cache;
        public SysUserInfoDataController(DataContext dataContext,IMemoryCache cache)
        {
            userInfoService = new SysUserInfoBLL(dataContext);
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchSysUserInfo model)
        {
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            return userInfoService.Select(model);

        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult ElementSelect(int pageSize, int currentPage, string userAccount, string userName)
        {
            currentPage--;
            return userInfoService.ElementSelect(pageSize, currentPage, userAccount, userName);
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FuncResult> Select(string id)
        {
            return await userInfoService.Select(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] SysUserInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await userInfoService.Add(model,HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]SysUserInfoModel model)
        {
            FuncResult data = await userInfoService.Update(id, model, HttpContext.CurrentUser(cache).Id);
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
            return await userInfoService.Delete(id, HttpContext.CurrentUser(cache).Id);

        }

        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await userInfoService.Delete(ids, HttpContext.CurrentUser(cache).Id);

        }

        [HttpGet]
        [Route("Export")]
        public async Task<IActionResult> Export() {
            var result =await  userInfoService.GetUserListBytes();
            return File(result, "application/ms-excel", $"系统用户.xlsx");
          
        }
    }
}