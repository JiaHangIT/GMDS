using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysProblemInfo.RequestModel
{
    public class SysProblemInfoModel
    {



        /// <summary>
        /// 常见问题类型ID
        /// </summary>
        [StringLength(40)]
        public string ProblemTypeId { get; set; }
        /// <summary>
        /// 常见问题标题
        /// </summary>
        [StringLength(200)]
        public string ProblemTitle { get; set; }
        /// <summary>
        /// 常见问题内容
        /// </summary>
        public string ProblemContent { get; set; }
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
