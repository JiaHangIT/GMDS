using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.DAL;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;
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
        //public FuncResult Select(int pageSize, int currentPage, string OrgName,string year) {
        //    try { 
        //    StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_SCORE_TOTAL");

        //        List<string> wheres = new List<string>();
        //    if (OrgName != null)
        //    {
        //        wheres.Add(" ORG_NAME like '%" + OrgName + "%'");
        //    }
        //       if (year != null)
        //    {
        //        wheres.Add(" PERIOD_YEAR =" +"'"+ year+"'" );
        //    }
        //    if (wheres.Count > 0)
        //    {
        //        string wh = string.Join(" and ", wheres.ToArray());

        //        sql.Append(" where " + wh);
        //    }
        //    List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
        //        foreach (var item in list) {
        //      item.OWNER_EQUITY= Math.Round(Convert.ToDecimal( item.OWNER_EQUITY/10000), 2);
        //        }
        //    int total = list.Count();
        //        if (pageSize * currentPage > total)
        //        {
        //            currentPage = 0;
        //        }
        //        var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
        //    return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        //    }
        //    catch (Exception ex) { throw new Exception(ex.Message); };
        //}
        public FuncResult Select(int pageSize, int currentPage, string OrgName, string year, string field, string desc)
        {
            try
            {
                StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_SCORE_TOTAL");

                List<string> wheres = new List<string>();
                if (OrgName != null)
                {
                    wheres.Add(" ORG_NAME like '%" + OrgName + "%'");
                }
                if (year != null)
                {
                    wheres.Add(" PERIOD_YEAR =" + "'" + year + "'");
                }
                if (wheres.Count > 0)
                {
                    string wh = string.Join(" and ", wheres.ToArray());

                    sql.Append(" where " + wh);
                }
                if (field != null)
                {
                    sql.Append(" order by " + field + " " + desc);
                }
                List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
                foreach (var item in list)
                {
                    item.OWNER_EQUITY = Math.Round(Convert.ToDecimal(item.OWNER_EQUITY / 10000), 2);
                }
                int total = list.Count();
                if (pageSize * currentPage > total)
                {
                    currentPage = 0;
                }
                var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
                return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex) { throw new Exception(ex.Message); };
        }
        public FuncResult GetAvarageScore(string year)
        {
            StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_AVERAGE");
            if (year != null)
            {
                sql.Append(" where  PERIOD_YEAR=" + year);
            }
            List<AvarageScore> list = OracleDbHelper.Query<AvarageScore>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        //根据城镇下拉框改变获取数据
        public FuncResult SelectTownDdata(int pageSize, int currentPage, string Town, string OrgName, string year, string Industry)
        {

            StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_SCORE_TOTAL");
            List<string> wheres = new List<string>();
            if (Town == "全部")
            {
                Town = null;
            }
            if (Town != null)
            {
                wheres.Add(" TOWN = " + "'" + Town + "'");
            }
            if (OrgName != null)
            {
                wheres.Add(" ORG_NAME like '%" + OrgName + "%'");
            }
            if (year != null)
            {
                wheres.Add(" PERIOD_YEAR =" + "'" + year + "'");
            }
            if (Industry == "全部")
            {
                Industry = null;
            }
            if (Industry != null)
            {
                wheres.Add(" INDUSTRY = " + "'" + Industry + "'");
            }
            if (wheres.Count > 0)
            {
                string wh = string.Join(" and ", wheres.ToArray());

                sql.Append(" where " + wh);
            }
            List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
            int total = list.Count();
            if (pageSize * currentPage > total)
            {
                currentPage = 0;
            }
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        //根据行业下拉框改变获取数据
        public FuncResult SelectIndustryData(int pageSize, int currentPage, string Industry, string OrgName, string year, string Town)
        {

            StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_SCORE_TOTAL");
            List<string> wheres = new List<string>();
            if (Industry == "全部")
            {
                Industry = null;
            }
            if (Industry != null)
            {
                wheres.Add(" INDUSTRY = " + "'" + Industry + "'");
            }
            if (Town == "全部")
            {
                Town = null;
            }
            if (Town != null)
            {
                wheres.Add(" TOWN = " + "'" + Town + "'");
            }
            if (OrgName != null)
            {
                wheres.Add(" ORG_NAME like '%" + OrgName + "%'");
            }
            if (year != null)
            {
                wheres.Add(" PERIOD_YEAR =" + "'" + year + "'");
            }
            if (wheres.Count > 0)
            {
                string wh = string.Join(" and ", wheres.ToArray());

                sql.Append(" where " + wh);
            }
            List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
            int total = list.Count();
            if (pageSize * currentPage > total)
            {
                currentPage = 0;
            }
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public async Task<byte[]> ExportAll(string orgname, string year, string industy, string town, string field, string desc)

        {
            var comlumHeadrs = new[] { "年份", "企业名称", "所属行业", "所在街道(园区)", "综合评分", "亩均税收得分", "亩均增加值得分", "全员劳动生产率得分", "单位排污权增加值得分", "单位能耗增加值得分", "净资产收益率得分", "研发经费投入比得分", "所有者权益（万元）", "年职工人数", "工业增加值", "主要污染排放量", "税收实际贡献", "用地面积", "净利润", "净资产", "主营业务收入", "研发经费支出", "综合能耗", "亩均税收", "亩均增加值", "单位能耗增加值", "单位排污增加值", "全员劳动生产率", "净资产收益率", "研发经费投入比" };
            byte[] result;
            StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_SCORE_TOTAL");
            List<string> wheres = new List<string>();
            if (orgname != null)
            {
                wheres.Add(" ORG_NAME like '%" + orgname + "%'");
            }
            if (year != null)
            {
                wheres.Add(" PERIOD_YEAR =" + "'" + year + "'");
            }
            if (industy == "全部")
            {
                industy = null;
            }
            if (industy != null)
            {
                wheres.Add(" INDUSTRY = " + "'" + industy + "'");
            }
            if (town == "全部")
            {
                town = null;
            }
            if (town != null)
            {
                wheres.Add(" TOWN = " + "'" + town + "'");
            }
            if (wheres.Count > 0)
            {
                string wh = string.Join(" and ", wheres.ToArray());

                sql.Append(" where " + wh);
            }
            if (field != null)
            {
                sql.Append(" order by " + field + " " + desc);
            }
            var data = _context.SysUserInfo.ToList();
            List<ReturnDate> datas = OracleDbHelper.Query<ReturnDate>(sql.ToString());
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
                                                                       //First add the headers
            for (var i = 0; i < comlumHeadrs.Count(); i++)
            {
                worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
            }
            //Add values
            var j = 2;
            // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            await Task.Run(() =>
            {
                foreach (var obj in datas)
                {
                    worksheet.Cells["A" + j].Value = obj.PERIOD_YEAR;
                    worksheet.Cells["B" + j].Value = obj.ORG_NAME;
                    worksheet.Cells["C" + j].Value = obj.INDUSTRY;
                    worksheet.Cells["D" + j].Value = obj.TOWN;
                    worksheet.Cells["E" + j].Value = obj.COMPOSITE_SCORE;
                    worksheet.Cells["F" + j].Value = obj.TAX_PER_MU_SCORE;
                    worksheet.Cells["G" + j].Value = obj.ADD_VALUE_PER_MU_SCORE;
                    worksheet.Cells["H" + j].Value = obj.PRODUCTIVITY_SCORE;
                    worksheet.Cells["I" + j].Value = obj.POLLUTANT_DISCHARGE_SCORE;
                    worksheet.Cells["J" + j].Value = obj.ENERGY_CONSUMPTION_SCORE;
                    worksheet.Cells["K" + j].Value = obj.NET_ASSETS_PROFIT_SCORE;
                    worksheet.Cells["L" + j].Value = obj.R_D_EXPENDITURE_RATIO_SCORE;
                    worksheet.Cells["M" + j].Value = obj.OWNER_EQUITY;
                    worksheet.Cells["N" + j].Value = obj.WORKER_MONTH;
                    worksheet.Cells["0" + j].Value = obj.Industrial_added_value;
                    worksheet.Cells["P" + j].Value = obj.pollutant_discharge2;
                    worksheet.Cells["Q" + j].Value = obj.fact_tax;
                    worksheet.Cells["R" + j].Value = obj.LAND_AREA;
                    worksheet.Cells["S" + j].Value = obj.PROFIT;
                    worksheet.Cells["T" + j].Value = obj.ASSETS;
                    worksheet.Cells["U" + j].Value = obj.MAIN_BUSINESS_INCOME;
                    worksheet.Cells["V" + j].Value = obj.R_D_EXPENDITURE;
                    worksheet.Cells["W" + j].Value = obj.Energy_consumption2;
                    worksheet.Cells["X" + j].Value = obj.TAX_PER_MU;
                    worksheet.Cells["Y" + j].Value = obj.ADD_VALUE_PER_MU;
                    worksheet.Cells["Z" + j].Value = obj.ENERGY_CONSUMPTION;
                    worksheet.Cells["AA" + j].Value = obj.POLLUTANT_DISCHARGE;
                    worksheet.Cells["AB" + j].Value = obj.PRODUCTIVITY;
                    worksheet.Cells["AC" + j].Value = obj.NET_ASSETS_PROFIT;
                    worksheet.Cells["AD" + j].Value = obj.R_D_EXPENDITURE_RATIO;
                    j++;
                }
            });


            result = package.GetAsByteArray();
            return result;
        }
        public FuncResult SelectORGInfo(int pageSize, int currentPage, string OrgCode)
        {
            StringBuilder sql = new StringBuilder("select * from APD_DIM_SUB_ORG where PARENT_ORG_CODE=" + "'" + OrgCode + "'");
            List<SubOrgInfo> list = OracleDbHelper.Query<SubOrgInfo>(sql.ToString());
            int total = list.Count();
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public FuncResult GetTown()
        {
            string sql = "select * from APD_DIM_TOWN";
            List<Town> list = OracleDbHelper.Query<Town>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult GetIndustry()
        {
            string sql = "select * from APD_DIM_INDUSTRY";
            List<Town> list = OracleDbHelper.Query<Town>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult IndustryDetail(string code)
        {
            string sql = "select * from APD_DIM_ORG_V where ORG_CODE=" + "'" + code + "'";
            List<IndustryInfo> list = OracleDbHelper.Query<IndustryInfo>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult BenefiteValuationInfo(string code)
        {
            StringBuilder sql = new StringBuilder("select * from VIEW_COMPANY_INDEX_SCORE_TOTAL where ORG_CODE=" + "'" + code + "'");
            string sql2 = "select count(*) as count  from apd_dim_org";

            List<Counts> result = OracleDbHelper.Query<Counts>(sql2.ToString());
            decimal industyCount = 0;
            foreach (var item in result)
            {
                industyCount = item.Count;
            }
            List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());

            return new FuncResult() { IsSuccess = true, Content = new { list, industyCount } };
        }
    }
    public class Counts
    {
        public decimal Count { get; set; }
    }
    public class Town
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class Industry
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class ReturnDate
    {
        //亩均税收得分
        public decimal TAX_PER_MU_SCORE { get; set; }
        //亩均增加值得分
        public decimal ADD_VALUE_PER_MU_SCORE { get; set; }
        //全员劳动生产率得分
        public decimal PRODUCTIVITY_SCORE { get; set; }
        //单位排污权增加值得分
        public decimal POLLUTANT_DISCHARGE_SCORE { get; set; }
        //单位能耗增加值得分
        public decimal ENERGY_CONSUMPTION_SCORE { get; set; }
        // 净资产收益率得分
        public decimal NET_ASSETS_PROFIT_SCORE { get; set; }
        //研发经费投入比得分
        public decimal R_D_EXPENDITURE_RATIO_SCORE { get; set; }
        //所有者权益（万元）
        public decimal OWNER_EQUITY { get; set; }
        public decimal PERIOD_YEAR { get; set; }
        public string ORG_CODE { get; set; }
        public string ORG_NAME { get; set; }
        public string INDUSTRY { get; set; }
        public string TOWN { get; set; }
        public decimal COMPOSITE_SCORE { get; set; }
        public decimal TAX_PER_MU { get; set; }
        public decimal ADD_VALUE_PER_MU { get; set; }
        public decimal PRODUCTIVITY { get; set; }
        public decimal POLLUTANT_DISCHARGE { get; set; }
        public decimal ENERGY_CONSUMPTION { get; set; }
        public decimal NET_ASSETS_PROFIT { get; set; }
        public decimal R_D_EXPENDITURE_RATIO { get; set; }
        //年职工人数
        public decimal WORKER_MONTH { get; set; }
        //工业增加值
        public decimal Industrial_added_value { get; set; }
        //主要污染排放量
        public decimal pollutant_discharge2 { get; set; }
        //税收实际贡献
        public decimal fact_tax { get; set; }
        //用地面积
        public decimal LAND_AREA { get; set; }
        //净利润
        public decimal PROFIT { get; set; }
        //净资产
        public decimal ASSETS { get; set; }
        //主营业务收入
        public decimal MAIN_BUSINESS_INCOME { get; set; }
        //研发经费支出
        public decimal R_D_EXPENDITURE { get; set; }
        //综合能耗
        public decimal Energy_consumption2 { get; set; }
        public decimal ONE_RANK { get; set; }
        public decimal SIX_RANK { get; set; }
        public decimal COMPOSITE_RANK { get; set; }
    }
    public class IndustryInfo
    {
        public string ORG_CODE { get; set; }
        public string ORG_NAME { get; set; }
        public string TOWN_Code { get; set; }
        public string TOWN_Name { get; set; }
        public string REGISTRATION_TYPE { get; set; }
        public string ADDRESS { get; set; }
        public string LEGAL_REPRESENTATIVE { get; set; }
        public string PHONE { get; set; }
        public string LINK_MAN { get; set; }
        public string PHONE2 { get; set; }
        public string CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string LAST_UPDATE_DATE { get; set; }

        public string LAST_UPDATED_BY { get; set; }
        public string REGISTRATION_STATUS { get; set; }
        public string INDUSTRY_CODE { get; set; }
        public string INDUSTRY_Name { get; set; }
        public string REGISTRATION_MONEY { get; set; }
        public string REGISTRATION_DATE { get; set; }
    }
    public class SubOrgInfo
    {
        public string PARENT_ORG_CODE { get; set; }
        public string SUB_ORG_CODE { get; set; }
        public string SUB_ORG_NAME { get; set; }
        public string PROVINCE { get; set; }
        public string REGISTRATION_STATUS { get; set; }
        public string REGISTRATION_DATE { get; set; }
        public string CREATION_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string LAST_UPDATE_DATE { get; set; }
        public string LAST_UPDATED_BY { get; set; }
    }
    public class AvarageScore
    {
        public decimal PERIOD_YEAR { get; set; }
        public decimal TAX_PER_MU { get; set; }
        public decimal ADD_VALUE_PER_MU { get; set; }
        public decimal PRODUCTIVITY { get; set; }
        public decimal POLLUTANT_DISCHARGE { get; set; }
        public decimal ENERGY_CONSUMPTION { get; set; }
        public decimal NET_ASSETS_PROFIT { get; set; }
        public decimal R_D_EXPENDITURE_RATIO { get; set; }
    }
}
