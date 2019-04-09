using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.DcsCustomerBLL
{
    public class DcsCustomerBLL
    {
        private readonly DataContext _context;
        public DcsCustomerBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(int pageSize, int currentPage, string customerName, string customerMobile)
        {
            var query = _context.DcsCustomerInfo
                .Where(e => string.IsNullOrWhiteSpace(customerName) || e.CustomerName.Contains(customerName))
                .Where(e => string.IsNullOrWhiteSpace(customerMobile) || e.ContactMobile.Contains(customerMobile))
                        .OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(pageSize * currentPage).Take(pageSize);
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, DcsCustomerInfo model, string currentUserId)
        {
            //var entity = await _context.DcsCustomerInfo.FindAsync(id);
            if (string.IsNullOrWhiteSpace(id))
            {
                return new FuncResult() { IsSuccess = false, Message = "客户ID错误!" };
            }

            model.LastUpdatedBy = currentUserId;
            model.LastUpdateDate = DateTime.Now;


            try
            {
                _context.DcsCustomerInfo.Update(model);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };

            }
            return new FuncResult() { IsSuccess = true, Content = model, Message = "修改成功" };
        }

        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            var entity = await _context.DcsCustomerInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.DcsCustomerInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<DcsCustomerInfo> entitys = _context.DcsCustomerInfo.Where(e => ids.Contains(e.CustomerId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (var obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.DcsCustomerInfo.Update(obj);
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
        public async Task<FuncResult> Add(DcsCustomerInfo model, string currentUserId)
        {

            model.CustomerId = Guid.NewGuid().ToString();
            model.CreatedBy = currentUserId;
            model.CreationDate = DateTime.Now;
            model.LastUpdatedBy = currentUserId;
            model.LastUpdateDate = DateTime.Now;
            await _context.DcsCustomerInfo.AddAsync(model);

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



    }
}
