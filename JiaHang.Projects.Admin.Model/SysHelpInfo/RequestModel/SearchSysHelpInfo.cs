using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysHelpInfo.RequestModel
{
    public  class SearchSysHelpInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Help_Type_Id { get; set; }
        public string Help_Title { get; set; }
        public int? Audit_Flag { get; set; }
        public int? Important_Flag { get; set; }
    }
}
