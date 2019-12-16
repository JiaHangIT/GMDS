using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Common
{
    public interface ICheckDataFormat
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        string Check<T>(T t) where T : class, new();
    }
}
