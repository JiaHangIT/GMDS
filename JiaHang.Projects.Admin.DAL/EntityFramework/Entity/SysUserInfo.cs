using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysUserInfo
    {
        public string UserId { get; set; }
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserOrgId { get; set; }
        public string UserGroupNames { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public string UserTel { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
