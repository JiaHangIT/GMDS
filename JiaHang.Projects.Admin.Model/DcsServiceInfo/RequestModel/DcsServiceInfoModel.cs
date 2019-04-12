using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsServiceInfo.RequestModel
{
    public class DcsServiceInfoModel
    {
        /// <summary>
        /// 接口ID
        /// </summary>
        [StringLength(40)]
        public string ServiceId { get; set; }

        /// <summary>
        /// 接口类型ID
        /// </summary>
        [StringLength(40)]
        public string ServiceGroupId { get; set; }

        /// <summary>
        /// 接口编号
        /// </summary>
        [StringLength(30)]
        public string ServiceNo { get; set; }

        /// <summary>
        /// 接口代码
        /// </summary>
        [StringLength(60)]
        public string ServiceCode { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [StringLength(100)]
        public string ServiceName { get; set; }

        /// <summary>
        /// 接口说明
        /// </summary>
        [StringLength(30)]
        public string ServiceDesc { get; set; }

        /// <summary>
        /// 接口版本
        /// </summary>
        [StringLength(100)]
        public string ServiceVersion { get; set; }

        /// <summary>
        /// 接口技术类型
        /// WebService：WEBSERVICE；API：API
        /// </summary>
        [StringLength(30)]
        public string ServiceTech { get; set; }

        /// <summary>
        /// 接口业务类型
        /// 共享：SHARE；采集：COLLECT
        /// </summary>
        [StringLength(30)]
        public string ServiceType { get; set; }

        /// <summary>
        /// 接口返回格式
        /// XML：XML；JSON：JSON
        /// </summary>
        [StringLength(30)]
        public string ServiceReturn { get; set; }

        /// <summary>
        /// 接口状态
        /// 正常：Y；停用：N；
        /// </summary>
        [StringLength(30)]
        public string ServiceStatus { get; set; }

        /// <summary>
        /// 是否分页
        /// 是：1；否：0；
        /// </summary>
        public int? DataPageFlag { get; set; }

        /// <summary>
        /// 是否多条结果
        /// 是：1；否：0；
        /// </summary>
        public int? DataMultiFlag { get; set; }

        /// <summary>
        /// 数据源字段信息
        /// </summary>
        public string DatasourceId { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int? SortKey { get; set; }

        public List<DcsParamsModel> lsparam { get; set; }

        public List<DcsShareModel> lsshare { get; set; }

        public List<DcsCollectModel> lscollect { get; set; }
    }

    /// <summary>
    /// 参数接收模型
    /// </summary>
    public class DcsParamsModel
    {
        public string ParamId { get; set; }
        public string ParamCode { get; set; }
        public string ParamName { get; set; }
        public decimal? ParamTypeId { get; set; }
        public string ParamDesc { get; set; }
        public decimal? ParamNullable { get; set; }
        public decimal? TimestampFlag { get; set; }
        public string RelaFieldId { get; set; }
    }

    /// <summary>
    /// 共享返回接收模型
    /// </summary>
    public class DcsShareModel
    {
        public string FieldId { get; set; }
    }

    /// <summary>
    /// 采集返回接收模型
    /// </summary>
    public class DcsCollectModel
    {
        public string ReFieldName { get; set; }
        public string ToFieldId { get; set; }
        public decimal DimTransFlag { get; set; }
    }

    /// <summary>
    /// 接口审核接收参数实体
    /// </summary>
    public class ServiceAudit
    {
        /// <summary>
        /// 接口主键
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// 审核通过标致
        /// </summary>
        public int AuditFlag { get; set; }
    }
}
