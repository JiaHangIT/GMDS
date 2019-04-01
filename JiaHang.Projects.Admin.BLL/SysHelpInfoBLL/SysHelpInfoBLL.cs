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
            IOrderedQueryable<SysHelpInfo> query = _context.SysHelpInfo.
                 Where(a =>
                 (
                 (string.IsNullOrWhiteSpace(model.Help_Type_Id) || a.HelpTypeId.Contains(model.Help_Type_Id))
                 && (string.IsNullOrWhiteSpace(model.Help_Title) || a.HelpTitle.Contains(model.Help_Title))
                 && (model.Audit_Flag == null || a.AuditFlag==model.Audit_Flag)
                 && (model.Important_Flag == null || a.ImportantFlag == model.Important_Flag)
                 )
                 ).OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(e => new
            {
                //Help_Id =              e.HelpId,
                //Help_Type_id =            e.HelpTypeId,
                //Help_Title =         e.HelpTitle,
                //Important_Flag =         e.ImportantFlag,
                //Audit_Flag =          e.AuditFlag,
                //Audited_Date =         e.AuditedDate,
                //Audited_By =            e.AuditedBy
                e.HelpId,
                e.HelpTypeId,
                e.HelpTitle,
                e.ImportantFlag,
                e.AuditFlag,
                e.AuditedDate,
                e.AuditedBy
            });
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
                return new FuncResult() { IsSuccess = false, Message = "公告ID错误!" };
            }
            entity.HelpTypeId = model.HelpTypeId;
            entity.HelpTitle = model.HelpTitle;
            entity.HelpContent = model.HelpContent;
            entity.ImportantFlag = model.ImportantFlag;
            entity.AuditFlag = model.AuditFlag;
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
                return new FuncResult() { IsSuccess = false, Message = "公告ID不存在!" };
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
                HelpTypeId = model.HelpTypeId,
                HelpTitle = model.HelpTitle,
                HelpContent = model.HelpContent,
                ImportantFlag = model.ImportantFlag,
                AuditFlag = model.AuditFlag,
               

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

        //public FuncResult<SysUserInfo> CheckUserLDAP(string userAccount)
        //{
        //    var result = new FuncResult<SysUserInfo>() { IsSuccess = false, Message = "不是LDAP" };

        //    var entity = _context.SysHelpInfo.FirstOrDefault(e => e.User_Account == userAccount);
        //    if (entity != null && entity.User_Is_Ldap == true)
        //    {
        //        result.Content = entity;
        //        result.Message = "是LDAP";
        //        result.IsSuccess = true;
        //    }
        //    else if (entity == null)
        //    {
        //        result.Content = null;
        //        result.Message = "不存在该用户";
        //        result.IsSuccess = false;
        //    }
        //    return result;
        //}


        //public async Task<byte[]> GetUserListBytes()

        //{

        //    var comlumHeadrs = new[] { "帮助ID", "帮助类型ID", "公告标题", "是否重要标志", "是否审核标志", "审核时间", "审核人"};
        //    byte[] result;
        //    var data = _context.SysHelpInfo.ToList();
        //    var package = new ExcelPackage();
        //    var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
        //                                                               //First add the headers
        //    for (var i = 0; i < comlumHeadrs.Count(); i++)
        //    {
        //        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
        //    }
        //    //Add values
        //    var j = 2;
        //    // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        //    await Task.Run(() =>
        //    {
        //        foreach (var obj in data)
        //        {
        //            var rt = obj.GetType();
        //            var rp = rt.GetProperties();

        //            worksheet.Cells["A" + j].Value = obj.HelpId;
        //            worksheet.Cells["B" + j].Value = obj.HelpTypeId;
        //            worksheet.Cells["C" + j].Value = obj.HelpTitle;
        //            worksheet.Cells["D" + j].Value = obj.ImportantFlag;
        //            worksheet.Cells["E" + j].Value = obj.AuditFlag;
        //            worksheet.Cells["F" + j].Value = obj.AuditedDate;
        //            worksheet.Cells["G" + j].Value = obj.AuditedBy;

        //            j++;
        //        }
        //    });

        //    result = package.GetAsByteArray();



        //    return result;
        //}

    }

}


