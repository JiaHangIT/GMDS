using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.Common;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace TestElement.Controllers.API
{
    [Route("api/[controller]")]
    //[ApiController]
    public class QiYeFileController : ControllerBase
    {
        public readonly DataContext context;

        public QiYeFileController(DataContext _context) { this.context = _context; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public FuncResult GetList()
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };

            var query = from t1 in context.ApdFctLandTown
                        join t2 in context.ApdFctLandTown2 on t1.T2Id equals t2.RecordId
                        join o in
context.ApdDimOrg on t1.OrgCode equals o.OrgCode
                        select new
                        {
                            //Array = listnew,
                            Count = t2.Count,
                            Key = t2.RecordId,
                            OrgName = o.OrgName,
                            Town = o.Town,
                            OrgCode = o.OrgCode,
                            RegistrationType = o.RegistrationType,
                            Address = o.Address,
                            LegalRepresentative = o.LegalRepresentative,
                            Phone = o.Phone,
                            LinkMan = o.LinkMan,
                            Phone2 = o.Phone2,
                            OwnershipLand = t1.OwnershipLand,
                            ProtectionLand = t1.ProtectionLand,
                            ReduceLand = t1.ReduceLand,
                            FactLand = t2.FactLand,
                            RentLand = t2.RentLand,
                            LeaseLand = t2.LeaseLand,
                            Remark = t2.Remark,
                            Create = t2.CreationDate
                        };
            query = query.OrderBy(o => o.Create);
            var l = query.GroupBy(g => new { g.OrgCode, g.RegistrationType, g.FactLand, g.RentLand, g.LeaseLand,g.Key }).OrderBy(o=>o.Key.Key);

            var list = new List<int>();

            foreach (var item in l)
            {
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
                    listnew.Add(list[i - 1] + list[i - 2]);
                }
            }
            result.Content = new { data = query, array = listnew };
            return result;
        }

        /// <summary>
        /// excel数据导入到数据库(mapcoderela表)
        /// </summary>
        /// <param name="excelfile"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        public FuncResult Import()
        {
            FuncResult result = new FuncResult() { IsSuccess = true, Message = "Success" };
            try
            {
                var excelfile = Request.Form.Files[0];
                List<Demo> datalist = new List<Demo>();
                if (excelfile.Length > 0)
                {
                    DataTable dt = new DataTable();
                    string strMsg;
                    string filename = excelfile.FileName;//文件名即为区域名
                    //利用IFormFile里面的OpenReadStream()方法直接读取文件流
                    dt = ExcelHelper.ExcelToDatatable(excelfile.OpenReadStream(), Path.GetExtension(excelfile.FileName), out strMsg);
                    if (!string.IsNullOrEmpty(strMsg))
                    {
                        result.IsSuccess = false;
                        result.Message = strMsg;
                        return result;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        var listorgan = context.ApdDimOrg.ToList();
                        //需要导入到数据库的数据
                        datalist = JsonConvert.DeserializeObject<List<Demo>>(JsonConvert.SerializeObject(dt));
                        List<Demo> filterdata = datalist.Where(f => !(f.G1 == "" && f.G2 == "" && f.G3 == "" && f.G4 == "" && f.G5 == "" && f.G6 == "" && f.G7 == "" && f.G8 == "")).ToList();

                        //处理筛选过后的数据
                        string g1 = string.Empty;
                        string g3 = string.Empty;
                        foreach (var item in filterdata)
                        {
                            if (!string.IsNullOrEmpty(item.G1))
                            {
                                g1 = item.G1;
                            }
                            if (string.IsNullOrEmpty(item.G1))
                            {
                                //为上一个有值的g1
                                item.G1 = g1;
                            }

                            if (!string.IsNullOrEmpty(item.G3))
                            {
                                g3 = item.G3;
                            }
                            if (string.IsNullOrEmpty(item.G3))
                            {
                                //为上一个有值的g3
                                item.G3 = g3;
                            }
                        }

                        var groupdata_1 = filterdata.GroupBy(g => new { g.G1, g.G3, g.G10, g.G11, g.G12 }).Select(s => new ReflctModel1
                        {
                            OWNERSHIPLAND = s.Key.G10,
                            PROTECTIONLAN = s.Key.G11,
                            REDUCELAND = s.Key.G12,
                            ORGANIZATION = s.Key.G1,
                            ORGCODE = s.Key.G3
                        });

                        var groupdata_2 = filterdata.GroupBy(g => new { g.G1, g.G3, g.G13, g.G14, g.G15, g.G16 }).Select(s => new ReflctModel2
                        {
                            FACTLAND = s.Key.G13,
                            RENTLAND = s.Key.G14,
                            LEASELAND = s.Key.G15,
                            REMARK = s.Key.G16,
                            ORGANIZATION = s.Key.G1,
                            ORGCODE = s.Key.G3
                        }).Where(f => (f.FACTLAND != null && f.RENTLAND != null && f.LEASELAND != null) == true);

                        decimal currenttown2key = 0;//本次apd_fct_town2表的主键
                        var town2context = context.ApdFctLandTown2.OrderByDescending(o => o.RecordId).ToList();
                        if (town2context == null || town2context.Count() <= 0)
                        {
                            currenttown2key = 1;
                        }
                        else
                        {
                            currenttown2key = town2context[0].RecordId + 1;
                        }
                        //存在orgcode不存在的情况就整个都不写入
                        foreach (var item in groupdata_1)
                        {
                            var currentorganization = listorgan.FirstOrDefault(f => f.OrgCode.Equals(item.ORGCODE));
                            if (currentorganization == null)
                            {
                                result.IsSuccess = false;
                                result.Message = $"此机构号:{item.ORGCODE}找不到对应机构，导入失败！";
                                return result;
                            }
                            ApdFctLandTown t1 = new ApdFctLandTown()
                            {
                                OrgCode = item.ORGCODE,
                                OwnershipLand = item.OWNERSHIPLAND,
                                ProtectionLand = item.PROTECTIONLAN,
                                ReduceLand = item.REDUCELAND,
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                PeriodYear = DateTime.Now.Year,
                                RecordId = new Random().Next(1, 999),
                                T2Id = currenttown2key
                            };
                            context.ApdFctLandTown.Add(t1);
                        }

                        foreach (var item in groupdata_2)
                        {
                            ApdFctLandTown2 t2 = new ApdFctLandTown2()
                            {
                                OrgCode = item.ORGCODE,
                                FactLand = item.FACTLAND,
                                RentLand = item.RENTLAND,
                                LeaseLand = item.LEASELAND,
                                Remark = item.REMARK,
                                PeriodYear = DateTime.Now.Year,
                                RecordId = new Random().Next(1, 999),
                                CreationDate = DateTime.Now,
                                LastUpdateDate = DateTime.Now,
                                Count = groupdata_1.Count()
                            };
                            context.ApdFctLandTown2.Add(t2);
                        }

                        using (IDbContextTransaction trans = context.Database.BeginTransaction())
                        {
                            try
                            {
                                context.SaveChangesAsync();
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {

                                throw new Exception("error", ex);
                            }
                        }

                        //将groupdata_1、groupdata_2写入到两张表

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "excel无数据！";
                    }
                }



                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"{ex.Message},{ex.InnerException}";
                return result;
            }

        }
    }

    public class Demo
    {
        public string G1 { get; set; }

        public string G2 { get; set; }

        public string G3 { get; set; }

        public string G4 { get; set; }

        public string G5 { get; set; }

        public string G6 { get; set; }

        public string G7 { get; set; }

        public string G8 { get; set; }

        public string G9 { get; set; }

        public int? G10 { get; set; }

        public int? G11 { get; set; }

        public int? G12 { get; set; }

        public int? G13 { get; set; }

        public int? G14 { get; set; }

        public int? G15 { get; set; }

        public string G16 { get; set; }
    }

    /// <summary>
    /// APD_FCT_LAND_TOWN
    /// </summary>
    public class ReflctModel1
    {
        /// <summary>
        /// G10
        /// </summary>
        public int? OWNERSHIPLAND { get; set; }

        /// <summary>
        /// G11
        /// </summary>
        public int? PROTECTIONLAN { get; set; }

        /// <summary>
        /// G12
        /// </summary>
        public int? REDUCELAND { get; set; }

        /// <summary>
        /// G1
        /// </summary>
        public string ORGANIZATION { get; set; }

        /// <summary>
        /// G3
        /// </summary>
        public string ORGCODE { get; set; }
    }

    /// <summary>
    /// APD_FCT_LAND_TOWN2
    /// </summary>
    public class ReflctModel2
    {
        /// <summary>
        /// G13
        /// </summary>
        public int? FACTLAND { get; set; }

        /// <summary>
        /// G14
        /// </summary>
        public int? RENTLAND { get; set; }

        /// <summary>
        /// G15
        /// </summary>
        public int? LEASELAND { get; set; }

        /// <summary>
        /// G16
        /// </summary>
        public string REMARK { get; set; }

        /// <summary>
        /// G1
        /// </summary>
        public string ORGANIZATION { get; set; }

        /// <summary>
        /// G3
        /// </summary>
        public string ORGCODE { get; set; }
    }
}