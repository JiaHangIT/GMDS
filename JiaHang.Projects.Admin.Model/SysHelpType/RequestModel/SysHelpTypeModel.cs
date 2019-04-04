using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysHelpType.RequestModel
{
    public class SysHelpTypeModel
    {
        
        /// <summary>
        /// 帮助类型ID
        /// </summary>
        [StringLength(40)]
        public string HelpTypeId { get; set; }
        /// <summary>
        /// 帮助类型名称
        /// </summary>
        [StringLength(60)]
        public string HelpTypeName { get; set; }
    }
}
