using System.Linq;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web
{
    public static class HttpContextExtensions
    {
        public static AccountModel CurrentUser(this HttpContext httpContext, IMemoryCache cache)
        {

            IRequestCookieCollection cookies = httpContext.Request.Cookies;
            string token = cookies["token"];
            CredentialsManage credentialsManage = new CredentialsManage(cache);
            AccountModel account = credentialsManage.GetAccount(token);
#if DEBUG
            if (account == null)
            {
                account = new AccountModel()
                {
                    Id = "6e3ad26e6056472c9e0e415d37cde247",
                    UserName = "admin",
                    UserAccount = "admin",
                    MobileNo = "admin",
                    Email = "admin@admin.com",

                };
            }
#endif
            return account;
        }

        public static string CurrentPathId(this HttpContext httpContext, IMemoryCache cache)
        {
            IRequestCookieCollection cookies = httpContext.Request.Cookies;
            string token = cookies["token"];
            string path = httpContext.Request.Headers["Referer"];
            string cu_controller = path.Split("/")[3].ToUpper();
            CredentialsManage credentialsManage = new CredentialsManage(cache);
            //从缓存中获取当前用户routes
            System.Collections.Generic.List<BLL.Relation.CurrentUserRouteBLL.UserRouteRawModel> routes = credentialsManage.GetAccountRoute(token);
            //从routes匹配得到controllerid
            return routes.FirstOrDefault(e => e.ControllerPath.ToUpper() == cu_controller)?.ControllerId;

        }
    }
}
