using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.ApdFtcWater;
using JiaHang.Projects.Admin.Model.ExcelSearchMode.Gas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.ExcelFctWaterBLL
{
    public class WaterBll
    {
        private readonly DataContext context;

        public WaterBll(DataContext _context) { this.context = _context; }


        public async Task<FuncResult> GetListPagination(SearchExcelModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                //var q= context.ApdFctWater
                var query = from c in context.ApdFctWater
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
                                Water=c.Water,
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
                var query = from c in context.ApdFctWater
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new ReturnPollutantModel()
                            {
                                RecordId = c.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                PeriodYear = o.PeriodYear,
                                Water = c.Water,
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
        public bool WriteData(IEnumerable<ApdFctWaterDal> list, string year)
        {
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdFctWater.Select(g => g.OrgCode).Contains(f.OrgCode));

                if (dm != null && dm.Count() > 0)
                {
                    return false;
                }
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                context.ApdFctWater.AddRange(list);
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
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>       
        public async Task<FuncResult> Update(int id, ApdFtcWaterModel model)
        {
            ApdFctWaterDal entity;
            try
            {
                entity = await context.ApdFctWater.FirstOrDefaultAsync(m => m.RecordId == model.RecordId);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
                }
                entity.OrgCode = model.OrgCode;
                entity.Water = model.Water;
                entity.Other = model.Other;
                entity.Remark = model.Remark;

                //entity.LAST_UPDATED_BY = HttpContext.CurrentUser(cache).Id;
                entity.LastUpdateDate = DateTime.Now;

                context.ApdFctWater.Update(entity);
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
            ApdFctWaterDal entity = await context.ApdFctWater.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.DeleteFlag = 1;
            //entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            context.ApdFctWater.Update(entity);
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
        public decimal? Water { get; set; }
        public decimal? Other { get; set; }
        public string Remark { get; set; }
    }
}
