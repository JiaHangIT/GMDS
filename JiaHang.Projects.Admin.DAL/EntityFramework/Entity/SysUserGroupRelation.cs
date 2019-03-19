using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysUserGroupRelation
    {
        public string SysUserGroupRelationId { get; set; }
        public string UserGroupId { get; set; }
        public string UserId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
