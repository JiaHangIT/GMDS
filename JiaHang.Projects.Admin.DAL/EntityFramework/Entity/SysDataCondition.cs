using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysDataCondition
    {
        public string SysDataConditionId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string ConditionName { get; set; }
        public int SortValue { get; set; }
        public string ParentId { get; set; }
        public string ConditionValue { get; set; }
        public string ConditionDesc { get; set; }
        public string TableName { get; set; }
        public string ParentColumn { get; set; }
        public string ConditionValueDesc { get; set; }
        public string ChildColumn { get; set; }
        public int MasterSlaveFlag { get; set; }
    }
}
