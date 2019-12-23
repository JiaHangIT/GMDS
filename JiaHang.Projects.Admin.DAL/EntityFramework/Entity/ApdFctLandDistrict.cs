using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public class ApdFctLandDistrict
    {
        public string OrgCode { get; set; }
        public string LandNo { get; set; }
        public decimal? Area { get; set; }
        public string ShareDesc { get; set; }
        public string RightType { get; set; }
        public string Purpose { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
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
