using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.InsuranceModel
{
    public class InsuranceModelS
    {
        public decimal RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public decimal PeriodYear { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public decimal? InsuranceMonth { get; set; }
        public string Remark { get; set; }
    }
}
