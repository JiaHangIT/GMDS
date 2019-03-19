using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsCustsveDatarightInfo
    {
        public string CustomerId { get; set; }
        public string ServiceId { get; set; }
        public string DatarightTypeId { get; set; }
        public string RightValue1 { get; set; }
        public string RightValue2 { get; set; }
        public string RightValue3 { get; set; }
        public int? UseChildrenLevel { get; set; }
        public int? ValueRelativePath { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
