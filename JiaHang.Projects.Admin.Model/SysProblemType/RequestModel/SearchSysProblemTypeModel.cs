using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysProblemType.RequestModel
{
    public class SearchSysProblemTypeModel
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string Problem_Type_Name { get; set; }
    }
}
