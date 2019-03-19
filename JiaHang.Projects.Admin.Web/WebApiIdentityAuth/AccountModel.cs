using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model.Enumerations.Sys_User;
using System;
using System.ComponentModel.DataAnnotations;

namespace JiaHang.Projects.Admin.Web.WebApiIdentityAuth
{

    public class AccountModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string UserAccount { get; set; }
        public string Email { get; set; }
        public int? UserIsLdap { get; set; }
        public string MobileNo { get; set; }
        public int? IsLock { get; set; }
    }
}
