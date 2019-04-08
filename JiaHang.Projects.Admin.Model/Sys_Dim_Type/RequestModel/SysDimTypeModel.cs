using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.Sys_Dim_Type.RequestModel
{
    public class SysDimTypeModel
    {
        public string Dim_Type_Code { get; set; }
        public string Dim_Type_Name { get; set; }
        public DateTime? Creation_Date { get; set; }
        public string Created_By { get; set; }
        public DateTime? Last_Updata_Date { get; set; }
        public string Last_Updata_By { get; set; }
    }
}
