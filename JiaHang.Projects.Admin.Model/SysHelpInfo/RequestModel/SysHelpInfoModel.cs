using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysHelpInfo.RequestModel
{
    public class SysHelpInfoModel
    {



        /// <summary>
        /// 帮助类型ID
        /// </summary>
        [StringLength(40)]
        public string HelpTypeId { get; set; }
        /// <summary>
        /// 公告标题
        /// </summary>
        [StringLength(200)]
        public string HelpTitle { get; set; }
        /// <summary>
        /// 公告内容
        /// </summary>
        public string HelpContent { get; set; }
        /// <summary>
        /// 是否重要
        /// </summary>
        public int ImportantFlag { get; set; }
        /// <summary>
        /// 是否审核
        /// </summary>
        public int AuditFlag { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditedDate { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        [StringLength(40)]
        public string AuditedBy { get; set; }
    }
}
