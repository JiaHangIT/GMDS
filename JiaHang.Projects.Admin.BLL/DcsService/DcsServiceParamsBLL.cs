using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceParams.RequestModel;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.DcsService
{
    public class DcsServiceParamsBLL
    {
        private readonly DAL.EntityFramework.DataContext context;

        public DcsServiceParamsBLL(DAL.EntityFramework.DataContext datacontext)
        {
            this.context = datacontext;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentuser"></param>
        /// <returns></returns>
        public FuncResult Select(SearchDcsServiceParams model, string currentuser)
        {
            List<DcsServiceParams> query = context.DcsServiceParams.OrderByDescending(o => o.CreationDate).ToList();

            int total = query.Count();
            var data = query.Skip(model.page * model.limit).Take(model.limit).ToList().Select(s => new
            {
                //根据需求返回字段


            });

            return new FuncResult() { IsSuccess = true, Content = new { total, data } };
        }

        /// <summary>
        /// 查询（by key）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            FuncResult result = new FuncResult();
            if (string.IsNullOrEmpty(id))
            {
                result.IsSuccess = false;
                result.Message = "主键参数为空";
                return result;
            }

            DcsServiceParams entity = await context.DcsServiceParams.FindAsync(id);

            result.IsSuccess = true;
            result.Content = entity;

            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(DcsServiceParamsModel model, string currentuserid)
        {
            DcsServiceParams entity = new DcsServiceParams()
            {
                ParamId = Guid.NewGuid().ToString("N").ToUpper(),
                ServiceId = model.ServiceId,
                ParamCode = model.ParamCode,
                ParamName = model.ParamName,
                ParamTypeId = model.ParamTypeId,
                ParamDesc = model.ParamDesc,
                ParamNullable = model.ParamNullable,
                TimestampFlag = model.TimestampFlag,
                RelaFieldId = model.RelaFieldId,

                CreationDate = DateTime.Now,
                CreatedBy = currentuserid,
                LastUpdateDate = DateTime.Now,
                LastUpdatedBy = currentuserid
            };

            await context.DcsServiceParams.AddAsync(entity);
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

            return new FuncResult() { IsSuccess = true, Content = entity };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentuserid)
        {
            FuncResult result = new FuncResult();
            if (string.IsNullOrEmpty(id))
            {
                result.IsSuccess = false;
                result.Message = "主键ID为空";
                return result;
            }
            DcsServiceParams entity = await context.DcsServiceParams.FindAsync(id);

            if (entity == null)
            {
                result.IsSuccess = false;
                result.Message = "主键ID有误";
                return result;
            }

            entity.DeleteFlag = 1;
            context.DcsServiceParams.Update(entity);
            await context.SaveChangesAsync();

            result.IsSuccess = true;
            result.Message = "删除成功";
            return result;
        }

        /// <summary>
        /// 删除（批）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string[] ids, string currentuserid)
        {
            IQueryable<DcsServiceParams> entities = context.DcsServiceParams.Where(s => ids.Contains(s.ParamId));
            if (entities != null && entities.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }

            foreach (DcsServiceParams obj in entities)
            {
                obj.DeleteFlag = 1;
                context.DcsServiceParams.Update(obj);
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
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentuserid"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id,DcsServiceParamsModel model, string currentuserid)
        {
            FuncResult result = new FuncResult();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    result.IsSuccess = false;
                    result.Message = "主键ID为空";
                    return result;
                }
                DcsServiceParams entity = await context.DcsServiceParams.FindAsync(id);
                if (entity == null)
                {
                    result.IsSuccess = false;
                    result.Message = "主键ID有误";
                    return result;
                }
                entity.ServiceId = model.ServiceId;
                entity.ParamCode = model.ParamCode;
                entity.ParamName = model.ParamName;
                entity.ParamTypeId = model.ParamTypeId;
                entity.ParamDesc = model.ParamDesc;
                entity.ParamNullable = model.ParamNullable;
                entity.TimestampFlag = model.TimestampFlag;
                entity.RelaFieldId = model.RelaFieldId;

                entity.LastUpdateDate = DateTime.Now;
                entity.LastUpdatedBy = currentuserid;


                context.DcsServiceParams.Update(entity);
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
