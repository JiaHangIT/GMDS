using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsCustomerLogInfo
    {
        public string LogId { get; set; }
        public string CustomerId { get; set; }
        public DateTime? LogDate { get; set; }
        public string LogType { get; set; }
        public string LogInfo { get; set; }
    }
}
