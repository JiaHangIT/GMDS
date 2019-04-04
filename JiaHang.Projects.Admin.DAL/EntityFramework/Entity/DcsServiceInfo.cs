using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsServiceInfo
    {
        public string ServiceId { get; set; }
        public string ServiceGroupId { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDesc { get; set; }
        public string ServiceVersion { get; set; }
        public string ServiceTech { get; set; }
        public string ServiceType { get; set; }
        public string ServiceReturn { get; set; }
        public string ServiceStatus { get; set; }
        public int? DataPageFlag { get; set; }
        public int? DataMultiFlag { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
        public string DatasourceId { get; set; }
    }
}
