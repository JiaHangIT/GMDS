using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysMessageInfo.RequestModel
{
    public class SysMessageInfoModel
    {
        public string Message_ID{ get; set; }
        public string Message_title { get; set; }
        public string Message_Content { get; set; }
        public int? Important_Flag { get; set; }
        public int? Audit_Flag { get; set; }
        public DateTime? Audited_Date { get; set; }
        public string Audited_By { get; set; }
        public DateTime? Creation_Date { get; set; }
        public string Creation_By { get; set; }
        public DateTime? Last_Updata_Date { get; set; }
        public string Last_Updata_By { get; set; }
    }
}
