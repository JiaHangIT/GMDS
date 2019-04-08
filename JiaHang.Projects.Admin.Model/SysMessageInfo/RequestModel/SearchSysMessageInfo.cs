using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysMessageInfo
{
    public class SearchSysMessageInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Message_title{get;set;}
        public string Created_By { get; set; }
        public int? Audit_Flag { get; set; }
        //public DateTime? Audited_Date { get; set; }
        public int? Delete_Flag { get; set; }
    }
}
