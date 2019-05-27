using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysDimInfo.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace JiaHang.Projects.Admin.BLL.SysDimInfoBLL
{
    public class SysDimInfoBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysDimInfoBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysDimInfo model)
        {
            try
            {


                int total = _context.SysDimInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Dim_Name) || a.DimName.Contains(model.Dim_Name))
                        && (string.IsNullOrWhiteSpace(model.Created_By) || a.CreatedBy == (model.Created_By))
                        && (a.DeleteFlag != 1)
                        )).Count();

                var result = _context.SysDimInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Dim_Name) || a.DimName.Contains(model.Dim_Name))
                        && (string.IsNullOrWhiteSpace(model.Created_By) || a.CreatedBy == (model.Created_By))
                        && (a.DeleteFlag != 1)
                        )).Skip(model.limit * model.page).Take(model.limit).ToList();
                var data = result.Select(e => new
                {
                    dimId=e.DimId,
                    dimTypecode = e.DimTypeCode,
                    dimName = e.DimName ?? "",
                    dimValue=e.DimValue??"",
                    creationDate = e.CreationDate != null ? Convert.ToDateTime(e.CreationDate).ToString("yyyy-MM-dd") : "",
                    createdBy = e.CreatedBy ?? "",
                    lastUpdateDate = e.LastUpdateDate != null ? Convert.ToDateTime(e.LastUpdateDate).ToString("yyyy-MM-dd") : "",
                    lastUpdatedBy = e.LastUpdatedBy ?? "",

                });
                return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = true, Message = "数据错误" };
                throw ex;
            }

        }
        public FuncResult ElemeSelect(int pageSize, int currentPage, string dimName, string createdBy)
        {
            try
            {


                int total = _context.SysDimInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(dimName) || a.DimName.Contains(dimName))
                        && (string.IsNullOrWhiteSpace(createdBy) || a.CreatedBy == (createdBy))
                        && (a.DeleteFlag != 1)
                        )).Count();

                var result = _context.SysDimInfo.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(dimName) || a.DimName.Contains(dimName))
                        && (string.IsNullOrWhiteSpace(createdBy) || a.CreatedBy == (createdBy))
                        && (a.DeleteFlag != 1)
                        )).OrderByDescending(a => a.CreationDate).Skip(pageSize * currentPage).Take(pageSize).ToList();
                var data = result.Select(e => new
                {
                    dimId = e.DimId,
                    dimTypecode = e.DimTypeCode,
                    dimName = e.DimName ?? "",
                    dimValue = e.DimValue ?? "",
                    creationDate = e.CreationDate != null ? Convert.ToDateTime(e.CreationDate).ToString("yyyy-MM-dd") : "",
                    createdBy = e.CreatedBy ?? "",
                    lastUpdateDate = e.LastUpdateDate != null ? Convert.ToDateTime(e.LastUpdateDate).ToString("yyyy-MM-dd") : "",
                    lastUpdatedBy = e.LastUpdatedBy ?? "",

                });
                return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = true, Message = "数据错误" };
                throw ex;
            }

        }
        /// <summary>
        /// 查询单个
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<FuncResult> Details(string DimTypeCode)
        {
            var entity = await _context.SysDimInfo.FirstOrDefaultAsync(m => m.DimId == DimTypeCode);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string DimTypeCode, SysDimInfoModel model, string currentuserId)
        {
            SysDimInfo entity = await _context.SysDimInfo.FindAsync(DimTypeCode);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }
            entity.DimTypeCode = model.Dim_Type_Code;
            entity.DimName = model.Dime_Name;
            entity.DimValue = model.Dim_Value;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            //entity.AuditFlag = model.Audit_Flag;
            //entity.AuditedDate = model.Audited_Date;          
            _context.SysDimInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DimTypeCode"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string DimTypeCode, string currentUserId)
        {
            SysDimInfo entity = await _context.SysDimInfo.FindAsync(DimTypeCode);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysDimInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] DimTypeCodes, string currentuserId)
        {
            IQueryable<SysDimInfo> entitys = _context.SysDimInfo.Where(e => DimTypeCodes.Contains(e.DimId));
            if (entitys.Count() != DimTypeCodes.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysDimInfo obj in entitys)
            {
                obj.DeleteBy = currentuserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysDimInfo.Update(obj);
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
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{DimTypeCodes.Length}条记录" };

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysDimInfoModel model, string currentUserId)
        {
            if (_context.SysDimInfo.Count(e => e.DimId == model.Dim_Id) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "已经存在相同的公告编码。" };
            }
            SysDimInfo entity = new SysDimInfo
            {
                DimId= Guid.NewGuid().ToString(),
                DimTypeCode = model.Dim_Type_Code,
                DimName = model.Dime_Name,
                DimValue = model.Dim_Value,           
                CreationDate = System.DateTime.Now,
                CreatedBy = currentUserId,
                LastUpdateDate = System.DateTime.Now,
                LastUpdatedBy = currentUserId
            };
            await _context.SysDimInfo.AddAsync(entity);

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
                    Console.WriteLine(ex.Message);
                    return new FuncResult() { IsSuccess = false, Content = ex.Message };
                }
            }


            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }

        /// <summary>
        /// 取维度
        /// </summary>
        /// <returns></returns>
        public object GetDimType()
        {
            try
            {

                int total = _context.SysDimType.Count();

                var result = _context.SysDimType.ToList();
                var data = result.Select(e => new
                {
                    e.DimTypeCode,
                    e.DimTypeName,
                });
                return data;
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = true, Message = "数据错误" };
                throw ex;
            }

        }
    }
}
