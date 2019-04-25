using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Sys_Dim_Type.RequestModel;
using Microsoft.EntityFrameworkCore;

namespace JiaHang.Projects.Admin.BLL.SysDimTypeBLL
{
    public class Sys_Dim_TypeBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public Sys_Dim_TypeBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysDimType model)
        {
            try
            {
               

                int total = _context.SysDimType.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Dim_Type_Name) || a.DimTypeName.Contains(model.Dim_Type_Name))
                        && (string.IsNullOrWhiteSpace(model.Creation_By) || a.CreatedBy == (model.Creation_By))
                        && (a.DeleteFlag != 1)

                        )).Count();

                var result = _context.SysDimType.
                        Where(a =>
                        (
                        (string.IsNullOrWhiteSpace(model.Dim_Type_Name) || a.DimTypeName.Contains(model.Dim_Type_Name))
                        && (string.IsNullOrWhiteSpace(model.Creation_By) || a.CreatedBy == (model.Creation_By))
                        && (a.DeleteFlag != 1)
                        )).Skip(model.limit * model.page).Take(model.limit).ToList();
                var data = result.Select(e => new
                {
                    dimTypecode = e.DimTypeCode,
                    messageTitle = e.DimTypeName?? "",
                    creationDate = e.CreationDate != null ? Convert.ToDateTime(e.CreationDate).ToString("yyyy-MM-dd") : "",
                    createdBy = e.CreatedBy?? "",
                    lastUpdateDate = e.LastUpdateDate != null ? Convert.ToDateTime(e.LastUpdateDate).ToString("yyyy-MM-dd") : "",
                    lastUpdatedBy = e.LastUpdatedBy??"",
                 
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
            var entity = await _context.SysDimType.FirstOrDefaultAsync(m => m.DimTypeCode == DimTypeCode);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string DimTypeCode, SysDimTypeModel model, string currentuserId)
        {
            SysDimType entity = await _context.SysDimType.FindAsync(DimTypeCode);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号错误!" };
            }
            entity.DimTypeCode = DimTypeCode;
            entity.DimTypeName = model.Dim_Type_Name;
            entity.LastUpdateDate = System.DateTime.Now;
            entity.LastUpdatedBy = currentuserId;
            //entity.AuditFlag = model.Audit_Flag;
            //entity.AuditedDate = model.Audited_Date;          
            _context.SysDimType.Update(entity);
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
            SysDimType entity = await _context.SysDimType.FindAsync(DimTypeCode);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "公告编号不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysDimType.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] DimTypeCodes, string currentuserId)
        {
            IQueryable<SysDimType> entitys = _context.SysDimType.Where(e => DimTypeCodes.Contains(e.DimTypeCode));
            if (entitys.Count() != DimTypeCodes.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysDimType obj in entitys)
            {
                obj.DeleteBy = currentuserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysDimType.Update(obj);
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
        public async Task<FuncResult> Add(SysDimTypeModel model, string currentUserId)
        {
            if (_context.SysDimType.Count(e => e.DimTypeCode == model.Dim_Type_Code) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "已经存在相同的公告编码。" };
            }
            SysDimType entity = new SysDimType
            {
                DimTypeCode = model.Dim_Type_Code,
                DimTypeName = model.Dim_Type_Name,
                CreationDate = System.DateTime.Now,
                CreatedBy = currentUserId,
                LastUpdateDate = System.DateTime.Now,
                LastUpdatedBy = currentUserId
            };
            await _context.SysDimType.AddAsync(entity);

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
    }
}
