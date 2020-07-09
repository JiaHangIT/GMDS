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
        private readonly DAL.EntityFramework.DataContext _context;
        public SysUserInfoBLL(DAL.EntityFramework.DataContext context)
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
                User_Password = e.UserPassword,
                User_Mobile_No = e.UserMobile,
                CreationDate = e.CreationDate.ToString("yyyy-MM-dd HH:mm:ss")
            });

             return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public FuncResult ElementSelect(int pageSize, int currentPage,string userAccount,string userName)
        {
            IOrderedQueryable<SysUserInfo> query = _context.SysUserInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(userAccount) || a.UserAccount.Contains(userAccount))
                        && (string.IsNullOrWhiteSpace(userName) || a.UserName.Contains(userName))
                        )
                        ).OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(pageSize * currentPage).Take(pageSize).ToList().Select(e => new
            {
                User_Id = e.UserId,
                User_Account = e.UserAccount,
                User_Name = e.UserName,
                User_Org_Id = e.UserOrgId,
                User_Group_Names = e.UserGroupNames,
                User_Email = e.UserEmail,
                User_Password = e.UserPassword,
                User_Mobile_No = e.UserMobile,
                CreationDate = e.CreationDate.ToString("yyyy-MM-dd HH:mm:ss")
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
            //entity.UserOrgId = model.UserOrgId;
            entity.UserAccount = model.UserAccount;
            //entity.UserGroupNames = model.UserGroupNames;
            entity.UserEmail = model.UserEmail;
            entity.UserMobile = model.UserMobileNo;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            if (entity.UserAccount.Length < 6)
            {
                return new FuncResult() { IsSuccess = false, Message = "账户至少设置6位!" };
            }

            var pr = passwordverify(entity.UserPassword);
            if (pr != Strength.Normal)
            {
                if (pr == Strength.Invalid)
                {
                    return new FuncResult() { IsSuccess = false, Message = "账户至少设置6位!" };
                }
                return new FuncResult() { IsSuccess = false, Message = "密码至少8位，由数字、字母或特殊字符中2种方式组成!" };
            }

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
            entity.DeleteBy = currentUserId;
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
                obj.DeleteBy = currentUserId;
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

        /// <summary>
        /// 账户至少6位，密码需要复杂度检测
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysUserInfoModel model, string currentUserId)
        {
            SysUserInfo entity = new SysUserInfo
            {
                UserId=Guid.NewGuid().ToString(),
                UserAccount = model.UserAccount,
                UserName = model.UserName,
                UserPassword = model.UserPassword,
               // UserOrgId = model.UserOrgId,
                UserGroupNames = model.UserGroupNames,
                UserEmail = model.UserEmail,
                UserMobile = model.UserMobileNo,
                //LanguageCode = model.LanguageCode,
                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                CreatedBy = currentUserId

            };
            if (entity.UserAccount.Length < 6)
            {
                return new FuncResult() { IsSuccess = false, Message = "账户至少设置6位!" };
            }
          
            var pr = passwordverify(entity.UserPassword);
            if (pr != Strength.Normal)
            {
                if (pr == Strength.Invalid)
                {
                    return new FuncResult() { IsSuccess = false, Message = "密码至少8位!" };
                }
                return new FuncResult() { IsSuccess = false, Message = "密码至少8位，由数字、字母或特殊字符中2种方式组成!" };
            }
            
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

        /// <summary>
        /// 密码至少8位，由数字、字母或特殊字符中2种方式组成
        /// </summary>
        public static Func<string, Strength> passwordverify = (password) =>
           {
                //空字符串强度值为0
                if (password == "" || password.Length < 8) return Strength.Invalid;

                //字符统计
                int iNum = 0, iLtt = 0, iSym = 0;
               foreach (char c in password)
               {
                   if (c >= '0' && c <= '9') iNum++;
                   else if (c >= 'a' && c <= 'z') iLtt++;
                   else if (c >= 'A' && c <= 'Z') iLtt++;
                   else iSym++;
               }
               if (iLtt == 0 && iSym == 0) return Strength.Weak; //纯数字密码
                if (iNum == 0 && iLtt == 0) return Strength.Weak; //纯符号密码
                if (iNum == 0 && iSym == 0) return Strength.Weak; //纯字母密码
                if (password.Length <= 6) return Strength.Weak; //长度不大于6的密码
                if (iLtt == 0) return Strength.Normal; //数字和符号构成的密码
                if (iSym == 0) return Strength.Normal; //数字和字母构成的密码
                if (iNum == 0) return Strength.Normal; //字母和符号构成的密码
                if (password.Length <= 10) return Strength.Normal; //长度不大于10的密码
                return Strength.Strong; //由数字、字母、符号构成的密码
            };

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
                        UserMobile = model.UserMobileNo,
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

            //var comlumHeadrs = new[] { "用户ID", "登录帐号", "用户名称", "用户组织ID", "用户组别名称", "用户电子邮件地址", "用户手机号码", "创建时间" };
            var comlumHeadrs = new[] { "用户ID", "登录帐号", "用户名称", "用户电子邮件地址", "用户手机号码", "创建时间" };
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
                    worksheet.Cells["D" + j].Value = obj.UserEmail;
                    worksheet.Cells["E" + j].Value = obj.UserMobile;
                    worksheet.Cells["F" + j].Value = obj.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
                    j++;
                }
            });

            result = package.GetAsByteArray();



            return result;
        }



    }

    /// <summary>
    /// 密码强度
    /// </summary>
    public enum Strength
    {
        Invalid = 0, //无效密码
        Weak = 1, //低强度密码
        Normal = 2, //中强度密码
        Strong = 3 //高强度密码
    };
}
