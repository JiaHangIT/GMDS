using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDataBaseConnection.RequestModel
{
  public  class SearchContentConnection
    {
        public string ConnectionId { get; set; }
        public string ConnectionName { get; set; }
        public int? DatabaseTypeId { get; set; }
        public string ConnectionString { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
        public string Database_Type_Code { get; set; }
        public string Database_Type_Name { get; set; }
        public int DATABASE_TYPE_ID { get; set; }
    }
}
