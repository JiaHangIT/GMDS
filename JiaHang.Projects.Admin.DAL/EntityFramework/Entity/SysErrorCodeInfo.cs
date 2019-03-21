using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysErrorCodeInfo
    {
        public string ErrorCodeId { get; set; }
        public string ErrorCodeCode { get; set; }
        public string ErrorCodeName { get; set; }
        public string ErrorCodeDesc { get; set; }
        public int? ImportantFlag { get; set; }
        public int? AuditFlag { get; set; }
        public DateTime? AuditedDate { get; set; }
        public string AuditedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
