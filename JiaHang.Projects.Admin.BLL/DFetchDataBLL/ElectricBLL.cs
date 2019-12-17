using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DFetchData.Electric;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.DFetchDataBLL
{
    public class ElectricBLL
    {
        private readonly DataContext context;

        public ElectricBLL(DataContext _context)
        {
            this.context = _context;
        }

        public FuncResult GetList(SearchElectricModel model = null)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from e in context.ApdFctElectric
                            join o in context.ApdDimOrg on e.OrgCode equals o.OrgCode
                            select new ReturnElectricModel()
                            {
                                PeriodYear=e.PeriodYear,
                                RecordId = e.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                NetSupply = e.NetSupply,
                                Spontaneous = e.Spontaneous,
                                Remark = e.Remark
                            };

                query = query.Where(f => (
                                  (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
                                  (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
                                  (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
                                  ));

                fr.Content = query.ToList();
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }

        public FuncResult GetListPagination(SearchElectricModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from e in context.ApdFctElectric
                            join o in context.ApdDimOrg on e.OrgCode equals o.OrgCode
                            select new
                            {
                                PeriodYear = e.PeriodYear,
                                RecordId = e.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                NetSupply = e.NetSupply,
                                Spontaneous = e.Spontaneous,
                                Remark = e.Remark
                            };
                query = query.Where(f => (
                                    (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Contains(model.orgcode)) &&
                                    (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Contains(model.orgname)) &&
                                    (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
                                    ));
                int count = query.Count();
                if (model.limit * model.page >= count)
                {
                    model.page = 0;
                }
                var pagination = query.Skip(model.limit * model.page).Take(model.limit);
                fr.Content = new { total = count, data = pagination };
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }

        public FuncResult Update(string recordid, PostElectricModel model,string userid)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                if (string.IsNullOrWhiteSpace(recordid))
                {
                    fr.IsSuccess = false;
                    fr.Message = "参数接收异常!";
                    return fr;
                }
                ApdFctElectric electric = context.ApdFctElectric.FirstOrDefault(f => f.RecordId.Equals(Convert.ToDecimal(recordid)));
                electric.NetSupply = model.NetSupply;
                electric.Spontaneous = model.Spontaneous;
                electric.Remark = model.Remark;
                electric.LastUpdateDate = DateTime.Now;
                electric.LastUpdatedBy = Convert.ToDecimal(userid);

                context.ApdFctElectric.Update(electric);
                context.SaveChanges();
                return fr;
            }
            catch (Exception ex)
            {
                fr.IsSuccess = false;
                fr.Message = $"{ex.InnerException},{ex.Message}";
                throw new Exception("error", ex);
            }
        }

        public FuncResult WriteData(IEnumerable<ApdFctElectric> list,string year,string userid)
        {
            /*
             * 同一年，一个企业只能导入一次
             * **/
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                if (dm != null && dm.Count() > 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = $"未找到配置的企业信息，统一信息代码为{string.Join(',', dm.Select(g => g.OrgCode))}！";
                    return fr;
                }
                var orgcodegroupby = list.GroupBy(g => new { g.OrgCode, g.PeriodYear }).Select(s => new {OrgCode = s.Key.OrgCode,PeriodYear = s.Key.PeriodYear, Count = s.Count() });
                foreach (var item in orgcodegroupby)
                {
                    if (item.Count > 1)
                    {
                        fr.IsSuccess = false;
                        fr.Message = $"统一信用代码为：{item.OrgCode}的企业在{item.PeriodYear}年上传了{item.Count}条数据!" ;
                        return fr;
                    }
                }
                foreach (var item in list)
                {
                    if (isAlreadyExport(item.OrgCode,year))
                    {
                        //删掉
                        //continue;
                    }
                    item.CreationDate = DateTime.Now;
                    item.CreatedBy = Convert.ToDecimal(userid);
                    context.ApdFctElectric.Add(item);
                }
                //context.ApdFctElectric.AddRange(list);  
                using (IDbContextTransaction trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogService.WriteError(ex);
                        trans.Rollback();
                        fr.IsSuccess = false;
                        fr.Message = $"{ex.InnerException},{ex.Message}!";
                        return fr;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError(ex);
                fr.IsSuccess = false;
                fr.Message = $"{ex.InnerException},{ex.Message}!";
                return fr;
            }

            return fr;
        }

        /// <summary>
        /// 处理某机构某年是否已导入数据(存在的话直接删掉，不用返回数据)
        /// </summary>
        /// <param name="orgcode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool isAlreadyExport(string orgcode, string year)
        {
            try
            {
                var formatyear = Convert.ToDecimal(year);
                var elec = context.ApdFctElectric.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (elec != null || elec.Count() > 0)
                {
                    context.ApdFctElectric.RemoveRange(elec);
                    context.SaveChanges();
                }
                return elec != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }

    public class ReturnElectricModel
    {
        public decimal? PeriodYear { get; set; }
        public decimal RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public decimal? NetSupply { get; set; }
        public decimal? Spontaneous { get; set; }
        public string Remark { get; set; }
    }
}
