using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.LandDistrictImportA
{
    [Route("api/[controller]")]
    //[ApiController]
    public class LandDistrictImportController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment hosting;

        public LandDistrictImportController(DataContext _context, IHostingEnvironment _hosting, IMemoryCache _cache) { this.context = _context; this.hosting = _hosting; this.cache = _cache; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetList")]
        public FuncResult GetList([FromBody] RequestLandTown model = null)
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };

            //条件查询情况下，需要重新考虑Count值的问题
            model.page--; if (model.page < 0)
            {
                model.page = 0;
            }

            var query = from t1 in context.ApdFctLandDistrict.Where(f => f.DeleteFlag == 0)                       
                        join o in context.ApdDimOrg on t1.OrgCode equals o.OrgCode
                        select new DistricModel
                        {
                            //Array = listnew,
                            OrgName = o.OrgName,
                            Town = o.Town,
                            OrgCode = o.OrgCode,
                            RegistrationType = o.RegistrationType,
                            Address = o.Address,
                            LegalRepresentative = o.LegalRepresentative,
                            Phone = o.Phone,
                            LinkMan = o.LinkMan,
                            Phone2 = o.Phone2,
                            LandNo = t1.LandNo,
                            Area = t1.Area,
                            ShareDesc = t1.ShareDesc,
                            RightType = t1.RightType,
                            Purpose= t1.Purpose,
                            BeginDate = t1.BeginDate,
                            End_Date=t1.EndDate
                        };
            query = query.Where(f => (
            (string.IsNullOrWhiteSpace(model.orgcode) || f.OrgCode.Equals(model.orgcode)) &&
            (string.IsNullOrWhiteSpace(model.orgname) || f.OrgName.Equals(model.orgname)) &&
            (string.IsNullOrWhiteSpace(model.year) || f.PeriodYear.Equals(Convert.ToDecimal(model.year)))
            )).OrderBy(o => o.Create);

            /*
             * 通过groupby data来分页
             * groupby 处理后，再根据groupby data自来query data
             * **/

            var querygroup = query.GroupBy(g => new { g.OrgCode, g.RegistrationType,g.LeaseLand, g.Key }).OrderBy(o => o.Key.Key);
            int count = querygroup.Count();
            var l = querygroup.Skip(model.limit * model.page).Take(model.limit);


            var list = new List<int>();
            //重新定义query里count的值

            var queryr = new List<DistricModel>();
            foreach (var item in l)
            {
                //query.Where(f => f.Key == item.Key.Key).ToList().ForEach(p => p.Count = item.Count());

                var currentquery = query.Where(f => f.Key == item.Key.Key).ToList();
                foreach (var itemquery in currentquery)
                {
                    itemquery.Count = item.Count();
                    queryr.Add(itemquery);
                }
                //listResut.Where(w => w.CategoryID > 30 && w.CategoryID < 40).ToList().ForEach(p => p.CategoryName = p.CategoryName + "bb");
                int c = item.Count();
                list.Add(c);
            }

            var listnew = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    listnew.Add(0);
                }
                else if (i == 1)
                {
                    listnew.Add(list[0]);
                }
                else
                {
                    //listnew.Add(list[i - 1] + list[i - 2]);
                    listnew.Add(list.Take(i).Sum());
                }
            }
            result.Content = new { data = queryr, array = listnew, total = count };
            return result;
        }
    }
    public class DistricModel
    {
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