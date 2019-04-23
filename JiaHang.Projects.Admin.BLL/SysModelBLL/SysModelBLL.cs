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

namespace JiaHang.Projects.Admin.BLL.SysModelBLL
{
    public class SysModelBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysModelBLL(DAL.EntityFramework.DataContext context)
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
            var query = from a in _context.SysModelInfo
                        join b in _context.SysModelGroup
                        on a.ModelGroupId equals b.ModelGroupId
                        orderby a.SortKey descending
                        orderby b.SortKey   descending
                        
                        select new
                        {
                            a.ModelId,
                            a.ModelName,
                            a.ModelUrl,
                            OutUrlFlag = a.OutUrlFlag.ToString(),
                            a.SortKey,
                            CreationDate = a.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            ModelGroupName =   b.ModelGroupName,
                            ModelGroupId = b.ModelGroupId,
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
        public async Task<FuncResult> Update(SysModelInfo model, string currentUserId)
        {
            //检查是否重名
            if (string.IsNullOrWhiteSpace(model.ModelGroupId)) {
                return new FuncResult() { IsSuccess = false, Message = "所属模块组不能为空" };
            }
            var entity = _context.SysModelInfo.FirstOrDefault(e => e.ModelId == model.ModelId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块id错误!" };
            }
            if (_context.SysModelInfo.Count(e => e.ModelName == model.ModelName && e.ModelId != model.ModelId) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }

            entity.ModelName = model.ModelName;
            entity.ModelUrl = model.ModelUrl;
            entity.OutUrlFlag = model.OutUrlFlag;
            entity.SortKey = model.SortKey;
            entity.ModelGroupId = model.ModelGroupId;

            entity.LastUpdateDate = DateTime.Now;
            entity.LastUpdatedBy = currentUserId;


            _context.SysModelInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = model, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            var entity = await _context.SysModelInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块ID不存在!" };
            }

            //将该模块下的所有绑定用户 也进行删除   
            var deletes = _context.SysOperRightInfo.Where(e => e.ModelId == id);
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
            _context.SysModelInfo.Update(entity);
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
                    return new FuncResult() { IsSuccess = false, Message = $"删除模块[{entity.ModelName}]时发生预料之外的错误,请重试" };
                }
            }

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        public async Task<FuncResult> Add(SysModelInfo model, string currentUserId)
        {

            //模块名称不能重复
            if (_context.SysModelInfo.FirstOrDefault(e => e.ModelName == model.ModelName) != null)
            {
                return new FuncResult() { IsSuccess = false, Message = "模块名不能重复!" };
            }
            if (string.IsNullOrWhiteSpace(model.ModelGroupId)) {
                return new FuncResult() { IsSuccess = false, Message = "所属模块组不能为空" };
            }

            model.ModelId = Guid.NewGuid().ToString();
            model.LastUpdatedBy = currentUserId;
            model.LastUpdateDate = DateTime.Now;
            model.CreatedBy = currentUserId;
            model.CreationDate = DateTime.Now;

            await _context.SysModelInfo.AddAsync(model);

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
                    return new FuncResult() { IsSuccess = false, Message = ex.Message };
                }
            }
            return new FuncResult() { IsSuccess = true, Content = model, Message = "添加成功" };
        }

        /// <summary>
        /// 获取二级模块组
        /// </summary>
        /// <returns></returns>
        public FuncResult GetParentModule()
        {
            var data = _context.SysModelGroup
                //.Where(e => string.IsNullOrWhiteSpace( e.ParentId))
                .Select(e => new
            {
                e.ModelGroupId,
                e.ModelGroupName
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }

    }
}
