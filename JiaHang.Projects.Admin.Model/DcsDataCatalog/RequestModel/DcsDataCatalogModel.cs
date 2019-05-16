using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsDataCatalog.RequestModel
{
   public class DcsDataCatalogModel
    {
        public string DataCatalogId { get; set; }
        public string DataCatalogCode { get; set; }
        public string DataCatalogName { get; set; }
        public string ParentId { get; set; }
        public string ParentIdTree { get; set; }
        public int? DataCountSelf { get; set; }
        public int? DataCountTree { get; set; }
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
