using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysErrorCodeInfo.RequestModel
{
    public class SearchSysErrorCodeInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Error_Code_Code { get; set; }
        public string Error_Code_Name { get; set; }
        public string Created_By { get; set; }
        public int? Audit_Flag { get; set; }
    }
}
