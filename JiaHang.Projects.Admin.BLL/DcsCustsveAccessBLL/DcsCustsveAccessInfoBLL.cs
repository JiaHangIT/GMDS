using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;

namespace JiaHang.Projects.Admin.BLL
{
    public class DcsCustsveAccessInfoBLL
    {
        private readonly DataContext _context;
        public DcsCustsveAccessInfoBLL(DataContext context)
        {
            _context = context;
        }

        public FuncResult List(int pageSize,int currentPage,string accountId) {
            var query = from acsinfo in _context.DcsCustsveAccessInfo
                        //where acsinfo.CustomerId== accountId
                        join serviceInfo in _context.DcsServiceInfo on acsinfo.ServiceId equals serviceInfo.ServiceId
                        orderby acsinfo.AccessDate descending
                        select new
                        {
                            acsinfo.AccessId,
                            AccessExeTime=acsinfo.AccessExeTime+" 毫秒",
                            acsinfo.AccessIp,
                            AccessDate=acsinfo.AccessDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            acsinfo.AccessResultFlag,
                            acsinfo.ReturnDataNum,
                            CreationDate= acsinfo.CreationDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            serviceInfo.ServiceCode,
                        };
            var total = query.Count();
            var data = query.Skip(pageSize * currentPage).Take(pageSize);
            return new FuncResult { IsSuccess=true,Content=new { data,total} };
        }

        public FuncResult Detail(string accessId) {
            var data = _context.DcsCustsveAccessInfo.FirstOrDefault(e => e.AccessId == accessId);
            return new FuncResult() { IsSuccess=true,Content=data};
        }
    }

}
