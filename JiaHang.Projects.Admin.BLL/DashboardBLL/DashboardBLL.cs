using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.DashboardBLL
{
    public class DashboardBLL
    {
        private readonly DataContext _context;
        public DashboardBLL(DataContext dataContext)
        {
            _context = dataContext;
        }
        public FuncResult Statistics()
        {
            var totalCount = _context.DcsCustsveAccessInfo.Count();//累计接口调用数量
            var start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var todayData = _context.DcsCustsveAccessInfo.Where(e => e.AccessDate >= start).ToList();
            var todayCount = todayData.Count();//当日接口调用数量

            //var totalgrowth = todayCount / totalCount * 100 + "%"; //累计同比增长
            //var yesterdayCount = _context.DcsCustsveAccessInfo.
            //    Where(e => e.AccessDate > Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00")) 
            //    && e.AccessDate< Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"))).Count();// 昨天接口调用数量

            //var todaygrowth = todayCount/yesterdayCount *100 +"%";//今天同比增长

            var serviceCount = _context.DcsServiceInfo.Count();//接口数量
            var customerCount = _context.DcsCustomerInfo.Count();//用户数量

            var chart1data = Chart1Data(todayData);
            var chart2data = Chart2Data(todayData);

            var recentlog = todayData.OrderByDescending(e => e.AccessDate).Take(5).ToList();
            var recentlogdata = from a in recentlog
                                join b in _context.DcsServiceInfo on a.ServiceId equals b.ServiceId
                                join c in _context.DcsCustomerInfo on a.CustomerId equals c.CustomerId
                                select new
                                {
                                    b.ServiceName,
                                    AccessResultFlag= a.AccessResultFlag==1?"成功":"失败",
                                    AccessDate = a.AccessDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                    AccessExeTime= a.AccessExeTime+" 毫秒",
                                    c.CustomerName
                                };

            return new FuncResult() { IsSuccess = true, Content = new { totalCount, todayCount, serviceCount, customerCount, chart1data , chart2data, recentlogdata } };
        }
        private object Chart1Data(List<DcsCustsveAccessInfo> todaydata)
        {
            //5分钟统计一次数量
            var start = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var end = Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString());
            List<object> data = new List<object>();
            while (start <= end)
            {
                var count = todaydata.Where(e => e.AccessDate >= start && e.AccessDate <= start.AddMinutes(5)).Count();
                var ec = (long)(start - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                data.Add(new object[] { ec, count });
                start = start.AddMinutes(5);
            }
            return data;
        }

        /// <summary>
        /// 当天每个分类的接口调用情况
        /// 取调用最多的前三
        /// </summary>
        /// <param name="todaydata"></param>
        /// <returns></returns>
        private object Chart2Data(List<DcsCustsveAccessInfo> todaydata)
        {
            var serviceids = todaydata.Select(e => e.ServiceId).Distinct().ToList();
            var serviceinfo = (from a in _context.DcsServiceInfo
                              where serviceids.Contains(a.ServiceId)
                              join b in _context.DcsServiceGroup
                              on a.ServiceGroupId equals b.ServiceGroupId
                              select new
                              {
                                  a.ServiceId,
                                  b.ServiceGroupId,
                                  b.ServiceGroupName
                              }).ToList();

            var servicelog = from a in todaydata
                             join b in serviceinfo
                             on a.ServiceId equals b.ServiceId
                             select new
                             {                                 
                                 b.ServiceGroupId,
                                 b.ServiceGroupName
                             };

            var groups = servicelog.GroupBy(e => e.ServiceGroupId).Select(c => new
            {
                c.Key,
                c.First().ServiceGroupName,
                count = c.Count()
            });
            var tops = groups.OrderByDescending(e => e.count).Take(5);
            var categories = new List<string>();
            var counts = new List<int>();
            foreach (var obj in tops) {
                categories.Add(obj.ServiceGroupName);
                counts.Add(obj.count);
            }
            return (categories,counts);
        }


    }
}
