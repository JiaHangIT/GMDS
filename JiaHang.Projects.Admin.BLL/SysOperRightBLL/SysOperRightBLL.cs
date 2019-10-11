using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
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
        public FuncResult Select(string modelGroupId)
        {
            var query = from a in _context.SysModelInfo
                        join b in _context.SysModelGroup.Where(a => a.ModelGroupId == modelGroupId)
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
                            a.SortKey,


                            operReightId = c_ifnull == null ? "" : c_ifnull.RecordId,
                            userIsNull = d_ifnull == null,
                            userName = d_ifnull == null ? "" : d_ifnull.UserName,
                            userAccount = d_ifnull == null ? "" : d_ifnull.UserAccount,

                            userGroupIsNull = e_ifnull == null,
                            userGroupName = e_ifnull == null ? "" : e_ifnull.UserGroupName
                        };

            var data = query.ToList().GroupBy(e => new { e.ModelGroupName, e.ModelGroupId }).Select(modelgroup => new
            {

                modelgroup.Key.ModelGroupId,
                modelgroup.Key.ModelGroupName,
                model = modelgroup.GroupBy(g => new { g.ModelId, g.ModelName }).Select(model => new
                {
                    model.Key.ModelName,
                    model.Key.ModelId,
                    model.First().SortKey,
                    users = model.Where(u => !u.userIsNull).Select(c => new
                    {
                        c.operReightId,
                        c.userName,
                        c.userAccount,
                        isRemove = false,
                    }).Distinct(),

                    userGroups = model.Where(g => !g.userGroupIsNull).Select(c => new
                    {
                        c.operReightId,
                        c.userGroupName,
                        isRemove = false,
                    }).Distinct(),
                    deloperids = new List<string>(),

                }).OrderByDescending(e => e.SortKey),
            });

            return new FuncResult() { IsSuccess = data.Count() > 0, Content = data.FirstOrDefault() };
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


        public async Task<FuncResult> Delete(string currentUserId, SysOperRightDeleteParamsModel model)
        {

            var dels = _context.SysOperRightInfo.Where(e => model.operids.Contains(e.RecordId) && e.ModelId == model.ModelId);


            await Task.Run(() =>
            {
                foreach (var u in dels)
                {
                    u.DeleteBy = currentUserId;
                    u.DeleteDate = DateTime.Now;
                    u.DeleteFlag = 1;
                }

            });
            _context.SysOperRightInfo.UpdateRange(dels);
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

        public FuncResult<List<UserRouteModel>> CurrentUserRoutes(string currentUserId, bool isAdmin = false)
        {
            var listusers = _context.SysUserGroupRelation.Where(e => e.UserId == currentUserId).Select(c => c.UserId);
            var opers = _context.SysOperRightInfo.Where(e => e.UserId == currentUserId || listusers.Contains(e.UserId)).Select(e => e.ModelId);
            //获取当前用户 所拥有的页面权限信息
            var query = (from a in _context.SysModelInfo.Where(e => isAdmin == true || opers.Contains(e.ModelId))
                         join b in _context.SysModelGroup.Where(e => string.IsNullOrWhiteSpace(e.ParentId)) on a.ModelGroupId equals b.ModelGroupId

                         //join c in _context.SysModelGroup on b.ParentId equals c.ModelGroupId
                         //into c_temp
                         //from c_ifnull in c_temp.DefaultIfEmpty()

                         orderby a.SortKey descending
                         orderby b.SortKey descending
                         select new
                         {
                             b.ModelGroupId,
                             b.ModelGroupName,
                             b.ModelGroupUrl,
                             GroupOutUrlFlag = b.OutUrlFlag,
                             b.SortKey,

                             a.ModelId,
                             a.ModelName,
                             a.ModelUrl,
                             a.OutUrlFlag,
                         }).ToList();

            var query1 = (from a in _context.SysModelInfo.Where(e => isAdmin == true || opers.Contains(e.ModelId))
                          join b in _context.SysModelGroup on a.ModelGroupId equals b.ModelGroupId
                          join c in _context.SysModelGroup on b.ParentId equals c.ModelGroupId


                          orderby a.SortKey descending
                          orderby b.SortKey descending
                          orderby c.SortKey descending
                          select new
                          {
                              b.ModelGroupId,
                              b.ModelGroupName,
                              b.ModelGroupUrl,
                              GroupOutUrlFlag = b.OutUrlFlag,


                              a.ModelId,
                              a.ModelName,
                              a.ModelUrl,
                              a.OutUrlFlag,

                              hModelGroupId = c.ModelGroupId,
                              hModelGroupUrl = c.ModelGroupUrl,
                              hModelGroupName = c.ModelGroupName,
                              hOutUrlFlag = c.OutUrlFlag,
                              c.SortKey,
                          }).ToList();

            var data1 = query1.GroupBy(e => new { e.hModelGroupId, e.hModelGroupName, e.hModelGroupUrl, e.hOutUrlFlag, e.SortKey }).Select(c => new UserRouteModel
            {
                ModelGroupId = c.Key.hModelGroupId,
                ModelGroupUrl = c.Key.hOutUrlFlag == 1 ? $"/iframecontainer/index?path={c.Key.hModelGroupUrl}" : c.Key.hModelGroupUrl,
                OutUrlFlag = c.Key.hOutUrlFlag == 1,
                ModelGroupName = c.Key.hModelGroupName,
                SortKey = c.Key.SortKey.Value,
                ModelGroups = c.GroupBy(g => new { g.ModelGroupId, g.ModelGroupName, g.ModelGroupUrl, g.OutUrlFlag }).Select(gc => new UserRouteModel
                {
                    ModelGroupId = gc.Key.ModelGroupId,
                    OutUrlFlag = gc.Key.OutUrlFlag == 1,
                    ModelGroupUrl = gc.Key.OutUrlFlag == 1 ? $"/iframecontainer/index?path={gc.Key.ModelGroupUrl}" : gc.Key.ModelGroupUrl,
                    ModelGroupName = gc.Key.ModelGroupName,

                    Models = gc.Select(m => new UserModuleRoute()
                    {
                        ModelId = m.ModelId,
                        ModelName = m.ModelName,
                        ModelUrl = m.OutUrlFlag == 1 ? $"/iframecontainer/index?path={m.ModelUrl}" : m.ModelUrl,
                        OutUrlFlag = m.OutUrlFlag == 1,
                    }).ToList()
                }).ToList()
            }).ToList();
            var data = query.GroupBy(e => new { e.ModelGroupId, e.ModelGroupName, e.ModelGroupUrl, e.GroupOutUrlFlag, e.SortKey })
                .Select(g => new UserRouteModel
                {
                    ModelGroupId = g.Key.ModelGroupId,
                    ModelGroupName = g.Key.ModelGroupName,
                    ModelGroupUrl = g.Key.GroupOutUrlFlag == 1 ? $"/iframecontainer/index?path={g.Key.ModelGroupUrl}" : g.Key.ModelGroupUrl,
                    OutUrlFlag = g.Key.GroupOutUrlFlag == 1,
                    SortKey = g.Key.SortKey.Value,
                    Models = g.Select(m => new UserModuleRoute
                    {
                        ModelId = m.ModelId,
                        ModelName = m.ModelName,
                        ModelUrl = m.OutUrlFlag == 1 ? $"/iframecontainer/index?path={m.ModelUrl}" : m.ModelUrl,
                        OutUrlFlag = m.OutUrlFlag == 1
                    }).ToList()
                }).ToList();

            foreach (var obj in data1)
            {
                for (var i = 0; i < data.Count; i++)
                {
                    if (obj.ModelGroupId == data[i].ModelGroupId)
                    {
                        obj.Models.AddRange(data[i].Models);
                        data.Remove(data[i]);
                    }
                }
            }

            data1.AddRange(data);
            return new FuncResult<List<UserRouteModel>>() { IsSuccess = true, Content = data1.OrderByDescending(e => e.SortKey).ToList() };
        }
    }
}
