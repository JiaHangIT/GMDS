using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
   public class ApdDimRatio
    {
            public int? PeriodYear { get; set; }
           public decimal? TaxPerMu { get; set; }
           public decimal? AddValuePerMu { get; set; }
            public decimal? Procuctivity { get; set; }
            public decimal? PollutantDischarge { get; set; }
            public decimal? EnergyConsumption { get; set; }
            public decimal? NetAssesProfit { get; set; }
            public decimal? RDExpenditureRatio { get; set; }
            public int DeleteFlag { get; set; }

    }
}
