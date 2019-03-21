using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceGroup.RequestModel;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace JiaHang.Projects.Admin.BLL.DcsServiceGroupBLL
{
    public class DcsServiceGroupBLL
    {
        private readonly DataContext _context;
        public DcsServiceGroupBLL(DataContext datacontext)
        {
            _context = datacontext;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchDesServiceGroup model)
        {
            IOrderedQueryable<DcsServiceGroup> query = _context.DcsServiceGroup
                .OrderByDescending(o => o.CreationDate);

            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(s => new
            {
                //需要的字段
                Service_Group_Id = s.ServiceGroupId,
                Service_Group_Code = s.ServiceGroupCode,
                Service_Group_Name = s.ServiceGroupName
            });

            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }

        /// <summary>
        /// 查找一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            DcsServiceGroup entity = await _context.DcsServiceGroup.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(DcsServiceGroupModel model, string currentuserid)
        {
            DcsServiceGroup entity = new DcsServiceGroup()
            {
                ServiceGroupId = model.ServiceGroupId,
                ServiceGroupCode = model.ServiceGroupCode,
                ServiceGroupName = model.ServiceGroupName,
                ImageUrl = model.ImageUrl,

                CreatedBy = currentuserid,
                CreationDate = DateTime.Now,
                LastUpdatedBy = currentuserid,
                LastUpdateDate = DateTime.Now
            };

            await _context.DcsServiceGroup.AddAsync(entity);

            using (IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = ex.Message };
                }

            }

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string[] ids, string currentuserid)
        {
            IQueryable<DcsServiceGroup> entitys = _context.DcsServiceGroup.Where(f => ids.Contains(f.ServiceGroupId));
            if (entitys != null && ( entitys.Count() != ids.Length ))
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }

            foreach (DcsServiceGroup obj in entitys)
            {
                obj.DeletedBy = currentuserid;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
            }

            using (IDbContextTransaction trans  = _context.Database.BeginTransaction())
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

            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{ids.Length}条记录" };
        }

        /// <summary>
        /// 更新数据 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, DcsServiceGroupModel model, string currentuserid)
        {
            FuncResult result = new FuncResult();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    result.IsSuccess = false;
                    result.Message = "主键参数为空";
                    return result;
                }
                DcsServiceGroup entity = await _context.DcsServiceGroup.FindAsync(id);

                if (entity == null)
                {
                    result.IsSuccess = false;
                    result.Message = "用户ID错误";
                    return result;
                }

                entity.ServiceGroupCode = model.ServiceGroupCode;
                entity.ServiceGroupName = model.ServiceGroupName;
                entity.ImageUrl = model.ImageUrl;

                entity.LastUpdatedBy = currentuserid;
                entity.LastUpdateDate = DateTime.Now;

                _context.DcsServiceGroup.Update(entity);
                await _context.SaveChangesAsync();

                result.IsSuccess = true;
                result.Content = entity;
                result.Message = "修改更新";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }
    }
}
