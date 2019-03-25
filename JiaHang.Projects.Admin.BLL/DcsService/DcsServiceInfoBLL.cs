using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceInfo.RequestModel;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.DcsService
{
    public class DcsServiceInfoBLL
    {
        private readonly DataContext context;

        public DcsServiceInfoBLL(DataContext datacontext)
        {
            context = datacontext;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchDcsServiceInfo model)
        {
            List<DcsServiceInfo> query = context.DcsServiceInfo.OrderByDescending(o => o.CreationDate).ToList();

            int total = query.Count();
            var data = query.Where(s => (string.IsNullOrWhiteSpace(model.ServiceCode) || s.ServiceCode.Contains(model.ServiceCode)) &&
            (string.IsNullOrWhiteSpace(model.ServiceName) || s.ServiceCode.Contains(model.ServiceName)) &&
            (string.IsNullOrWhiteSpace(model.ServiceNo) || s.ServiceCode.Contains(model.ServiceNo))
            ).Skip(model.limit * model.page).Take(model.limit).ToList().Select(s => new
            {
                //需要的列
            });
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }

        /// <summary>
        /// 查询（根据主键）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            FuncResult result = new FuncResult();
            if (string.IsNullOrEmpty(id))
            {
                result.Message = "主键参数为空";
                result.IsSuccess = false;
                return result;
            }

            DcsServiceInfo entity = await context.DcsServiceInfo.FindAsync(id);
            result.IsSuccess = true;
            result.Content = entity;

            return result;
        }

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(DcsServiceInfoModel model, string currentuserid)
        {
            DcsServiceInfo entity = new DcsServiceInfo()
            {
                ServiceId = Guid.NewGuid().ToString("N").ToUpper(),
                ServiceGroupId = model.ServiceGroupId,
                ServiceNo = model.ServiceNo,
                ServiceCode = model.ServiceCode,
                ServiceName = model.ServiceName,
                ServiceDesc = model.ServiceDesc,
                ServiceTech = model.ServiceTech,
                ServiceType = model.ServiceType,
                ServiceReturn = model.ServiceReturn,
                ServiceStatus = model.ServiceStatus,
                DataPageFlag = model.DataPageFlag,
                DataMultiFlag = model.DataMultiFlag,

                CreationDate = DateTime.Now,
                CreatedBy = currentuserid,
                LastUpdateDate = DateTime.Now,
                LastUpdatedBy = currentuserid
            };

            await context.DcsServiceInfo.AddAsync(entity);

            using (IDbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    await context.SaveChangesAsync();
                    trans.Commit();
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
        /// 删除（一个）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentuserid)
        {
            DcsServiceInfo entity = await context.DcsServiceInfo.FindAsync(id);

            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "主键有误" };
            }

            entity.DeleteBy = currentuserid;
            entity.DeleteDate = DateTime.Now;
            entity.DeleteFlag = 1;

            context.DcsServiceInfo.Update(entity);
            await context.SaveChangesAsync();

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        /// <summary>
        /// 删除（批）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string[] ids, string currentuserid)
        {
            IQueryable<DcsServiceInfo> entities = context.DcsServiceInfo.Where(s => ids.Contains(s.ServiceId));
            if (entities != null && entities.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }

            foreach (DcsServiceInfo obj in entities)
            {
                obj.DeleteBy = currentuserid;
                obj.DeleteDate = DateTime.Now;
                obj.DeleteFlag = 1;
                context.DcsServiceInfo.Update(obj);
            }

            using (IDbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    await context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = ex.Message };
                }
            }

            return new FuncResult() { IsSuccess = true, Message = "删除成功" };
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="model"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id,DcsServiceInfoModel model, string currentuserid)
        {
            FuncResult result = new FuncResult();
            try
            {
                if (string .IsNullOrEmpty(id))
                {
                    result.IsSuccess = false;
                    result.Message = "主键参数为空";
                    return result;
                }

                DcsServiceInfo entity = await context.DcsServiceInfo.FindAsync(id);
                if (entity == null)
                {
                    result.IsSuccess = false;
                    result.Message = "主键ID错误";
                    return result;
                }

                entity.ServiceGroupId = model.ServiceGroupId;
                entity.ServiceNo = model.ServiceNo;
                entity.ServiceCode = model.ServiceCode;
                entity.ServiceName = model.ServiceName;
                entity.ServiceDesc = model.ServiceDesc;
                entity.ServiceTech = model.ServiceTech;
                entity.ServiceType = model.ServiceType;
                entity.ServiceReturn = model.ServiceReturn;
                entity.ServiceStatus = model.ServiceStatus;
                entity.DataPageFlag = model.DataPageFlag;
                entity.DataMultiFlag = model.DataMultiFlag;

                entity.LastUpdateDate = DateTime.Now;
                entity.LastUpdatedBy = currentuserid;

                context.DcsServiceInfo.Update(entity);
                await context.SaveChangesAsync();

                result.IsSuccess = true;
                result.Content = entity;
                result.Message = "更新成功";
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
