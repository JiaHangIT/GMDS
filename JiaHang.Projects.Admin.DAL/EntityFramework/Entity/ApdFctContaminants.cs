using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class ApdFctContaminants
    {
        public string OrgCode { get; set; }
        public string IsInSystem { get; set; }
        public decimal? Oxygen { get; set; }
        public decimal? AmmoniaNitrogen { get; set; }
        public decimal? SulfurDioxide { get; set; }
        public decimal? NitrogenOxide { get; set; }
        public decimal? Coal { get; set; }
        public decimal? FuelOil { get; set; }
        public decimal? Hydrogen { get; set; }
        public decimal? Firewood { get; set; }
        public string Remark { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
        public decimal PeriodYear { get; set; }
        public string RecordId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
