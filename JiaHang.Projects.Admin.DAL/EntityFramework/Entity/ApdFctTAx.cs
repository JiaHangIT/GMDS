using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public class ApdFctTAx
    {
        /// <summary>
        /// 企业代码
        /// </summary>       
        public string ORG_CODE { get; set; }

        /// <summary>
        /// 社会保险费
        /// </summary>       
        public int? SOCIAL_INSURANCE { get; set; }

        /// <summary>
        /// 政策性减免税额
        /// </summary>       
        public int? POLICY_DEDUCTION { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>       
        public DateTime CREATION_DATE { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>       
        public int CREATED_BY { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>       
        public DateTime LAST_UPDATE_DATE { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>       
        public int LAST_UPDATED_BY { get; set; }

        /// <summary>
        /// 年份
        /// </summary>       
        public int PERIOD_YEAR { get; set; }

        /// <summary>
        /// ID
        /// </summary>       
        public int RECORD_ID { get; set; }

        /// <summary>
        /// 职工薪酬
        /// </summary>       
        public int? EMPLOYEE_REMUNERATION { get; set; }

        /// <summary>
        /// 固定资产折旧
        /// </summary>       
        public int? DEPRECIATION { get; set; }

        /// <summary>
        /// 营业利润
        /// </summary>       
        public int? PROFIT { get; set; }

        /// <summary>
        /// 资产总额
        /// </summary>       
        public int? ASSETS { get; set; }

        /// <summary>
        /// 负债总额
        /// </summary>       
        public int? LIABILITIES { get; set; }

        /// <summary>
        /// 主营业务收入（万元）
        /// </summary>       
        public int? MAIN_BUSINESS_INCOME { get; set; }

        /// <summary>
        /// 企业实缴税金
        /// </summary>       
        public int? ENT_PAID_TAX { get; set; }

        /// <summary>
        /// 利润总额
        /// </summary>       
        public int? TOTAL_PROFIT { get; set; }

        /// <summary>
        /// 所有者权益
        /// </summary>       
        public int? OWNER_EQUITY { get; set; }
        /// <summary>
        /// 平均从业人数（人）
        /// </summary>
        public int? NUMBER_OF_EMPLOYEES { get; set; }
        /// <summary>
        /// 允许扣除的研发费用
        /// </summary>
        public int? RAD_EXPENSES { get; set; }
    }
}
