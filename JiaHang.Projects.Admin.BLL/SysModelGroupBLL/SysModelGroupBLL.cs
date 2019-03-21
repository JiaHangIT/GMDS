using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Enumerations;
using JiaHang.Projects.Admin.Model.SysModelGroup.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysModelGroupBLL
{
    public class SysModelGroupBLL
    {
        private readonly DataContext _context;
        public SysModelGroupBLL(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysModelGroupModel model)
        {
            var query = _context.SysModelGroup;          
            int total = query.Count();
            // var data = query.Skip(model.limit * model.page).Take(model.limit);
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().OrderBy(c=>c.ModelGroupName).Select(e => new
            {

                Model_Group_Id = e.ModelGroupId,
                Model_Group_Code = e.ModelGroupCode,
                Model_Group_Name = e.ModelGroupName,
                Parent_Id = e.ParentId,
                Sort_Flag = e.SortKey,
                Enable_Flag = (e.EnableFlag==1) ? "有效" : "无效",
                Image_Url = e.ImageUrl,
                Group_Belong = "",
                Biz_sys_Code = "",
                CreationDate = e.CreationDate.ToString("yyyy-MM-dd"),
                count = new Random().NextDouble() * 100
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
            var entity = await _context.SysModelGroup.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysModelGroupModel model, string currentUserId)
        {
            var entity = await _context.SysModelGroup.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID错误!" };
            }


            entity.ModelGroupCode = model.ModelGroupCode;
            entity.ModelGroupName = model.ModelGroupName;
            entity.ParentId = model.ParentId;
            entity.SortKey = model.SortFlag;
            entity.EnableFlag = model.EnableFlag;
            entity.ImageUrl = model.ImageUrl;
            //entity.GroupBelong = model.GroupBelong;
            //entity.BizSysCode = model.BizSysCode;

            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;


            _context.SysModelGroup.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            var entity = await _context.SysUserInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysUserInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            var entitys = _context.SysModelGroup.Where(e => ids.Contains(e.ModelGroupId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (var obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysModelGroup.Update(obj);
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
        public async Task<FuncResult> Add(SysModelGroupModel model, string currentUserId)
        {
            var entity = new SysModelGroup()
            {
                ModelGroupCode = model.ModelGroupCode,
                ModelGroupName = model.ModelGroupName,
                ParentId = model.ParentId,
                SortKey = model.SortFlag,
                EnableFlag = model.EnableFlag,
                ImageUrl = model.ImageUrl,
                //GroupBelong = model.GroupBelong,
                //BizSysCode = model.BizSysCode,

                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,

                CreatedBy = currentUserId,
                CreationDate = DateTime.Now
            };
            await _context.SysModelGroup.AddAsync(entity);

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
