using JiaHang.Projects.Admin.DAL;
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
        private readonly DAL.EntityFramework.DataContext context;

        public DcsServiceInfoBLL(DAL.EntityFramework.DataContext datacontext)
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
            List<DcsServiceInfo> query = context.DcsServiceInfo.Where(s =>
                                        (string.IsNullOrWhiteSpace(model.ServiceName) || s.ServiceName.Contains(model.ServiceName)) &&
                                        (string.IsNullOrWhiteSpace(model.ServiceNo) || s.ServiceNo.Contains(model.ServiceNo)) &&
                                        (s.DeleteFlag == 0)).OrderByDescending(o => o.CreationDate).ToList();

            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(s => new
            {
                //需要的列
                Service_Id = s.ServiceId,
                Service_Group_Id = s.ServiceGroupId,
                Service_No = s.ServiceNo,
                Service_Code = s.ServiceCode,
                Service_Name = s.ServiceName,
                Service_Version = s.ServiceVersion,
                Service_Tech = s.ServiceTech,
                Service_Type = s.ServiceType,
                Service_Status = s.ServiceStatus,
                Datasource_Id = s.DatasourceId
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

            await Task.Run(() => 
            {
                foreach (var param in model.lsparam)
                {
                    var pEntity = new DcsServiceParams()
                    {
                        ParamId = Guid.NewGuid().ToString("N").ToUpper(),
                        ServiceId = model.ServiceId,
                        ParamCode = param.ParamCode,
                        ParamName = param.ParamName,
                        ParamDesc = param.ParamDesc,
                        ParamNullable = param.ParamNullable,
                        TimestampFlag = param.TimestampFlag,
                        RelaFieldId = param.RelaFieldId,

                        CreationDate = DateTime.Now,
                        CreatedBy = currentuserid,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = currentuserid,
                        DeleteFlag = 0
                    };
                    context.DcsServiceParams.Add(pEntity);
                }

                foreach (var share in model.lsshare)
                {
                    var sEntity = new DcsServiceSResults()
                    {
                        FieldId = share.FieldId,
                        ServiceId = model.ServiceId,

                        CreationDate = DateTime.Now,
                        CreatedBy = currentuserid,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = currentuserid,
                        DeleteFlag = 0
                    };
                    context.DcsServiceSResults.Add(sEntity);
                }

                foreach (var collect in model.lscollect)
                {
                    var cEntity = new DcsServiceCResults()
                    {
                        ServiceId = model.ServiceId,
                        ToFieldId = collect.ToFieldId,
                        //DimTransFlag = collect.DimTransFlag,

                        CreationDate = DateTime.Now,
                        CreatedBy = currentuserid,
                        LastUpdateDate = DateTime.Now,
                        LastUpdatedBy = currentuserid,
                        DeleteFlag = 0
                    };
                    context.DcsServiceCResults.Add(cEntity);
                }
            });

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

            await Task.Run(() =>
            {
                foreach (DcsServiceInfo obj in entities)
                {
                    obj.DeleteBy = currentuserid;
                    obj.DeleteDate = DateTime.Now;
                    obj.DeleteFlag = 1;
                    context.DcsServiceInfo.Update(obj);
                }
            });

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

        /// <summary>
        /// 返回接口基本信息视图
        /// </summary>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        public FuncResult GetServiceInfoView(string serviceid)
        {
            FuncResult result = new FuncResult();
            try
            {
                //接口基本信息
                var serviceInfo = context.DcsServiceInfo.Find(serviceid);
                if (serviceInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = "主键参数异常！";
                    return result;
                }


                //接口参数信息
                var param = context.DcsServiceParams.Where(w=>w.ServiceId.Contains(serviceid));


                //共享接口返回字段信息
                var shareresult = context.DcsServiceSResults.Where(w => w.ServiceId.Contains(serviceid));


                //采集接口返回字段信息
                var collectresult = context.DcsServiceCResults.Where(w => w.ServiceId.Contains(serviceid));


                result.IsSuccess = true; result.Content = new { ServiceInfo = serviceInfo, Params = param, ShareResult = shareresult, CollectResult = collectresult };
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
           
            return result;
        }

        /// <summary>
        /// 根据DataSourceId获取到FieldId（从SYS_DATASOURCE_FIELD表获取）
        /// </summary>
        /// <param name="datasourceid"></param>
        /// <returns></returns>
        public dynamic GetFieldIdByDataSourceId(string datasourceid)
        {
            try
            {
                //string sql = string.Format("select FIELD_ID AS key ,FIELD_NAME as value  from SYS_DATASOURCE_FIELD  where DATASOURCE_ID='{0}'", datasourceid);
                //var query = OracleDbHelper.Query(sql);
                //return null;

                var query = from a in context.SysDatasourceField
                            where a.DatasourceId.Contains(datasourceid)
                            select new
                            {
                                key = a.FieldId,
                                value = a.FieldName
                            };
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        
        }
    }
}
