using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Relation;

namespace JiaHang.Projects.Admin.BLL.Relation
{
    public class SysUserGroupRelationBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysUserGroupRelationBLL(DAL.EntityFramework.DataContext dataContext)
        {
            _context = dataContext;
        }

        public FuncResult Select()
        {
            var query = from a in _context.SysUserGroup
                        join b in _context.SysUserGroupRelation on a.UserGroupId equals b.UserGroupId
                       into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        join c in _context.SysUserInfo on b_ifnull.UserId equals c.UserId
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        orderby a.UserGroupName
                        select new
                        {
                            SysUserGroupName = a.UserGroupName,
                            SysUserGroupId = a.UserGroupId,
                            userid = c_ifnull != null ? c_ifnull.UserId : "",
                            username = c_ifnull != null ? c_ifnull.UserName : null,
                            UserAccount = c_ifnull != null ? c_ifnull.UserAccount : null,
                            UserMobileNo = c_ifnull != null ? c_ifnull.UserMobile : null,
                            RelationId = b_ifnull != null ? b_ifnull.SysUserGroupRelationId : null,
                            UserExists = c_ifnull != null
                        };
            var data = query.GroupBy(e => e.SysUserGroupId).Select(c => new
            {
                id = c.Key,
                name = c.First().SysUserGroupName,
                Children = c.Where(w => w.UserExists).Select(s => new
                {
                    s.RelationId,
                    s.UserAccount,
                    s.UserMobileNo,
                    id = s.userid,
                    name = s.username
                })
            });


            return new FuncResult() { IsSuccess = true, Content = data };
        }

        public FuncResult NotBindUser(string groupId,string condition)
        {
            //var data=_context.SysUserGroupRelation.Where(e=>e.UserGroupId)

            List<string> ids = _context.SysUserGroupRelation.
                Where(e => string.IsNullOrWhiteSpace(groupId) || e.UserGroupId == groupId).Select(e => e.UserId).ToList();

            var data = _context.SysUserInfo.Where(e => !ids.Contains(e.UserId)).Select(c => new
            {
                id = c.UserId,
                name = c.UserName,
                UserAccount = c.UserAccount,
                UserMobileNo = c.UserMobile
            }).Where(e=>string.IsNullOrWhiteSpace(condition)||(e.name.Contains(condition)||e.UserAccount.Contains(condition)));
            return new FuncResult() { IsSuccess = true, Content = data };
        }


        public async Task<FuncResult> Add(List<SysUserGroupRelationModel> model, string currentUserId)
        {

            //var data=_context.SysUserGroupRelation.Where(e=>model e.UserGroupId )

            //检查groupid 是否正确
            if (_context.SysUserGroup.Count(e => model.Select(s => s.GroupId).Contains(e.UserGroupId)) != model.Select(e => e.GroupId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "请传递正确SysUserGroupId" };
            }

            //检查userid 是否正确
            if (_context.SysUserInfo.Count(e => model.Select(s => s.UserId).Contains(e.UserId)) != model.Select(e => e.UserId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "请传递正确UserId" };
            }
            List<SysUserGroupRelation> add_entitys = new List<SysUserGroupRelation>();
            await Task.Run(() =>
            {
                foreach (SysUserGroupRelationModel obj in model)
                {
                    if (_context.SysUserGroupRelation.FirstOrDefault(e => e.UserGroupId == obj.GroupId && e.UserId == obj.UserId) == null)
                    {
                        add_entitys.Add(new SysUserGroupRelation()
                        {
                            SysUserGroupRelationId = Guid.NewGuid().ToString("N"),
                            UserGroupId = obj.GroupId,
                            UserId = obj.UserId,

                            CreatedBy = currentUserId,
                            CreationDate = DateTime.Now,
                            LastUpdatedBy = currentUserId,
                            LastUpdateDate = DateTime.Now
                        });
                    }
                }
            });

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {

                try
                {
                    await _context.SysUserGroupRelation.AddRangeAsync(add_entitys);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex.Message);
                    return new FuncResult() { IsSuccess = false, Message = "发生了预料之外的错误" };
                }
            }

            return new FuncResult() { IsSuccess = true, Message = "添加成功" };
        }

        public async Task<FuncResult> Delete(string id, string currentUserId)
        {

            SysUserGroupRelation entity = await _context.SysUserGroupRelation.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "id错误" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteTime = DateTime.Now;

            _context.Update(entity);
            _context.SaveChanges();
            return new FuncResult() { IsSuccess = false, Message = "删除成功" };

        }
    }
}
