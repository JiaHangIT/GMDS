using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.OrgBLL
{
    public class DimOrgBll
    {
        private readonly DataContext context;

        public DimOrgBll(DataContext _context)
        {
            this.context = _context;
        }

        public FuncResult GetListPagination(SearchOrgModel model)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                var query = from o in context.ApdDimOrg select o;
                int count = query.Count();
                if (model.limit * model.page > count)
                {
                    model.page = 0;
                }
                query = query.Where(f => (string.IsNullOrEmpty(model.orgname) || f.OrgName.Equals(model.orgname)) &&
                                         (string.IsNullOrEmpty(model.orgcode) || f.OrgCode.Equals(model.orgcode)));
                var data = query.Skip(model.limit * model.page).Take(model.limit);

                fr.Content = new { data = data, total = count };
                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult Add(PostOrgModel mode,string userid)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "操作成功" };
            try
            {
                ApdDimOrg org = new ApdDimOrg()
                {
                    RecordId = new Random().Next(1, 9999),
                    CreationDate = DateTime.Now,
                    //CreatedBy = Convert.ToDecimal(userid),
                    LastUpdateDate = DateTime.Now,
                    //LastUpdatedBy = Convert.ToDecimal(userid)
                };
                org = MappingHelper.Mapping(org, mode);

                context.ApdDimOrg.Add(org);
                context.SaveChanges();
                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult Update(string recordid,PostOrgModel model,string userid)
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

                ApdDimOrg existorg = context.ApdDimOrg.FirstOrDefault(f => f.RecordId.Equals(Convert.ToDecimal(recordid)));
                if (existorg == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到企业信息，请确定是否已删除!";
                    return fr;
                }
                existorg.LastUpdateDate = DateTime.Now;
                //existorg.LastUpdatedBy = Convert.ToDecimal(userid);
                existorg = MappingHelper.Mapping(existorg, model);
                context.ApdDimOrg.Update(existorg);
                context.SaveChanges();

                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public FuncResult Delete(string recordid,string userid)
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

                ApdDimOrg existorg = context.ApdDimOrg.FirstOrDefault(f => f.RecordId.Equals(Convert.ToDecimal(recordid)));
                if (existorg == null)
                {
                    fr.IsSuccess = false;
                    fr.Message = "未找到企业信息，请确定是否已删除!";
                    return fr;
                }

                existorg.LastUpdateDate = DateTime.Now;
                //existorg.LastUpdatedBy = Convert.ToDecimal(userid);
                existorg.DeleteFlag = 1;
                existorg.DeleteDate = DateTime.Now;
                existorg.DeleteBy = userid;
                context.ApdDimOrg.Update(existorg);
                context.SaveChanges();

                return fr;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
