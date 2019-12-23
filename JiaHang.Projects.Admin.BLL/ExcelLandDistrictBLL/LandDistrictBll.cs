using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.ExcelLandDistrictBLL
{
    public class LandDistrictBll
    {
        private readonly DataContext context;

        public LandDistrictBll(DataContext _context) { this.context = _context; }
        /// <summary>
        /// 写入到数据表
        /// ?
        /// </summary>
        /// <returns></returns>
        public FuncResult WriteData(IEnumerable<ApdFctLandDistrict> list, string year)
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
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
                foreach (var item in list)
                {
                    if (isAlreadyExport(item.OrgCode, year))
                    {
                        //continue;
                    }
                    context.ApdFctLandDistrict.Add(item);
                }
                //list.ToList().ForEach(c => c.PeriodYear = _year);
                //context.ApdFctGas.AddRange(list);
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
                throw new Exception("error", ex);
            }
            return fr;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public FuncResult GetList()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                var query = from c in context.ApdFctLandDistrict  
                            join o in context.ApdDimOrg on c.OrgCode equals o.OrgCode
                            select new DistricModelT()
                            {
                                //re = c.RecordId,
                                OrgName = o.OrgName,
                                Town = o.Town,
                                OrgCode = o.OrgCode,
                                RegistrationType = o.RegistrationType,
                                Address = o.Address,
                                PeriodYear = o.PeriodYear,
                                LandNo = c.LandNo,
                                Area = c.Area,
                                ShareDesc = c.ShareDesc,
                                RightType = c.RightType,
                                Purpose = c.Purpose,
                                BeginDate = c.BeginDate,
                                End_Date=c.EndDate,
                                Remark = c.Remark
                            };

                fr.Content = query.ToList();
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
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
                var pollu = context.ApdFctGas.Where(f => f.OrgCode.Equals(orgcode) && f.PeriodYear.Equals(formatyear));
                if (pollu != null || pollu.Count() > 0)
                {
                    context.ApdFctGas.RemoveRange(pollu);
                    context.SaveChanges();
                }
                return pollu != null;
            }
            catch (Exception ex)
            {

                throw new Exception("error", ex);
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<ApdFctLandDistrict> entitys = context.ApdFctLandDistrict.Where(e => ids.Contains(e.RecordId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (ApdFctLandDistrict obj in entitys)
            {
                //删除
                context.ApdFctLandDistrict.Remove(obj);
            }
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = context.Database.BeginTransaction())
            {
                try
                {
                    await context.SaveChangesAsync();
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
    public class DistricModelT
    {
        //public decimal RecordId { get; set; }
        public decimal PeriodYear { get; set; }
        public decimal Count { get; set; }
        public decimal Key { get; set; }
        public string OrgName { get; set; }
        public string Town { get; set; }
        public string OrgCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string LegalRepresentative { get; set; }
        public string Phone { get; set; }
        public string LinkMan { get; set; }
        public string Phone2 { get; set; }

        public string LandNo { get; set; }
        public decimal? Area { get; set; }
        public string ShareDesc { get; set; }
        public string RightType { get; set; }
        public string Purpose { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? End_Date { get; set; }

        public decimal? LeaseLand { get; set; }
        public string Remark { get; set; }
        public DateTime? Create { get; set; }
    }
}
