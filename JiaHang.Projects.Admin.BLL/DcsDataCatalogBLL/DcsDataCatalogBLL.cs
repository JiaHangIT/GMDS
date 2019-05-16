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
using JiaHang.Projects.Admin.Model.DcsDataCatalog.RequestModel;
namespace JiaHang.Projects.Admin.BLL.DcsDataCatalogBLL
{
  public  class DcsDataCatalogBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public DcsDataCatalogBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        public FuncResult Select(int pageSize, int currentPage, string dataCatalogName)
        {
            var querys = from a in _context.DcsDataCatalog
                         where (
                            (string.IsNullOrWhiteSpace(dataCatalogName) || a.DataCatalogName.Contains(dataCatalogName))
                            )
                         join b in _context.SysDatasourceInfo on a.DataCatalogId equals b.DataCatalogId
                          into a_temp
                         from a_ifnull in a_temp.DefaultIfEmpty() orderby a.CreationDate descending
                         select new
                         {
                            DataCatalogId=a.DataCatalogId,
                             DataCatalogCode=a.DataCatalogCode,
                             DataCatalogName= a.DataCatalogName,
                             ParentId=a.ParentId,
                             ParentIdTree= a.ParentIdTree,
                             DataCountSelf= a.DataCountSelf,
                             DataCountTree=a.DataCountTree,
                             ImageUrl=a.ImageUrl,
                             CreatedBy = a.CreatedBy,
                             DatasourceName=a_ifnull.DatasourceName,
                             CreationDate = a.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")
                         };
            int total = querys.Count();
            var data = querys.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(DcsDataCatalogModel model, string userId)
        {
            DcsDataCatalog entity = new DcsDataCatalog
            {
            DataCatalogId = Guid.NewGuid().ToString("N"),
            DataCatalogCode=model.DataCatalogCode,
            DataCatalogName=model.DataCatalogName,
            ParentId=model.ParentId,
            ParentIdTree=model.ParentIdTree,
            DataCountSelf=model.DataCountSelf,
            DataCountTree=model.DataCountTree,
            ImageUrl=model.ImageUrl,
            CreatedBy = userId,
            CreationDate = DateTime.Now
            };
            await _context.DcsDataCatalog.AddAsync(entity);

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
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, DcsDataCatalogModel model, string currentUserId)
        {
            DcsDataCatalog entity = await _context.DcsDataCatalog.FindAsync(id);

            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据目录ID错误!" };
            }
            entity.DataCatalogCode = model.DataCatalogCode;
            entity.DataCatalogName = model.DataCatalogName;
            entity.ParentId = model.ParentId;
            entity.ParentIdTree = model.ParentIdTree;
            entity.DataCountSelf = model.DataCountSelf;
            entity.DataCountTree = model.DataCountTree;
            entity.ImageUrl = model.ImageUrl;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            _context.DcsDataCatalog.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            DcsDataCatalog entity = await _context.DcsDataCatalog.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据目录ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.DcsDataCatalog.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<DcsDataCatalog> entitys = _context.DcsDataCatalog.Where(e => ids.Contains(e.DataCatalogId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (DcsDataCatalog obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.DcsDataCatalog.Update(obj);
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
    }
}
