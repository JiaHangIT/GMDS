using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.DAL;
using System.IO;
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
        public FuncResult Select(int pageSize, int currentPage, string OrgName) {

            StringBuilder sql = new StringBuilder("select * from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL");
            List<string> wheres = new List<string>();
            if (OrgName != null)
            {
                wheres.Add(" ORG_NAME like '%" + OrgName + "%'");
            }
            if (wheres.Count > 0)
            {
                string wh = string.Join(" and ", wheres.ToArray());

                sql.Append(" where " + wh);
            }
            List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
            int total = list.Count();
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        //根据城镇下拉框改变获取数据
        public FuncResult SelectTownDdata(int pageSize, int currentPage, string Town)
        {

            StringBuilder sql = new StringBuilder("select * from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL");
            List<string> wheres = new List<string>();
            if (Town == "全部")
            {
                Town = null;
            }
            if (Town != null)
            {
                wheres.Add(" TOWN = " + "'" + Town + "'");
            }
            if (wheres.Count > 0)
            {
                string wh = string.Join(" and ", wheres.ToArray());

                sql.Append(" where " + wh);
            }
            List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
            int total = list.Count();
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        //根据行业下拉框改变获取数据
        public FuncResult SelectIndustryData(int pageSize, int currentPage, string Industry)
        {

            StringBuilder sql = new StringBuilder("select * from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL");
            List<string> wheres = new List<string>();
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
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public FuncResult SelectORGInfo(int pageSize, int currentPage, string OrgCode)
        {
            StringBuilder sql = new StringBuilder("select * from JH_APD.APD_DIM_SUB_ORG where PARENT_ORG_CODE=" + "'"+ OrgCode + "'");
            List<SubOrgInfo> list = OracleDbHelper.Query<SubOrgInfo>(sql.ToString());
            int total = list.Count();
            var data = list.ToList().Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        public FuncResult GetTown() {
            string sql = "select * from JH_APD.APD_DIM_TOWN";
            List<Town> list = OracleDbHelper.Query<Town>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult GetIndustry()
        {
            string sql = "select * from JH_APD.APD_DIM_INDUSTRY";
            List<Town> list = OracleDbHelper.Query<Town>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult IndustryDetail(string code) {
            string sql = "select * from JH_APD.APD_DIM_ORG_V where ORG_CODE=" + "'" + code + "'";
            List<IndustryInfo> list = OracleDbHelper.Query<IndustryInfo>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
        public FuncResult BenefiteValuationInfo(string code) {
            StringBuilder sql = new StringBuilder("select * from JH_APD.VIEW_COMPANY_INDEX_SCORE_TOTAL where ORG_CODE="+"'"+code+"'");
            List<ReturnDate> list = OracleDbHelper.Query<ReturnDate>(sql.ToString());
            return new FuncResult() { IsSuccess = true, Content = new { list } };
        }
    }

    public class Town {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class Industry
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class ReturnDate {
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
    }
    public class IndustryInfo {
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
    public class SubOrgInfo {
        public string PARENT_ORG_CODE { get; set; }
        public string SUB_ORG_CODE { get;set;}
        public string SUB_ORG_NAME { get;set;}
        public string PROVINCE { get;set;}
        public string REGISTRATION_STATUS { get;set;}
        public string REGISTRATION_DATE { get;set;}
        public string CREATION_DATE { get;set;}
        public string CREATED_BY { get;set;}
        public string LAST_UPDATE_DATE { get;set;}
        public string LAST_UPDATED_BY { get; set; }
     }
}
