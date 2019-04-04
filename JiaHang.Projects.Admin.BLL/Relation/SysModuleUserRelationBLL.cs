using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Relation;
using System.Linq;
using System.Collections.Generic;
using JiaHang.Projects.Admin.Model.Relation.Response;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using System;
using JiaHang.Projects.Admin.Model.Enumerations.SysModule;
using JiaHang.Projects.Admin.Model.SysRoute;

namespace JiaHang.Projects.Admin.BLL.Relation
{
    /// <summary>
    /// 模块和用户绑定
    /// </summary>
    public class SysModuleUserRelationBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        private readonly SysModuleRouteRelationBLL sysModuleRoute;
        public SysModuleUserRelationBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
            sysModuleRoute = new SysModuleRouteRelationBLL(context);
        }



        /// <summary>
        /// 查询
        /// 获取接连的模块 所绑定的用户
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> Select()
        {
            var query = from a in _context.SysModule
                        join b in _context.SysModuleUserRelation on a.Id equals b.ModuleId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()// SysModule与 sysmoduleUserRelation 的左连接
                        join c in _context.SysUserGroup on b_ifnull.UserGroupId equals c.UserGroupId
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                        join d in _context.SysUserInfo on b_ifnull.UserId equals d.UserId
                        into d_temp
                        from d_ifnull in d_temp.DefaultIfEmpty()
                        orderby a.ModuleName
                        select new
                        {
                            RelationId = b_ifnull == null ? null : b_ifnull.Id,
                            ModuleId = a.Id,
                            a.ModuleName,
                            ModuleLevel = a.Level,
                            ModuleParentId = a.ParentId,
                            ModuleUserRelation = b_ifnull != null ? b_ifnull.ModuleUserRelation : 0,
                            PermissionType = b_ifnull != null ? b_ifnull.PermissionType : 0,

                            UserGroupIsNull = c_ifnull == null,
                            UserGroupName = c_ifnull != null ? c_ifnull.UserGroupName : null,
                            UserGroupId = c_ifnull != null ? c_ifnull.UserGroupId : null,
                            UserGroupLevel = c_ifnull != null ? c_ifnull.UserGroupLevel : 0,

                            UserIsNull = d_ifnull == null,
                            UserName = d_ifnull != null ? d_ifnull.UserName : null,
                            UserId = d_ifnull != null ? d_ifnull.UserId : "",
                            UserAccount = d_ifnull != null ? d_ifnull.UserAccount : null
                        };
            var data = query.ToList().GroupBy(e => e.ModuleId).Select(s => new SysModuleUserRelationRawResponseModel
            {

                ModuleId = s.Key,
                ModuleName = s.First().ModuleName,
                ModuleLevel = s.First().ModuleLevel,
                ModuleParentId = s.First().ModuleParentId,
                //ModuleUserRelation = s.First().ModuleUserRelation,

                ListUser = s.Where(user => !user.UserIsNull).Select(q => new User
                {
                    RelationId = q.RelationId,
                    UserId = q.UserId,
                    UserAccount = q.UserAccount,
                    UserName = q.UserName,
                    PermissionType = (PermissionType)q.PermissionType


                }).ToList(),
                ListUserGroup = s.Where(group => !group.UserGroupIsNull).Select(q => new UserGroup
                {
                    RelationId = q.RelationId,
                    UserGroupId = q.UserGroupId,
                    UserGroupName = q.UserGroupName,
                    UserGroupLevel =(int) q.UserGroupLevel,
                    PermissionType = (PermissionType)q.PermissionType
                }).ToList()
            }).ToList();
            var routes = await sysModuleRoute.Select();

            var result = await AcquisitionModule(data,routes.Content);
            //递归整理出树形结构

            return new FuncResult() { IsSuccess = true, Content = result };
        }


        public async Task<FuncResult> AddOrUpdate(List<SysModuleUserRelationModel> data, string currentUserId)
        {
            if (data.Count <= 0) return new FuncResult() { IsSuccess = false, Message = "请传递正确参数" };
            //不能重复绑定
            var list = _context.SysModuleUserRelation.Where(e => data.Select(m => m.ModuleId).Contains(e.ModuleId)).ToList();
            if (list.Count > 0)
            {
                if ( list.Where(e => data.Where(c =>!string.IsNullOrWhiteSpace( c.UserGroupId )&& string.IsNullOrWhiteSpace(c.Id)).Select(q => q.UserGroupId).Contains(e.UserGroupId)).Count() > 0)
                {
                    return new FuncResult() { IsSuccess = false, Message = "同一用户组不能重复绑定到同一模块上" };
                }
                if ( list.Where(e => data.Where(c => string.IsNullOrWhiteSpace(c.Id)).Select(q => q.UserId).Contains(e.UserId)).Count() > 0)
                {
                    return new FuncResult() { IsSuccess = false, Message = "同一用户不能重复绑定到同一模块上" };
                }
            }

            //检查module 是否存在

            if (_context.SysModule.Count(q => data.Select(e => e.ModuleId).Contains(q.Id)) != data.Select(e => e.ModuleId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "模块id错误" };
            }
            //检查 userid/usergroupid是否存在
            if (_context.SysUserGroup.Count(q => data.Select(e => e.UserGroupId).Contains(q.UserGroupId)) != data.Select(e => e.UserGroupId).Distinct().Count(q => !string.IsNullOrWhiteSpace(q)))
            {
                return new FuncResult() { IsSuccess = false, Message = "用户组id错误" };
            }
            if (_context.SysUserInfo.Count(q => data.Select(e => e.UserId).Contains(q.UserId)) != data.Select(e => e.UserId).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "用户id错误" };
            }

            // 检查更新模块与用户关联记录时  关联id是否正确

            if (_context.SysModuleUserRelation.Count(e => data.Select(c => c.Id).Contains(e.Id)) != data.Select(e => e.Id).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct().Count())
            {
                return new FuncResult() { IsSuccess = false, Message = "更新模块与用户绑定记录时，未能根据关联id找到对应的关联记录" };
            }

            //添加
            var adds = data.Where(e => string.IsNullOrWhiteSpace(e.Id));
            var modifys = data.Where(e => !string.IsNullOrWhiteSpace(e.Id));
            await Task.Run(() =>
            {
                foreach (var add in adds)
                {
                    var add_entity = new SysModuleUserRelation()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ModuleId = add.ModuleId,
                        UserGroupId = add.UserGroupId,
                        UserId = add.UserId,
                        ModuleUserRelation =(int)( string.IsNullOrWhiteSpace(add.UserGroupId) ? ModuleUserRelation.ModuleUser : ModuleUserRelation.ModuleUserGroup),
                        PermissionType = (int)add.PermissionType,

                        CreationDate = DateTime.Now,
                        CreatedBy = currentUserId,
                        LastUpdatedBy = currentUserId,
                        LastUpdateDate = DateTime.Now
                    };
                    _context.SysModuleUserRelation.Add(add_entity);
                }

                //更新
                foreach (var modify in modifys)
                {
                    var modify_enity = _context.SysModuleUserRelation.Find(modify.Id);

                    modify_enity.ModuleId = modify.ModuleId;
                    modify_enity.UserGroupId = modify_enity.UserGroupId;
                    modify_enity.UserId = modify_enity.UserId;
                    modify_enity.ModuleUserRelation =(int)( string.IsNullOrWhiteSpace(modify.UserGroupId) ? ModuleUserRelation.ModuleUser : ModuleUserRelation.ModuleUserGroup);
                    modify_enity.PermissionType =(int) modify.PermissionType;
                    modify_enity.LastUpdatedBy = currentUserId;
                    modify_enity.LastUpdateDate = DateTime.Now;
                    _context.SysModuleUserRelation.Update(modify_enity);
                }
            });


            using (var trans = _context.Database.BeginTransaction())
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
                    return new FuncResult() { IsSuccess = false, Message = "发生了预料之外的错误" };
                }
            }

            return new FuncResult() { IsSuccess = true };
        }

        /// <summary>
        /// 获取未绑定用户/用户组
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public FuncResult NotBindUsers(string moduleId)
        {
            var exists = _context.SysModuleUserRelation.Where(e => e.ModuleId == moduleId).ToList();

            //未绑定用户
            var users = _context.SysUserInfo.Where(e => !exists.Select(c => c.UserId).Contains(e.UserId)).Select(q => new
            {
                UserId = q.UserId,
                UserName = q.UserName,
                UserAccount = q.UserAccount
            });

            //未绑定用户组
            var userGroups = _context.SysUserGroup.Where(e => !exists.Select(c => c.UserGroupId).Contains(e.UserGroupId)).Select(q => new
            {
                UserGroupName = q.UserGroupName,
                UserGroupId = q.UserGroupId,
                UserGroupLevel = q.UserGroupLevel
            });
            return new FuncResult() { IsSuccess = true, Content = new { users, userGroups } };
        }

        public async Task<FuncResult> Delte(string id, string currentUserId)
        {
            var entity = await _context.SysModuleUserRelation.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "删除模块与用户绑定记录时，未能根据关联id找到对应的关联记录" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteTime = DateTime.Now;
            entity.DeleteBy = currentUserId;

            _context.SysModuleUserRelation.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        #region 递归获取模块
        /// <summary>
        /// 递归获取模块
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysModuleUserRelationResponseModel>> AcquisitionModule(List<SysModuleUserRelationRawResponseModel> data,List<ModuleRouteRelationResponseModel> routes)
        {
            var parents = data.Where(e => string.IsNullOrWhiteSpace(e.ModuleParentId)).ToList();
            List<SysModuleUserRelationResponseModel> list = new List<SysModuleUserRelationResponseModel>();
            await Task.Run(() =>
            {
                foreach (var obj in parents)
                {
                    list.Add(Recursive(data, obj,routes));
                }
            });
            return list;
        }

        /// <summary>
        /// 递归获取功能模块
        /// 并整理成树形结构
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private SysModuleUserRelationResponseModel Recursive(List<SysModuleUserRelationRawResponseModel> data, SysModuleUserRelationRawResponseModel parent,List<ModuleRouteRelationResponseModel> routes)
        {

            var module = new SysModuleUserRelationResponseModel()
            {
                ModuleId = parent.ModuleId,
                ModuleLevel = parent.ModuleLevel,
                ModuleName = parent.ModuleName,
                ListUser = parent.ListUser,
                ListUserGroup = parent.ListUserGroup,
                Controllers=routes.FirstOrDefault(e=>parent.ModuleId==e.ModuleId)?.Controllers
            };
            //移除自身
            var childs = data.Where(e => e.ModuleParentId == parent.ModuleId).ToList();
            if (childs.Count == 0)
            {
                return module;
            }
            var parent_entity = data.FirstOrDefault(e => e.ModuleId == parent.ModuleId);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (var child in childs)
            {
                module.ListChildren.Add(Recursive(data.Where(e => e.ModuleParentId == child.ModuleId).ToList(), child,routes));
            }
            return module;
        }

        //public FuncResult ModuleConditionAndProperty(string moduleId)
        //{
        //    try
        //    {
        //        var moduleProperty = from a in _context.DataConditionProperties
        //                             join b in _context.MethodConditionProperties on a.PrototypeValue equals b.ConditionPropertyId// into
        //                             where b.ModuleId.Equals(moduleId)
        //                             select new DataConditionPropertyModel()
        //                             {
        //                                 Id = a.PrototypeValue,
        //                                 DataConditionId = a.DataConditionId,
        //                                 PrototypeName = a.PrototypeName,
        //                                 PrototypeValue = a.PrototypeValue,
        //                                 ParentPrototypeValue = a.ParentPrototypeValue,
        //                                 SortValue = a.SortValue
        //                             };
        //        var moduleCondition = from a in _context.SysDataCondition
        //                              join b in _context.SysMethodConditions on a.Id equals b.ConditionId
        //                              where b.ModuleId.Equals(moduleId)
        //                              select new DataConditionModel()
        //                              {
        //                                  Id = a.Id,
        //                                  ConditionName = a.ConditionName,
        //                                  SortValue = a.SortValue,
        //                                  ParentId = a.ParentId,
        //                              };
        //        //var mpry = from a in _context.MethodConditionProperties where a.ModuleId == moduleId select (a.ConditionPropertyId);
        //        return new FuncResult() { IsSuccess = true, Content = new { property = BuildTreeData.TreeStructData(moduleProperty.ToList()), condition = BuildTreeData.TreeStructData(moduleCondition.ToList()) },Message="查询成功" };
        //    }
        //    catch (Exception)
        //    {

        //        return new FuncResult() { IsSuccess = false, Content = null, Message = "查询失败" };
        //    }
         
        //}
        #endregion
    
    }
}
