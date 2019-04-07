using System;
using System.Collections.Generic;
using System.Text;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Enumerations;
using JiaHang.Projects.Admin.Model.SysDataSource.RequestModel;
using System.Threading.Tasks;
using System.Linq;

namespace JiaHang.Projects.Admin.BLL.SysDataSourceBLL
{
  public  class SysDataSourceBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysDataSourceBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchDatasource model)
        {
            var querys = from a in _context.SysDatasourceField
                         where (
(string.IsNullOrWhiteSpace(model.DataSource_Code) || model.DataSource_Code.Contains(model.DataSource_Code))
&& (string.IsNullOrWhiteSpace(model.DataSource_Name) || model.DataSource_Name.Contains(model.DataSource_Name))
&& (string.IsNullOrWhiteSpace(model.DataSource_Type) || model.DataSource_Type.Contains(model.DataSource_Type))
&& (string.IsNullOrWhiteSpace(model.DataSource_Use) || model.DataSource_Use.Contains(model.DataSource_Use))
&& a.DeleteFlag == 0
)
                         join b in _context.SysDatasourceInfo on a.DatasourceId equals b.DatasourceId
                       into a_temp
                         from a_ifnull in a_temp.DefaultIfEmpty()
                         join c in _context.SysConnectionInfo on a_ifnull.ConnectionId equals c.ConnectionId
                         into b_temp
                         from b_ifnull in b_temp.DefaultIfEmpty()
                         join d in _context.SysFieldType on a.FieldTypeId equals d.FieldTypeId
                         into c_temp
                         from c_ifnull in c_temp.DefaultIfEmpty()
                         select new
                         {
                             DatasourceId= a_ifnull.DatasourceId,
                             DatasourceCode= a_ifnull.DatasourceCode,
                             DatasourceName = a_ifnull.DatasourceName,
                             DatasourceType = a_ifnull.DatasourceType,
                             DatasourceUse = a_ifnull.DatasourceUse,
                             ConnectionId = a_ifnull.ConnectionId,
                             CreationDate = a_ifnull.CreationDate,
                             CreatedBy = a_ifnull.CreatedBy,
                             ConnectionName = b_ifnull.ConnectionName,
                             FieldTypeId = c_ifnull.FieldTypeId,
                             FieldTypeCode = c_ifnull.FieldTypeCode,
                             FieldTypeName = c_ifnull.FieldTypeName,
                             FieldId=a.FieldId,
                             FieldCode=a.FieldCode,
                             FieldName=a.FieldName,
                             FieldLength=a.FieldLength,
                             FieldNullable = a.FieldNullable==1? "否" :"是" ,
                             FieldKeyFlag = a.FieldKeyFlag,
                             FieldIndexFlag = a.FieldIndexFlag == 1 ? "否" : "是",
                             FieldValue = a.FieldValue,
                             DimFlag = a.DimFlag == 1 ? "否" : "是",
                             TimestampFlag = a.TimestampFlag == 1 ? "否" : "是",
                             DimTableName = a.DimTableName,
                             DimFieldCode = a.DimFieldCode,
                             DimFieldName = a.DimFieldName,
                             OraSequenceCode = a.OraSequenceCode,
                         };

            int total = querys.Count();
            var data = querys.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询数据源主表信息
        /// </summary>
        /// <returns></returns>
        public FuncResult SelectDataSourceInfo(SearchDatasource model) {
            var query = from a in _context.SysDatasourceInfo
                        where (
                            (string.IsNullOrWhiteSpace(model.DataSource_Code) || a.DatasourceCode.Contains(model.DataSource_Code))
                         && (string.IsNullOrWhiteSpace(model.DataSource_Name) || a.DatasourceName.Contains(model.DataSource_Name))
                         && (string.IsNullOrWhiteSpace(model.DataSource_Type) || a.DatasourceType.Contains(model.DataSource_Type))
                         && (string.IsNullOrWhiteSpace(model.DataSource_Use) || a.DatasourceUse.Contains(model.DataSource_Use))
                         && a.DeleteFlag == 0
                               )
                        join b in _context.SysConnectionInfo on a.ConnectionId equals b.ConnectionId
                        into a_temp
                        from a_ifnull in a_temp.DefaultIfEmpty()
                        join c in _context.SysDatabaseType on a_ifnull.DatabaseTypeId equals c.DatabaseTypeId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        select new
                        {
                            DataSourceId=a.DatasourceId,
                            DataSourceCode=a.DatasourceCode,
                            DataSourceName=a.DatasourceName,
                            DataSourceType=a.DatasourceType,
                            DataSourceUse=a.DatasourceUse,
                            ConnectionId=a.ConnectionId,
                            CreationDate=a.CreationDate,
                            CreateBy=a.CreatedBy,
                            ConnectionName=a_ifnull.ConnectionName,
                            DataBaseTypeId=a_ifnull.DatabaseTypeId,
                            ConnectionString=a_ifnull.ConnectionString,
                            DataBaseTypeCode=b_ifnull.DatabaseTypeCode,
                            DatabaseTypeName=b_ifnull.DatabaseTypeName
                        };
            int total = query.Count();
            var data = query.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询数据源的所有字段
        /// </summary>
        /// <param name="datasourceId"></param>
        /// <returns></returns>
        public FuncResult SelectDataSourceFiled(SerchByDatasourceId model) {
            var querys = from a in _context.SysDatasourceField where (model.dataSourceId == a.DatasourceId)
                        select new{
                            FieldId = a.FieldId,
                            FieldCode = a.FieldCode,
                            FieldName = a.FieldName,
                            FieldLength = a.FieldLength,
                            FieldNullable = a.FieldNullable == 1 ? "否" : "是",
                            FieldKeyFlag = a.FieldKeyFlag,
                            FieldIndexFlag = a.FieldIndexFlag == 1 ? "否" : "是",
                            FieldValue = a.FieldValue,
                            DimFlag = a.DimFlag == 1 ? "否" : "是",
                            TimestampFlag = a.TimestampFlag == 1 ? "否" : "是",
                            DimTableName = a.DimTableName,
                            DimFieldCode = a.DimFieldCode,
                            DimFieldName = a.DimFieldName,
                            OraSequenceCode = a.OraSequenceCode,
                        };
            int total = querys.Count();
            var data = querys.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询数据源不存在的字段
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult SelectNotDataSouceField(SerchByDatasourceId model) {
            var querys = from a in _context.SysDatasourceField
                         where (model.dataSourceId != a.DatasourceId)
                         select new
                         {
                             DatasourceId=a.DatasourceId,
                             FieldId = a.FieldId,
                             FieldCode = a.FieldCode,
                             FieldName = a.FieldName,
                             FieldLength = a.FieldLength,
                             FieldNullable = a.FieldNullable == 1 ? "否" : "是",
                             FieldKeyFlag = a.FieldKeyFlag,
                             FieldIndexFlag = a.FieldIndexFlag == 1 ? "否" : "是",
                             FieldValue = a.FieldValue,
                             DimFlag = a.DimFlag == 1 ? "否" : "是",
                             TimestampFlag = a.TimestampFlag == 1 ? "否" : "是",
                             DimTableName = a.DimTableName,
                             DimFieldCode = a.DimFieldCode,
                             DimFieldName = a.DimFieldName,
                             OraSequenceCode = a.OraSequenceCode,
                         };
            int total = querys.Count();
            var data = querys.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询数据库连接信息
        /// </summary>
        /// <returns></returns>
         public async Task<FuncResult> SelectConnection()
        {
            var query = from a in _context.SysConnectionInfo
                        where (
                                a.DeleteFlag == 0
                               )
                        select new
                        {
                            ConnectionId=a.ConnectionId,
                            ConnectionName=a.ConnectionName
                        };
            object data = null;
            await Task.Run(() =>
            {
                data = query.ToList();
            });
                return new FuncResult() { IsSuccess = true, Content = data };
        }
        /// <summary>
        /// 查询数据源字段类型
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<FuncResult> SelectFieldInfo() {
            var query = from a in _context.SysFieldType
                        where (
                                a.DeleteFlag == 0
                               )
                        select new
                        {
                           FieldTypeId=a.FieldTypeId,
                           FieldTypeName=a.FieldTypeName
                        };
            object data = null;
            await Task.Run(() =>
            {
                data = query.ToList();
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }
        /// <summary>
        /// 添加方法(数据源信息)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> AddDataSourceInfo(SysDataSourceModel model, string userId)
        {
            SysDatasourceInfo entity = new SysDatasourceInfo
            {
                DatasourceId = Guid.NewGuid().ToString("N"),
                DatasourceCode = model.DatasourceCode,
                DatasourceName=model.DatasourceName,
                DatasourceType=model.DatasourceType,
                DatasourceUse=model.DatasourceUse,
                ConnectionId=model.ConnectionId,
                CreatedBy=userId,
                CreationDate = DateTime.Now
            };
            await _context.SysDatasourceInfo.AddAsync(entity);

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
        /// <summary>
        /// 添加一个或者多个(数据源字段信息)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> AddDataSourceField(List<AddFieldInfoParm> model, string userId)
        {
            SysDatasourceField entity = null;
            foreach (var item in model)
            {
                entity = new SysDatasourceField
                {
                    FieldId = Guid.NewGuid().ToString("N"),
                    DatasourceId = item.DatasourceId,
                    FieldCode = item.FieldCode,
                    FieldName = item.FieldName,
                    FieldTypeId = item.FieldTypeId,
                    FieldLength = item.FieldLength,
                    FieldNullable = item.FieldNullable,
                    FieldKeyFlag = item.FieldKeyFlag,
                    FieldIndexFlag = item.FieldIndexFlag,
                    FieldValue = item.FieldValue,
                    DimFlag = item.DimFlag,
                    TimestampFlag = item.TimestampFlag,
                    DimTableName = item.DimTableName,
                    DimFieldCode = item.DimFieldCode,
                    DimFieldName = item.DimFieldName,
                    OraSequenceCode = item.OraSequenceCode,
                    CreatedBy = userId,
                    CreationDate = DateTime.Now
                };
                await _context.SysDatasourceField.AddAsync(entity);
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
            }
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };

        }
        /// <summary>
        /// 修改（数据源信息）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> UpdateDataSourceInfo(string id, SysDataSourceModel model, string currentUserId)
        {
            SysDatasourceInfo entity = await _context.SysDatasourceInfo.FindAsync(id);
           
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据源ID错误!" };
            }
            entity.DatasourceCode = model.DatasourceCode;
            entity.DatasourceName = model.DatasourceName;
            entity.DatasourceType = model.DatasourceType;
            entity.DatasourceUse = model.DatasourceUse;
            entity.ConnectionId = model.ConnectionId;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            _context.SysDatasourceInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 修改(数据源字段信息)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> UpdateDataSourceField(string id, SysDataSourceFieldModel model, string currentUserId)
        {
            SysDatasourceField entity = await _context.SysDatasourceField.FindAsync(id);

            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据源ID错误!" };
            }
            entity.DatasourceId = model.DatasourceId;
            entity.FieldCode = model.FieldCode;
            entity.FieldName = model.FieldName;
            entity.FieldTypeId = model.FieldTypeId;
            entity.FieldLength = model.FieldLength;
            entity.FieldNullable = model.FieldNullable;
            entity.FieldKeyFlag = model.FieldKeyFlag;
            entity.FieldIndexFlag = model.FieldIndexFlag;
            entity.FieldValue = model.FieldValue;
            entity.DimFlag = model.DimFlag;
            entity.TimestampFlag = model.TimestampFlag;
            entity.DimTableName = model.DimTableName;
            entity.DimFieldCode = model.DimFieldCode;
            entity.DimFieldName = model.DimFieldName;
            entity.OraSequenceCode = model.OraSequenceCode;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;

            _context.SysDatasourceField.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除单个(数据源信息)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> DeleteDataSourceInfo(string id, string currentUserId)
        {
            SysDatasourceInfo entity = await _context.SysDatasourceInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据源ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysDatasourceInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        /// <summary>
        /// 删除单个(数据源字段信息)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> DeleteDataSourceField(string id, string currentUserId)
        {
            SysDatasourceField entity = await _context.SysDatasourceField.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "数据源字段ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysDatasourceField.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        /// <summary>
        /// 获取多个ID进行删除(数据源信息)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> DeleteDataSourceInfos(string[] ids, string currentUserId)
        {
            IQueryable<SysDatasourceInfo> entitys = _context.SysDatasourceInfo.Where(e => ids.Contains(e.DatasourceId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysDatasourceInfo obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysDatasourceInfo.Update(obj);
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
        /// 获取多个ID进行删除(数据源字段信息)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> DeleteDataSourceFields(string[] ids, string currentUserId)
        {
            IQueryable<SysDatasourceField> entitys = _context.SysDatasourceField.Where(e => ids.Contains(e.FieldId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysDatasourceField obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysDatasourceField.Update(obj);
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
    }
}
