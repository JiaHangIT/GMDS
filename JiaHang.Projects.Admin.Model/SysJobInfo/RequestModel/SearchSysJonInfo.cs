using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysJobInfo.RequestModel
{
    public class SearchSysJonInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Job_Code { get; set; }
        public string Job_Name { get; set; }
        public string Job_Type { get; set; }
        
    }
}
