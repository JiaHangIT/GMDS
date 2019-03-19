using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsServiceParams
    {
        public string ParamId { get; set; }
        public string ServiceId { get; set; }
        public string ParamCode { get; set; }
        public string ParamName { get; set; }
        public int? ParamTypeId { get; set; }
        public string ParamDesc { get; set; }
        public int? ParamNullable { get; set; }
        public int? TimestampFlag { get; set; }
        public string RelaFieldId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
