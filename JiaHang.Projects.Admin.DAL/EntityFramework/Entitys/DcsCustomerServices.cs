using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entitys
{
    public partial class DcsCustomerServices
    {
        public string CustomerId { get; set; }
        public string ServiceId { get; set; }
        public int DatarightFlag { get; set; }
        public int LimitDay { get; set; }
        public int LimitMonth { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }
        public string Param6 { get; set; }
        public string Param7 { get; set; }
        public string Param8 { get; set; }
        public string Param9 { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
