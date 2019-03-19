using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysControllerRoute
    {
        public string SysControllerRouteId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public string AreaId { get; set; }
        public int IsApi { get; set; }
        public string ControllerPath { get; set; }
        public string ControllerAlias { get; set; }
        public int SortValue { get; set; }
    }
}
