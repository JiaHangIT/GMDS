﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDataSource.RequestModel
{
 public   class SysDataSourceModel
    {
        public string DatasourceId { get; set; }
        public string DatasourceCode { get; set; }
        public string DatasourceName { get; set; }
        public string DatasourceType { get; set; }
        public string DatasourceUse { get; set; }
        public string ConnectionId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}