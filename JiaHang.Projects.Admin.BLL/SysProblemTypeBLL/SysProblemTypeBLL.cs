using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysProblemType.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysProblemTypeBLL
{
    public class SysProblemTypeBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysProblemTypeBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysProblemTypeModel model)
        {
            IOrderedQueryable<SysProblemType> query = _context.SysProblemType.
                Where(a =>
                (
                (string.IsNullOrWhiteSpace(model.Problem_Type_Name) || a.ProblemTypeName.Contains(model.Problem_Type_Name))
                )
                ).OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(e => new
            {

                Problem_Type_Id = e.ProblemTypeId,
                Problem_Type_Name = e.ProblemTypeName,
                Creation_Date = e.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")

            });
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public FuncResult ElemeSelect(int pageSize, int currentPage, string problemTypeName)
        {
            IOrderedQueryable<SysProblemType> query = _context.SysProblemType.
                Where(a =>
                (
                (string.IsNullOrWhiteSpace(problemTypeName) || a.ProblemTypeName.Contains(problemTypeName))
                )
                ).OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(pageSize * currentPage).Take(pageSize).ToList().Select(e => new
            {

                Problem_Type_Id = e.ProblemTypeId,
                Problem_Type_Name = e.ProblemTypeName,
                Creation_Date = e.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss")

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
            SysProblemType entity = await _context.SysProblemType.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysProblemTypeModel model, string currentUserId)
        {
            SysProblemType entity = await _context.SysProblemType.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "帮助类型ID错误!" };
            }

            entity.ProblemTypeName = model.ProblemTypeName;
            entity.LastUpdateDate = DateTime.Now;
            entity.LastUpdatedBy = currentUserId;

            _context.SysProblemType.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            SysProblemType entity = await _context.SysProblemType.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "问题类型ID不存在!" };
            }
            entity.DeleteFlag = 1;
            //entity.DeleteFlag = true;

            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysProblemType.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<SysProblemType> entitys = _context.SysProblemType.Where(e => ids.Contains(e.ProblemTypeId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysProblemType obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                //obj.DeleteFlag = true;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysProblemType.Update(obj);
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
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysProblemTypeModel model, string currentUserId)
        {
            SysProblemType entity = new SysProblemType
            {
                ProblemTypeId = Guid.NewGuid().ToString(),
                ProblemTypeName = model.ProblemTypeName,

                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                CreatedBy = currentUserId,
                DeleteBy = "00000000000000000000000000000000"
            };
            await _context.SysProblemType.AddAsync(entity);
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
