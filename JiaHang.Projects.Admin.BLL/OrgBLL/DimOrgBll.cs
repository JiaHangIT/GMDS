using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Org;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.OrgBLL
{
    public class DimOrgBll
    {
        private readonly DataContext context;

        public DimOrgBll(DataContext _context)
        {
            this.context = _context;
        }

        public FuncResult GetList(SearchOrgModel model = null)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from o in context.ApdDimOrg select o;

                query = query.Where(f => (string.IsNullOrEmpty(model.orgname) || f.OrgName.Equals(model.orgname)) &&
                                        (string.IsNullOrEmpty(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
                                        (string.IsNullOrEmpty(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year))));
                int count = query.Count();
                if (model.limit * model.page >= count)
                {
                    model.page = 0;
                }
                fr.Content = query.ToList();
                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult GetListPagination(SearchOrgModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from o in context.ApdDimOrg select o;

                query = query.Where(f => (string.IsNullOrEmpty(model.orgname) || f.OrgName.Contains(model.orgname)) &&
                                         (string.IsNullOrEmpty(model.orgcode) || f.OrgCode.Contains(model.orgcode)) &&
                                         (string.IsNullOrEmpty(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year))));
                int count = query.Count();
                if (model.limit * model.page >= count)
                {
                    model.page = 0;
                }
                var data = query.Skip(model.limit * model.page).Take(model.limit);

                fr.Content = new { data = data, total = count };
                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult Add(PostOrgModel mode,string userid)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                ApdDimOrg org = new ApdDimOrg()
                {
                    RecordId = Guid.NewGuid().ToString(),
                    CreationDate = DateTime.Now,
                    //CreatedBy = Convert.ToDecimal(userid),
                    LastUpdateDate = DateTime.Now,
                    //LastUpdatedBy = Convert.ToDecimal(userid)
                };
                org = MappingHelper.Mapping(org, mode);

                context.ApdDimOrg.Add(org);
                context.SaveChanges();
                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult Update(string recordid,PostOrgModel model,string userid)
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

                ApdDimOrg existorg = context.ApdDimOrg.FirstOrDefault(f => f.RecordId.Equals(recordid));
                if (existorg == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到企业信息，请确定是否已删除!";
                    return fr;
                }
                existorg.LastUpdateDate = DateTime.Now;
                //existorg.LastUpdatedBy = Convert.ToDecimal(userid);
                existorg = MappingHelper.Mapping(existorg, model);
                context.ApdDimOrg.Update(existorg);
                context.SaveChanges();

                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult Deletes(string[] ids)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功!" };
            try
            {
                using (IDbContextTransaction trans = context.Database.BeginTransaction())
                {
                    IQueryable<ApdDimOrg> list = context.ApdDimOrg.Where(f => ids.Select(g => g).Contains(f.RecordId));
                    context.ApdDimOrg.RemoveRange(list);

                    try
                    {
                        context.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        fr.IsSuccess = false;
                        fr.Message = $"{ex.InnerException}{ex.Message}";
                        return fr;
                    }
                }
                return fr;
            }
            catch (Exception ex)
            {
                fr.IsSuccess = false;
                fr.Message = $"{ex.InnerException}{ex.Message}";
                return fr;
            }
        }

        public FuncResult Delete(string recordid,string userid)
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

                ApdDimOrg existorg = context.ApdDimOrg.FirstOrDefault(f => f.RecordId.Equals(Convert.ToDecimal(recordid)));
                if (existorg == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到企业信息，请确定是否已删除!";
                    return fr;
                }

                //existorg.LastUpdateDate = DateTime.Now;
                ////existorg.LastUpdatedBy = Convert.ToDecimal(userid);
                //existorg.DeleteFlag = 1;
                //existorg.DeleteDate = DateTime.Now;
                //existorg.DeleteBy = userid;
                context.ApdDimOrg.Remove(existorg);
                context.SaveChanges();

                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult WriteData(IEnumerable<ApdDimOrg> list, string year, string userid)
        {
            /*
            * 同一年，一个企业只能导入一次
            * 更新，导入时，以年份为维度删除数据
            * **/
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {

                var _year = Convert.ToDecimal(year);
                //var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                //if (dm != null && dm.Count() > 0)
                //{
                //    fr.IsSuccess = false;
                //    fr.Message = "未找到配置的企业信息!";
                //    return fr;
                //}
                var existdata = context.ApdDimOrg.Where(f => f.PeriodYear.Equals(Convert.ToDecimal(year)));
                context.ApdDimOrg.RemoveRange(existdata);
                foreach (var item in list)
                {
                    //if (isAlreadyExport(item.OrgCode, year))
                    //{
                    //    //continue;
                    //}
                    item.CreationDate = DateTime.Now;
                    item.CreatedBy = Convert.ToDecimal(userid);
                    context.ApdDimOrg.Add(item);
                }
                //context.ApdFctWorker.AddRange(list);
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
                var org = context.ApdDimOrg.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (org != null || org.Count() > 0)
                {
                    context.ApdDimOrg.RemoveRange(org);
                    context.SaveChanges();
                }
                return org != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }
}
