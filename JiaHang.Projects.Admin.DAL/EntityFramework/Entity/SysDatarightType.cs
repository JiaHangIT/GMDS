using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysDatarightType
    {
        public string DatarightTypeId { get; set; }
        public string DatarightTypeCode { get; set; }
        public string DatarightTypeName { get; set; }
        public string DatasourceCode { get; set; }
        public int? HaveDataLevel { get; set; }
        public string DataLevelColumn { get; set; }
        public string RootLevelValue { get; set; }
        public string RightValueColumnId1 { get; set; }
        public string RightValueColumnCode1 { get; set; }
        public string RightValueColumnName1 { get; set; }
        public string RightValueColumnId2 { get; set; }
        public string RightValueColumnCode2 { get; set; }
        public string RightValueColumnName2 { get; set; }
        public string RightValueColumnId3 { get; set; }
        public string RightValueColumnCode3 { get; set; }
        public string RightValueColumnName3 { get; set; }
        public int? EnableFlag { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int? DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
