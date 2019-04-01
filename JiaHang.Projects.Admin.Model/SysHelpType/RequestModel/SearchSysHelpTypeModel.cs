using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysHelpType.RequestModel
{
   public class SearchSysHelpTypeModel
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string help_Type_Name { get; set; }
    }
}
