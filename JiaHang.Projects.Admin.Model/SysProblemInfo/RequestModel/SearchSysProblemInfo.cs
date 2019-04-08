using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysProblemInfo.RequestModel
{
    public class SearchSysProblemInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Problem_Type_Id { get; set; }
        public string Problem_Title { get; set; }
        public int? Audit_Flag { get; set; }

    }
}
