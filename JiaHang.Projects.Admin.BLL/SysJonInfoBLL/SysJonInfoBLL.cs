using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysJobInfo.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace JiaHang.Projects.Admin.BLL.SysJonInfoBLL
{
    public class SysJonInfoBLL
    {
        private readonly DataContext _context;
        public SysJonInfoBLL(DataContext context)
        {
            _context = context;
        }

        public FuncResult Select(SearchSysJonInfo model)
        {
            try
            {

                int total = _context.SysJobInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Job_Code) || a.JobCode.Contains(model.Job_Code))
                        && (string.IsNullOrWhiteSpace(model.Job_Name) || a.JobName == (model.Job_Name))
                        && (string.IsNullOrWhiteSpace(model.Job_Type) || a.JobType == (model.Job_Type))
                        && (a.DeleteFlag != 1)
                        //&& (string.IsNullOrWhiteSpace(model.Audited_Date.ToString()) || a.AuditedDate.ToString().Contains(model.Audited_Date.ToString()))
                        )).Count();

                var result = _context.SysJobInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Job_Code) || a.JobCode.Contains(model.Job_Code))
                        && (string.IsNullOrWhiteSpace(model.Job_Name) || a.JobName == (model.Job_Name))
                        && (string.IsNullOrWhiteSpace(model.Job_Type) || a.JobType == (model.Job_Type))
                        && (a.DeleteFlag != 1)
                        //&& (model.Audited_Date.ToString() !=null || a.AuditedDate == (model.Audited_Date))
                        )).Skip(model.limit * model.page).Take(model.limit).ToList();
                var data = result.Select(e => new
                {
                    jobId = e.JobId,
                    jobCode = e.JobCode ?? "",
                    jobName = e.JobName ?? "",
                    enableFiag = e.EnableFlag,
                    jobType = e.JobType,
                     jobDesc = e.JobDesc,
                    createdBy = e.CreatedBy,
                    creationDate = e.CreationDate,
                    LastUpdatedBy = e.LastUpdatedBy,
                    jobLastRuntime = e.JobLastRuntime != null ? Convert.ToDateTime(e.JobLastRuntime).ToString("yyyy-MM-dd") : ""
                    
                });
                return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = true, Message = "数据错误" };
                throw ex;
            }

        }
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<FuncResult> Details(string MessageId)
        {
            var entity = await _context.SysJobInfo.FirstOrDefaultAsync(m => m.JobId == MessageId);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string MessageId, SysJobInfoModel model, string currentuserId)
        {
            SysJobInfo entity = await _context.SysJobInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }
            entity.JobId = MessageId;
            entity.JobCode = model.JobCode;
            entity.JobName = model.JobName;
            entity.EnableFlag = model.EnableFlag;
            entity.JobType = model.JobType;
            entity.JobDesc = model.JobDesc;

            entity.JobLastRuntime = System.DateTime.Now;
            entity.OnetimesDate = model.OnetimesDate;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.CycleFrequeceType = model.CycleFrequeceType;
            entity.CycleStartDate = model.CycleStartDate;
            entity.CycleEndDate = model.CycleEndDate;
            entity.CycleDayFrequeceType = model.CycleDayFrequeceType;
            entity.CycleDayOnetimesTime = model.CycleDayOnetimesTime;
            entity.CycleDayIntervalType = model.CycleDayIntervalType;
            entity.CycleDayIntervalNumber = model.CycleDayIntervalNumber;
            entity.CycleWeekEnabledMon = model.CycleWeekEnabledMon;
            entity.CycleWeekEnabledTue = model.CycleWeekEnabledTue;
            entity.CycleWeekEnabledWed = model.CycleWeekEnabledWed;
            entity.CycleWeekEnabledThu = model.CycleWeekEnabledThu;
            entity.CycleWeekEnabledFri = model.CycleWeekEnabledFri;
            entity.CycleWeekEnabledSat = model.CycleWeekEnabledSat;
            entity.CycleWeekEnabledSun = model.CycleWeekEnabledSun;
            entity.CycleWeekFrequeceType = model.CycleWeekFrequeceType;

            entity.CycleWeekOnetimesTime = model.CycleWeekOnetimesTime;
            entity.CycleWeekIntervalType = model.CycleWeekIntervalType;
            entity.CycleWeekIntervalNumber = model.CycleWeekIntervalNumber;
            entity.CycleMonthType = model.CycleMonthType;
            entity.CycleMonthDaytimes = model.CycleMonthDaytimes;
            entity.CycleMonthWeekType = model.CycleMonthWeekType;
            entity.CycleMonthWeekNumber = model.CycleMonthWeekNumber;
            entity.CycleMonthFrequeceType = model.CycleMonthFrequeceType;
            entity.CycleMonthOnetimesTime = model.CycleMonthOnetimesTime;
            entity.CycleMonthIntervalType = model.CycleMonthIntervalType;
            entity.CycleMonthIntervalNumber = model.CycleMonthIntervalNumber;
            _context.SysJobInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="code"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string MessageId, string currentUserId)
        {
            SysJobInfo entity = await _context.SysJobInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysJobInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] MessageIds, string currentuserId)
        {
            IQueryable<SysJobInfo> entitys = _context.SysJobInfo.Where(e => MessageIds.Contains(e.JobId));
            if (entitys.Count() != MessageIds.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysJobInfo obj in entitys)
            {
                obj.DeleteBy = currentuserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysJobInfo.Update(obj);
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
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{MessageIds.Length}条记录" };

        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysJobInfoModel model, string currentUserId)
        {
            if (_context.SysJobInfo.Count(e => e.JobId == model.JobId) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "已经存在相同的公告编码。" };
            }
            SysJobInfo entity = new SysJobInfo
            {
                JobId = Guid.NewGuid().ToString(),
                JobCode = model.JobCode,
                JobName = model.JobName,
                EnableFlag = model.EnableFlag,
                JobType = model.JobType,
                JobDesc = model.JobDesc,
                
                JobLastRuntime = System.DateTime.Now,
                OnetimesDate = model.OnetimesDate,
                LastUpdateDate = System.DateTime.Now,
                CycleFrequeceType = model.CycleFrequeceType,
                CycleStartDate = model.CycleStartDate,
                CycleEndDate = model.CycleEndDate,
                CycleDayFrequeceType = model.CycleDayFrequeceType,
                CycleDayOnetimesTime = model.CycleDayOnetimesTime,
                CycleDayIntervalType = model.CycleDayIntervalType,
                CycleDayIntervalNumber = model.CycleDayIntervalNumber,
                CycleWeekEnabledMon = model.CycleWeekEnabledMon,
                CycleWeekEnabledTue = model.CycleWeekEnabledTue,
                CycleWeekEnabledWed = model.CycleWeekEnabledWed,
                CycleWeekEnabledThu = model.CycleWeekEnabledThu,
                CycleWeekEnabledFri = model.CycleWeekEnabledFri,
                CycleWeekEnabledSat = model.CycleWeekEnabledSat,
                CycleWeekEnabledSun = model.CycleWeekEnabledSun,
                CycleWeekFrequeceType = model.CycleWeekFrequeceType,
                
                CycleWeekOnetimesTime = model.CycleWeekOnetimesTime,
                CycleWeekIntervalType = model.CycleWeekIntervalType,
                CycleWeekIntervalNumber = model.CycleWeekIntervalNumber,
                CycleMonthType = model.CycleMonthType,
                CycleMonthDaytimes = model.CycleMonthDaytimes,
                CycleMonthWeekType = model.CycleMonthWeekType,
                CycleMonthWeekNumber = model.CycleMonthWeekNumber,
                CycleMonthFrequeceType = model.CycleMonthFrequeceType,
                CycleMonthOnetimesTime = model.CycleMonthOnetimesTime,
                CycleMonthIntervalType = model.CycleMonthIntervalType,
                CycleMonthIntervalNumber = model.CycleMonthIntervalNumber,
          
                CreationDate = System.DateTime.Now,
                CreatedBy = currentUserId,

                LastUpdatedBy = currentUserId,

            };
            await _context.SysJobInfo.AddAsync(entity);

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
                    Console.WriteLine(ex.Message);
                    return new FuncResult() { IsSuccess = false, Content = ex.Message };
                }
            }


            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }
    }
}
