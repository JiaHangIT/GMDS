using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysProblemInfo.RequestModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysProblemInfoBLL
{
    public class SysProblemInfoBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysProblemInfoBLL(DAL.EntityFramework.DataContext context)
        {

            _context = context;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysProblemInfo model)
        {
            //try
            //{
            //    int total = _context.SysProblemInfo.
            //         Where(a =>
            //         (
            //         (string.IsNullOrWhiteSpace(model.Problem_Type_Id) || a.ProblemTypeId.Contains(model.Problem_Type_Id))
            //         &&(string.IsNullOrWhiteSpace(model.Problem_Title) || a.ProblemTitle.Contains(model.Problem_Title))
            //         && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
            //         && (a.DeleteFlag != 1)
            //         )
            //         ).Count();
            //    var result = _context.SysProblemInfo.
            //         Where(a =>
            //         (
            //(string.IsNullOrWhiteSpace(model.Problem_Type_Id) || a.ProblemTypeId.Contains(model.Problem_Type_Id))
            //&& (string.IsNullOrWhiteSpace(model.Problem_Title) || a.ProblemTitle.Contains(model.Problem_Title))
            //&& (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a.AuditFlag == (model.Audit_Flag))
            //&& (a.DeleteFlag != 1)
            //         )
            //         ).Skip(model.limit * model.page).Take(model.limit).ToList();
            //    var data = result.Select(e => new
            //    {
            //Problem_Id = e.ProblemId,
            //        Problem_Type_Id = e.ProblemTypeId,
            //        Problem_Title = e.ProblemTitle,
            //        Audit_Flag = e.AuditFlag > 0 ? "是" : "否",
            //        Audited_Date = e.AuditedDate,
            //        Audited_By = e.AuditedBy,
            //        problem_Contant = e.ProblemContent
            //    });
            //    return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            //}
            //catch (Exception ex)
            //{
            //    return new FuncResult() { IsSuccess = true, Message = "数据错误" };
            //    throw ex;
            //}
            var query = from a in _context.SysProblemType
                        join b in _context.SysProblemInfo on
                        a.ProblemTypeId equals b.ProblemTypeId
                        into a_temp
                        from a_ifnull in a_temp.DefaultIfEmpty()
                        where ((string.IsNullOrWhiteSpace(model.Problem_Type_Id) || a_ifnull.ProblemTypeId.Contains(model.Problem_Type_Id))
                               && (string.IsNullOrWhiteSpace(model.Problem_Title) || a_ifnull.ProblemTitle.Contains(model.Problem_Title))
                               && (string.IsNullOrWhiteSpace(Convert.ToString(model.Audit_Flag)) || a_ifnull.AuditFlag == (model.Audit_Flag))
                        )
                        select new {
                            Problem_Id = a_ifnull.ProblemId,
                            Problem_Type_Id = a_ifnull.ProblemTypeId,
                            Problem_Title = a_ifnull.ProblemTitle,
                            Audit_Flag = a_ifnull.AuditFlag > 0 ? "是" : "否",
                            Audited_Date = a_ifnull.AuditedDate,
                            Audited_By = a_ifnull.AuditedBy,
                            problem_Contant = a_ifnull.ProblemContent,
                            problem_Type_Name=a.ProblemTypeName,
                            
                        };
            int total = query.Count();
            var data = query.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            SysProblemInfo entity = await _context.SysProblemInfo.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysProblemInfoModel model, string currentUserId)
        {
            SysProblemInfo entity = await _context.SysProblemInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告ID错误!" };
            }
            entity.ProblemTypeId = model.ProblemTypeId;
            entity.ProblemTitle = model.ProblemTitle;
            entity.ProblemContent = model.ProblemContent;
            entity.AuditFlag = model.AuditFlag;
            //entity.AuditedDate = model.AuditedDate;
            //entity.AuditedBy = model.AuditedBy;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            _context.SysProblemInfo.Update(entity);
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
            SysProblemInfo entity = await _context.SysProblemInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告ID不存在!" };
            }
            entity.DeleteFlag = 1;
            //entity.DeleteFlag = true;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysProblemInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<SysProblemInfo> entitys = _context.SysProblemInfo.Where(e => ids.Contains(e.ProblemId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysProblemInfo obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                //obj.DeleteFlag = true;
                obj.DeleteDate = DateTime.Now;
                _context.SysProblemInfo.Update(obj);
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
        public async Task<FuncResult> Add(SysProblemInfoModel model, string currentUserId)
        {
            SysProblemInfo entity = new SysProblemInfo
            {
                ProblemId = Guid.NewGuid().ToString(),
                ProblemTypeId = model.ProblemTypeId,
                ProblemTitle = model.ProblemTitle,
                ProblemContent = model.ProblemContent,
                AuditFlag = model.AuditFlag,


                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                CreatedBy = currentUserId

            };
            await _context.SysProblemInfo.AddAsync(entity);
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
        public async Task<FuncResult> UpdateExamine(SysProblemInfoModel model, string currentuserId)
        {
            SysProblemInfo entity = await _context.SysProblemInfo.FindAsync(model.ProblemId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "常见问题编号错误!" };
            }

            entity.AuditFlag = model.AuditFlag;
            entity.AuditedDate = System.DateTime.Now;

            entity.AuditedBy = currentuserId;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            _context.SysProblemInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "审核成功" };
        }
        /// <summary>
        /// 查询问题类型
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> SelectProblemType()
        {
            var query = from a in _context.SysProblemType
                        select new
                        {
                          problemTypeId=a.ProblemTypeId,
                          problemTypaName=a.ProblemTypeName
                        };
            object data = null;
            await Task.Run(() =>
            {
                data = query.ToList();
            });
            return new FuncResult() { IsSuccess = true, Content = data };
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


