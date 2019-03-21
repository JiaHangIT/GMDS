using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsServiceGroup.RequestModel
{
    public class SearchDesServiceGroup
    {
        public int limit { get; set; }

        public int page { get; set; }

        public string Servoce_Group_Name { get; set; }
    }
}
