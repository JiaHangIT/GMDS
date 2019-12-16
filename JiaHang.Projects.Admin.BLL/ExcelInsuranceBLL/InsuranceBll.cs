using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.Model.DFetchData.Rd;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using JiaHang.Projects.Admin.Model.InsuranceModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JiaHang.Projects.Admin.BLL.ExcelInsuranceBLL
{
    public class InsuranceBll
    {
        private readonly DataContext context;

        public InsuranceBll(DataContext _context) { this.context = _context; }

        public FuncResult GetList()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from r in context.ApdFctInsurance
                            join o in context.ApdDimOrg on r.OrgCode equals o.OrgCode
                            select new ReturnWaterModel()
                            {
                                RecordId = r.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                InsuranceMonth=r.InsuranceMonth,
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
        public FuncResult GetListPagination(SearchInsModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from r in context.ApdFctInsurance
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
                                InsuranceMonth = r.InsuranceMonth,
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
        public FuncResult WriteData(IEnumerable<ApdFctInsuranceDal> list, string year)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
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
                    if (isAlreadyExport(item.OrgCode, year))
                    {
                        //continue;
                    }
                    context.ApdFctInsurance.Add(item);
                }
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                //context.ApdFctInsurance.AddRange(list);
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
                throw new Exception("error", ex);
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
                var pollu = context.ApdFctInsurance.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (pollu != null || pollu.Count() > 0)
                {
                    context.ApdFctInsurance.RemoveRange(pollu);
                    context.SaveChanges();
                }
                return pollu != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
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
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>       
        public async Task<FuncResult> Update(int id, InsuranceModelS model)
        {
            ApdFctInsuranceDal entity;
            try
            {
                entity = await context.ApdFctInsurance.FirstOrDefaultAsync(m => m.RecordId == model.RecordId);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
                }
                entity.OrgCode = model.OrgCode;
                entity.InsuranceMonth = model.InsuranceMonth;
                entity.Remark = model.Remark;

                //entity.LAST_UPDATED_BY = HttpContext.CurrentUser(cache).Id;
                entity.LastUpdateDate = DateTime.Now;

                context.ApdFctInsurance.Update(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex);
                return new FuncResult() { IsSuccess = false, Message = "修改时发生了意料之外的错误" };
            }
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };

        }

    }
    public class ReturnWaterModel
    {
        public string RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public decimal PeriodYear { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public decimal? InsuranceMonth { get; set; }
        public string Remark { get; set; }
    }
}
