using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public class ApdFctGas
    {
        public string OrgCode { get; set; }
        public decimal? Gas { get; set; }
        public decimal? Other { get; set; }
        public string Remark { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal RecordId { get; set; }
    }
}
