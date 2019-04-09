using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysErrorCodeInfo.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace JiaHang.Projects.Admin.BLL.SysErrorCodeInfoBLL
{
    public class SysErrorCodeInfoBLL
    {
        private readonly DataContext _context;
        public SysErrorCodeInfoBLL(DataContext context)
        {
            _context = context;
        }
        public FuncResult Select(SearchSysErrorCodeInfo model)
        {
            try
            {

                int total = _context.SysErrorCodeInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Error_Code_Code) || a.ErrorCodeCode.Contains(model.Error_Code_Code))
                         && (string.IsNullOrWhiteSpace(model.Error_Code_Name) || a.ErrorCodeName == (model.Error_Code_Name))
                        && (string.IsNullOrWhiteSpace(model.Created_By) || a.CreatedBy == (model.Created_By))
                        && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
                        && (a.DeleteFlag != 1)
                        //&& (string.IsNullOrWhiteSpace(model.Audited_Date.ToString()) || a.AuditedDate.ToString().Contains(model.Audited_Date.ToString()))
                        )).Count();

                var result = _context.SysErrorCodeInfo.
                        Where(a =>
                        (
                      (string.IsNullOrWhiteSpace(model.Error_Code_Code) || a.ErrorCodeCode.Contains(model.Error_Code_Code))
                         && (string.IsNullOrWhiteSpace(model.Error_Code_Name) || a.ErrorCodeName == (model.Error_Code_Name))
                        && (string.IsNullOrWhiteSpace(model.Created_By) || a.CreatedBy == (model.Created_By))
                        && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
                        && (a.DeleteFlag != 1)
                        //&& (model.Audited_Date.ToString() !=null || a.AuditedDate == (model.Audited_Date))
                        )).Skip(model.limit * model.page).Take(model.limit).ToList();
                var data = result.Select(e => new
                {
                   
                    errorCodeId = e.ErrorCodeId,
                    errorCodeCode = e.ErrorCodeCode ?? "",
                    errorCodeName = e.ErrorCodeName ?? "",
                    errorCodeDesc = e.ErrorCodeDesc ?? "",
                    importantFlag = e.ImportantFlag > 0 ? "是" : "否",
                    auditFlag = e.AuditFlag > 0 ? "通过审核" : "未通过审核",
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
            var entity = await _context.SysErrorCodeInfo.FirstOrDefaultAsync(m => m.ErrorCodeId == MessageId);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string MessageId, SysErrorCodeInfoModel model, string currentuserId)
        {
            SysErrorCodeInfo entity = await _context.SysErrorCodeInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }
            entity.ErrorCodeId = model.ErrorCodeId;
            entity.ErrorCodeCode = model.ErrorCodeCode;
            entity.ErrorCodeName = model.ErrorCodeName;
            entity.ErrorCodeDesc = model.ErrorCodeDesc;
            entity.ImportantFlag = model.ImportantFlag;
            //entity.AuditFlag = model.Audit_Flag;
            //entity.AuditedDate = model.Audited_Date;

            //entity.AuditedBy = model.Audited_By;
            entity.CreatedBy = currentuserId;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            _context.SysErrorCodeInfo.Update(entity);
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
            SysErrorCodeInfo entity = await _context.SysErrorCodeInfo.FindAsync(MessageId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysErrorCodeInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] MessageIds, string currentuserId)
        {
            IQueryable<SysErrorCodeInfo> entitys = _context.SysErrorCodeInfo.Where(e => MessageIds.Contains(e.ErrorCodeId));
            if (entitys.Count() != MessageIds.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysErrorCodeInfo obj in entitys)
            {
                obj.DeleteBy = currentuserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysErrorCodeInfo.Update(obj);
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
        public async Task<FuncResult> Add(SysErrorCodeInfoModel model, string currentUserId)
        {
            if (_context.SysErrorCodeInfo.Count(e => e.ErrorCodeId == model.ErrorCodeId) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "已经存在相同的公告编码。" };
            }
            SysErrorCodeInfo entity = new SysErrorCodeInfo
            {
                ErrorCodeId = model.ErrorCodeId,
                ErrorCodeCode = model.ErrorCodeCode,
                ErrorCodeName = model.ErrorCodeName,
                ErrorCodeDesc = model.ErrorCodeDesc,
                ImportantFlag = model.ImportantFlag,
                AuditFlag = 0,
                //entity.AuditedDate = model.Audited_Date;

                //entity.AuditedBy = model.Audited_By;
                CreationDate= System.DateTime.Now,
                CreatedBy = currentUserId,
                LastUpdateDate = System.DateTime.Now,
                LastUpdatedBy = currentUserId

            };
            await _context.SysErrorCodeInfo.AddAsync(entity);

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
        public async Task<FuncResult> UpdateExamine(SysErrorCodeInfoModel model, string currentuserId)
        {
            SysErrorCodeInfo entity = await _context.SysErrorCodeInfo.FindAsync(model.ErrorCodeId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }

            entity.AuditFlag = model.AuditFlag;
            entity.AuditedDate = System.DateTime.Now;

            entity.AuditedBy = currentuserId;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            _context.SysErrorCodeInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "处理成功！" };
        }
    }
}
