using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysMessageInfo;
using JiaHang.Projects.Admin.Model.SysMessageInfo.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace JiaHang.Projects.Admin.BLL.SysMessageInfoBLL
{
    public class SysMessageInfoBLL
    {
        private readonly DataContext _context;
        public SysMessageInfoBLL(DataContext context)
        {
            _context = context;
        }

        public FuncResult Select(SearchSysMessageInfo model)
        {
            try
            {

                int total = _context.SysMessageInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Message_title) || a.MessageTitle.Contains(model.Message_title))
                        && (string.IsNullOrWhiteSpace(model.Created_By) || a.CreatedBy == (model.Created_By))
                        && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
                        && (a.DeleteFlag != 1)
                        //&& (string.IsNullOrWhiteSpace(model.Audited_Date.ToString()) || a.AuditedDate.ToString().Contains(model.Audited_Date.ToString()))
                        )).Count();

                var result = _context.SysMessageInfo.
                        Where(a =>
                        (
                       (string.IsNullOrWhiteSpace(model.Message_title) || a.MessageTitle.Contains(model.Message_title))
                        && (string.IsNullOrWhiteSpace(model.Created_By) || a.CreatedBy == (model.Created_By))
                        && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
                        && (a.DeleteFlag != 1)
                        //&& (model.Audited_Date.ToString() !=null || a.AuditedDate == (model.Audited_Date))
                        )).Skip(model.limit * model.page).Take(model.limit).ToList();
                var data = result.Select(e => new
                {
                    messageId = e.MessageId,
                    messageTitle = e.MessageTitle ?? "",
                    messageContent = e.MessageContent ?? "",
                    importantFlag = e.ImportantFlag.ToString() ?? "",
                    auditFlag = e.AuditFlag,
                    auditedDate = e.AuditedDate != null ? Convert.ToDateTime(e.AuditedDate).ToString("yyyy-MM-dd") : "",
                    auditedBy = e.AuditedBy ?? "",
                    creationDate = e.CreationDate != null ? Convert.ToDateTime(e.CreationDate).ToString("yyyy-MM-dd") : "",
                    createdBy = e.CreatedBy
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
            var entity = await _context.SysMessageInfo.FirstOrDefaultAsync(m => m.MessageId == MessageId);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string MessageId, SysMessageInfoModel model, string currentuserId)
        {
            SysMessageInfo entity = await _context.SysMessageInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }
            entity.MessageId = model.Message_ID;
            entity.MessageTitle = model.Message_title;
            entity.MessageContent = model.Message_Content;
            entity.ImportantFlag = model.Important_Flag;
            //entity.AuditFlag = model.Audit_Flag;
            //entity.AuditedDate = model.Audited_Date;

            //entity.AuditedBy = model.Audited_By;
            entity.CreatedBy = currentuserId;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            _context.SysMessageInfo.Update(entity);
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
            SysMessageInfo entity = await _context.SysMessageInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysMessageInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] MessageIds, string currentuserId)
        {
            IQueryable<SysMessageInfo> entitys = _context.SysMessageInfo.Where(e => MessageIds.Contains(e.MessageId));
            if (entitys.Count() != MessageIds.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysMessageInfo obj in entitys)
            {
                obj.DeleteBy = currentuserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysMessageInfo.Update(obj);
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
        public async Task<FuncResult> Add(SysMessageInfoModel model, string currentUserId)
        {
            if (_context.SysMessageInfo.Count(e => e.MessageId == model.Message_ID) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "已经存在相同的公告编码。" };
            }
            SysMessageInfo entity = new SysMessageInfo
            {
                MessageId = model.Message_ID,
                MessageTitle = model.Message_title,
                MessageContent = model.Message_Content,
                ImportantFlag = model.Important_Flag,
                AuditFlag = model.Audit_Flag,
                AuditedDate = model.Audited_Date,

                AuditedBy = model.Audited_By,
                CreationDate = System.DateTime.Now,
                CreatedBy = currentUserId,
                LastUpdateDate = System.DateTime.Now,
                LastUpdatedBy = currentUserId,

            };
            await _context.SysMessageInfo.AddAsync(entity);

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
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> UpdateExamine(string MessageId,string currentuserId)
        {
            SysMessageInfo entity = await _context.SysMessageInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }

            entity.AuditFlag = 1;
            entity.AuditedDate = System.DateTime.Now;

            entity.AuditedBy = currentuserId;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            _context.SysMessageInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "审核成功" };
        }
    }
}
