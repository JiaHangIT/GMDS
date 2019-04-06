using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysRoute;

namespace JiaHang.Projects.Admin.BLL
{
    public class DataConditionBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public DataConditionBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        public async Task<FuncResult> Add(DataConditionModel model, string currentUserId)
        {
            if (_context.SysDataCondition.FirstOrDefault(e => e.TableName.ToUpper() == model.TableName.ToUpper()) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据维度表名不能重复" };
            }

            SysDataCondition entity = new SysDataCondition
            {
                SysDataConditionId = Guid.NewGuid().ToString("N"),
                ConditionName = model.ConditionName,
                ParentId = model.ParentId,
                SortValue = model.SortValue,
                ConditionDesc = model.ConditionDesc,
                ConditionValue = model.ConditionValue,

                TableName = model.TableName,
                ParentColumn = model.ParentColumn,
                ConditionValueDesc = model.ConditionValueDesc,
                ChildColumn = model.ChildColumn,
                MasterSlaveFlag = model.MasterSlaveFlag==true?1:0,

                CreationDate = DateTime.Now,
                CreatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                LastUpdatedBy = currentUserId

            };
            await _context.SysDataCondition.AddAsync(entity);

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                    return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Content = ex.Message, Message = "添加失败" };
                }
            }


        }
        public  FuncResult Update(DataConditionModel model, string currentUserId)
        {
            if (_context.SysDataCondition.FirstOrDefault(e => e.TableName.ToUpper() == model.TableName.ToUpper() && e.SysDataConditionId != model.Id) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据维度表名不能重复" };
            }
            var entity = _context.SysDataCondition.FirstOrDefault(e => e.SysDataConditionId == model.Id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "id错误" };
            }

            entity.SysDataConditionId = model.Id;
            entity.ConditionName = model.ConditionName;
            entity.ParentId = model.ParentId;
            entity.SortValue = model.SortValue;
            entity.ConditionDesc = model.ConditionDesc;
            entity.ConditionValue = model.ConditionValue;
            entity.TableName = model.TableName;
            entity.ParentColumn = model.ParentColumn;
            entity.ConditionValueDesc = model.ConditionValueDesc;
            entity.MasterSlaveFlag = model.MasterSlaveFlag?1:0;
            entity.ChildColumn = "";
            entity.LastUpdateDate = DateTime.Now;
            entity.LastUpdatedBy = currentUserId;

            if (entity.MasterSlaveFlag==1)
            {
                if (string.IsNullOrWhiteSpace(model.ChildColumn))
                {
                    return new FuncResult() { IsSuccess = false, Message = "数据权限具有从属关系时,数据子级值所在列名不能为空" };
                }
                entity.ChildColumn = model.ChildColumn;
            }

            _context.SysDataCondition.Update(entity);

            _context.SaveChanges();

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            try
            {
                SysDataCondition entity = await _context.SysDataCondition.FindAsync(id);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "id错误" };
                }
                entity.DeleteFlag = 1;
                entity.DeleteTime = DateTime.Now;
                entity.LastUpdatedBy = currentUserId;

                _context.Update(entity);
                _context.SaveChanges();
                return new FuncResult() { IsSuccess = true, Message = "删除成功" };
            }
            catch (Exception)
            {
                return new FuncResult() { IsSuccess = false, Message = "删除失败" };
            }


        }


        public FuncResult Bind(List<SysUserDataCondition> model, string currentUserId)
        {
            //检查是否重复绑定
            foreach (SysUserDataCondition obj in model)
            {
                SysUserDataCondition entity = _context.SysUserDataCondition.
                    FirstOrDefault(e => ((!string.IsNullOrWhiteSpace(obj.UserGroupId) && e.UserGroupId == obj.UserGroupId) ||(e.UserId == obj.UserId)) &&
                    e.ConditionId == obj.ConditionId && e.ConditionValue == obj.ConditionValue
                    && e.ControllerId == obj.ControllerId);
                if (entity != null)
                {
                    return new FuncResult() { IsSuccess = false, Message = $"[{obj.ConditionValue}] 已经绑定请勿重复操作!" };
                }

                entity = new SysUserDataCondition()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    UserId = obj.UserId,
                    UserGroupId = obj.UserGroupId,
                    ConditionName = obj.ConditionName,
                    ConditionId = obj.ConditionId,
                    ConditionValue = obj.ConditionValue,
                    ControllerId = obj.ControllerId,
                    SortValue = obj.SortValue,

                    CreatedBy = currentUserId,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = currentUserId,
                    LastUpdateDate = DateTime.Now
                };
                _context.SysUserDataCondition.Add(entity);
            }

            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = "添加数据时发生了预料之外的错误,请稍后重试" };
                }
            }
            return new FuncResult() { IsSuccess = true };
        }

        public FuncResult Unbind(string id, string currentUserId)
        {
            SysUserDataCondition entity = _context.SysUserDataCondition.Find(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "id错误" };
            }

            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteTime = DateTime.Now;

            _context.SysUserDataCondition.Update(entity);
            _context.SaveChanges();
            return new FuncResult() { IsSuccess = true };
        }

        public FuncResult Select()
        {
            var data = _context.SysDataCondition
                .Select(e => new
                {
                   Id= e.SysDataConditionId,
                    e.ConditionName,
                    e.TableName,
                    e.ConditionValueDesc,
                    e.ParentColumn,
                    e.MasterSlaveFlag,
                    e.ChildColumn,
                    e.SortValue,
                    CreationDate = e.CreationDate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .OrderByDescending(e => e.SortValue).ThenBy(e => e.CreationDate);
            return new FuncResult() { IsSuccess = true, Content = new { data = data, total = data.Count() } };
        }

        private List<ConditionRawValue> GetConditionRawValueList()
        {
            var conditions = _context.SysDataCondition.Select(e => new
            {
                e.ConditionName,
                e.ConditionValueDesc,
                e.TableName,
                Id = e.SysDataConditionId,
                e.ParentColumn,
                e.ChildColumn,
                e.MasterSlaveFlag,
                e.SortValue
            }).ToList();
            List<string> listsql = new List<string>();
            foreach (var obj in conditions)
            {
                string sqlstr = "";
                if (!(obj.MasterSlaveFlag==1))
                {
                    sqlstr += $"select '{obj.Id}' as ConditionId ,'{obj.SortValue}' as SortValue , '{obj.ConditionName}' as ConditionName,'{obj.MasterSlaveFlag}' as MasterSlaveFlag,'{obj.ConditionValueDesc}' as ConditionValueDesc,'{obj.TableName}' as TableName,cast({obj.ParentColumn} as nvarchar2(36))  as ParentCode,cast('' as nvarchar2(36)) as Code from {obj.TableName} ";
                }
                else
                {
                    sqlstr += $"select '{obj.Id}' as ConditionId ,'{obj.SortValue}' as SortValue , '{obj.ConditionName}' as ConditionName,'{obj.MasterSlaveFlag}' as MasterSlaveFlag,'{obj.ConditionValueDesc}' as ConditionValueDesc, '{obj.TableName}' as TableName,cast({obj.ParentColumn} as nvarchar2(36))  as ParentCode,cast({obj.ChildColumn} as nvarchar2(36))  as Code from {obj.TableName} ";
                }
                listsql.Add(sqlstr);
            }
            string sql = string.Join("union ", listsql);
            List<ConditionRawValue> data = OracleDbHelper.Query<ConditionRawValue>(sql);
            if (data == null) return new List<ConditionRawValue>();
            return data;
        }

        /// <summary>
        /// 获取所有的数据权限值
        /// </summary>
        /// <returns></returns>
        private List<ConditionModel> GetAllConditionValues()
        {
            var data = GetConditionRawValueList();
            var list = data.GroupBy(e => new { e.ConditionId, e.TableName }).Select(c => new
            {
                c.Key.ConditionId,
                c.Key.TableName,
                c.First().ConditionName,
                c.First().MasterSlaveFlag,
                List = c.Select(q => new ConditionRawValue
                {
                    Code = q.Code,
                    ConditionName = q.ConditionName,
                    ConditionValueDesc = q.ConditionValueDesc,
                    TableName = q.TableName,
                    ConditionId = q.ConditionId,
                    ParentCode = q.ParentCode,
                    SortValue = q.SortValue

                }).ToList()
            }).ToList();
            //将权限名 与权限值关联
            List<ConditionModel> result = new List<ConditionModel>();
            foreach (var par in list)
            {
                List<ConditionValue> values = new List<ConditionValue>();
                if (!par.MasterSlaveFlag)
                {
                    //无主从关系
                    foreach (ConditionRawValue l in par.List)
                    {
                        values.Add(new ConditionValue()
                        {
                            Id = l.ParentCode,
                            ConditionId = l.ConditionId,
                            ConditionName = l.ConditionName,
                            SortValue = l.SortValue
                        });
                    }
                }
                else
                {
                    List<ConditionRawValue> parents = par.List.Where(e => string.IsNullOrWhiteSpace(e.ParentCode)).ToList();
                    foreach (ConditionRawValue p in parents)
                    {
                        values.Add(BuilderValuesTree(par.List, p));
                    }
                }


                ConditionModel model = new ConditionModel()
                {
                    Id = par.ConditionId,
                    Label = par.ConditionName,
                    Children = values,
                };
                result.Add(model);
            }
            return result;
        }

        public List<ConditionModel> GetNotBindConditionValues(string userId, string controllerId)
        {
            //获取用户已经绑定的

            List<string> exists = _context.SysUserDataCondition.Where(e => e.UserId == userId && e.ControllerId == controllerId).Select(e => e.ConditionValue).ToList();

            //获取所有权限值
            List<ConditionModel> allvalues = GetAllConditionValues();

            //筛选未绑定的
            List<ConditionModel> result = new List<ConditionModel>();
            foreach (ConditionModel obj in allvalues)
            {
                obj.Children = AcquisitionGetValue(obj.Children, exists, false);
                if (obj.Children.Count > 0)
                {
                    result.Add(obj);
                }
            }
            return result;
        }


        /// <summary>
        /// 获取用户组 未绑定的数据权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="controllerId"></param>
        /// <returns></returns>
        public List<ConditionModel> GetUserGroupNotBindConditionValues(string userGroupId, string controllerId)
        {
            //获取用户已经绑定的

            List<string> exists = _context.SysUserDataCondition.Where(e => e.UserGroupId == userGroupId && e.ControllerId == controllerId).Select(e => e.ConditionValue).ToList();

            //获取所有权限值
            List<ConditionModel> allvalues = GetAllConditionValues();

            //筛选未绑定的
            List<ConditionModel> result = new List<ConditionModel>();
            foreach (ConditionModel obj in allvalues)
            {
                obj.Children = AcquisitionGetValue(obj.Children, exists, false);
                if (obj.Children.Count > 0)
                {
                    result.Add(obj);
                }
            }
            return result;
        }

        public object GetUserRoutes(string userName, string userGroupName)
        {
            //用户
            var query = from a in _context.SysUserInfo
                        where a.UserAccount != "admin" && string.IsNullOrWhiteSpace(userName) || a.UserName.Contains(userName)
                        join b in _context.SysModuleUserRelation

                        on a.UserId equals b.UserId

                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()

                        join c in _context.SysModuleRoute on b_ifnull.ModuleId equals c.ModuleId

                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()

                        join d in _context.SysControllerRoute on c_ifnull.ControllerRouteId equals d.SysControllerRouteId

                        into d_temp
                        from d_ifnull in d_temp.DefaultIfEmpty()

                        join e in _context.SysUserDataCondition on new { userid = a.UserId, controllerid = d_ifnull.SysControllerRouteId }
                        equals new { userid = e.UserId, controllerid = e.ControllerId }

                        into e_temp
                        from e_ifnull in e_temp.DefaultIfEmpty()
                        select new
                        {
                            UserId=  a.UserId,
                            User_Name=  a.UserName,

                            ControllerIfNull = d_ifnull == null,
                            ControllerId = d_ifnull == null ? null : d_ifnull.SysControllerRouteId,
                            ControllerAlias = d_ifnull == null ? null : d_ifnull.ControllerAlias,
                            ControllerSort = d_ifnull == null ? 0 : d_ifnull.SortValue,

                            ConditionIfNull = e_ifnull == null,
                            ConditionId = e_ifnull == null ? null : e_ifnull.ConditionId,
                            ConditionValue = e_ifnull == null ? null : e_ifnull.ConditionValue,
                            ConditionName = e_ifnull == null ? null : e_ifnull.ConditionName,
                            UserConditionId = e_ifnull == null ? null : e_ifnull.Id,
                            ConditionSort = e_ifnull == null ? 0 : e_ifnull.SortValue,
                        };

            //用户组
            var query1 = from a in _context.SysUserGroup
                         where string.IsNullOrWhiteSpace(userGroupName) || a.UserGroupName.Contains(userGroupName)
                         join b in _context.SysModuleUserRelation

                         on a.UserGroupId equals b.UserGroupId

                         into b_temp
                         from b_ifnull in b_temp.DefaultIfEmpty()

                         join c in _context.SysModuleRoute on b_ifnull.ModuleId equals c.ModuleId

                         into c_temp
                         from c_ifnull in c_temp.DefaultIfEmpty()

                         join d in _context.SysControllerRoute on c_ifnull.ControllerRouteId equals d.SysControllerRouteId

                         into d_temp
                         from d_ifnull in d_temp.DefaultIfEmpty()

                         join e in _context.SysUserDataCondition on new { usergroupId = a.UserGroupId, controllerid = d_ifnull.SysControllerRouteId }
                         equals new { usergroupId = e.UserGroupId, controllerid = e.ControllerId }

                         into e_temp
                         from e_ifnull in e_temp.DefaultIfEmpty()
                         select new
                         {
                             UserGroupId = a.UserGroupId,
                             a.UserGroupName,
                             a.CreationDate,

                             ControllerIfNull = d_ifnull == null,
                             ControllerId = d_ifnull == null ? null : d_ifnull.SysControllerRouteId,
                             ControllerAlias = d_ifnull == null ? null : d_ifnull.ControllerAlias,
                             ControllerSort = d_ifnull == null ? 0 : d_ifnull.SortValue,

                             ConditionIfNull = e_ifnull == null,
                             ConditionId = e_ifnull == null ? null : e_ifnull.ConditionId,
                             ConditionValue = e_ifnull == null ? null : e_ifnull.ConditionValue,
                             ConditionName = e_ifnull == null ? null : e_ifnull.ConditionName,
                             UserConditionId = e_ifnull == null ? null : e_ifnull.Id,
                             ConditionSort = e_ifnull == null ? 0 : e_ifnull.SortValue,
                         };
            //获取所有的权限值 树形结构

            List<ConditionModel> allvalues = GetAllConditionValues();

            var data = query.OrderBy(e => e.UserId).ToList().GroupBy(e => e.UserId).Select(user => new
            {
                id = user.Key,
                label = user.First().User_Name,
                children = user.Where(cw => !cw.ControllerIfNull).GroupBy(c => c.ControllerId).Select(controller => new
                {
                    id = controller.Key,
                    label = controller.First().ControllerAlias,
                    isController = true,
                    userId = user.Key,
                    sort = controller.First().ControllerSort,
                    children = controller.Where(cd => !cd.ConditionIfNull).OrderByDescending(od => od.ConditionSort).GroupBy(condition => condition.ConditionId).Select(co => new
                    {
                        id = co.Key,
                        label = co.First().ConditionName,
                        children = AcquisitionGetValue(allvalues.FirstOrDefault(e => e.Id == co.Key)?.Children, co.Select(e => new MathModel { ConditionValue = e.ConditionValue, UserConditionId = e.UserConditionId }).ToList(), true)
                    })
                }).OrderByDescending(e => e.sort)
            });

            var data2 = query1.OrderBy(e => e.CreationDate).ToList().GroupBy(e => e.UserGroupId).Select(user => new
            {
                id = user.Key,
                label = user.First().UserGroupName,
                children = user.Where(cw => !cw.ControllerIfNull).GroupBy(c => c.ControllerId).Select(controller => new
                {
                    id = controller.Key,
                    label = controller.First().ControllerAlias,
                    isController = true,
                    userGroupId = user.Key,
                    sort = controller.First().ControllerSort,
                    children = controller.Where(cd => !cd.ConditionIfNull).OrderByDescending(od => od.ConditionSort).GroupBy(condition => condition.ConditionId).Select(co => new
                    {
                        id = co.Key,
                        label = co.First().ConditionName,
                        children = AcquisitionGetValue(allvalues.FirstOrDefault(e => e.Id == co.Key)?.Children, co.Select(e => new MathModel { ConditionValue = e.ConditionValue, UserConditionId = e.UserConditionId }).ToList(), true)
                    })
                }).OrderByDescending(e => e.sort)
            });
            return new { users = data, usergroups = data2 };
        }

        /// <summary>
        /// 获取当前用户 在该控制器下所拥有的数据权限
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="controllerId"></param>
        /// <param name="conditionId">若该值不为null,则返回该值下的数据权限，否则返回该用户下的所有数据权限</param>
        /// <returns>返回树结构</returns>
        public List<ConditionModel> GetCurrentUserConditionTree(string currentUserId, string controllerId, string conditionId = null)
        {
            var allvalue = GetAllConditionValues();
            if (_context.SysUserInfo.FirstOrDefault(e => e.UserId == currentUserId && e.UserAccount == "admin") != null) return allvalue.Where(e => e.Id == conditionId || string.IsNullOrWhiteSpace(conditionId)).ToList();


            var groupids = _context.SysUserGroupRelation.Where(e => e.UserId == currentUserId).Select(e => e.UserGroupId).ToList();

            var conditions = _context.SysUserDataCondition.Where(e => (groupids.Contains(e.UserGroupId) || e.UserId == currentUserId)
              && ((string.IsNullOrWhiteSpace(conditionId) || e.ConditionId == conditionId) && e.ControllerId == controllerId))
            .GroupBy(g => g.ConditionId).Select(s => new ConditionModel
            {
                Id = s.Key,
                Label = s.First().ConditionName,
                Children = AcquisitionGetValue(allvalue.First(a => a.Id == s.Key).Children, s.Select(v => v.ConditionValue).ToList(), true)
            });
            return conditions.ToList();
            //获取用户该控制器下的所有condtionoid 与value
        }

       

        /// <summary>
        /// 用于验证当前用户是否拥有该数据查询权限
        /// </summary>
        /// <param name="conditionId"></param>
        /// <param name="code"></param>
        /// <param name="controllerId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public bool DataConditionMathch(string conditionId, string code, string controllerId, string currentUserId)
        {
            if (_context.SysUserInfo.FirstOrDefault(e => e.UserId == currentUserId && e.UserAccount == "admin") != null)
            {
                //管理员默认拥有所有权限
                return true;
            }
            //获取该用户所有用户组id
            var groupids = _context.SysUserGroupRelation.Where(e => e.UserId == currentUserId).Select(e => e.UserGroupId);
            var result = _context.SysUserDataCondition.FirstOrDefault(e =>
              (e.UserId == currentUserId || groupids.Contains(e.UserGroupId))
              && e.ConditionId == conditionId && e.ControllerId == controllerId && e.ConditionValue == code
            );
            return result != null;
        }

        ///// <summary>
        ///// 获取当前用户 在该控制器下所拥有的数据权限
        ///// </summary>
        ///// <param name="currentUserId"></param>
        ///// <param name="controllerId"></param>
        ///// <param name="conditionId">若该值不为null,则返回该值下的数据权限，否则返回该用户下的所有数据权限</param>
        ///// <returns>返回List</returns>
        //public List<ConditionValueObject> GetCurrentUserConditionList(string currentUserId, string controllerId, string conditionId = null)
        //{
        //    if (_context.SysUserInfo.FirstOrDefault(e => e.UserId == currentUserId && e.UserAccount == "admin") != null)
        //    {
        //        var list = GetConditionRawValueList();
        //        return list.Where(e => e.ConditionId == conditionId || string.IsNullOrWhiteSpace(conditionId)).Select(c => new ConditionValueObject
        //        {
        //            ConditionId = c.ConditionId,
        //            ConditionName = c.ConditionName,
        //            ConditionValue = c.Code,
        //        }).Distinct().ToList();
        //    }

        //    //获取用户所属的用户组
        //    var groupsid = _context.SysUserGroupRelation.Where(e => e.UserId == currentUserId).Select(e => e.UserGroupId);
        //    var group_data = _context.UserDataCondition
        //        .Where(e => groupsid.Contains(e.UserGroupId)
        //    && e.ControllerId == controllerId
        //    && (string.IsNullOrWhiteSpace(conditionId) || e.ConditionId == conditionId))
        //    .Select(c => new ConditionValueObject
        //    {
        //        ConditionId = c.ConditionId,
        //        ConditionName = c.ConditionName,
        //        ConditionValue = c.ConditionValue
        //    }).ToList();
        //    var data = _context.UserDataCondition
        //        .Where(e => e.UserId == currentUserId
        //    && e.ControllerId == controllerId
        //    && (string.IsNullOrWhiteSpace(conditionId) || e.ConditionId == conditionId))
        //    .Select(c => new ConditionValueObject
        //    {
        //        ConditionId = c.ConditionId,
        //        ConditionName = c.ConditionName,
        //        ConditionValue = c.ConditionValue,
        //    }).ToList();
        //    group_data.AddRange(data);
        //    return group_data.Distinct().ToList();
        //}

        private List<ConditionValue> AcquisitionGetValue(List<ConditionValue> ConditionValues, List<string> codes, bool isbind)
        {
            List<ConditionValue> result = new List<ConditionValue>();
            if (ConditionValues.Count <= 0)
            {
                return result;
            }

            foreach (ConditionValue obj in ConditionValues)
            {


                if (codes.Contains(obj.Id) == isbind)
                {
                    ConditionValue c = new ConditionValue()
                    {
                        Id = obj.Id,
                        ConditionId = obj.ConditionId,
                        ParentCode = obj.ParentCode,
                        ConditionName = obj.ConditionName,
                        SortValue = obj.SortValue
                    };
                    if (obj.Children.Count > 0)
                    {
                        c.Children = AcquisitionGetValue(obj.Children, codes, isbind);
                    }
                    result.Add(c);
                }
                else
                {
                    if (obj.Children.Count > 0)
                    {
                        List<ConditionValue> childrens = AcquisitionGetValue(obj.Children, codes, isbind);
                        if (childrens.Count > 0)
                        {
                            result.AddRange(childrens);
                        }
                    }
                }
            }
            return result;

        }
        private List<ConditionValue> AcquisitionGetValue(List<ConditionValue> ConditionValues, List<MathModel> codes, bool isbind)
        {
            List<ConditionValue> result = new List<ConditionValue>();
            if (ConditionValues == null || ConditionValues.Count <= 0)
            {
                return result;
            }

            foreach (ConditionValue obj in ConditionValues)
            {
                MathModel code = codes.FirstOrDefault(e => e.ConditionValue == obj.Id);
                if (code != null)
                {
                    ConditionValue c = new ConditionValue()
                    {
                        Id = obj.Id,
                        ConditionId = obj.ConditionId,
                        ParentCode = obj.ParentCode,
                        ConditionName = obj.ConditionName,
                        UserConditionId = code.UserConditionId,
                        SortValue = obj.SortValue,
                    };
                    if (obj.Children.Count > 0)
                    {
                        c.Children = AcquisitionGetValue(obj.Children, codes, isbind);
                    }
                    result.Add(c);
                }
                else
                {
                    if (obj.Children.Count > 0)
                    {
                        List<ConditionValue> childrens = AcquisitionGetValue(obj.Children, codes, isbind);
                        if (childrens.Count > 0)
                        {
                            result.AddRange(childrens);
                        }
                    }
                }

                //if (codes.Contains(obj.Id) == isbind)
                //{
                //    var c = new ConditionValue()
                //    {
                //        Id = obj.Id,
                //        ConditionId = obj.ConditionId,
                //        ParentCode = obj.ParentCode,
                //        ConditionName = obj.ConditionName
                //    };
                //    if (obj.Children.Count > 0)
                //    {
                //        c.Children = AcquisitionGetValue(obj.Children, codes, isbind);
                //    }
                //    result.Add(c);
                //}
                //else
                //{
                //    if (obj.Children.Count > 0)
                //    {
                //        var childrens = AcquisitionGetValue(obj.Children, codes, isbind);
                //        if (childrens.Count > 0) result.AddRange(childrens);
                //    }
                //}
            }
            return result;

        }



        /// <summary>
        /// 将数据权限值 构建成一颗树
        /// </summary>
        private ConditionValue BuilderValuesTree(List<ConditionRawValue> data, ConditionRawValue parent)
        {
            ConditionValue conditionValue = new ConditionValue()
            {
                ConditionId = parent.ConditionId,
                ParentCode = parent.ParentCode,
                Id = parent.Code,
                ConditionName = parent.ConditionName,
                SortValue = parent.SortValue
            };
            //移除自身
            List<ConditionRawValue> childs = data.Where(e => e.ParentCode == parent.Code).ToList();
            if (childs.Count == 0)
            {
                return conditionValue;
            }
            ConditionRawValue parent_entity = data.FirstOrDefault(e => e.Code == parent.Code);
            if (parent_entity != null)
            {
                data.Remove(parent_entity);
            }
            foreach (ConditionRawValue child in childs)
            {
                conditionValue.Children.Add(BuilderValuesTree(data.Where(e => e.ParentCode == child.Code).ToList(), child));
            }
            return conditionValue;
        }
    }


    public class MathModel
    {
        public string UserConditionId { get; set; }
        public string ConditionValue { get; set; }
    }
    public class ConditionModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public bool MasterSlaveFlag { get; set; }
        public List<ConditionValue> Children { get; set; }
        public ConditionModel()
        {
            Children = new List<ConditionValue>();
        }
    }
    public class ConditionValueObject
    {
        public string ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string ConditionValue { get; set; }
    }

    public class ConditionValue
    {
        public string UserConditionId { get; set; }//用于删除绑定记录
        public string ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string ParentCode { get; set; }
        public string Id { get; set; }
        public string Label => Id;
        public bool IsEdit => true;
        public int SortValue { get; set; }
        public List<ConditionValue> Children { get; set; }
        public ConditionValue()
        {
            Children = new List<ConditionValue>();
        }
    }

    public class ConditionRawValue
    {
        public bool MasterSlaveFlag { get; set; }
        public string ConditionId { get; set; }
        public string ConditionName { get; set; }
        public string ConditionValueDesc { get; set; }
        public string TableName { get; set; }
        public string ParentCode { get; set; }
        public string Code { get; set; }
        public int SortValue { get; set; }
    }
}
