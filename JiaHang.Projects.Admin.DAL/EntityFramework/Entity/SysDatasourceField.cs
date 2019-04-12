using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysDatasourceField
    {
        public string FieldId { get; set; }
        public string DatasourceId { get; set; }
        public string FieldCode { get; set; }
        public string FieldName { get; set; }
        public decimal? FieldTypeId { get; set; }
        public decimal? FieldLength { get; set; }
        public decimal? FieldNullable { get; set; }
        public decimal? FieldKeyFlag { get; set; }
        public decimal? FieldIndexFlag { get; set; }
        public string FieldValue { get; set; }
        public decimal? DimFlag { get; set; }
        public decimal? TimestampFlag { get; set; }
        public string DimTableName { get; set; }
        public string DimFieldCode { get; set; }
        public string DimFieldName { get; set; }
        public string OraSequenceCode { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
