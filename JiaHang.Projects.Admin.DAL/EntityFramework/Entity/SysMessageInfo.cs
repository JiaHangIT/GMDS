using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysMessageInfo
    {
        public string MessageId { get; set; }
        public string MessageTitle { get; set; }
        public string MessageContent { get; set; }
        public int? ImportantFlag { get; set; }
        public int? AuditFlag { get; set; }
        public DateTime? AuditedDate { get; set; }
        public string AuditedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int? DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
