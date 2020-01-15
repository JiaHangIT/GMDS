using JiaHang.Projects.Admin.DAL.EntityFramework;
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

        public FuncResult GetListPagination(SearchExcelModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from c in context.ApdFctGas
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new
                            {
                                CreationDate = c.CreationDate,
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
                query = query.Where(f => (
                                   (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Contains(model.orgcode)) &&
                                   (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Contains(model.orgname)) &&
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
        /// 列表
        /// </summary>
        /// <returns></returns>
        public FuncResult GetList(SearchExcelModel model)
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
                query = query.Where(f => (
                                (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Contains(model.orgcode)) &&
                                (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Contains(model.orgname)) &&
                                (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
                                ));
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
        public FuncResult WriteData(IEnumerable<ApdFctGas> list, string year,string userid)
        {
            /*
            * 同一年，一个企业只能导入一次
            * 更新，导入时，以年份为维度删除数据
            * **/
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功！" };
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                if (dm != null && dm.Count() > 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = $"未找到配置的企业信息，统一信息代码为{string.Join(',', dm.Select(g => g.OrgCode))}！";
                    return fr;
                }
                var orgcodegroupby = list.GroupBy(g => new { g.OrgCode, g.PeriodYear }).Select(s => new { OrgCode = s.Key.OrgCode, PeriodYear = s.Key.PeriodYear, Count = s.Count() });
                foreach (var item in orgcodegroupby)
                {
                    if (item.Count > 1)
                    {
                        fr.IsSuccess = false;
                        fr.Message = $"统一信用代码为：{item.OrgCode}的企业在{item.PeriodYear}年上传了{item.Count}条数据!";
                        return fr;
                    }
                }
                var existdata = context.ApdFctGas.Where(f => f.PeriodYear.Equals(Convert.ToDecimal(year)));
                context.ApdFctGas.RemoveRange(existdata);
                foreach (var item in list)
                {
                    //if (isAlreadyExport(item.OrgCode, year))
                    //{
                    //    //continue;
                    //}

                    item.CreationDate = DateTime.Now;
                    item.CreatedBy = Convert.ToDecimal(userid);
                    context.ApdFctGas.Add(item);
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
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<ApdFctGas> entitys = context.ApdFctGas.Where(e => ids.Contains(e.RecordId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (ApdFctGas obj in entitys)
            {
                //删除
                context.ApdFctGas.Remove(obj);
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
    public class ReturnPollutantModel
    {
        public string RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public decimal PeriodYear { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public decimal? Gas { get; set; }        
        public string Other { get; set; }      
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
