using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysRoute
{
    public class DataConditionModel
    {
        public DataConditionModel()
        {
            children = new List<DataConditionModel>();
        }
        public string Id { get; set; }

        /// <summary>
        /// 条件名称
        /// </summary>
        public string ConditionName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortValue { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 树形节点名
        /// </summary>
        public string label
        {
            get
            {
                return ConditionName;
            }
        }

        public List<DataConditionModel> children { get; set; }
        public string ConditionValue { get; set; }
        public string ConditionDesc { get; set; }


        /// <summary>
        /// 表名
        /// 数据来源于哪张表 维度表
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///数据值 父级列名
        /// </summary>
        public string ParentColumn { get; set; }
        public string ConditionValueDesc { get; set; }

        /// <summary>
        /// 数据值 子级列名
        /// </summary>
        public string ChildColumn { get; set; }
        /// <summary>
        /// 是否从属
        /// </summary>
        public bool MasterSlaveFlag { get; set; }
    }
}
