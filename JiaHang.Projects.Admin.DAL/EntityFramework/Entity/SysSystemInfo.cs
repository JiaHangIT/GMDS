using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysSystemInfo
    {
        public int SystemId { get; set; }
        public string SystemCode { get; set; }
        public string SystemName { get; set; }
        public string SystemUrl { get; set; }
        public int DeleteFlag { get; set; }
    }
}
