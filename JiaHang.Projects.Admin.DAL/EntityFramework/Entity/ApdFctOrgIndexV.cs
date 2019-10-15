using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
   public partial class ApdFctOrgIndexV
    {
        public string RecordId { get; set; }
        public int? PerIodYear { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string Industry { get; set; }
       public int? CompositeScore { get; set; }
        public int? TaxPerMu { get; set; }
        public int? AddValuePerMu { get; set; }
        public int? Productivity { get; set; }
        public int DeleteFlag { get; set; }
    }
}
