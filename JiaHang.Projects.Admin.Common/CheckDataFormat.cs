using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Common
{
    public class CheckDataFormat : ICheckDataFormat
    {
        /// <summary>
        /// 检查T里面每个属性值是否规范
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public string Check<T>(T t) where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
