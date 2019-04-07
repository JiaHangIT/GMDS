using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDimInfo.RequestModel
{
    public class SearchSysDimInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Dim_Name { get; set; }
        public string Created_By { get; set; }
    }
}
