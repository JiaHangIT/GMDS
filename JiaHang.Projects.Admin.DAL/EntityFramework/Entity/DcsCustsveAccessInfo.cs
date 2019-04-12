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
        public decimal? AccessResultFlag { get; set; }
        public decimal? AccessExeTime { get; set; }
        public decimal? ReturnDataNum { get; set; }
        public DateTime? CreationDate { get; set; }
        public int DeleteFlag { get; set; }
    }
}
