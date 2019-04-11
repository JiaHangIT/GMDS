using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysModelGroup
    {
        public string ModelGroupId { get; set; }
        public string ModelGroupCode { get; set; }
        public string ModelGroupName { get; set; }
        public string ParentId { get; set; }
        public string ParentIdTree { get; set; }
        public decimal? SortKey { get; set; }
        public decimal? EnableFlag { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
