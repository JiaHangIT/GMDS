﻿using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DFetchData.Rd;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.DFetchDataBLL
{
    public class RdBLL
    {
        private readonly DataContext context;

        public RdBLL(DataContext _context) { this.context = _context; }

        public FuncResult GetList()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from r in context.ApdFctRD
                            join o in context.ApdDimOrg on r.OrgCode equals o.OrgCode
                            select new ReturnRDModel()
                            {
                                RecordId = r.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                IsHighTech = r.IsHighTech,
                                RDExpenditure = r.RDExpenditure,
                                Remark = r.Remark
                            };

                fr.Content = query.ToList();
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }

        public FuncResult GetListPagination(SearchRdModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from r in context.ApdFctRD
                            join o in context.ApdDimOrg on r.OrgCode equals o.OrgCode
                            select new
                            {
                                PeriodYear = r.PeriodYear,
                                RecordId = r.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                IsHighTech = r.IsHighTech,
                                RDExpenditure = r.RDExpenditure,
                                Remark = r.Remark
                            };
                query = query.Where(f => (
                                    (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
                                    (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
                                    (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
                                    ));
                int count = query.Count();
                if (model.limit * model.page > count)
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

        public FuncResult Update(string recordid, PostRdModel model,string userid)
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
                ApdFctRD rd = context.ApdFctRD.FirstOrDefault(f => f.RecordId.Equals(Convert.ToDecimal(recordid)));
                rd.IsHighTech = model.IsHighTech;
                rd.RDExpenditure = model.RDExpenditure;
                rd.Remark = model.Remark;
                rd.LastUpdateDate = DateTime.Now;
                rd.LastUpdatedBy = Convert.ToDecimal(userid);

                context.ApdFctRD.Update(rd);
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

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public FuncResult WriteData(IEnumerable<ApdFctRD> list,string year,string userid)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var _year = Convert.ToDecimal(year);
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                if (dm != null && dm.Count() > 0)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到配置的企业信息";
                    return fr;
                }
                foreach (var item in list)
                {
                    if (isAlreadyExport(item.OrgCode,year))
                    {
                        //continue;
                    }
                    item.CreationDate = DateTime.Now;
                    item.CreatedBy = Convert.ToDecimal(userid);
                    context.ApdFctRD.Add(item);
                }
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                //context.ApdFctRD.AddRange(list);
                using (IDbContextTransaction trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.SaveChanges();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        fr.IsSuccess = false;
                        fr.Message = $"{ex.InnerException},{ex.Message}!";
                        return fr;
                        throw new Exception("error", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                fr.IsSuccess = false;
                fr.Message = $"{ex.InnerException},{ex.Message}!";
                return fr;
                throw new Exception("error",ex);
            }
            return fr;
        }

        public FuncResult Delete()
        {
            try
            {
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 处理某机构某年是否已导入数据
        /// </summary>
        /// <param name="orgcode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool isAlreadyExport(string orgcode, string year)
        {
            try
            {
                var formatyear = Convert.ToDecimal(year);
                var rd = context.ApdFctRD.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (rd != null || rd.Count() > 0)
                {
                    context.ApdFctRD.RemoveRange(rd);
                    context.SaveChanges();
                }
                return rd != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
    }

    public class ReturnRDModel
    {
        public decimal RecordId { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string IsHighTech { get; set; }
        public decimal? RDExpenditure { get; set; }
        public string Remark { get; set; }
    }
}
