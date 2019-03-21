using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsServiceCollectResults
    {
        public string ServiceId { get; set; }
        public string ReFieldName { get; set; }
        public string ToFieldId { get; set; }
        public int DimTransFlag { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
    }
}
