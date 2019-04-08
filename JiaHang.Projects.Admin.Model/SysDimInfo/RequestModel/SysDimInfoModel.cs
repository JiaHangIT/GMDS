using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDimInfo.RequestModel
{
    public class SysDimInfoModel
    {
        public string Dim_Id{ get; set; }
        public string Dim_Type_Code { get; set; }
        public string Dime_Name { get; set; }
        public string Dim_Value { get; set; }
        public DateTime? Creation_Date { get; set; }
        public string Created_By { get; set; }
        public DateTime? Last_Updata_Date { get; set; }
        public string Last_Updata_By { get; set; }
    }
}
