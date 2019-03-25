using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsServiceParams.RequestModel
{
    public class DcsServiceParamsModel
    {
        /// <summary>
        /// 接口主键
        /// </summary>
        [StringLength(40)]
        public string ServiceId { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        [StringLength(40)]
        public string ParamCode { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        [StringLength(60)]
        public string ParamName { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        [StringLength(60)]
        public int? ParamTypeId { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string ParamDesc { get; set; }

        /// <summary>
        /// 可以为空
        /// 是：1；否：0
        /// </summary>
        public int? ParamNullable { get; set; }

        /// <summary>
        /// 是否时间戳参数
        /// 是：1；否：0
        /// </summary>
        public int? TimestampFlag { get; set; }

        /// <summary>
        /// 关联字段主键
        /// SYS_DATASOURCE_FIELD.FIELD_ID
        /// </summary>
        [StringLength(40)]
        public string RelaFieldId { get; set; }
    }
}
