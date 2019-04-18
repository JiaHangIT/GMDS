using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsCustomerServiceParams.RequestModel;
using JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.DcsCustomerBLL
{
    public class DcsCustomerServiceBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public DcsCustomerServiceBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(int pageSize, int currentPage, string customerName)
        {
            var query = from a in _context.DcsCustomerInfo
                        where string.IsNullOrWhiteSpace(customerName)|| a.CustomerName.Contains(customerName)
                        join b in _context.DcsCustomerServices
                        on a.CustomerId equals b.CustomerId
                        into b_temp
                        from b_ifnull in b_temp.DefaultIfEmpty()
                        join serviceinfo in _context.DcsServiceInfo on b_ifnull.ServiceId equals serviceinfo.ServiceId
                        into serviceinfo_temp
                        from service_ifnull in serviceinfo_temp.DefaultIfEmpty()
                        join c in _context.DcsCustsveFieldList
                        on new { b_ifnull.CustomerId, b_ifnull.ServiceId } equals new { c.CustomerId, c.ServiceId }
                        into c_temp
                        from c_ifnull in c_temp.DefaultIfEmpty()
                            //已授权字段信息
                        join d in _context.SysDatasourceField on c_ifnull.FieldId equals d.FieldId
                        into d_temp
                        from d_ifnull in d_temp.DefaultIfEmpty()


                            //  接口所有的返回字段信息
                        join e in _context.DcsServiceSResults on b_ifnull.ServiceId equals e.ServiceId
                        into e_temp
                        from e_ifnull in e_temp.DefaultIfEmpty()
                        join f in _context.SysDatasourceField on e_ifnull.FieldId equals f.FieldId
                        into f_temp
                        from f_ifnul in f_temp.DefaultIfEmpty()
                        select new
                        {
                            a.CustomerName,
                            a.CustomerId,
                            serviceIsNull = service_ifnull == null,
                            ServiceName = service_ifnull == null ? "" : service_ifnull.ServiceName,
                            ServiceCode = service_ifnull == null ? "" : service_ifnull.ServiceCode,
                            ServiceId = service_ifnull == null ? "" : service_ifnull.ServiceId,

                            alreadyIsNull = d_ifnull == null,
                            alreadyFieldId = d_ifnull == null ? "" : d_ifnull.FieldId,
                            alreadyFieldCode = d_ifnull == null ? "" : d_ifnull.FieldCode,
                            alreadyFieldName = d_ifnull == null ? "" : d_ifnull.FieldName,

                            allIsNull = f_ifnul == null,
                            allFieldId = f_ifnul == null ? "" : f_ifnul.FieldId,
                            allFieldCode = f_ifnul == null ? "" : f_ifnul.FieldCode,
                            allFieldName = f_ifnul == null ? "" : f_ifnul.FieldName
                        };
            var data = query.GroupBy(customer => new { customer.CustomerName, customer.CustomerId }).Select(c => new
            {
                c.Key.CustomerName,
                c.Key.CustomerId,
                key = Guid.NewGuid().ToString(),
                serviceinfos = c.Where(e => !e.serviceIsNull).GroupBy(e => e.ServiceId).Select(s => new
                {
                    serviceName = s.First().ServiceName,
                    ServiceCode = s.First().ServiceCode,
                    serviceId = s.First().ServiceId,

                    //已经授权
                    alreadyFields = s.Where(n => !n.alreadyIsNull).Select(ns => new
                    {
                        fieldId = ns.alreadyFieldId,
                        fieldCode = ns.alreadyFieldCode,
                        fieldName = ns.alreadyFieldName
                    }).Distinct(),

                    //未授权
                    noryetfields = s.Where(a => !a.allIsNull && !s.Where(n => !n.alreadyIsNull).Select(cc => cc.alreadyFieldId).Contains(a.allFieldId)).Select(als => new
                    {
                        fieldId = als.allFieldId,
                        fieldName = als.allFieldName,
                        fieldCode = als.allFieldCode,

                    }).Distinct()
                })
            });
            return new FuncResult() { IsSuccess = true, Content =new {total=data.Count(), data=data.Skip(pageSize * currentPage).Take(pageSize)}  };
        }


        /// <summary>
        /// 返回当前用户未授权的接口列表
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public FuncResult NotBind(string customerId)
        {

            var query = from a in _context.DcsServiceInfo
                        join b in _context.DcsServiceSResults
                        on a.ServiceId equals b.ServiceId
                        join c in _context.SysDatasourceField on b.FieldId equals c.FieldId
                        where !_context.DcsCustomerServices.Where(e => e.CustomerId == customerId).Select(e => e.ServiceId).Contains(a.ServiceId)
                        select new
                        {
                            a.ServiceId,
                            a.ServiceName,
                            a.ServiceCode,
                            c.FieldId,
                            c.FieldCode,
                            c.FieldName
                        };

            var data = query.ToList().GroupBy(e => e.ServiceId).Select(e => new
            {
                ServiceId = e.Key,
                e.First().ServiceName,
                e.First().ServiceCode,
                checkAll = false,
                isIndeterminate = false,
                fields = e.Select(c => new
                {
                    c.FieldName,
                    c.FieldId,
                    c.FieldCode,
                    c.ServiceId
                }),
                checkedFields = new List<string>()
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }


        public async Task<FuncResult> Add(string currentUserId, DcsCustomerServiceParamsModel model)
        {

            List<DcsCustomerServices> entitys = new List<DcsCustomerServices>();
            List<DcsCustsveFieldList> listfield = new List<DcsCustsveFieldList>();
            foreach (var obj in model.ServiceInfos)
            {
                var entity = new DcsCustomerServices()
                {
                    CustomerId = model.CustomerId,
                    ServiceId = obj.ServiceId,

                    CreatedBy = currentUserId,
                    CreationDate = DateTime.Now,
                    LastUpdatedBy = currentUserId,
                    LastUpdateDate = DateTime.Now,
                };
                entitys.Add(entity);


                foreach (var field in obj.FieldIds)
                {
                    var fieldentity = new DcsCustsveFieldList()
                    {
                        CustomerId = model.CustomerId,
                        FieldId = field,
                        ServiceId = entity.ServiceId,

                        CreatedBy = currentUserId,
                        CreationDate = DateTime.Now,
                        LastUpdatedBy = currentUserId,
                        LastUpdateDate = DateTime.Now,
                    };
                    listfield.Add(fieldentity);
                }

            }
            await _context.DcsCustomerServices.AddRangeAsync(entitys);
            await _context.DcsCustsveFieldList.AddRangeAsync(listfield);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = "添加授权接口时，发生了预料之外的错误" };
                }
            }
            return new FuncResult() { IsSuccess = true, Message = "已成功添加授权接口" };
        }

        /// <summary>
        /// 更新授权接口字段信息
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> UpdateField(string currentUserId, DcsCustomerServiceParamsModel model)
        {
            var list = new List<DcsCustsveFieldList>();
            foreach (var obj in model.ServiceInfos) {
                var entitys = _context.DcsCustsveFieldList.Where(e => e.CustomerId == model.CustomerId && e.ServiceId == obj.ServiceId);
                _context.DcsCustsveFieldList.RemoveRange(entitys);
                foreach (var field in obj.FieldIds) {
                    var fieldentity = new DcsCustsveFieldList()
                    {
                        CustomerId = model.CustomerId,
                        ServiceId = obj.ServiceId,
                        FieldId = field,

                        CreatedBy = currentUserId,
                        CreationDate = DateTime.Now,
                        LastUpdatedBy = currentUserId,
                        LastUpdateDate = DateTime.Now,
                    };
                    list.Add(fieldentity);
                }
            }
            await _context.DcsCustsveFieldList.AddRangeAsync(list);
            using (var trans = _context.Database.BeginTransaction()) {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = "编辑授权接口字段时，发生了预料之外的错误" };
                }
            }
            return new FuncResult() { IsSuccess = true, Message = "已成功编辑授权接口" };

        }
        public async Task<FuncResult> Delete(string customerId, string serviceId, string currentUserId)
        {
            var entity = _context.DcsCustomerServices.FirstOrDefault(e => e.ServiceId == serviceId && e.CustomerId == customerId);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "serviceId错误" };
            }
            _context.DcsCustomerServices.Remove(entity);

            //将字段也删除
            var fied = _context.DcsCustsveFieldList.Where(e => e.CustomerId == customerId && e.ServiceId == serviceId);
            _context.DcsCustsveFieldList.RemoveRange(fied);
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogService.WriteError(ex);
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Message = "移除授权接口时，发生了预料之外的错误" };
                }
            }

            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }


    }
}
