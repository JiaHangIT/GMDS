using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysRoute
{
    public class DataConditionPropertyModel
    {
        public DataConditionPropertyModel()
        {
            children = new List<DataConditionPropertyModel>();
        }
        public string Id { get; set; }

        public string DataConditionId { get; set; }

        public string PrototypeName { get; set; }

        public string PrototypeValue { get; set; }

        public string ParentPrototypeValue { get; set; }

        public int SortValue { get; set; }

        /// <summary>
        /// 树形节点名
        /// </summary>
        public string label
        {
            get
            {
                return PrototypeName;
            }
        }

        public List<DataConditionPropertyModel> children { get; set; }

        public string PropertyValue { get; set; }

        public string PropertyDesc { get; set; }
    }
}
