using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.Model;
namespace JiaHang.Projects.Admin.BLL
{
   public class ApdFctOrgIndexVBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public ApdFctOrgIndexVBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        //查询所有
        public FuncResult Select(int pageSize, int currentPage,string OrgName) {
            var query = from a in _context.ApdFctOrgIndexV
                        where (
                               (string.IsNullOrWhiteSpace(OrgName) || a.OrgName.ToString().Contains(OrgName))
                             )
                        select new
            {
                            PerIodYear = a.PerIodYear,
                            OrgCode = a.OrgCode,
                            OrgName = a.OrgName,
                            Industry = a.Industry,
                            CompositeScore = a.CompositeScore,
                            TaxPerMu = a.TaxPerMu,
                            AddValuePerMu = a.AddValuePerMu,
                            Productivity=   a.Productivity
            };
            int total = query.Count();
            var data = query.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
    }
}
