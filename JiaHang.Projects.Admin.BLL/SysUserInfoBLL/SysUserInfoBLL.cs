using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Enumerations;
using JiaHang.Projects.Admin.Model.Enumerations.Sys_User;
using JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel;
using OfficeOpenXml;

namespace JiaHang.Projects.Admin.BLL.SysUserInfoervice
{
    public class SysUserInfoBLL
    {
        private readonly DataContext _context;
        public SysUserInfoBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysUserInfo model)
        {
            IOrderedQueryable<SysUserInfo> query = _context.SysUserInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.User_Account) || a.UserAccount.Contains(model.User_Account))
                        && (string.IsNullOrWhiteSpace(model.User_Name) || a.UserName.Contains(model.User_Name))
                        && (model.User_Is_Ldap == null || a.UserIsLdap == model.User_Is_Ldap)
                        && (model.User_Ower == null || model.User_Ower == 0)
                        )
                        ).OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(e => new
            {
                User_Id = e.UserId,
                User_Account = e.UserAccount,
                User_Name = e.UserName,
                User_Org_Id = e.UserOrgId,
                User_Group_Names = e.UserGroupNames,
                User_Email = e.UserEmail,
                User_Is_Ldap = e.UserIsLdap,
                User_Password = e.UserPassword,
                User_Is_Lock = e.UserIsLock,
                User_Mobile_No = e.UserMobile,
                User_Ower = UserOwerType.Kds,
                Language_Code = e.LanguageCode,

                Eff_Start_Date = e.EffStartDate.ToString("yyyy-MM-dd"),
                Eff_End_Date = e.EffEndDate.ToString("yyyy-MM-dd"),
                CreationDate = e.CreationDate.ToString("yyyy-MM-dd")
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
            SysUserInfo entity = await _context.SysUserInfo.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysUserInfoModel model, string currentUserId)
        {
            SysUserInfo entity = await _context.SysUserInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
            }


            entity.UserName = model.UserName;
            entity.UserPassword = model.UserPassword;
            entity.UserOrgId = model.UserOrgId;
            entity.UserGroupNames = model.UserGroupNames;
            entity.UserEmail = model.UserEmail;
            entity.UserIsLdap = model.UserIsLdap;
            entity.UserMobile = model.UserMobileNo;
            entity.LanguageCode = model.LanguageCode;
            entity.UserIsLock = model.UserIsLock;
            entity.EffStartDate = model.EffStartDate;
            entity.EffEndDate = model.EffEndDate;

            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            _context.SysUserInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }

        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            SysUserInfo entity = await _context.SysUserInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeletedBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysUserInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<SysUserInfo> entitys = _context.SysUserInfo.Where(e => ids.Contains(e.UserId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysUserInfo obj in entitys)
            {
                obj.DeletedBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysUserInfo.Update(obj);
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
        public async Task<FuncResult> Add(SysUserInfoModel model, string currentUserId)
        {
            SysUserInfo entity = new SysUserInfo
            {
                UserAccount = model.UserAccount,
                UserName = model.UserName,
                UserPassword = model.UserPassword,
                UserOrgId = model.UserOrgId,
                UserGroupNames = model.UserGroupNames,
                UserEmail = model.UserEmail,
                UserIsLdap = model.UserIsLdap,
                UserMobile = model.UserMobileNo,
                LanguageCode = model.LanguageCode,
                UserIsLock = model.UserIsLock,
                EffStartDate = model.EffStartDate,
                EffEndDate = model.EffEndDate,

                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                CreatedBy = currentUserId

            };
            await _context.SysUserInfo.AddAsync(entity);

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

        public FuncResult<SysUserInfo> Login(string userAccount, string password)
        {
            var result = new FuncResult<SysUserInfo>() { IsSuccess = false, Message = "账号或密码错误!" };

            var entity = _context.SysUserInfo.FirstOrDefault(e => e.UserAccount == userAccount);
            if (entity != null && entity.UserPassword == password)
            {
                result.Content = entity;
                result.Message = "登录成功";
                result.IsSuccess = true;
            }
            return result;
        }
        private async Task<List<SysUserInfo>> AddTestAccount(SysUserInfoModel model)
        {

            List<SysUserInfo> list = new List<SysUserInfo>();
            await Task.Run(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    SysUserInfo entity = new SysUserInfo
                    {
                        UserAccount = Guid.NewGuid().ToString("N").Substring(0, 6),

                        UserName = Guid.NewGuid().ToString("N").Substring(0, 6),
                        UserPassword = model.UserPassword,
                        UserOrgId = model.UserOrgId,
                        UserGroupNames = model.UserGroupNames,
                        UserEmail = Guid.NewGuid().ToString("N").Substring(0, 6) + "@gmail.com",
                        UserIsLdap = model.UserIsLdap,
                        UserMobile = model.UserMobileNo,
                        LanguageCode = model.LanguageCode,
                        UserIsLock = model.UserIsLock,
                        EffStartDate = model.EffStartDate,
                        EffEndDate = model.EffEndDate,

                        CreationDate = DateTime.Now,
                        CreatedBy = "admin"

                    };
                    list.Add(entity);
                }
            });
            return list;

        }


        public async Task<byte[]> GetUserListBytes()

        {

            var comlumHeadrs = new[] { "用户ID", "登录帐号", "用户名称", "用户组织ID", "用户组别名称", "用户电子邮件地址", "用户手机号码", "创建时间" };
            byte[] result;
            var data = _context.SysUserInfo.ToList();
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
                                                                       //First add the headers
            for (var i = 0; i < comlumHeadrs.Count(); i++)
            {
                worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
            }
            //Add values
            var j = 2;
            // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            await Task.Run(() =>
            {
                foreach (var obj in data)
                {
                    var rt = obj.GetType();
                    var rp = rt.GetProperties();

                    worksheet.Cells["A" + j].Value = obj.UserId;
                    worksheet.Cells["B" + j].Value = obj.UserAccount;
                    worksheet.Cells["C" + j].Value = obj.UserName;
                    worksheet.Cells["D" + j].Value = obj.UserOrgId;
                    worksheet.Cells["E" + j].Value = obj.UserGroupNames;
                    worksheet.Cells["F" + j].Value = obj.UserEmail;
                    worksheet.Cells["G" + j].Value = obj.UserMobile;
                    worksheet.Cells["H" + j].Value = obj.CreationDate;
                    j++;
                }
            });

            result = package.GetAsByteArray();



            return result;
        }



    }
}
