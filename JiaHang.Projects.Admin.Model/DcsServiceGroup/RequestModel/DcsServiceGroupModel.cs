using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsServiceGroup.RequestModel
{
    public class DcsServiceGroupModel
    {
        /// <summary>
        /// 接口类型ID
        /// </summary>
        [StringLength(40)]
        public string ServiceGroupId { get; set; }

        /// <summary>
        /// 接口类型代码
        /// </summary>
        [StringLength(30)]
        public string ServiceGroupCode { get; set; }

        /// <summary>
        /// 接口类型名称
        /// </summary>
        [StringLength(60)]
        public string ServiceGroupName { get; set; }

        /// <summary>
        /// 接口类型图片
        /// </summary>
        [StringLength(100)]
        public string ImageUrl { get; set; }
    }
}
