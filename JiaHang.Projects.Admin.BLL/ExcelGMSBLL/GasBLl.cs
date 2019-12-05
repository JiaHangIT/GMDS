using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.ExcelSearchMode.Gas;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.ExcelGMSBLL
{
    public class GasBLl
    {
        private readonly DataContext context;

        public GasBLl(DataContext _context) { this.context = _context; }

        public async Task<FuncResult> GetListPagination(SearchExcelModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from c in context.ApdFctGas
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new
                            {
                                RecordId = c.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                Gas = c.Gas,
                                Other = c.Other,
                                Remark = c.Remark
                            };

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
        /// 列表
        /// </summary>
        /// <returns></returns>
        public FuncResult GetList()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from c in context.ApdFctGas
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new ReturnPollutantModel()
                            {
                                RecordId = c.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,                             
                                Gas = c.Gas,
                                Other = c.Other,
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
        public bool WriteData(IEnumerable<ApdFctGas> list, string year)
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
                context.ApdFctGas.AddRange(list);
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
                throw new Exception("error", ex);
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
        public decimal? Gas { get; set; }        
        public decimal? Other { get; set; }      
        public string Remark { get; set; }
    }
}
