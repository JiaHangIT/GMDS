using System;
using System.Collections.Generic;
using System.Text;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Enumerations;
using JiaHang.Projects.Admin.Model.SysConnection.RequestModel;
using System.Threading.Tasks;
using System.Linq;
using JiaHang.Projects.Admin.Model.SysDataBaseConnection.RequestModel;

namespace JiaHang.Projects.Admin.BLL.SysConnectionBLL
{
   public class SysDataBaseConnectionBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysDataBaseConnectionBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysConnection model)
        {
            var querys = from a in _context.SysConnectionInfo
                         where (
                            (string.IsNullOrWhiteSpace(model.ConnectionName) || a.ConnectionName.Contains(model.ConnectionName))
                             && (string.IsNullOrWhiteSpace(model.DataBaseTypeId) || a.DatabaseTypeId.ToString().Contains(model.DataBaseTypeId))
                             
                            )
                         join b in _context.SysDatabaseType on a.DatabaseTypeId equals b.DatabaseTypeId
                          into a_temp
                         from a_ifnull in a_temp.DefaultIfEmpty()
                         orderby a.CreationDate descending
                         select new
                         {
                             ConnectionId = a.ConnectionId,
                             ConnectionName = a.ConnectionName,
                             ConnectionString = a.ConnectionString,
                             CreatedBy = a.CreatedBy,
                             DatabaseTypeCode = a_ifnull.DatabaseTypeCode,
                             DatabaseTypeName = a_ifnull.DatabaseTypeName,
                             DatabaseTypeId = a_ifnull.DatabaseTypeId,
                             CreationDate = a.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                         };
            int total = querys.Count();
            var data = querys.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            SysConnectionInfo entity = await _context.SysConnectionInfo.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        public FuncResult ElementSelect(int pageSize, int currentPage, string connectionName, string databaseType)
        {
            var querys = from a in _context.SysConnectionInfo
                         where (
                            (string.IsNullOrWhiteSpace(connectionName) || a.ConnectionName.Contains(connectionName))
                             && (string.IsNullOrWhiteSpace(databaseType) || a.DatabaseTypeId.ToString().Contains(databaseType))

                            )
                         join b in _context.SysDatabaseType on a.DatabaseTypeId equals b.DatabaseTypeId
                          into a_temp
                         from a_ifnull in a_temp.DefaultIfEmpty()
                         orderby a.CreationDate descending
                         select new
                         {
                             ConnectionId = a.ConnectionId,
                             ConnectionName = a.ConnectionName,
                             ConnectionString = a.ConnectionString,
                             CreatedBy = a.CreatedBy,
                             DatabaseTypeCode = a_ifnull.DatabaseTypeCode,
                             DatabaseTypeName = a_ifnull.DatabaseTypeName,
                             DatabaseTypeId = a_ifnull.DatabaseTypeId,
                             CreationDate = a.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                         };
            int total = querys.Count();
            var data = querys.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };

        }
        /// <summary>
        /// 查询数据库类型
        /// </summary>
        /// <returns></returns>
        public async Task<FuncResult> SelectDatabaseType() {
            var query = from a in _context.SysDatabaseType
                        select new
                        {
                            DatabaseTypeId = a.DatabaseTypeId,
                            DatabaseTypeName = a.DatabaseTypeName
                        };
            object data = null;
            await Task.Run(() =>
            {
                data = query.ToList();
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }
        /// <summary>
        /// 修改 同时修改数据库类型和数据库连接表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SearchContentConnection model, string currentUserId)
        {
            SysConnectionInfo entity = await _context.SysConnectionInfo.FindAsync(id);
          
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "连接ID错误!" };
            }
            entity.DatabaseTypeId = model.DatabaseTypeId;
            entity.ConnectionName = model.ConnectionName;
            entity.ConnectionString = model.ConnectionString;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;
         
            _context.SysConnectionInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除单个方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            SysConnectionInfo entity = await _context.SysConnectionInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "连接ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysConnectionInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        /// <summary>
        /// 获取多个ID进行删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<SysConnectionInfo> entitys = _context.SysConnectionInfo.Where(e => ids.Contains(e.ConnectionId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysConnectionInfo obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysConnectionInfo.Update(obj);
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
        /// 添加方法
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysConnectionModel model1, string connnectionId)
        {
            SysConnectionInfo entity = new SysConnectionInfo
            {
                ConnectionId= Guid.NewGuid().ToString("N"),
                ConnectionName = model1.ConnectionName,
                ConnectionString = model1.ConnectionString,
                CreatedBy = connnectionId,
                DatabaseTypeId = model1.DatabaseTypeId,
                CreationDate = DateTime.Now,
            };
            await _context.SysConnectionInfo.AddAsync(entity);

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

    }
}
