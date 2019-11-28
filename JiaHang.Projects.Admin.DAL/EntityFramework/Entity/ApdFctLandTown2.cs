using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class ApdFctLandTown2
    {
        public string OrgCode { get; set; }
        public decimal? FactLand { get; set; }
        public decimal? RentLand { get; set; }
        public decimal? LeaseLand { get; set; }
        public string Remark { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal RecordId { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
    }
}
