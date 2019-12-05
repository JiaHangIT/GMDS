using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DFetchData.Rd;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.DFetchDataBLL
{
    public class RdBLL
    {
        private readonly DataContext context;

        public RdBLL(DataContext _context) { this.context = _context; }

        public FuncResult GetList()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from r in context.ApdFctRD
                            join o in context.ApdDimOrg on r.OrgCode equals o.OrgCode
                            select new ReturnRDModel()
                            {
                                RecordId = r.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                IsHighTech = r.IsHighTech,
                                RDExpenditure = r.RDExpenditure,
                                Remark = r.Remark
                            };

                fr.Content = query.ToList();
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }

        public FuncResult GetListPagination(SearchRdModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from r in context.ApdFctRD
                            join o in context.ApdDimOrg on r.OrgCode equals o.OrgCode
                            select new
                            {
                                PeriodYear = r.PeriodYear,
                                RecordId = r.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                IsHighTech = r.IsHighTech,
                                RDExpenditure = r.RDExpenditure,
                                Remark = r.Remark
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

                throw new Exception("error", ex);
            }
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool WriteData(IEnumerable<ApdFctRD> list,string year)
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
                context.ApdFctRD.AddRange(list);
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

        public FuncResult Delete()
        {
            try
            {
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class ReturnRDModel
    {
        public decimal RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string IsHighTech { get; set; }
        public decimal? RDExpenditure { get; set; }
        public string Remark { get; set; }
    }
}
