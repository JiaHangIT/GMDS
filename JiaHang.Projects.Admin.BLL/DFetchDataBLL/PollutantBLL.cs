using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.DFetchDataBLL
{
    public class PollutantBLL
    {
        private readonly DataContext context;

        public PollutantBLL(DataContext _context) { this.context = _context; }

        public async Task<FuncResult> GetListPagination()
        {
            FuncResult fr = new FuncResult() { IsSuccess = true, Message = "Ok" };
            try
            {
                return fr;
            }
            catch (Exception ex)
            {

                throw new Exception("error",ex);
            }
        }

        /// <summary>
        /// 写入到数据表
        /// ?
        /// </summary>
        /// <returns></returns>
        public bool WriteData(IEnumerable<ApdFctContaminants> list)
        {
            try
            {
               
                var dm = list.Where(f => !context.ApdDimOrg.Select(g => g.OrgCode).Contains(f.OrgCode));
                if (dm != null || dm.Count() > 0)
                {
                    return false;
                }

                context.ApdFctContaminants.AddRange(list);
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
                        return false;
                        throw new Exception("error", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("error",ex);
            }
            return true;
        }
    }
}
