using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysModelInfo
    {
        public string ModelId { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public string ModelGroupId { get; set; }
        public int? SortKey { get; set; }
        public int? EnableFlag { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
