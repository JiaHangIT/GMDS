using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class ApdDimOrg
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
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
        public string Industry { get; set; }
        public string RegistrationStatus { get; set; }
        public decimal? RegistrationMoney { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal? RecordId { get; set; }
    }
}
