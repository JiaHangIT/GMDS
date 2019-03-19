using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Web.WebApiIdentityAuth
{
    public class CurrentUser
    {
        // \此种方法不能保证该次请求中 值不会发生变化
        //HttpContext.CurrentUser(cache) 改用此方法调用
        private static volatile AccountModel UserInfo;
        public static AccountModel Get() => UserInfo;        
        public static void Set(AccountModel model) => UserInfo = model;
        public static void Dispod() => UserInfo = null;

        
    }
}
