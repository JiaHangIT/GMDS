using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entitys
{
    public partial class SysHelpInfo
    {
        public string HelpId { get; set; }
        public string HelpTypeId { get; set; }
        public string HelpTitle { get; set; }
        public string HelpContent { get; set; }
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
