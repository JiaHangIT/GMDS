﻿using System;
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
        /// 建立日期
        /// </summary>       
        public DateTime CREATION_DATE { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>       
        public decimal? CREATED_BY { get; set; }

        /// <summary>
        /// 最后更新日期
        /// </summary>       
        public DateTime LAST_UPDATE_DATE { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>       
        public decimal? LAST_UPDATED_BY { get; set; }

        /// <summary>
        /// 年份
        /// </summary>       
        public decimal PERIOD_YEAR { get; set; }

        /// <summary>
        /// ID
        /// </summary>       
        public string RECORD_ID { get; set; }

        /// <summary>
        /// 职工薪酬
        /// </summary>       
        public decimal? EMPLOYEE_REMUNERATION { get; set; }

        /// <summary>
        /// 固定资产折旧
        /// </summary>       
        public decimal? DEPRECIATION { get; set; }

        /// <summary>
        /// 营业利润
        /// </summary>       
        public decimal? PROFIT { get; set; }

        /// <summary>
        /// 负债总额
        /// </summary>
        public decimal? LIABILITIES { get; set; }

        /// <summary>
        /// 主营业务收入（万元）
        /// </summary>       
        public decimal? MAIN_BUSINESS_INCOME { get; set; }

        /// <summary>
        /// 企业实缴税金
        /// </summary>       
        public decimal? ENT_PAID_TAX { get; set; }

        /// <summary>
        /// 利润总额
        /// </summary>       
        public decimal? TOTAL_PROFIT { get; set; }

        /// <summary>
        /// 所有者权益
        /// </summary>       
        public decimal? OWNER_EQUITY { get; set; }
        /// <summary>
        /// 平均从业人数（人）
        /// </summary>
        public decimal? NUMBER_OF_EMPLOYEES { get; set; }
        /// <summary>
        /// 允许扣除的研发费用
        /// </summary>
        public decimal? RAD_EXPENSES { get; set; }

        public string Remark { get; set; }

        public int DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeleteBy { get; set; }
    }
}
