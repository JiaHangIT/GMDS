using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysAreaRoute
    {
        public string SysAreaRouteId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteBy { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public string AreaPath { get; set; }
        public string AreaAlias { get; set; }
    }
}
