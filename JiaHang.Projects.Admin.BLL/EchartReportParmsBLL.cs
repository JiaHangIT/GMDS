using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.DAL;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;

namespace JiaHang.Projects.Admin.BLL
{
    public class EchartReportParmsBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public EchartReportParmsBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        public FuncResult GetMuAdd()
        {
            string sql = "select * from JH_APD.VIEW_ADDED_VALUE_PER_MU";
            List<JHAPDViewAddedValuePERMU> list = OracleDbHelper.Query<JHAPDViewAddedValuePERMU>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult GetIndustryData(string name,int score1,int score2)
        {
            string sql = "";
            if (name == "" || name == null)
            {
                sql = "select INDUSTRY,count(*) as Count from  JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL WHERE COMPOSITE_SCORE <="+ score2 + " and COMPOSITE_SCORE >" + score1 + "  group by INDUSTRY";
            }
            else
            {
                sql = "select INDUSTRY,sum(" + name + ") as Count from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL WHERE COMPOSITE_SCORE <=" + score2 + " and COMPOSITE_SCORE >" + score1 + " group by INDUSTRY";
            }
            List<IndustryCount> list = OracleDbHelper.Query<IndustryCount>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult GetTownData(string name,int score1,int score2)
        {
            string sql = "";
            if (name == "" || name == null)
            {
                sql = "select Town,count(*) as Count from  JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL  WHERE COMPOSITE_SCORE <=" + score2 + " and COMPOSITE_SCORE >" + score1 + " group by Town";
            }
            else
            {
                sql = "select Town,sum(" + name + ") as Count from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL  WHERE COMPOSITE_SCORE <=" + score2 + " and COMPOSITE_SCORE >" + score1 + " group by Town";
            }
            List<TownCount> list = OracleDbHelper.Query<TownCount>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult GetScorePercentage() {
            string sql= "select count(*) as count  from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL";
            string sql1 = "select count(*) as count  from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL where COMPOSITE_SCORE > 90";
            string sql2 = "select count(*) as count  from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL where COMPOSITE_SCORE > 80 and COMPOSITE_SCORE <=90";
            string sql3 = "select count(*) as count  from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL where COMPOSITE_SCORE > 60 and COMPOSITE_SCORE <=80";
            string sql4 = "select count(*) as count  from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL where COMPOSITE_SCORE <=60";
            List<ScoreCount> list = OracleDbHelper.Query<ScoreCount>(sql.ToString());
            List<ScoreCount> list1 = OracleDbHelper.Query<ScoreCount>(sql1.ToString());
            List<ScoreCount> list2 = OracleDbHelper.Query<ScoreCount>(sql2.ToString());
            List<ScoreCount> list3 = OracleDbHelper.Query<ScoreCount>(sql3.ToString());
            List<ScoreCount> list4 = OracleDbHelper.Query<ScoreCount>(sql4.ToString());
            decimal value = 0;
            decimal value1 = 0;
            decimal value2 = 0;
            decimal value3 = 0;
            decimal value4 = 0;
            foreach (var item in list) {
                value = (decimal)item.Count;
            };
            foreach (var item in list1)
            {
                value1 = (decimal)item.Count;
            };
            foreach (var item in list2)
            {
                value2 = (decimal)item.Count;
            };
            foreach (var item in list3)
            {
                value3 = (decimal)item.Count;
            };
            foreach (var item in list4)
            {
                value4 = (decimal)item.Count;
            };
            string list1percentage = (Convert.ToDouble(value1) / Convert.ToDouble(value)).ToString("0.00%");
            string list2percentage = (Convert.ToDouble(value2) / Convert.ToDouble(value)).ToString("0.00%");
            string list3percentage = (Convert.ToDouble(value3) / Convert.ToDouble(value)).ToString("0.00%");
            string list4percentage = (Convert.ToDouble(value4) / Convert.ToDouble(value)).ToString("0.00%");
            return new FuncResult() { IsSuccess = true, Content = new { value1, value2, value3, value4, list1percentage ,list2percentage ,list3percentage ,list4percentage } };
        }
    }
    public class ScoreCount {
        public decimal Count { get; set; }
    }
    public class TownCount {
        public string Town { get; set; }
        public decimal Count { get; set; }
    }
}
