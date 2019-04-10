using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysConnection.RequestModel
{
  public  class SearchSysConnection
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DataBaseTypeId { get; set; }
    }
}
