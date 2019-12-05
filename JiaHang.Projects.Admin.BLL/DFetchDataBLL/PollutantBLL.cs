using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DFetchData.Pollutant;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.DFetchDataBLL
{
    public class PollutantBLL
    {
        private readonly DataContext context;

        public PollutantBLL(DataContext _context) { this.context = _context; }

        public async Task<FuncResult> GetListPagination(SearchModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from c in context.ApdFctContaminants
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new
                            {
                                PeriodYear = c.PeriodYear,
                                RecordId = c.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                IsInSystem = c.IsInSystem,
                                Oxygen=c.Oxygen,
                                AmmoniaNitrogen = c.AmmoniaNitrogen,
                                SulfurDioxide = c.SulfurDioxide,
                                NitrogenOxide = c.NitrogenOxide,
                                Coal = c.Coal,
                                FuelOil = c.FuelOil,
                                Hydrogen = c.Hydrogen,
                                Firewood = c.Firewood,
                                Remark = c.Remark
                            };
                query = query.Where(f => (
                                    (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
                                    (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
                                    (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
                                    ));
                int count = query.Count();
                var pagination = query.Skip(model.limit * model.page).Take(model.limit);
                fr.Content = new { total = count, data = pagination };
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
            }
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public FuncResult GetList()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from c in context.ApdFctContaminants
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new ReturnPollutantModel()
                            {
                                RecordId = c.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                IsInSystem = c.IsInSystem,
                                Oxygen = c.Oxygen,
                                AmmoniaNitrogen = c.AmmoniaNitrogen,
                                SulfurDioxide = c.SulfurDioxide,
                                NitrogenOxide = c.NitrogenOxide,
                                Coal = c.Coal,
                                FuelOil = c.FuelOil,
                                Hydrogen = c.Hydrogen,
                                Firewood = c.Firewood,
                                Remark = c.Remark
                            };

                fr.Content = query.ToList();
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }

        /// <summary>
        /// 写入到数据表
        /// ?
        /// </summary>
        /// <returns></returns>
        public bool WriteData(IEnumerable<ApdFctContaminants> list,string year)
        {
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                if (dm != null && dm.Count() > 0)
                {
                    return false;
                }
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                context.ApdFctContaminants.AddRange(list);
                using (IDbContextTransaction trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                        throw new Exception("error", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("error",ex);
            }
            return true;
        }
    }

    public class ReturnPollutantModel
    {
        public decimal RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
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
