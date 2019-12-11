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

        public FuncResult GetListPagination(SearchModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
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
                if (model.limit * model.page < model.limit)
                {
                    model.page = 0;
                }
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
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
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

        public FuncResult Update(string recordid, PostPolluantModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                if (string.IsNullOrWhiteSpace(recordid))
                {
                    fr.IsSuccess = false;
                    fr.Message = "参数接收异常!";
                    return fr;
                }
                ApdFctContaminants polluant = context.ApdFctContaminants.FirstOrDefault(f => f.RecordId.Equals(Convert.ToDecimal(recordid)));

                polluant.IsInSystem = model.IsInSystem;
                polluant.Oxygen = model.Oxygen;
                polluant.AmmoniaNitrogen = model.AmmoniaNitrogen;
                polluant.SulfurDioxide = model.SulfurDioxide;
                polluant.NitrogenOxide = model.NitrogenOxide;
                polluant.Coal = model.Coal;
                polluant.FuelOil = model.FuelOil;
                polluant.Hydrogen = model.Hydrogen;
                polluant.Firewood = model.Firewood;
                polluant.Remark = model.Remark;
                context.ApdFctContaminants.Update(polluant);
                context.SaveChanges();
                return fr;
            }
            catch (Exception ex)
            {
                fr.IsSuccess = false;
                fr.Message = $"{ex.InnerException},{ex.Message}";
                throw new Exception("error", ex);
            }
        }

        /// <summary>
        /// 写入到数据表
        /// ?
        /// </summary>
        /// <returns></returns>
        public FuncResult WriteData(IEnumerable<ApdFctContaminants> list,string year)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                if (dm != null && dm.Count() > 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到配置的企业信息";
                    return fr;
                }
                foreach (var item in list)
                {
                    if (isAlreadyExport(item.OrgCode,year))
                    {
                        //continue;
                    }
                    context.ApdFctContaminants.Add(item);
                }
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                //context.ApdFctContaminants.AddRange(list);
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
                        trans.Rollback();
                        fr.IsSuccess = false;
                        fr.Message = $"{ex.InnerException},{ex.Message}!";
                        return fr;
                        throw new Exception("error", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                fr.IsSuccess = false;
                fr.Message = $"{ex.InnerException},{ex.Message}!";
                return fr;
                throw new Exception("error",ex);
            }
            return fr;
        }

        /// <summary>
        /// 处理某机构某年是否已导入数据
        /// </summary>
        /// <param name="orgcode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool isAlreadyExport(string orgcode, string year)
        {
            try
            {
                var formatyear = Convert.ToDecimal(year);
                var pollu = context.ApdFctContaminants.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (pollu != null || pollu.Count() > 0)
                {
                    context.ApdFctContaminants.RemoveRange(pollu);
                    context.SaveChanges();
                }
                return pollu != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
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
