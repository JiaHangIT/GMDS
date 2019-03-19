using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsCustsveAccessInfo
    {
        public string AccessId { get; set; }
        public string CustomerId { get; set; }
        public string ServiceId { get; set; }
        public DateTime? AccessDate { get; set; }
        public string AccessIp { get; set; }
        public int? AccessResultFlag { get; set; }
        public int? AccessExeTime { get; set; }
        public int? ReturnDataNum { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
