using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class DcsServiceGroup
    {
        public string ServiceGroupId { get; set; }
        public string ServiceGroupCode { get; set; }
        public string ServiceGroupName { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
        public int? SortKey { get; set; }
    }
}
