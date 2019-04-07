using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDataSource.RequestModel
{
  public  class SearchDatasource
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string DataSource_Code { get; set; }
        public string DataSource_Name { get; set; }
        public string DataSource_Type { get; set; }
        public string DataSource_Use { get; set; }
    }
}
