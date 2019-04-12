using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace JiaHang.Projects.Admin.Common
{
    /// <summary>
    /// 实体映射帮助类
    /// </summary>
    public static class MappingHelper
    {
        /// <summary>
        /// 实体值
        /// </summary>
        /// <typeparam name="R">目标类型</typeparam>
        /// <typeparam name="T">传入参数</typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        public static R Mapping<R, T>(R target,T source)
        {
            try
            {
                R result = target;
                foreach (PropertyInfo info in typeof(R).GetProperties())
                {
                    PropertyInfo pro = typeof(T).GetProperty(info.Name);
                    if (pro != null)
                        info.SetValue(result, pro.GetValue(source));
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
    }
}
