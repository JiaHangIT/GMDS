using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsCustomerInfo
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string LoginAccount { get; set; }
        public string LoginPassword { get; set; }
        public string ContactName { get; set; }
        public string ContactTel { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string CustomerStatus { get; set; }
        public string ServerIp { get; set; }
        public int? IpLimitFlag { get; set; }
        public string IpLimitList { get; set; }
        public int? ConcurrentLimit { get; set; }
        public DateTime? EffEndDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
