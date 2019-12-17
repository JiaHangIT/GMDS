using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.ExcelTaxBLL
{
    public class TaxBll
    {
        private readonly DataContext context;

        public TaxBll(DataContext _context) { this.context = _context; }
        /// <summary>
        /// 写入到数据表
        /// ?
        /// </summary>
        /// <returns></returns>
        public FuncResult WriteData(IEnumerable<ApdFctTAx> list, string year, string userid)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.ORG_CODE));
                if (dm != null && dm.Count() > 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到配置的企业信息";
                    return fr;
                }
                var orgcodegroupby = list.GroupBy(g => new { g.ORG_CODE, g.PERIOD_YEAR }).Select(s => new { OrgCode = s.Key.ORG_CODE, PeriodYear = s.Key.PERIOD_YEAR, Count = s.Count() });
                foreach (var item in orgcodegroupby)
                {
                    if (item.Count > 1)
                    {
                        fr.IsSuccess = false;
                        fr.Message = $"统一信用代码为：{item.OrgCode}的企业在{item.PeriodYear}年上传了{item.Count}条数据!";
                        return fr;
                    }
                }
                foreach (var item in list)
                {
                    //if (isAlreadyExport(item.ORG_CODE, year))
                    //{
                    //    //continue;
                    //}
                    item.CREATION_DATE = DateTime.Now;
                    item.CREATED_BY = Convert.ToDecimal(userid);
                    context.ApdFctTAx.Add(item);
                }
               
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                //context.ApdFctGas.AddRange(list);
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
                var pollu = context.ApdFctTAx.Where(f => f.ORG_CODE.Equals(orgcode) && f.PERIOD_YEAR.Equals(formatyear));
                if (pollu != null || pollu.Count() > 0)
                {
                    context.ApdFctTAx.RemoveRange(pollu);
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
}
