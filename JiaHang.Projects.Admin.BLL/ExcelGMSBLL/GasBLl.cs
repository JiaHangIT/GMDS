﻿using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.ApdFtcGas;
using JiaHang.Projects.Admin.Model.ExcelSearchMode.Gas;
using Microsoft.EntityFrameworkCore;
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
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
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
                                PeriodYear = o.PeriodYear,
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
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
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
                                PeriodYear=o.PeriodYear,
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
        public FuncResult WriteData(IEnumerable<ApdFctGas> list, string year)
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
                    context.ApdFctGas.Add(item);
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
                var pollu = context.ApdFctGas.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (pollu != null || pollu.Count() > 0)
                {
                    context.ApdFctGas.RemoveRange(pollu);
                    context.SaveChanges();
                }
                return pollu != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>       
        public async Task<FuncResult> Update(int id, ApdFtcGasModel model)
        {
            ApdFctGas entity;
            try
            {
                entity = await context.ApdFctGas.FirstOrDefaultAsync(m => m.RecordId == model.RecordId);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
                }
                entity.OrgCode = model.OrgCode;
                entity.Gas= model.Gas;
                entity.Other = model.Other;                
                entity.Remark = model.Remark;
                
                //entity.LAST_UPDATED_BY = HttpContext.CurrentUser(cache).Id;
                entity.LastUpdateDate = DateTime.Now;

                context.ApdFctGas.Update(entity);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex);
                return new FuncResult() { IsSuccess = false, Message = "修改时发生了意料之外的错误" };
            }
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };

        }
        public async Task<FuncResult> Delete(string id/*, string currentUserId*/)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new FuncResult() { IsSuccess = false, Message = "未接收到参数信息!" };
            }
            ApdFctGas entity = await context.ApdFctGas.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.DeleteFlag = 1;
            //entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            context.ApdFctGas.Update(entity);
            await context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
    }
    public class ReturnPollutantModel
    {
        public decimal RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public decimal PeriodYear { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public decimal? Gas { get; set; }        
        public decimal? Other { get; set; }      
        public string Remark { get; set; }
    }
    //public class ApdFctGasModels
    //{
    //    public string OrgCode { get; set; }
    //    public decimal? Gas { get; set; }
    //    public decimal? Other { get; set; }
    //    public string Remark { get; set; }
    //    public DateTime? CreationDate { get; set; }
    //    public decimal? CreatedBy { get; set; }
    //    public DateTime? LastUpdateDate { get; set; }
    //    public decimal? LastUpdatedBy { get; set; }
    //    public decimal PeriodYear { get; set; }
    //    public decimal RecordId { get; set; }
    //}
}
