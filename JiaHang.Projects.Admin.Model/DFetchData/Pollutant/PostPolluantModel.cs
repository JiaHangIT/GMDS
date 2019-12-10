using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DFetchData.Pollutant
{
    public class PostPolluantModel
    {
        public string IsInSystem { get; set; }
        public decimal? Oxygen { get; set; }
        public decimal? AmmoniaNitrogen { get; set; }
        public decimal? SulfurDioxide { get; set; }
        public decimal? NitrogenOxide { get; set; }
        public decimal? Coal { get; set; }
        public decimal? FuelOil { get; set; }
        public decimal? Hydrogen { get; set; }
        public decimal? Firewood { get; set; }
        public string Remark { get; set; }
    }
}
