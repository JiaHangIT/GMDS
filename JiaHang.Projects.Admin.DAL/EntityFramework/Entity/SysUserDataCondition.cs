using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysUserDataCondition
    {
        public string Id { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string UserId { get; set; }
        public string UserGroupId { get; set; }
        public string ConditionName { get; set; }
        public string ConditionId { get; set; }
        public string ControllerId { get; set; }
        public string ConditionValue { get; set; }
        public int SortValue { get; set; }
    }
}
