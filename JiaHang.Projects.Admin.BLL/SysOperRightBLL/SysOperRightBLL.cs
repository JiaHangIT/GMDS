using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsCustomerServiceParams.RequestModel;
using JiaHang.Projects.Admin.Model.SysOperRightParamsModel.RequestModel;
using JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysOperRightBLL
{
    public class SysOperRightBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysOperRightBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(int pageSize, int currentPage, string modelName)
        {
            var query = from a in _context.SysModelInfo
                        .Where(e => string.IsNullOrWhiteSpace(modelName) || e.ModelName.Contains(modelName))
                        .Skip(pageSize * currentPage).Take(pageSize)

                        join b in _context.SysModelGroup
                        on a.ModelGroupId equals b.ModelGroupId
                        join c in _context.SysOperRightInfo
                        on a.ModelId equals c.ModelId
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        join d in _context.SysUserInfo on c_ifnull.UserId equals d.UserId
                        into d_temp
                        from d_ifnull in d_temp.DefaultIfEmpty()
                        join e in _context.SysUserGroup on c_ifnull.UserGroupId equals e.UserGroupId
                        into e_temp
                        from e_ifnull in e_temp.DefaultIfEmpty()
                        select new
                        {
                            a.ModelId,
                            a.ModelName,
                            a.ModelGroupId,
                            b.ModelGroupName,


                            operReightId = c_ifnull == null ? "" : c_ifnull.RecordId,
                            userIsNull = d_ifnull == null,
                            userName = d_ifnull == null ? "" : d_ifnull.UserName,
                            userAccount = d_ifnull == null ? "" : d_ifnull.UserAccount,

                            userGroupIsNull = e_ifnull == null,
                            userGroupName = e_ifnull == null ? "" : e_ifnull.UserGroupName
                        };

            var data = query.GroupBy(e => new { e.ModelId, e.ModelName, e.ModelGroupName, e.ModelGroupId }).Select(model => new
            {
                model.Key.ModelId,
                model.Key.ModelName,
                model.Key.ModelGroupId,
                model.Key.ModelGroupName,

                users = model.Where(u => !u.userIsNull).Select(c => new
                {
                    c.operReightId,
                    c.userName,
                    c.userAccount,
                }).Distinct(),
                userGroups = model.Where(g => !g.userGroupIsNull).Select(c => new
                {
                    c.userGroupName
                }).Distinct()
            });

            return new FuncResult() { IsSuccess = true, Content = new { data, total = _context.SysModelInfo.Count() } };
        }


        /// <summary>
        /// 返回当前模块未绑定的用户以及用户组
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public FuncResult NotBind(string modelId)
        {

            var query = _context.SysOperRightInfo.Where(c => c.ModelId == modelId).Select(c => new
            {
                c.UserId,
                c.UserGroupId
            });
            var users = _context.SysUserInfo.Where(e => !query.Select(c => c.UserId).Contains(e.UserId)).Select(e => new
            {
                e.UserId,
                e.UserName,
            });
            var userGroups = _context.SysUserGroup.Where(e => !query.Select(c => c.UserGroupId).Contains(e.UserGroupId)).Select(e => new
            {
                e.UserGroupName,
                e.UserGroupId
            });
            return new FuncResult() { IsSuccess = true, Content = new { users, userGroups } };
        }


        public async Task<FuncResult> Add(string currentUserId, SysOperRightParamsModel model)
        {
            List<SysOperRightInfo> rights = new List<SysOperRightInfo>();
            await Task.Run(() =>
            {
                foreach (var u in model.userIds)
                {
                    var entity = new SysOperRightInfo()
                    {
                        RecordId = Guid.NewGuid().ToString(),
                        ModelId = model.ModelId,
                        UserId = u,

                        CreatedBy = currentUserId,
                        CreationDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = currentUserId,

                    };
                    rights.Add(entity);
                }

            });
            await Task.Run(() =>
            {
                foreach (var g in model.userGroupIds)
                {
                    var entity = new SysOperRightInfo()
                    {
                        RecordId = Guid.NewGuid().ToString(),
                        ModelId = model.ModelId,
                        UserGroupId = g,

                        CreatedBy = currentUserId,
                        CreationDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = currentUserId,

                    };
                    rights.Add(entity);
                }

            });
            await _context.SysOperRightInfo.AddRangeAsync(rights);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = "添加数据时，发生了预料之外的错误" };
                }
            }
            return new FuncResult() { IsSuccess = true, Message = "已成功添加数据" };
        }


        public async Task<FuncResult> Delete(string currentUserId, SysOperRightParamsModel model)
        {

            var users = _context.SysOperRightInfo.Where(e => model.userIds.Contains(e.UserId) && e.ModelId == model.ModelId);

            var usergroups = _context.SysOperRightInfo.Where(e => model.userGroupIds.Contains(e.UserGroupId) && e.ModelId == model.ModelId);

           
            await Task.Run(() =>
            {
                foreach (var u in users)
                {
                    u.DeleteBy = currentUserId;
                    u.DeleteDate = DateTime.Now;
                    u.DeleteFlag = 1;
                }
                foreach (var g in usergroups)
                {
                    g.DeleteBy = currentUserId;
                    g.DeleteDate = DateTime.Now;
                    g.DeleteFlag = 1;
                }
            });
            _context.SysOperRightInfo.UpdateRange(users);
            _context.SysOperRightInfo.UpdateRange(usergroups);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = "删除数据时，发生了预料之外的错误" };
                }
            }

            return new FuncResult() { IsSuccess = true, Message = "删除成功" };
        }


    }
}
