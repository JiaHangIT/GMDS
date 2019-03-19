using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysRoute
{
    public class BuildTreeData
    {
        /// <summary>
        /// 构建条件值树形结构数据（条件值）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<DataConditionPropertyModel> TreeStructData(List<DataConditionPropertyModel> data)
        {
            List<DataConditionPropertyModel> list = new List<DataConditionPropertyModel>();
            list = data.Where(e => e.ParentPrototypeValue == "0").OrderBy(o => o.SortValue).ToList();
            foreach (var item in list)
            {
                getChildNode(data, item);
            }
            return list;
        }
        public static void getChildNode(List<DataConditionPropertyModel> data, DataConditionPropertyModel childNode)
        {
            foreach (var item in data)
            {
                if (item.ParentPrototypeValue == childNode.PrototypeValue)
                {
                    childNode.children.Add(item);
                    getChildNode(data, item);
                }
            }
        }
        /// <summary>
        /// 构建条件树形结构数据（条件）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<DataConditionModel> TreeStructData(List<DataConditionModel> data)
        {
            List<DataConditionModel> list = new List<DataConditionModel>();
            list = data.Where(e => e.ParentId == "0").OrderBy(e => e.SortValue).ToList();
            foreach (var item in list)
            {
                getChildNode(data, item);
            }
            return list;
        }
        public static void getChildNode(List<DataConditionModel> data, DataConditionModel current)
        {
            foreach (var item in data)
            {
                if (current.Id == item.ParentId)
                {
                    current.children.Add(item);
                    getChildNode(data, item);
                }
            }
        }
    }
}
