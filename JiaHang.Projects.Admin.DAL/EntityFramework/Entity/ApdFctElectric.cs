﻿using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class ApdFctElectric
    {
        public string OrgCode { get; set; }
        public decimal? NetSupply { get; set; }
        public decimal? Spontaneous { get; set; }
        public string Remark { get; set; }
        public DateTime? CreationDate { get; set; }
        public decimal? CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? LastUpdatedBy { get; set; }
        public decimal PeriodYear { get; set; }
        public string RecordId { get; set; }
        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
