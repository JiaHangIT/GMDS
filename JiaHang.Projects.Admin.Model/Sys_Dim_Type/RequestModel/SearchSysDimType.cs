using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.Sys_Dim_Type.RequestModel
{
    public class SearchSysDimType
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Dim_Type_Name { get; set; }
        public string Creation_By { get; set; }  
    }
}
