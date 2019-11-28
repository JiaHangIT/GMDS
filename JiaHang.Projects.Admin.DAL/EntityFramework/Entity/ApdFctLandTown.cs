using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class ApdFctLandTown
    {
        public string OrgCode { get; set; }
        public decimal? OwnershipLand { get; set; }
        public decimal? ProtectionLand { get; set; }
        public decimal? ReduceLand { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal RecordId { get; set; }
    }
}
