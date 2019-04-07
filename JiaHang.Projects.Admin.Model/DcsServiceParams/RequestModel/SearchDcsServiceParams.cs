using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsServiceParams.RequestModel
{
    public class SearchDcsServiceParams
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int limit { get; set; }
    }
}
