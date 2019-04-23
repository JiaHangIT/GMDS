using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysModule;
using JiaHang.Projects.Admin.Model.SysModule.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysModelGroupBLL
{
    public class SysModelGroupBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysModelGroupBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(int pageSize, int currentPage)
        {
            var query = from a in _context.SysModelGroup
                        join b in _context.SysModelGroup.Where(e => string.IsNullOrWhiteSpace(e.ParentId))
                        on a.ParentId equals b.ModelGroupId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        orderby a.SortKey descending
                        select new
                        {
                            a.ModelGroupId,
                            a.ModelGroupName,
                            a.ModelGroupUrl,
                            OutUrlFlag = a.OutUrlFlag.ToString(),
                            a.SortKey,
                            CreationDate = a.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            ModelGroupParentName = b_ifnull == null ? "暂未父模块组" : b_ifnull.ModelGroupName,
                            ParentId = b_ifnull == null ? "" : b_ifnull.ModelGroupId,
                        };

            int total = query.Count();
            var data = query.Skip(pageSize * currentPage).Take(pageSize);

            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(SysModelGroup model, string currentUserId)
        {
            SysModelGroup entity = entity = _context.SysModelGroup.FirstOrDefault(e => e.ModelGroupId == model.ModelGroupId);

            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块组id错误!" };
            }
            if (_context.SysModelGroup.Count(e => e.ModelGroupName == model.ModelGroupName && e.ModelGroupId != model.ModelGroupId) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块组名不能重复!" };
            }

            if (!string.IsNullOrWhiteSpace(model.ParentId))
            {// 该模块组变为二级模块组时
                var childs = _context.SysModelGroup.Where(e => e.ParentId == model.ModelGroupId);
                await Task.Run(() =>
                {
                    foreach (var child in childs)
                    {
                        child.ParentId = null;
                        child.LastUpdateDate = DateTime.Now;
                        child.LastUpdatedBy = currentUserId;
                        _context.SysModelGroup.Update(child);
                    }
                });
            }
            entity.ModelGroupName = model.ModelGroupName;
            entity.ModelGroupUrl = model.ModelGroupUrl;
            entity.OutUrlFlag = model.OutUrlFlag;
            entity.SortKey = model.SortKey;
            entity.ParentId = model.ParentId;

            entity.LastUpdateDate = DateTime.Now;
            entity.LastUpdatedBy = currentUserId;

            _context.SysModelGroup.Update(entity);
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
                    return new FuncResult() { IsSuccess = false, Message = ex.Message };
                }
            }
            return new FuncResult() { IsSuccess = true, Content = model, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            var entity = await _context.SysModelGroup.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块ID不存在!" };
            }

            //将该模块组下的所有模块 所有绑定用户 也进行删除   
            var mdels = _context.SysModelInfo.Where(e => e.ModelGroupId == id);
            await Task.Run(() =>
            {
                foreach (var mdel in mdels)
                {
                    mdel.DeleteFlag = 1;
                    mdel.DeleteDate = DateTime.Now;
                    mdel.DeleteBy = currentUserId;
                }
            });
            _context.SysModelInfo.UpdateRange(mdels);

            var deletes = _context.SysOperRightInfo.Where(e => mdels.Select(c => c.ModelId).Contains(e.ModelId));
            await Task.Run(() =>
            {
                foreach (var del in deletes)
                {
                    del.DeleteFlag = 1;
                    del.DeleteDate = DateTime.Now;
                    del.DeleteBy = currentUserId;
                }
            });
            _context.SysOperRightInfo.UpdateRange(deletes);
            entity.DeleteBy = currentUserId;
            entity.DeleteFlag = 1;
            entity.DeleteDate = DateTime.Now;
            _context.SysModelGroup.Update(entity);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = $"删除模块[{entity.ModelGroupName}]时发生预料之外的错误,请重试" };
                }
            }

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        public async Task<FuncResult> Add(SysModelGroup model, string currentUserId)
        {

            //模块名称不能重复
            if (_context.SysModelGroup.FirstOrDefault(e => e.ModelGroupName == model.ModelGroupName) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }


            model.ModelGroupId = Guid.NewGuid().ToString();
            model.LastUpdatedBy = currentUserId;
            model.LastUpdateDate = DateTime.Now;
            model.CreatedBy = currentUserId;
            model.CreationDate = DateTime.Now;

            await _context.SysModelGroup.AddAsync(model);

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
            return new FuncResult() { IsSuccess = true, Content = model, Message = "添加成功" };
        }

        /// <summary>
        /// 获取带选择的父级模块路径
        /// </summary>
        /// <returns></returns>
        public FuncResult GetParentModule(string id)
        {
            var data = _context.SysModelGroup.Where(e => string.IsNullOrWhiteSpace(e.ParentId) && e.ModelGroupId != id).Select(e => new
            {
                ParentId=e.ModelGroupId,
                e.ModelGroupName
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }

    }
}
