using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.Org
{
    public class PostOrgModel
    {
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string LegalRepresentative { get; set; }
        public string Phone { get; set; }
        public string LinkMan { get; set; }
        public string Phone2 { get; set; }
        public string Industry { get; set; }
        public string RegistrationStatus { get; set; }
        public decimal? RegistrationMoney { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public decimal PeriodYear { get; set; }
    }
}
