using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsServiceInfo.RequestModel
{
    public class SearchDcsServiceInfo
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 接口编号
        /// </summary>
        public string ServiceNo { get; set; }

        /// <summary>
        /// 接口代码
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 技术类型
        /// </summary>
        public string ServiceTech { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 接口状态
        /// </summary>
        public string ServiceStatus { get; set; }
    }
}
