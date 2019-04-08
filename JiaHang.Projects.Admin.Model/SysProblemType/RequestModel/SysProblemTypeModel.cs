using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysProblemType.RequestModel
{
    public class SysProblemTypeModel
    {
        /// <summary>
        /// 问题类型ID
        /// </summary>
        [StringLength(40)]
        public string ProblemTypeId { get; set; }
        /// <summary>
        /// 问题类型名称
        /// </summary>
        [StringLength(60)]
        public string ProblemTypeName { get; set; }
    }
}
