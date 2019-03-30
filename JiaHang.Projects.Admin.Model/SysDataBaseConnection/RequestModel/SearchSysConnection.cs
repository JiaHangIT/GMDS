using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysConnection.RequestModel
{
  public  class SearchSysConnection
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Connection_Name { get; set; }
        public string Connection_String { get; set; }
        public string Database_Type_Code { get; set; }
        public string Database_Type_Name { get; set; }
        public string deleteflag { get; set; }
    }
}
