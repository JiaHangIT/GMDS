﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.DcsService;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceInfo.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsService
{
    [Route("api/[controller]")]
    public class DcsServiceInfoDataController : Controller
    {
        private readonly DcsServiceInfoBLL DcsServiceInfo;
        private readonly DataContext context;
        private readonly IMemoryCache cache;

        public DcsServiceInfoDataController(DataContext datacontext, IMemoryCache cache)
        {
            DcsServiceInfo = new DcsServiceInfoBLL(datacontext);
            context = datacontext;
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchDcsServiceInfo model)
        {
            model.page--; if (model.page < 0)
                model.page = 0;

            return DcsServiceInfo.Select(model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DcsServiceInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            try
            {
                if (model.ServiceType == "SHARE")
                {
                    model.lscollect = new List<DcsCollectModel>();
                }
                else
                {
                    model.lsshare = new List<DcsShareModel>();
                }
                return await DcsServiceInfo.Add(model, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.InnerException.Message };
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute] string id)
        {
            try
            {
                return await DcsServiceInfo.Delete(id, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.InnerException.Message };
            }

        }

        /// <summary>
        /// 删除（批）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            try
            {
                return await DcsServiceInfo.Delete(ids, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.InnerException.Message };
            }

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]DcsServiceInfoModel model)
        {
            try
            {
                if (model.ServiceType == "SHARE")
                {
                    model.lscollect = new List<DcsCollectModel>();
                }
                else
                {
                    model.lsshare = new List<DcsShareModel>();
                }
                var data = await DcsServiceInfo.Update(id, model, HttpContext.CurrentUser(cache).Id);
                return data;
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.InnerException.Message };
            }

        }

        /// <summary>
        /// 获取数据源主键和键值 
        /// </summary>
        /// <returns></returns>
        [Route("GetDataSource")]
        [HttpPost]
        public dynamic GetDataSource()
        {
            try
            {
                var data = from a in context.SysDatasourceInfo
                           select new
                           {
                               key = a.DatasourceId,
                               value = a.DatasourceName
                           };
                return data;
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取参数类型（数据接口参数信息）
        /// SYS_FIELD_TYPE.FIELD_TYPE_ID
        /// </summary>
        /// <returns></returns>
        [Route("GetFieldType")]
        [HttpPost]
        public dynamic GetFieldType()
        {
            try
            {
                var data = from a in context.SysFieldType
                           select
                            new
                            {
                                key = a.FieldTypeId,
                                value = a.FieldTypeName
                            };
                return data;
                //var data = context.SysFieldType.Select(s => new
                //{
                //    key = s.FieldTypeId,
                //    value = s.FieldTypeName
                //});

                //return data;
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// 获取关联字段ID（数据接口参数信息）
        /// SYS_DATASOURCE_FIELD.FIELD_ID
        /// </summary>
        /// <returns></returns>
        [Route("GetRelaFieldId")]
        [HttpPost]
        public dynamic GetRelaFieldId()
        {
            try
            {
                var data = from a in context.SysDatasourceField
                           select
                            new
                            {
                                key = a.FieldId,
                                value = a.FieldName
                            };
                return data;
                //var data = context.SysDatasourceField.Select(s => new
                //{
                //    key = s.FieldId,
                //    value = s.FieldName
                //});

                //return data;
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };
            }
        }

        /// <summary>
        /// 返回接口基本信息视图
        /// </summary>
        /// <param name="serviceid"></param>
        /// <returns></returns>
        [Route("GetServiceInfoView")]
        [HttpGet]
        public FuncResult GetServiceInfoView(string serviceid)
        {
            return DcsServiceInfo.GetServiceInfoView(serviceid);
        }

        /// <summary>
        /// 根据DataSourceId获取到FieldId（从SYS_DATASOURCE_FIELD表获取）
        /// </summary>
        /// <returns></returns>
        [HttpGet("{datasourceid}")]
        public dynamic GetFieldIdByDataSourceId(string datasourceid)
        {
            try
            {
                if (datasourceid == "0")
                {
                    var query = from a in context.SysDatasourceField
                                select new
                                {
                                    key = a.FieldId,
                                    value = a.FieldName,
                                    datasourceid = a.DatasourceId
                                };
                    return query;
                }
                return DcsServiceInfo.GetFieldIdByDataSourceId(datasourceid);
                //  var query = context.SysDatasourceField.DefaultIfEmpty()
                //.Where(w => w.DatasourceId.Contains(datasourceid)).Select(s => new { key = s.FieldId, value = s.FieldName }).ToList();
                //  return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
