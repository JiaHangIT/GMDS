using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDataSource.RequestModel
{
public    class SysDataSourceFieldModel
    {
        public string FieldId { get; set; }
        public string DatasourceId { get; set; }
        public string FieldCode { get; set; }
        public string FieldName { get; set; }
        public int FieldTypeId { get; set; }
        public int FieldLength { get; set; }
        public int FieldNullable { get; set; }
        public int FieldKeyFlag { get; set; }
        public int FieldIndexFlag { get; set; }
        public string FieldValue { get; set; }
        public int DimFlag { get; set; }
        public int TimestampFlag { get; set; }
        public string DimTableName { get; set; }
        public string DimFieldCode { get; set; }
        public string DimFieldName { get; set; }
        public string OraSequenceCode { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
