using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysHelpInfo.RequestModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysHelpInfoBLL
{
    public class SysHelpInfoBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysHelpInfoBLL(DAL.EntityFramework.DataContext context)
        {

            _context = context;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysHelpInfo model)
        {
            var query = from a in _context.SysHelpInfo
                        join b in _context.SysHelpType on
                        a.HelpTypeId equals b.HelpTypeId
                        into a_temp
                        from a_ifnull in a_temp.DefaultIfEmpty()
                        orderby a.CreationDate descending
                        where ((string.IsNullOrWhiteSpace(model.Help_Type_Id) || a.HelpTypeId.Contains(model.Help_Type_Id))
                                && (string.IsNullOrWhiteSpace(model.Help_Title) || a.HelpTitle.Contains(model.Help_Title))
                                && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
                                && (string.IsNullOrWhiteSpace(Convert.ToString(model.Important_Flag)) || a.ImportantFlag == (model.Important_Flag))
                        )
                        select new
                        {
                            Help_Id = a.HelpId,
                            Help_Type_Id = a.HelpTypeId,
                            Help_Title = a.HelpTitle,
                            Important_Flag = a.ImportantFlag > 0 ? "是" : "否",
                            Audit_Flag = a.AuditFlag,
                            Audited_Date = a.AuditedDate,
                            Audited_By = a.AuditedBy,
                            help_Content = a.HelpContent,
                            help_Type_Name = a_ifnull.HelpTypeName,
                        };
            int total = query.Count();
            var data = query.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public FuncResult ElemeSelect(int pageSize, int currentPage, string helpTypeId, string helpTitle, int? auditFlag, int? importantFlag)
        {
            var query = from a in _context.SysHelpInfo
                        join b in _context.SysHelpType on
                        a.HelpTypeId equals b.HelpTypeId
                        into a_temp
                        from a_ifnull in a_temp.DefaultIfEmpty()
                        orderby a.CreationDate descending
                        where ((string.IsNullOrWhiteSpace(helpTypeId) || a.HelpTypeId.Contains(helpTypeId))
                                && (string.IsNullOrWhiteSpace(helpTitle) || a.HelpTitle.Contains(helpTitle))
                                && (string.IsNullOrWhiteSpace(Convert.ToString(auditFlag)) || a.AuditFlag == (auditFlag))
                                && (string.IsNullOrWhiteSpace(Convert.ToString(importantFlag)) || a.ImportantFlag == (importantFlag))
                        )
                        select new
                        {
                            Help_Id = a.HelpId,
                            Help_Type_Id = a.HelpTypeId,
                            Help_Title = a.HelpTitle,
                            Important_Flag = a.ImportantFlag > 0 ? "是" : "否",
                            Audit_Flag = a.AuditFlag ,
                            Audited_Date = a.AuditedDate,
                            Audited_By = a.AuditedBy,
                            help_Content = a.HelpContent,
                            help_Type_Name = a_ifnull.HelpTypeName,
                        };
            int total = query.Count();
            var data = query.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            SysHelpInfo entity = await _context.SysHelpInfo.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysHelpInfoModel model, string currentUserId)
        {
            SysHelpInfo entity = await _context.SysHelpInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "帮助ID错误!" };
            }
            entity.HelpTypeId = model.HelpTypeId;
            entity.HelpTitle = model.HelpTitle;
            entity.HelpContent = model.HelpContent;
            entity.ImportantFlag = model.ImportantFlag;
            //entity.AuditedDate = model.AuditedDate;
            //entity.AuditedBy = model.AuditedBy;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            _context.SysHelpInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            SysHelpInfo entity = await _context.SysHelpInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "帮助ID不存在!" };
            }
            entity.DeleteFlag = 1;
            //entity.DeleteFlag = true;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysHelpInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<SysHelpInfo> entitys = _context.SysHelpInfo.Where(e => ids.Contains(e.HelpId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysHelpInfo obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                //obj.DeleteFlag = true;
                obj.DeleteDate = DateTime.Now;
                _context.SysHelpInfo.Update(obj);
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
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{ids.Length}条记录" };

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysHelpInfoModel model, string currentUserId)
        {
            SysHelpInfo entity = new SysHelpInfo
            {
                HelpId = Guid.NewGuid().ToString(),
                HelpTypeId = model.HelpTypeId,
                HelpTitle = model.HelpTitle,
                HelpContent = model.HelpContent,
                ImportantFlag = model.ImportantFlag,
                AuditFlag =2,


                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                CreatedBy = currentUserId

            };
            await _context.SysHelpInfo.AddAsync(entity);
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
        public async Task<FuncResult> UpdateExamine(SysHelpInfoModel model, string currentuserId)
        {
            SysHelpInfo entity = await _context.SysHelpInfo.FindAsync(model.HelpTypeId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "帮助ID错误!" };
            }
            entity.AuditFlag = model.AuditFlag;
            entity.AuditedDate = System.DateTime.Now;
            entity.AuditedBy = currentuserId;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            _context.SysHelpInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "审核成功" };
        }
        /// <summary>
        /// 查询问题类型
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> SelectHelpType()
        {
            var query = from a in _context.SysHelpType
                        select new
                        {
                            helpTypeId = a.HelpTypeId,
                            helpTypaName = a.HelpTypeName
                        };
            object data = null;
            await Task.Run(() =>
            {
                data = query.ToList();
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }
       
    }

}


