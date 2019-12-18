using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.ExcelImportTax
{
    public class ExcelTaxImportBLL
    {
        private readonly DataContext _context;
        public ExcelTaxImportBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// elementUI列表查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <param name="title"></param>
        /// <param name="creationby"></param>
        /// <param name="auditflag"></param>
        /// <returns></returns>
        public FuncResult Select(int pageSize, int pageNum, string OrgName)
        {

            try
            {
              
                
                var query = from t1 in _context.ApdFctTAx
                            join o in
                            _context.ApdDimOrg on t1.ORG_CODE equals o.OrgCode
                            where (
                                  (string.IsNullOrWhiteSpace(OrgName) || o.OrgName.Contains(OrgName))
                                 )
                            select new
                            {
                                //Array = listnew,                          
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                LegalRepresentative = o.LegalRepresentative,
                                Phone = o.Phone,
                                LinkMan = o.LinkMan,
                                Phone2 = o.Phone2,
                                DEPRECIATION = t1.DEPRECIATION,
                                PROFIT = t1.PROFIT,
                                ENT_PAID_TAX = t1.ENT_PAID_TAX,
                                MAIN_BUSINESS_INCOME = t1.MAIN_BUSINESS_INCOME,
                                RAD_EXPENSES = t1.RAD_EXPENSES,
                                NUMBER_OF_EMPLOYEES = t1.NUMBER_OF_EMPLOYEES,
                                OWNER_EQUITY = t1.OWNER_EQUITY,
                                TOTAL_PROFIT = t1.TOTAL_PROFIT,

                            };
                
                //var query = _context.ApdFctTAx.
                //        Where(a =>
                //        (
                //         (string.IsNullOrWhiteSpace(ORG_CODE) || a.ORG_CODE.Contains(ORG_CODE))
                //        //&& (string.IsNullOrWhiteSpace(BUSINESS_TYPE_STATUS.ToString()) || a.BUSINESS_TYPE_STATUS == (BUSINESS_TYPE_STATUS))
                //        //&& (a.DELETE_FLAG != 1)
                //        )
                //        ).ToList().OrderByDescending(e => e.LAST_UPDATE_DATE);
                int total = query.Count();
                var data = query.Skip(pageSize * pageNum).Take(pageSize).ToList();
                
                return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Content = null, Message = ex.Message };
            }
        }

        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<ApdFctTAx> entitys = _context.ApdFctTAx.Where(e => ids.Contains(e.RECORD_ID));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (ApdFctTAx obj in entitys)
            {
                //删除
                _context.ApdFctTAx.Remove(obj);
            }
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LogService.WriteError(ex);
                    return new FuncResult() { IsSuccess = false, Message = "删除时发生了意料之外的错误" };
                }
            }
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{ids.Length}条记录" };

        }


    }
}
