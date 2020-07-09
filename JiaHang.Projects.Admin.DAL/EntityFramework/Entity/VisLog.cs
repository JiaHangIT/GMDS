using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public class VisLog
    {
        public string Id { get; set; }
        public string Method { get; set; }
        public string UserId { get; set; }
        public DateTime VisTime { get; set; }
        public string RequestUrl { get; set; }
        public string RequestMethod { get; set; }
        public string RequestBody { get; set; }
        public string Params { get; set; }
        public string Result { get; set; }
        public decimal TakeUpTime { get; set; }
        public int DeleteFlag { get; set; }
    }
}
