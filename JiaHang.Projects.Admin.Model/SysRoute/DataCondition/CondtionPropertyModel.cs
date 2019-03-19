using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysRoute
{
     public class CondtionPropertyModel
    {
        public List<DataConditionModel> ConditionList { get; set; }
        public List<DataConditionPropertyModel> PropertyList { get; set; }

        public string ModuleID { get; set; }
    }
}
