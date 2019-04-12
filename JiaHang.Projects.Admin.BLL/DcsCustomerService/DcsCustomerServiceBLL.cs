using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
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
        public FuncResult Select(int pageSize, int currentPage, string customerName, string customerMobile)
        {
            var query = from a in _context.DcsCustomerInfo
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
                            ServiceId = service_ifnull == null ? "" : service_ifnull.ServiceId,

                            noryetIsNull = d_ifnull == null,
                            noryetFieldId = d_ifnull == null ? "" : d_ifnull.FieldId,
                            noryetFieldName = d_ifnull == null ? "" : d_ifnull.FieldName,

                            alreadyIsNull = f_ifnul == null,
                            alreadyFieldId = f_ifnul == null ? "" : f_ifnul.FieldId,
                            alreadyFieldName = f_ifnul == null ? "" : f_ifnul.FieldName
                        };
            var data = query.ToList().GroupBy(customer => new
            {
            });
            return new FuncResult() { IsSuccess = true, Content = data };
        }



        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, DcsCustomerInfo model, string currentUserId)
        {
            //var entity = await _context.DcsCustomerInfo.FindAsync(id);
            if (string.IsNullOrWhiteSpace(id))
            {
                return new FuncResult() { IsSuccess = false, Message = "客户ID错误!" };
            }

            model.LastUpdatedBy = currentUserId;
            model.LastUpdateDate = DateTime.Now;


            try
            {
                _context.DcsCustomerInfo.Update(model);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };

            }
            return new FuncResult() { IsSuccess = true, Content = model, Message = "修改成功" };
        }

        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            var entity = await _context.DcsCustomerInfo.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "用户ID不存在!" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.DcsCustomerInfo.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<DcsCustomerInfo> entitys = _context.DcsCustomerInfo.Where(e => ids.Contains(e.CustomerId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (var obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.DcsCustomerInfo.Update(obj);
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
        public async Task<FuncResult> Add(DcsCustomerInfo model, string currentUserId)
        {

            model.CustomerId = Guid.NewGuid().ToString();
            model.CreatedBy = currentUserId;
            model.CreationDate = DateTime.Now;
            model.LastUpdatedBy = currentUserId;
            model.LastUpdateDate = DateTime.Now;
            await _context.DcsCustomerInfo.AddAsync(model);

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


            return new FuncResult() { IsSuccess = true, Content = model, Message = "添加成功" };
        }



    }
}
