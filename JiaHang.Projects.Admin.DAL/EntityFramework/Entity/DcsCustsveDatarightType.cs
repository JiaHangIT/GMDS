using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsCustsveDatarightType
    {
        public string CustomerId { get; set; }
        public string ServiceId { get; set; }
        public string DataRightId { get; set; }
        public int? DataLevel { get; set; }
        public string DataRightColumn1 { get; set; }
        public string DataRightColumn2 { get; set; }
        public string DataRightColumn3 { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
