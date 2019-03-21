using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsCustsveAccessResult
    {
        public string AccessId { get; set; }
        public string AccessParams { get; set; }
        public string ReturnResult { get; set; }
        public int DeleteFlag { get; set; }
    }
}
