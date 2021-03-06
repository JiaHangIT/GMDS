﻿using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            /*
            * 同一年，一个企业只能导入一次
            * 更新，导入时，以年份为维度删除数据
            * **/
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.ORG_CODE));
                if (dm != null && dm.Count() > 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = $"未找到配置的企业信息，统一信息代码为{string.Join(',', dm.Select(g => g.ORG_CODE))}！";
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
                var existdata = context.ApdFctTAx.Where(f => f.PERIOD_YEAR.Equals(Convert.ToDecimal(year)));
                context.ApdFctTAx.RemoveRange(existdata);
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

        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<ApdFctTAx> entitys = context.ApdFctTAx.Where(e => ids.Contains(e.RECORD_ID));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (ApdFctTAx obj in entitys)
            {
                //删除
                context.ApdFctTAx.Remove(obj);
            }
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    await context.SaveChangesAsync();
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
