using System;
using System.Linq;
using JiaHang.Projects.Admin.BLL.Relation;
using JiaHang.Projects.Admin.BLL.SysOperRightBLL;
using JiaHang.Projects.Admin.BLL.SysUserInfoervice;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Account.Request;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.Account
{
    /// <summary>
    ///用于用户 登录 / 注销  注册 / 重置密码 
    /// </summary>
    [Route("api/[controller]")]
    public class AccountDataController : ControllerBase
    {
        private readonly CredentialsManage credentialsManage;
        private readonly SysUserInfoBLL sysUserInfoService;
        private readonly CurrentUserRouteBLL currentUserRouteBLL;
        private readonly SysOperRightBLL sysOperRightBLL;
        public AccountDataController(DAL.EntityFramework.DataContext context, IMemoryCache cache)
        {
            credentialsManage = new CredentialsManage(cache);
            sysUserInfoService = new SysUserInfoBLL(context);
            currentUserRouteBLL = new CurrentUserRouteBLL(context);
            sysOperRightBLL = new SysOperRightBLL(context);
        }
        //GetSysUserInfoFirst


        [Route("login")]
        [HttpPost]
        public FuncResult Login([FromBody]AccountLogin model)
        {
            if (model == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "请输入正确用户名及密码" };
            }
            FuncResult<SysUserInfo> result = sysUserInfoService.Login(model.AccountName, model.Password);

            if (!result.IsSuccess)
            {
                return new FuncResult() { IsSuccess = result.IsSuccess, Message = result.Message };
            }

            //写入到缓存中
            AccountModel account = new AccountModel
            {
                Id = result.Content.UserId,
                UserName = result.Content.UserName,
                UserAccount = result.Content.UserAccount,
                Email = result.Content.UserEmail,
                IsLock = result.Content.UserIsLock,
                MobileNo = result.Content.UserMobile,
                UserIsLdap = result.Content.UserIsLdap
            };
            //获取用户资源
            //var routes_data = currentUserRouteBLL.GetRoutes(account.Id,result.Content.UserAccount=="admin");
            var routes_data = sysOperRightBLL.CurrentUserRoutes(account.Id, result.Content.UserAccount == "admin");
            //if (routes_data.ViewRoutes.Count <= 0 )
            //{
            //    return new FuncResult() { IsSuccess = false, Message = "当前用户暂无任何页面权限" };
            //}

            string token = credentialsManage.SetAccount(account, model.Remember);
            // 加当前用户所拥有的route 写入缓存
            credentialsManage.SetAccountRoute(routes_data.Content, token);

            var defaultUrl = "/";
            //try
            //{
            //    defaultUrl = routes_data.ViewRoutes.First().Controllers.First().Methods.First().CompletePath;
            //}
            //catch {
            //    defaultUrl = "/404";
            //}
               
           
            //defaultUrl = "/" + defaultUrl;
            HttpContext.Response.Cookies.Append("token", token, new CookieOptions() { Expires = DateTime.Now.AddDays(10) });//将token储存到本地
            return new FuncResult() { IsSuccess = result.IsSuccess, Content =new { account, viewRoutes=routes_data.Content, defaultUrl }, Message = result.Message };
        }

        [HttpDelete]
        public void SingOut()
        {
            string token = HttpContext.Request.Cookies["token"];
            HttpContext.Response.Cookies.Delete("token");
            credentialsManage.DeleteAccount(token);
        }
    }
}
