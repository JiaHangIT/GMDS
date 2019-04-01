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
                             ConnectionString = b_ifnull.ConnectionName,
                             FieldTypeId = c_ifnull.FieldTypeId,
                             FieldTypeCode = c_ifnull.FieldTypeCode,
                             FieldTypeName = c_ifnull.FieldTypeCode,
                             FieldId=a.FieldId,
                             FieldLength=a.FieldLength,
                             FieldNullable = a.FieldNullable,
                             FieldKeyFlag = a.FieldKeyFlag,
                             FieldIndexFlag = a.FieldIndexFlag,
                             FieldValue = a.FieldValue,
                             DimFlag = a.DimFlag,
                             TimestampFlag = a.TimestampFlag,
                             DimTableName = a.DimTableName,
                             DimFieldCode = a.DimFieldCode,
                             DimFieldName = a.DimFieldName,
                             OraSequenceCode = a.OraSequenceCode,
                         };

            int total = querys.Count();
            var data = querys.ToList().Skip(model.limit * model.page).Take(model.limit).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
    }
}
