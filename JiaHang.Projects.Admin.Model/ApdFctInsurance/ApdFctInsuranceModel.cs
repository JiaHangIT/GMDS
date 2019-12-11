using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.ApdFctInsurance
{
    public class ApdFctInsuranceModel
    {
        public string OrgCode { get; set; }
        public decimal? InsuranceMonth { get; set; }
        public string Remark { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal RecordId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
