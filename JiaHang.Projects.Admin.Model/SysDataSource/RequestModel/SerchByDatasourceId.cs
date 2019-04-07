using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDataSource.RequestModel
{
   public class SerchByDatasourceId
    {
      public  string dataSourceId { get; set; }
        public int page { get; set; }
        public int limit { get; set; }
    }
}
