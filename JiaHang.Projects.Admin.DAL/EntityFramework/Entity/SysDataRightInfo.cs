using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysDataRightInfo
    {
        public string UserId { get; set; }
        public string UserGroupId { get; set; }
        public string ModelId { get; set; }
        public string DatarightTypeId { get; set; }
        public string RightValue1 { get; set; }
        public string RightValue2 { get; set; }
        public string RightValue3 { get; set; }
        public int? UseChildrenLevel { get; set; }
        public int? ValueRelativePath { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
    }
}
