﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;

namespace JiaHang.Projects.Admin.DAL.EntityFramework
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApdFctGas> ApdFctGas { get; set; }
        public virtual DbSet<ApdFctWaterDal> ApdFctWater { get; set; }
        public virtual DbSet<ApdFctInsuranceDal> ApdFctInsurance { get; set; }
        public virtual DbSet<ApdFctLandDistrict> ApdFctLandDistrict { get; set; }       
        public virtual DbSet<DcsCustomerInfo> DcsCustomerInfo { get; set; }
        public virtual DbSet<DcsCustomerLogInfo> DcsCustomerLogInfo { get; set; }
        public virtual DbSet<DcsCustomerServices> DcsCustomerServices { get; set; }
        public virtual DbSet<DcsCustsveAccessInfo> DcsCustsveAccessInfo { get; set; }
        public virtual DbSet<DcsCustsveAcsResult> DcsCustsveAcsResult { get; set; }
        public virtual DbSet<DcsCustsveDatarightInfo> DcsCustsveDatarightInfo { get; set; }
        public virtual DbSet<DcsCustsveDatarightType> DcsCustsveDatarightType { get; set; }
        public virtual DbSet<DcsCustsveFieldList> DcsCustsveFieldList { get; set; }
        public virtual DbSet<DcsServiceCResults> DcsServiceCResults { get; set; }
        public virtual DbSet<DcsServiceGroup> DcsServiceGroup { get; set; }
        public virtual DbSet<DcsServiceInfo> DcsServiceInfo { get; set; }
        public virtual DbSet<DcsServiceParams> DcsServiceParams { get; set; }
        public virtual DbSet<DcsServiceSResults> DcsServiceSResults { get; set; }
        public virtual DbSet<SysAreaRoute> SysAreaRoute { get; set; }
        public virtual DbSet<SysConnectionInfo> SysConnectionInfo { get; set; }
        public virtual DbSet<SysControllerRoute> SysControllerRoute { get; set; }
        public virtual DbSet<SysDataCondition> SysDataCondition { get; set; }
        public virtual DbSet<SysDataRightInfo> SysDataRightInfo { get; set; }
        public virtual DbSet<SysDatabaseType> SysDatabaseType { get; set; }
        public virtual DbSet<SysDatarightType> SysDatarightType { get; set; }
        public virtual DbSet<SysDatasourceField> SysDatasourceField { get; set; }
        public virtual DbSet<SysDatasourceInfo> SysDatasourceInfo { get; set; }
        public virtual DbSet<SysDimInfo> SysDimInfo { get; set; }
        public virtual DbSet<SysDimType> SysDimType { get; set; }
        public virtual DbSet<SysErrorCodeInfo> SysErrorCodeInfo { get; set; }
        public virtual DbSet<SysFieldType> SysFieldType { get; set; }
        public virtual DbSet<SysHelpInfo> SysHelpInfo { get; set; }
        public virtual DbSet<SysHelpType> SysHelpType { get; set; }
        public virtual DbSet<SysJobInfo> SysJobInfo { get; set; }
        public virtual DbSet<SysMessageInfo> SysMessageInfo { get; set; }
        public virtual DbSet<SysMethodConditions> SysMethodConditions { get; set; }
        public virtual DbSet<SysMethodRoute> SysMethodRoute { get; set; }
        public virtual DbSet<SysModelDatarightType> SysModelDatarightType { get; set; }
        public virtual DbSet<SysModelGroup> SysModelGroup { get; set; }
        public virtual DbSet<SysModelInfo> SysModelInfo { get; set; }
        public virtual DbSet<ApdFctTAx> ApdFctTAx { get; set; }
        public virtual DbSet<SysModule> SysModule { get; set; }
        public virtual DbSet<SysModuleRoute> SysModuleRoute { get; set; }
        public virtual DbSet<SysModuleUserRelation> SysModuleUserRelation { get; set; }
        public virtual DbSet<SysOperRightInfo> SysOperRightInfo { get; set; }
        public virtual DbSet<SysProblemInfo> SysProblemInfo { get; set; }
        public virtual DbSet<SysProblemType> SysProblemType { get; set; }
        public virtual DbSet<SysSystemInfo> SysSystemInfo { get; set; }
        public virtual DbSet<SysUserDataCondition> SysUserDataCondition { get; set; }
        public virtual DbSet<SysUserGroup> SysUserGroup { get; set; }
        public virtual DbSet<SysUserGroupRelation> SysUserGroupRelation { get; set; }
        public virtual DbSet<SysUserInGroup> SysUserInGroup { get; set; }
        public virtual DbSet<SysUserInfo> SysUserInfo { get; set; }

        public virtual DbSet<SysUserRoute> SysUserRoute { get; set; }
        public virtual DbSet<SysUserRouteCondition> SysUserRouteCondition { get; set; }
        public virtual DbSet<DcsDataCatalog> DcsDataCatalog { get; set; }
        public virtual DbSet<ApdFctOrgIndexV> ApdFctOrgIndexV { get; set; }

        public virtual DbSet<ApdDimTown> ApdDimTown { get; set; }
        public virtual DbSet<ApdFctLandTown> ApdFctLandTown { get; set; }
        public virtual DbSet<ApdFctLandTown2> ApdFctLandTown2 { get; set; }
        public virtual DbSet<ApdFctRD> ApdFctRD { get; set; }
        public virtual DbSet<ApdDimOrg> ApdDimOrg { get; set; }
        public virtual DbSet<ApdFctContaminants> ApdFctContaminants { get; set; }
        public virtual DbSet<ApdFctElectric> ApdFctElectric { get; set; }
        public virtual DbSet<ApdFctWorker> ApdFctWorker { get; set; }
        public virtual DbSet<ApdDimRatio> ApdDimRatio { get; set; }
        public virtual DbSet<VisLog> VisLog { get; set; }
        // Unable to generate entity type for table 'DCSP_USER.AAAA_AAAA'. Please see the warning messages.
        // Unable to generate entity type for table 'DCSP_USER.TEMP_WXF'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //                optionsBuilder.UseOracle("DATA SOURCE=120.79.207.87:1521/DCSP; PASSWORD=123456;PERSIST SECURITY INFO=True;USER ID=dcsp_user;");
            //            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
            //    .HasAnnotation("Relational:DefaultSchema", "DCSP_USER");
            modelBuilder.Entity<ApdFctLandDistrict>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_LAND_DISTRICT_PK");

                entity.ToTable("APD_FCT_LAND_DISTRICT");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_LAND_DISTRICT_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                     .HasColumnName("RECORD_ID")
                      .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.OrgCode)
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.LandNo)
                    .HasColumnName("LAND_NO")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ShareDesc)
                    .HasColumnName("SHARE_DESC")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.RightType)
                    .HasColumnName("RIGHT_TYPE")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Purpose)
                    .HasColumnName("PURPOSE")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.BeginDate)
                    .HasColumnName("BEGIN_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.EndDate)
                    .HasColumnName("END_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Area)
                    .HasColumnName("AREA")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

            });
            modelBuilder.Entity<ApdDimOrg>(entity =>
            {
                //entity.Property(t => t.RecordId).ValueGeneratedOnAdd();
                entity.HasKey(e => new { e.OrgCode, e.PeriodYear })
                    .HasName("APD_DIM_ORG_PK");

                entity.ToTable("APD_DIM_ORG");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_DIM_ORG_UX");

                entity.HasIndex(e => new { e.OrgCode, e.PeriodYear })
                    .HasName("APD_DIM_ORG_PK")
                    .IsUnique();

                entity.Property(e => e.OrgCode)
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Address)
                    .HasColumnName("ADDRESS")
                    .HasColumnType("NVARCHAR2(1000)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Industry)
                    .HasColumnName("INDUSTRY")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.LegalRepresentative)
                    .HasColumnName("LEGAL_REPRESENTATIVE")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.LinkMan)
                    .HasColumnName("LINK_MAN")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.OrgName)
                    .HasColumnName("ORG_NAME")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.Phone2)
                    .HasColumnName("PHONE2")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnName("REGISTRATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.RegistrationMoney)
                    .HasColumnName("REGISTRATION_MONEY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.RegistrationStatus)
                    .HasColumnName("REGISTRATION_STATUS")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.RegistrationType)
                    .HasColumnName("REGISTRATION_TYPE")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.Town)
                    .HasColumnName("TOWN")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.DeleteBy)
                 .HasColumnName("DELETE_BY")
                 .HasColumnType("NVARCHAR2(400)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;
            });
            modelBuilder.Entity<ApdFctGas>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_GAS_PK");

                entity.ToTable("APD_FCT_GAS");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_GAS_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                     .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.OrgCode)
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.Gas)
                    .HasColumnName("GAS")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Other)
                    .HasColumnName("OTHER")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

            });
            modelBuilder.Entity<ApdFctInsuranceDal>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_INSURANCE_PK");

                entity.ToTable("APD_FCT_INSURANCE");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_INSURANCE_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.OrgCode)
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.InsuranceMonth)
                    .HasColumnName("INSURANCE_MONTH")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");
                
                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

            });
            modelBuilder.Entity<ApdFctWaterDal>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_WATER_PK");

                entity.ToTable("APD_FCT_WATER");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_WATER_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.OrgCode)
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.Water)
                    .HasColumnName("WATER")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Other)
                    .HasColumnName("OTHER")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

            });

            modelBuilder.Entity<ApdDimTown>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("APD_DIM_TOWN_PK");

                entity.ToTable("APD_DIM_TOWN");

                entity.HasIndex(e => e.Code)
                    .HasName("APD_DIM_TOWN_PK")
                    .IsUnique();

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasColumnType("NVARCHAR2(30)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.DeleteBy)
                 .HasColumnName("DELETE_BY")
                 .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;
            });

            modelBuilder.Entity<ApdFctLandTown>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_LAND_TOWN_PK");

                entity.ToTable("APD_FCT_LAND_TOWN");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_LAND_TOWN_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.OwnershipLand)
                    .HasColumnName("OWNERSHIP_LAND")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ProtectionLand)
                    .HasColumnName("PROTECTION_LAND")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ReduceLand)
                    .HasColumnName("REDUCE_LAND")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.T2Id)
                    .HasColumnName("T2_ID")
                     .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.DeleteBy)
                   .HasColumnName("DELETE_BY")
                   .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;
            });
            modelBuilder.Entity<ApdFctTAx>(entity =>
            {
                entity.HasKey(e => e.RECORD_ID)
                .HasName("APD_FCT_TAX_PK");

                entity.ToTable("APD_FCT_TAX");

                entity.HasIndex(e => e.RECORD_ID)
                    .HasName("APD_FCT_TAX_PK")
                    .IsUnique();

                entity.Property(e => e.RECORD_ID)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.EMPLOYEE_REMUNERATION)
                    .HasColumnName("EMPLOYEE_REMUNERATION")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DEPRECIATION)
                    .HasColumnName("DEPRECIATION")
                    .HasColumnType("NUMBER")
                    ;

                entity.Property(e => e.ORG_CODE)
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");
                

                entity.Property(e => e.CREATION_DATE)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CREATED_BY)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.LAST_UPDATE_DATE)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.LAST_UPDATED_BY)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.PERIOD_YEAR)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.PROFIT)
                    .HasColumnName("PROFIT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.LIABILITIES)
                 .HasColumnName("LIABILITIES")
                 .HasColumnType("NUMBER");

                entity.Property(e => e.MAIN_BUSINESS_INCOME)
                    .HasColumnName("MAIN_BUSINESS_INCOME")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.RAD_EXPENSES)
                    .HasColumnName("RAD_EXPENSES")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.ENT_PAID_TAX)
                    .HasColumnName("ENT_PAID_TAX")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.TOTAL_PROFIT)
                    .HasColumnName("TOTAL_PROFIT")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.OWNER_EQUITY)
                    .HasColumnName("OWNER_EQUITY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.NUMBER_OF_EMPLOYEES)
                   .HasColumnName("NUMBER_OF_EMPLOYEES")
                   .HasColumnType("NUMBER");
                
                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");
            });
            modelBuilder.Entity<ApdFctLandTown2>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_LAND_TOWN2_PK");

                entity.ToTable("APD_FCT_LAND_TOWN2");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_LAND_TOWN2_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.FactLand)
                    .HasColumnName("FACT_LAND")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.LeaseLand)
                    .HasColumnName("LEASE_LAND")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(200)");

                entity.Property(e => e.RentLand)
                    .HasColumnName("RENT_LAND")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Count)
                   .HasColumnName("COUNT")
                   .HasColumnType("NUMBER");

                entity.Property(e => e.DeleteBy)
                   .HasColumnName("DELETE_BY")
                   .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;
            });

            modelBuilder.Entity<ApdFctRD>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_R_D_PK");

                entity.ToTable("APD_FCT_R_D");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_R_D_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.IsHighTech)
                    .HasColumnName("IS_HIGH_TECH")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.RDExpenditure)
                    .HasColumnName("R_D_EXPENDITURE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.DeleteBy)
                  .HasColumnName("DELETE_BY")
                  .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;
            });

            modelBuilder.Entity<ApdFctContaminants>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_CONTAMINANTS_PK");

                entity.ToTable("APD_FCT_CONTAMINANTS");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_CONTAMINANTS_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                     .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.AmmoniaNitrogen)
                    .HasColumnName("AMMONIA_NITROGEN")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Coal)
                    .HasColumnName("COAL")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Firewood)
                    .HasColumnName("FIREWOOD")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.FuelOil)
                    .HasColumnName("FUEL_OIL")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Hydrogen)
                    .HasColumnName("HYDROGEN")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.IsInSystem)
                    .HasColumnName("IS_IN_SYSTEM")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.NitrogenOxide)
                    .HasColumnName("NITROGEN_OXIDE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.Oxygen)
                    .HasColumnName("OXYGEN")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.SulfurDioxide)
                    .HasColumnName("SULFUR_DIOXIDE")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DeleteBy)
                  .HasColumnName("DELETE_BY")
                  .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;
            });

            modelBuilder.Entity<ApdFctElectric>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_ELECTRIC_PK");

                entity.ToTable("APD_FCT_ELECTRIC");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_ELECTRIC_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.NetSupply)
                    .HasColumnName("NET_SUPPLY")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.Spontaneous)
                    .HasColumnName("SPONTANEOUS")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DeleteBy)
                  .HasColumnName("DELETE_BY")
                  .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");
            });

            modelBuilder.Entity<ApdFctWorker>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("APD_FCT_WORKER_PK");

                entity.ToTable("APD_FCT_WORKER");

                entity.HasIndex(e => e.RecordId)
                    .HasName("APD_FCT_WORKER_PK")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .HasColumnType("NVARCHAR2(50)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NUMBER")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.OrgCode)
                    .IsRequired()
                    .HasColumnName("ORG_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.PeriodYear)
                    .HasColumnName("PERIOD_YEAR")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Remark)
                    .HasColumnName("REMARK")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.WorkerMonth)
                    .HasColumnName("WORKER_MONTH")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.DeleteBy)
                 .HasColumnName("DELETE_BY")
                 .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");
            });

            modelBuilder.Entity<DcsCustomerInfo>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("DCS_CUSTOMER_INFO");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("PK_DCS_CUSTOMER_INFO")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConcurrentLimit)
                    .HasColumnName("CONCURRENT_LIMIT")
;

                entity.Property(e => e.ContactEmail)
                    .HasColumnName("CONTACT_EMAIL")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.ContactMobile)
                    .HasColumnName("CONTACT_MOBILE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ContactName)
                    .HasColumnName("CONTACT_NAME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ContactTel)
                    .HasColumnName("CONTACT_TEL")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CustomerName)
                    .HasColumnName("CUSTOMER_NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.CustomerStatus)
                    .HasColumnName("CUSTOMER_STATUS")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.EffEndDate)
                    .HasColumnName("EFF_END_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.IpLimitFlag)
                    .HasColumnName("IP_LIMIT_FLAG")
;

                entity.Property(e => e.IpLimitList)
                    .HasColumnName("IP_LIMIT_LIST")
                    .HasColumnType("NVARCHAR2(1000)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.LoginAccount)
                    .HasColumnName("LOGIN_ACCOUNT")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.LoginPassword)
                    .HasColumnName("LOGIN_PASSWORD")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.ServerIp)
                    .HasColumnName("SERVER_IP")
                    .HasColumnType("NVARCHAR2(30)");
            });

            modelBuilder.Entity<DcsCustomerLogInfo>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("DCS_CUSTOMER_LOG_INFO");

                entity.HasIndex(e => e.LogId)
                    .HasName("PK_DCS_CUSTOMER_LOG_INFO")
                    .IsUnique();

                entity.Property(e => e.LogId)
                    .HasColumnName("LOG_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LogDate)
                    .HasColumnName("LOG_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LogInfo)
                    .HasColumnName("LOG_INFO")
                    .HasColumnType("CLOB");

                entity.Property(e => e.LogType)
                    .HasColumnName("LOG_TYPE")
                    .HasColumnType("NVARCHAR2(30)");
            });

            modelBuilder.Entity<DcsCustomerServices>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ServiceId });

                entity.ToTable("DCS_CUSTOMER_SERVICES");

                entity.HasIndex(e => new { e.CustomerId, e.ServiceId })
                    .HasName("PK_DCS_CUSTOMER_SERVICES")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatarightFlag)
                    .HasColumnName("DATARIGHT_FLAG")
;

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastAccessDate)
                    .HasColumnName("LAST_ACCESS_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.LimitDay)
                    .HasColumnName("LIMIT_DAY")
;

                entity.Property(e => e.LimitMonth)
                    .HasColumnName("LIMIT_MONTH")
;

                entity.Property(e => e.Param1)
                    .HasColumnName("PARAM_1")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param2)
                    .HasColumnName("PARAM_2")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param3)
                    .HasColumnName("PARAM_3")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param4)
                    .HasColumnName("PARAM_4")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param5)
                    .HasColumnName("PARAM_5")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param6)
                    .HasColumnName("PARAM_6")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param7)
                    .HasColumnName("PARAM_7")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param8)
                    .HasColumnName("PARAM_8")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.Param9)
                    .HasColumnName("PARAM_9")
                    .HasColumnType("NVARCHAR2(60)");
            });

            modelBuilder.Entity<DcsCustsveAccessInfo>(entity =>
            {
                entity.HasKey(e => e.AccessId);

                entity.ToTable("DCS_CUSTSVE_ACCESS_INFO");

                entity.HasIndex(e => e.AccessId)
                    .HasName("PK_DCS_CUSTSVE_ACCESS_INFO")
                    .IsUnique();

                entity.HasIndex(e => new { e.AccessId, e.CustomerId, e.ServiceId, e.AccessDate })
                    .HasName("UX_DCS_CUSTSVE_ACCESS_INFO")
                    .IsUnique();

                entity.Property(e => e.AccessId)
                    .HasColumnName("ACCESS_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessDate)
                    .HasColumnName("ACCESS_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.AccessExeTime)
                    .HasColumnName("ACCESS_EXE_TIME")
;

                entity.Property(e => e.AccessIp)
                    .HasColumnName("ACCESS_IP")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.AccessResultFlag)
                    .HasColumnName("ACCESS_RESULT_FLAG")
;

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.ReturnDataNum)
                    .HasColumnName("RETURN_DATA_NUM")
;

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<DcsCustsveAcsResult>(entity =>
            {
                entity.HasKey(e => e.AccessId)
                    .HasName("PK_DCS_CUSTSVE_ACCESS_RESULT");

                entity.ToTable("DCS_CUSTSVE_ACS_RESULT");

                entity.HasIndex(e => e.AccessId)
                    .HasName("PK_DCS_CUSTSVE_ACCESS_RESULT")
                    .IsUnique();

                entity.Property(e => e.AccessId)
                    .HasColumnName("ACCESS_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessParams)
                    .HasColumnName("ACCESS_PARAMS")
                    .HasColumnType("NVARCHAR2(500)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.ReturnResult)
                    .HasColumnName("RETURN_RESULT")
                    .HasColumnType("CLOB");
            });

            modelBuilder.Entity<DcsCustsveDatarightInfo>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ServiceId, e.DatarightTypeId });

                entity.ToTable("DCS_CUSTSVE_DATARIGHT_INFO");

                entity.HasIndex(e => new { e.CustomerId, e.ServiceId, e.DatarightTypeId })
                    .HasName("PK_DCS_CUSTSVE_DATARIGHT_INFO")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DatarightTypeId)
                    .HasColumnName("DATARIGHT_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.RightValue1)
                    .HasColumnName("RIGHT_VALUE1")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.RightValue2)
                    .HasColumnName("RIGHT_VALUE2")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.RightValue3)
                    .HasColumnName("RIGHT_VALUE3")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.UseChildrenLevel)
                    .HasColumnName("USE_CHILDREN_LEVEL")
;

                entity.Property(e => e.ValueRelativePath)
                    .HasColumnName("VALUE_RELATIVE_PATH")
;
            });

            modelBuilder.Entity<DcsCustsveDatarightType>(entity =>
            {
                entity.HasKey(e => new { e.ServiceId, e.DataRightId, e.CustomerId });

                entity.ToTable("DCS_CUSTSVE_DATARIGHT_TYPE");

                entity.HasIndex(e => new { e.CustomerId, e.ServiceId, e.DataRightId })
                    .HasName("PK_DCS_CUSTSVE_DATARIGHT_TYPE")
                    .IsUnique();

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DataRightId)
                    .HasColumnName("DATA_RIGHT_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataLevel)
                    .HasColumnName("DATA_LEVEL")
;

                entity.Property(e => e.DataRightColumn1)
                    .HasColumnName("DATA_RIGHT_COLUMN1")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DataRightColumn2)
                    .HasColumnName("DATA_RIGHT_COLUMN2")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DataRightColumn3)
                    .HasColumnName("DATA_RIGHT_COLUMN3")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<DcsCustsveFieldList>(entity =>
            {
                entity.HasKey(e => new { e.ServiceId, e.FieldId, e.CustomerId });

                entity.ToTable("DCS_CUSTSVE_FIELD_LIST");

                entity.HasIndex(e => new { e.CustomerId, e.ServiceId, e.FieldId })
                    .HasName("PK_DCS_CUSTSVE_FIELD_LIST")
                    .IsUnique();

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.FieldId)
                    .HasColumnName("FIELD_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.DisplayName)
                    .HasColumnName("DISPLAY_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<DcsServiceCResults>(entity =>
            {
                entity.HasKey(e => new { e.ServiceId, e.ReFieldName })
                    .HasName("PK_DCS_SERVICE_COLLECT_RESULTS");

                entity.ToTable("DCS_SERVICE_C_RESULTS");

                entity.HasIndex(e => new { e.ServiceId, e.ReFieldName })
                    .HasName("PK_DCS_SERVICE_COLLECT_RESULTS")
                    .IsUnique();

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ReFieldName)
                    .HasColumnName("RE_FIELD_NAME")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.DimTransFlag)
                    .HasColumnName("DIM_TRANS_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ToFieldId)
                    .IsRequired()
                    .HasColumnName("TO_FIELD_ID")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<DcsServiceGroup>(entity =>
            {
                entity.HasKey(e => e.ServiceGroupId);

                entity.ToTable("DCS_SERVICE_GROUP");

                entity.HasIndex(e => e.ServiceGroupId)
                    .HasName("PK_DCS_SERVICE_GROUP")
                    .IsUnique();

                entity.Property(e => e.ServiceGroupId)
                    .HasColumnName("SERVICE_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceGroupCode)
                    .HasColumnName("SERVICE_GROUP_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ServiceGroupName)
                    .HasColumnName("SERVICE_GROUP_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
;
            });

            modelBuilder.Entity<DcsServiceInfo>(entity =>
            {
                entity.HasKey(e => e.ServiceId);

                entity.ToTable("DCS_SERVICE_INFO");

                entity.HasIndex(e => e.ServiceCode)
                    .HasName("UX_DCS_SERVICE_INFO")
                    .IsUnique();

                entity.HasIndex(e => e.ServiceId)
                    .HasName("PK_DCS_SERVICE_INFO")
                    .IsUnique();

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditDate)
                    .HasColumnName("AUDIT_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.AuditFlag)
                    .HasColumnName("AUDIT_FLAG");

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataMultiFlag)
                    .HasColumnName("DATA_MULTI_FLAG")
;

                entity.Property(e => e.DataPageFlag)
                    .HasColumnName("DATA_PAGE_FLAG")
;

                entity.Property(e => e.DatasourceId)
                    .HasColumnName("DATASOURCE_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceCode)
                    .IsRequired()
                    .HasColumnName("SERVICE_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.ServiceDesc)
                    .HasColumnName("SERVICE_DESC")
                    .HasColumnType("NVARCHAR2(300)");

                entity.Property(e => e.ServiceGroupId)
                    .IsRequired()
                    .HasColumnName("SERVICE_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasColumnName("SERVICE_NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.ServiceNo)
                    .HasColumnName("SERVICE_NO")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ServiceReturn)
                    .IsRequired()
                    .HasColumnName("SERVICE_RETURN")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ServiceStatus)
                    .HasColumnName("SERVICE_STATUS")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ServiceTech)
                    .IsRequired()
                    .HasColumnName("SERVICE_TECH")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ServiceType)
                    .IsRequired()
                    .HasColumnName("SERVICE_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ServiceVersion)
                    .HasColumnName("SERVICE_VERSION")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
;
            });

            modelBuilder.Entity<DcsServiceParams>(entity =>
            {
                entity.HasKey(e => e.ParamId);

                entity.ToTable("DCS_SERVICE_PARAMS");

                entity.HasIndex(e => e.ParamId)
                    .HasName("PK_DCS_SERVICE_PARAMS")
                    .IsUnique();

                entity.HasIndex(e => new { e.ServiceId, e.ParamCode })
                    .HasName("UX_DCS_SERVICE_PARAMS")
                    .IsUnique();

                entity.Property(e => e.ParamId)
                    .HasColumnName("PARAM_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ParamCode)
                    .IsRequired()
                    .HasColumnName("PARAM_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.ParamDesc)
                    .HasColumnName("PARAM_DESC")
                    .HasColumnType("CLOB");

                entity.Property(e => e.ParamName)
                    .IsRequired()
                    .HasColumnName("PARAM_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.ParamNullable)
                    .HasColumnName("PARAM_NULLABLE")
;

                entity.Property(e => e.ParamTypeId)
                    .HasColumnName("PARAM_TYPE_ID")
;

                entity.Property(e => e.RelaFieldId)
                    .HasColumnName("RELA_FIELD_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceId)
                    .IsRequired()
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.TimestampFlag)
                    .HasColumnName("TIMESTAMP_FLAG")
;
            });

            modelBuilder.Entity<DcsServiceSResults>(entity =>
            {
                entity.HasKey(e => new { e.FieldId, e.ServiceId })
                    .HasName("PK_DCS_SERVICE_SHARE_RESULTS");

                entity.ToTable("DCS_SERVICE_S_RESULTS");

                entity.HasIndex(e => new { e.FieldId, e.ServiceId })
                    .HasName("PK_DCS_SERVICE_SHARE_RESULTS")
                    .IsUnique();

                entity.Property(e => e.FieldId)
                    .HasColumnName("FIELD_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysAreaRoute>(entity =>
            {
                entity.ToTable("SYS_AREA_ROUTE");

                entity.HasIndex(e => e.SysAreaRouteId)
                    .HasName("PK_SYS_AREA_ROUTE")
                    .IsUnique();

                entity.Property(e => e.SysAreaRouteId)
                    .HasColumnName("SYS_AREA_ROUTE_ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaAlias)
                    .HasColumnName("AREA_ALIAS")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.AreaPath)
                    .HasColumnName("AREA_PATH")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
            });

            modelBuilder.Entity<SysConnectionInfo>(entity =>
            {
                entity.HasKey(e => e.ConnectionId);

                entity.ToTable("SYS_CONNECTION_INFO");

                entity.HasIndex(e => e.ConnectionId)
                    .HasName("PK_SYS_CONNECTION_INFO")
                    .IsUnique();

                entity.Property(e => e.ConnectionId)
                    .HasColumnName("CONNECTION_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConnectionName)
                    .HasColumnName("CONNECTION_NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.ConnectionString)
                    .HasColumnName("CONNECTION_STRING")
                    .HasColumnType("NVARCHAR2(200)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatabaseTypeId)
                    .HasColumnName("DATABASE_TYPE_ID")
;

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysControllerRoute>(entity =>
            {
                entity.ToTable("SYS_CONTROLLER_ROUTE");

                entity.HasIndex(e => e.SysControllerRouteId)
                    .HasName("PK_SYS_CONTROLLER_ROUTE")
                    .IsUnique();

                entity.Property(e => e.SysControllerRouteId)
                    .HasColumnName("SYS_CONTROLLER_ROUTE_ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaId)
                    .HasColumnName("AREA_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ControllerAlias)
                    .HasColumnName("CONTROLLER_ALIAS")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ControllerPath)
                    .HasColumnName("CONTROLLER_PATH")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.IsApi).HasColumnName("IS_API");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");
            });

            modelBuilder.Entity<SysDataCondition>(entity =>
            {
                entity.ToTable("SYS_DATA_CONDITION");

                entity.HasIndex(e => e.SysDataConditionId)
                    .HasName("PK_SYS_DATA_CONDITION")
                    .IsUnique();

                entity.Property(e => e.SysDataConditionId)
                    .HasColumnName("SYS_DATA_CONDITION_ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildColumn)
                    .HasColumnName("CHILD_COLUMN")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ConditionDesc)
                    .HasColumnName("CONDITION_DESC")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ConditionValue)
                    .HasColumnName("CONDITION_VALUE")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ConditionValueDesc)
                    .HasColumnName("CONDITION_VALUE_DESC")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.MasterSlaveFlag).HasColumnName("MASTER_SLAVE_FLAG");

                entity.Property(e => e.ParentColumn)
                    .HasColumnName("PARENT_COLUMN")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");

                entity.Property(e => e.TableName)
                    .HasColumnName("TABLE_NAME")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysDataRightInfo>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserGroupId, e.ModelId, e.DatarightTypeId });

                entity.ToTable("SYS_DATA_RIGHT_INFO");

                entity.HasIndex(e => new { e.UserId, e.UserGroupId, e.ModelId, e.DatarightTypeId })
                    .HasName("PK_SYS_DATA_RIGHT_INFO")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelId)
                    .HasColumnName("MODEL_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DatarightTypeId)
                    .HasColumnName("DATARIGHT_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.DisplayName)
                    .HasColumnName("DISPLAY_NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.RightValue1)
                    .HasColumnName("RIGHT_VALUE1")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.RightValue2)
                    .HasColumnName("RIGHT_VALUE2")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.RightValue3)
                    .HasColumnName("RIGHT_VALUE3")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.UseChildrenLevel)
                    .HasColumnName("USE_CHILDREN_LEVEL")
;

                entity.Property(e => e.ValueRelativePath)
                    .HasColumnName("VALUE_RELATIVE_PATH")
;
            });

            modelBuilder.Entity<SysDatabaseType>(entity =>
            {
                entity.HasKey(e => e.DatabaseTypeId);

                entity.ToTable("SYS_DATABASE_TYPE");

                entity.HasIndex(e => e.DatabaseTypeId)
                    .HasName("PK_SYS_DATABASE_TYPE")
                    .IsUnique();

                entity.Property(e => e.DatabaseTypeId)
                    .HasColumnName("DATABASE_TYPE_ID")
;

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatabaseTypeCode)
                    .HasColumnName("DATABASE_TYPE_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.DatabaseTypeName)
                    .HasColumnName("DATABASE_TYPE _NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysDatarightType>(entity =>
            {
                entity.HasKey(e => e.DatarightTypeId);

                entity.ToTable("SYS_DATARIGHT_TYPE");

                entity.HasIndex(e => e.DatarightTypeId)
                    .HasName("PK_SYS_DATARIGHT_TYPE")
                    .IsUnique();

                entity.Property(e => e.DatarightTypeId)
                    .HasColumnName("DATARIGHT_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataLevelColumn)
                    .HasColumnName("DATA_LEVEL_COLUMN")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DatarightTypeCode)
                    .HasColumnName("DATARIGHT_TYPE_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DatarightTypeName)
                    .HasColumnName("DATARIGHT_TYPE_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DatasourceCode)
                    .HasColumnName("DATASOURCE_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.EnableFlag)
                    .HasColumnName("ENABLE_FLAG")
;

                entity.Property(e => e.HaveDataLevel)
                    .HasColumnName("HAVE_DATA_LEVEL")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.RightValueColumnCode1)
                    .HasColumnName("RIGHT_VALUE_COLUMN_CODE1")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnCode2)
                    .HasColumnName("RIGHT_VALUE_COLUMN_CODE2")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnCode3)
                    .HasColumnName("RIGHT_VALUE_COLUMN_CODE3")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnId1)
                    .HasColumnName("RIGHT_VALUE_COLUMN_ID1")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnId2)
                    .HasColumnName("RIGHT_VALUE_COLUMN_ID2")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnId3)
                    .HasColumnName("RIGHT_VALUE_COLUMN_ID3")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnName1)
                    .HasColumnName("RIGHT_VALUE_COLUMN_NAME1")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnName2)
                    .HasColumnName("RIGHT_VALUE_COLUMN_NAME2")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RightValueColumnName3)
                    .HasColumnName("RIGHT_VALUE_COLUMN_NAME3")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.RootLevelValue)
                    .HasColumnName("ROOT_LEVEL_VALUE")
                    .HasColumnType("NVARCHAR2(30)");
            });

            modelBuilder.Entity<SysDatasourceField>(entity =>
            {
                entity.HasKey(e => e.FieldId);

                entity.ToTable("SYS_DATASOURCE_FIELD");

                entity.HasIndex(e => e.FieldId)
                    .HasName("PK_SYS_DATASOURCE_FIELD")
                    .IsUnique();

                entity.HasIndex(e => new { e.DatasourceId, e.FieldId })
                    .HasName("IX_SYS_DATASOURCE_FIELD");

                entity.Property(e => e.FieldId)
                    .HasColumnName("FIELD_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatasourceId)
                    .HasColumnName("DATASOURCE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.DimFieldCode)
                    .HasColumnName("DIM_FIELD_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DimFieldName)
                    .HasColumnName("DIM_FIELD_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DimFlag)
                    .HasColumnName("DIM_FLAG")
;

                entity.Property(e => e.DimTableName)
                    .HasColumnName("DIM_TABLE_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.FieldCode)
                    .HasColumnName("FIELD_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.FieldIndexFlag)
                    .HasColumnName("FIELD_INDEX_FLAG")
;

                entity.Property(e => e.FieldKeyFlag)
                    .HasColumnName("FIELD_KEY_FLAG")
;

                entity.Property(e => e.FieldLength)
                    .HasColumnName("FIELD_LENGTH")
;

                entity.Property(e => e.FieldName)
                    .HasColumnName("FIELD_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.FieldNullable)
                    .HasColumnName("FIELD_NULLABLE")
;

                entity.Property(e => e.FieldTypeId)
                    .HasColumnName("FIELD_TYPE_ID")
;

                entity.Property(e => e.FieldValue)
                    .HasColumnName("FIELD_VALUE")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.OraSequenceCode)
                    .HasColumnName("ORA_SEQUENCE_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.TimestampFlag)
                    .HasColumnName("TIMESTAMP_FLAG")
;
            });

            modelBuilder.Entity<SysDatasourceInfo>(entity =>
            {
                entity.HasKey(e => e.DatasourceId);

                entity.ToTable("SYS_DATASOURCE_INFO");

                entity.HasIndex(e => e.DatasourceCode)
                    .HasName("UX_SYS_DATASOURCE_INFO")
                    .IsUnique();

                entity.HasIndex(e => e.DatasourceId)
                    .HasName("PK_SYS_DATASOURCE_INFO")
                    .IsUnique();

                entity.Property(e => e.DatasourceId)
                    .HasColumnName("DATASOURCE_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConnectionId)
                    .HasColumnName("CONNECTION_ID")
                    .HasColumnType("NVARCHAR2(40)");
                entity.Property(e => e.DataCatalogId)
                   .HasColumnName("DATA_CATALOG_ID")
                   .HasColumnType("NVARCHAR2(40)");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatasourceCode)
                    .HasColumnName("DATASOURCE_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DatasourceName)
                    .HasColumnName("DATASOURCE_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DatasourceType)
                    .HasColumnName("DATASOURCE_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.DatasourceUse)
                    .HasColumnName("DATASOURCE_USE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysDimInfo>(entity =>
            {
                entity.HasKey(e => e.DimId);

                entity.ToTable("SYS_DIM_INFO");

                entity.HasIndex(e => e.DimId)
                    .HasName("PK_SYS_DIM_INFO")
                    .IsUnique();

                entity.HasIndex(e => e.DimTypeCode)
                    .HasName("IX_SYS_DIM_INFO");

                entity.Property(e => e.DimId)
                    .HasColumnName("DIM_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.DimName)
                    .HasColumnName("DIM_NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.DimTypeCode)
                    .HasColumnName("DIM_TYPE_CODE")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DimValue)
                    .HasColumnName("DIM_VALUE")
                    .HasColumnType("NVARCHAR2(200)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysDimType>(entity =>
            {
                entity.HasKey(e => e.DimTypeCode);

                entity.ToTable("SYS_DIM_TYPE");

                entity.HasIndex(e => e.DimTypeCode)
                    .HasName("PK_SYS_DIM_TYPE")
                    .IsUnique();

                entity.Property(e => e.DimTypeCode)
                    .HasColumnName("DIM_TYPE_CODE")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.DimTypeName)
                    .HasColumnName("DIM_TYPE_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysErrorCodeInfo>(entity =>
            {
                entity.HasKey(e => e.ErrorCodeId);

                entity.ToTable("SYS_ERROR_CODE_INFO");

                entity.HasIndex(e => e.ErrorCodeId)
                    .HasName("PK_SYS_ERROR_CODE_INFO")
                    .IsUnique();

                entity.Property(e => e.ErrorCodeId)
                    .HasColumnName("ERROR_CODE_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag)
                    .HasColumnName("AUDIT_FLAG")
;

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.ErrorCodeCode)
                    .IsRequired()
                    .HasColumnName("ERROR_CODE_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ErrorCodeDesc)
                    .HasColumnName("ERROR_CODE_DESC")
                    .HasColumnType("CLOB");

                entity.Property(e => e.ErrorCodeName)
                    .IsRequired()
                    .HasColumnName("ERROR_CODE_NAME")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.ImportantFlag)
                    .HasColumnName("IMPORTANT_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysFieldType>(entity =>
            {
                entity.HasKey(e => e.FieldTypeId);

                entity.ToTable("SYS_FIELD_TYPE");

                entity.HasIndex(e => e.FieldTypeId)
                    .HasName("PK_SYS_FIELD_TYPE")
                    .IsUnique();

                entity.Property(e => e.FieldTypeId)
                    .HasColumnName("FIELD_TYPE_ID")
;

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.FieldTypeCode)
                    .HasColumnName("FIELD_TYPE_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.FieldTypeName)
                    .HasColumnName("FIELD_TYPE_NAME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysHelpInfo>(entity =>
            {
                entity.HasKey(e => e.HelpId);

                entity.ToTable("SYS_HELP_INFO");

                entity.HasIndex(e => e.HelpId)
                    .HasName("PK_SYS_HELP_INFO")
                    .IsUnique();

                entity.HasIndex(e => e.HelpTypeId)
                    .HasName("IX_SYS_HELP_INFO");

                entity.Property(e => e.HelpId)
                    .HasColumnName("HELP_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag)
                    .HasColumnName("AUDIT_FLAG")
;

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.HelpContent)
                    .HasColumnName("HELP_CONTENT")
                    .HasColumnType("CLOB");

                entity.Property(e => e.HelpTitle)
                    .HasColumnName("HELP_TITLE")
                    .HasColumnType("NVARCHAR2(200)");

                entity.Property(e => e.HelpTypeId)
                    .IsRequired()
                    .HasColumnName("HELP_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ImportantFlag)
                    .HasColumnName("IMPORTANT_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysHelpType>(entity =>
            {
                entity.HasKey(e => e.HelpTypeId);

                entity.ToTable("SYS_HELP_TYPE");

                entity.HasIndex(e => e.HelpTypeId)
                    .HasName("PK_SYS_HELP_TYPE")
                    .IsUnique();

                entity.Property(e => e.HelpTypeId)
                    .HasColumnName("HELP_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .IsRequired()
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.HelpTypeName)
                    .IsRequired()
                    .HasColumnName("HELP_TYPE_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysJobInfo>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.ToTable("SYS_JOB_INFO");

                entity.HasIndex(e => e.JobId)
                    .HasName("PK_SYS_JOB_INFO")
                    .IsUnique();

                entity.Property(e => e.JobId)
                    .HasColumnName("JOB_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CycleDayFrequeceType)
                    .HasColumnName("CYCLE_DAY_FREQUECE_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleDayIntervalNumber)
                    .HasColumnName("CYCLE_DAY_INTERVAL_NUMBER")
;

                entity.Property(e => e.CycleDayIntervalType)
                    .HasColumnName("CYCLE_DAY_INTERVAL_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleDayOnetimesTime)
                    .HasColumnName("CYCLE_DAY_ONETIMES_TIME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleEndDate)
                    .HasColumnName("CYCLE_END_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CycleFrequeceType)
                    .HasColumnName("CYCLE_FREQUECE_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleMonthDaytimes)
                    .HasColumnName("CYCLE_MONTH_DAYTIMES")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.CycleMonthFrequeceType)
                    .HasColumnName("CYCLE_MONTH_FREQUECE_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleMonthIntervalNumber)
                    .HasColumnName("CYCLE_MONTH_INTERVAL_NUMBER")
;

                entity.Property(e => e.CycleMonthIntervalType)
                    .HasColumnName("CYCLE_MONTH_INTERVAL_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleMonthOnetimesTime)
                    .HasColumnName("CYCLE_MONTH_ONETIMES_TIME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleMonthType)
                    .HasColumnName("CYCLE_MONTH_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleMonthWeekNumber)
                    .HasColumnName("CYCLE_MONTH_WEEK_NUMBER")
;

                entity.Property(e => e.CycleMonthWeekType)
                    .HasColumnName("CYCLE_MONTH_WEEK_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleStartDate)
                    .HasColumnName("CYCLE_START_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CycleWeekEnabledFri)
                    .HasColumnName("CYCLE_WEEK_ENABLED_FRI")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekEnabledMon)
                    .HasColumnName("CYCLE_WEEK_ENABLED_MON")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekEnabledSat)
                    .HasColumnName("CYCLE_WEEK_ENABLED_SAT")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekEnabledSun)
                    .HasColumnName("CYCLE_WEEK_ENABLED_SUN")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekEnabledThu)
                    .HasColumnName("CYCLE_WEEK_ENABLED_THU")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekEnabledTue)
                    .HasColumnName("CYCLE_WEEK_ENABLED_TUE")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekEnabledWed)
                    .HasColumnName("CYCLE_WEEK_ENABLED_WED")
                    .HasColumnType("NVARCHAR2(1)");

                entity.Property(e => e.CycleWeekFrequeceType)
                    .HasColumnName("CYCLE_WEEK_FREQUECE_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleWeekIntervalNumber)
                    .HasColumnName("CYCLE_WEEK_INTERVAL_NUMBER")
;

                entity.Property(e => e.CycleWeekIntervalType)
                    .HasColumnName("CYCLE_WEEK_INTERVAL_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.CycleWeekOnetimesTime)
                    .HasColumnName("CYCLE_WEEK_ONETIMES_TIME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.EnableFlag)
                    .HasColumnName("ENABLE_FLAG")
;

                entity.Property(e => e.JobCode)
                    .IsRequired()
                    .HasColumnName("JOB_CODE")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.JobDesc)
                    .HasColumnName("JOB_DESC")
                    .HasColumnType("NVARCHAR2(200)");

                entity.Property(e => e.JobLastRuntime)
                    .HasColumnName("JOB_LAST_RUNTIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnName("JOB_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.JobType)
                    .HasColumnName("JOB_TYPE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.OnetimesDate)
                    .HasColumnName("ONETIMES_DATE")
                    .HasColumnType("DATE");
            });

            modelBuilder.Entity<SysMessageInfo>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.ToTable("SYS_MESSAGE_INFO");

                entity.HasIndex(e => e.MessageId)
                    .HasName("PK_SYS_MESSAGE_INFO")
                    .IsUnique();

                entity.Property(e => e.MessageId)
                    .HasColumnName("MESSAGE_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag)
                    .HasColumnName("AUDIT_FLAG")
;

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.ImportantFlag)
                    .HasColumnName("IMPORTANT_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.MessageContent)
                    .HasColumnName("MESSAGE_CONTENT")
                    .HasColumnType("CLOB");

                entity.Property(e => e.MessageTitle)
                    .HasColumnName("MESSAGE_TITLE")
                    .HasColumnType("NVARCHAR2(200)");
            });

            modelBuilder.Entity<SysMethodConditions>(entity =>
            {
                entity.ToTable("SYS_METHOD_CONDITIONS");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_METHOD_CONDITIONS")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("CONDITION_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysMethodRoute>(entity =>
            {
                entity.ToTable("SYS_METHOD_ROUTE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_METHOD_ROUTE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerId)
                    .HasColumnName("CONTROLLER_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.MethodAlias)
                    .HasColumnName("METHOD_ALIAS")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.MethodPath)
                    .HasColumnName("METHOD_PATH")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.MethodType)
                    .HasColumnName("METHOD_TYPE")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");
            });

            modelBuilder.Entity<SysModelDatarightType>(entity =>
            {
                entity.HasKey(e => new { e.ModelId, e.DataRightTypeId });

                entity.ToTable("SYS_MODEL_DATARIGHT_TYPE");

                entity.HasIndex(e => new { e.ModelId, e.DataRightTypeId })
                    .HasName("PK_SYS_MODEL_DATARIGHT_TYPE")
                    .IsUnique();

                entity.Property(e => e.ModelId)
                    .HasColumnName("MODEL_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DataRightTypeId)
                    .HasColumnName("DATA_RIGHT_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataLevel)
                    .HasColumnName("DATA_LEVEL")
;

                entity.Property(e => e.DataRightColumn1)
                    .HasColumnName("DATA_RIGHT_COLUMN1")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DataRightColumn2)
                    .HasColumnName("DATA_RIGHT_COLUMN2")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DataRightColumn3)
                    .HasColumnName("DATA_RIGHT_COLUMN3")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysModelGroup>(entity =>
            {
                entity.HasKey(e => e.ModelGroupId);

                entity.ToTable("SYS_MODEL_GROUP");

                entity.HasIndex(e => e.ModelGroupId)
                    .HasName("PK_SYS_MODEL_GROUP")
                    .IsUnique();

                entity.HasIndex(e => e.ParentId)
                    .HasName("IX_SYS_MODEL_GROUP");

                entity.Property(e => e.ModelGroupId)
                    .HasColumnName("MODEL_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("DELETE_FLAG")
                    ;

                entity.Property(e => e.EnableFlag)
                    .HasColumnName("ENABLE_FLAG")
;

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelGroupCode)
                    .HasColumnName("MODEL_GROUP_CODE")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelGroupName)
                    .HasColumnName("MODEL_GROUP_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.ModelGroupUrl)
                    .HasColumnName("MODEL_GROUP_URL")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.OutUrlFlag)
                    .IsRequired()
                    .HasColumnName("OUT_URL_FLAG")
                    ;

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ParentIdTree)
                    .HasColumnName("PARENT_ID_TREE")
                    .HasColumnType("NVARCHAR2(1000)");

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
;
            });

            modelBuilder.Entity<SysModelInfo>(entity =>
            {
                entity.HasKey(e => e.ModelId);

                entity.ToTable("SYS_MODEL_INFO");

                entity.HasIndex(e => e.ModelCode)
                    .HasName("UX_SYS_MODEL_INFO");

                entity.HasIndex(e => e.ModelId)
                    .HasName("PK_SYS_MODEL_INFO")
                    .IsUnique();

                entity.Property(e => e.ModelId)
                    .HasColumnName("MODEL_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");
                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");
                entity.Property(e => e.EnableFlag)
                    .HasColumnName("ENABLE_FLAG")
;

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelCode)
                    .HasColumnName("MODEL_CODE")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelGroupId)
                    .IsRequired()
                    .HasColumnName("MODEL_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelName)
                    .HasColumnName("MODEL_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.ModelUrl)
                    .HasColumnName("MODEL_URL")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.OutUrlFlag)
                    .HasColumnName("OUT_URL_FLAG")
                   ;

                entity.Property(e => e.SortKey)
                    .HasColumnName("SORT_KEY")
;
            });

            modelBuilder.Entity<SysModule>(entity =>
            {
                entity.ToTable("SYS_MODULE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_MODULE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.ModuleName)
                    .HasColumnName("MODULE_NAME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");
            });

            modelBuilder.Entity<SysModuleRoute>(entity =>
            {
                entity.ToTable("SYS_MODULE_ROUTE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_MODULE_ROUTE_RELATION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerRouteId)
                    .HasColumnName("CONTROLLER_ROUTE_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysModuleUserRelation>(entity =>
            {
                entity.ToTable("SYS_MODULE_USER_RELATION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_MODULE_USER_RELATION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ModuleUserRelation).HasColumnName("MODULE_USER_RELATION");

                entity.Property(e => e.PermissionType).HasColumnName("PERMISSION_TYPE");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysOperRightInfo>(entity =>
            {
                entity.HasKey(e => e.RecordId);

                entity.ToTable("SYS_OPER_RIGHT_INFO");

                entity.HasIndex(e => e.RecordId)
                    .HasName("PK_SYS_OPER_RIGHT_INFO")
                    .IsUnique();

                entity.Property(e => e.RecordId)
                    .HasColumnName("RECORD_ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .IsRequired()
                    .HasColumnName("DELETE_FLAG")
                    ;

                entity.Property(e => e.FunctionCode)
                    .HasColumnName("FUNCTION_CODE")
                    .HasColumnType("NVARCHAR2(10)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelGroupId)
                    .HasColumnName("MODEL_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ModelId)
                    .IsRequired()
                    .HasColumnName("MODEL_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysProblemInfo>(entity =>
            {
                entity.HasKey(e => e.ProblemId);

                entity.ToTable("SYS_PROBLEM_INFO");

                entity.HasIndex(e => e.ProblemId)
                    .HasName("PK_SYS_PROBLEM_INFO")
                    .IsUnique();

                entity.Property(e => e.ProblemId)
                    .HasColumnName("PROBLEM_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag)
                    .HasColumnName("AUDIT_FLAG")
;

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ProblemContent)
                    .HasColumnName("PROBLEM_CONTENT")
                    .HasColumnType("CLOB");

                entity.Property(e => e.ProblemTitle)
                    .HasColumnName("PROBLEM_TITLE")
                    .HasColumnType("NVARCHAR2(200)");

                entity.Property(e => e.ProblemTypeId)
                    .HasColumnName("PROBLEM_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)");
            });

            modelBuilder.Entity<SysProblemType>(entity =>
            {
                entity.HasKey(e => e.ProblemTypeId);

                entity.ToTable("SYS_PROBLEM_TYPE");

                entity.HasIndex(e => e.ProblemTypeId)
                    .HasName("PK_SYS_PROBLEM_TYPE")
                    .IsUnique();

                entity.Property(e => e.ProblemTypeId)
                    .HasColumnName("PROBLEM_TYPE_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ProblemTypeName)
                    .IsRequired()
                    .HasColumnName("PROBLEM_TYPE_NAME")
                    .HasColumnType("NVARCHAR2(100)");
            });

            modelBuilder.Entity<SysSystemInfo>(entity =>
            {
                entity.HasKey(e => e.SystemId);

                entity.ToTable("SYS_SYSTEM_INFO");

                entity.HasIndex(e => e.SystemId)
                    .HasName("PK_SYS_SYSTEM_INFO")
                    .IsUnique();

                entity.Property(e => e.SystemId)
                    .HasColumnName("SYSTEM_ID")
;

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;

                entity.Property(e => e.SystemCode)
                    .HasColumnName("SYSTEM_CODE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.SystemName)
                    .HasColumnName("SYSTEM_NAME")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.SystemUrl)
                    .HasColumnName("SYSTEM_URL")
                    .HasColumnType("NVARCHAR2(100)");
            });

            modelBuilder.Entity<SysUserDataCondition>(entity =>
            {
                entity.ToTable("SYS_USER_DATA_CONDITION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_USER_DATA_CONDITION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("CONDITION_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ConditionValue)
                    .HasColumnName("CONDITION_VALUE")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.ControllerId)
                    .HasColumnName("CONTROLLER_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysUserGroup>(entity =>
            {
                entity.HasKey(e => e.UserGroupId);

                entity.ToTable("SYS_USER_GROUP");

                entity.HasIndex(e => e.ParentId)
                    .HasName("IX_SYS_USER_GROUP");

                entity.HasIndex(e => e.UserGroupId)
                    .HasName("PK_SYS_USER_GROUP")
                    .IsUnique();

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
;

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ParentIdTree)
                    .HasColumnName("PARENT_ID_TREE")
                    .HasColumnType("NVARCHAR2(1000)");

                entity.Property(e => e.UserGroupLevel)
                    .HasColumnName("USER_GROUP_LEVEL")
;

                entity.Property(e => e.UserGroupName)
                    .HasColumnName("USER_GROUP_NAME")
                    .HasColumnType("NVARCHAR2(60)");
            });

            modelBuilder.Entity<SysUserGroupRelation>(entity =>
            {
                entity.ToTable("SYS_USER_GROUP_RELATION");

                entity.HasIndex(e => e.SysUserGroupRelationId)
                    .HasName("PK_SYS_USER_GROUP_RELATION")
                    .IsUnique();

                entity.Property(e => e.SysUserGroupRelationId)
                    .HasColumnName("SYS_USER_GROUP_RELATION_ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    ;

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysUserInGroup>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserGroupId })
                    .HasName("PX_SYS_USER_IN_GROUP");

                entity.ToTable("SYS_USER_IN_GROUP");

                entity.HasIndex(e => new { e.UserId, e.UserGroupId })
                    .HasName("PX_SYS_USER_IN_GROUP")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")

                    ;
            });

            modelBuilder.Entity<SysUserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("SYS_USER_INFO");
                entity.HasIndex(e => e.UserId)
                    .HasName("PK_SYS_USER_INFO")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ValidTime)
                .HasColumnName("VALIDTIME")
                .HasColumnType("DATE");

                entity.Property(e => e.UserAccount)
                    .IsRequired()
                    .HasColumnName("USER_ACCOUNT")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("USER_EMAIL")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.UserGroupNames)
                    .HasColumnName("USER_GROUP_NAMES")
                    .HasColumnType("NVARCHAR2(500)");

                entity.Property(e => e.UserMobile)
                    .HasColumnName("USER_MOBILE")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasColumnType("NVARCHAR2(30)");

                entity.Property(e => e.UserOrgId)
                    .HasColumnName("USER_ORG_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("USER_PASSWORD")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.UserTel)
                    .HasColumnName("USER_TEL")
                    .HasColumnType("NVARCHAR2(30)");
            });

            modelBuilder.Entity<SysUserRoute>(entity =>
            {
                entity.ToTable("SYS_USER_ROUTE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_USER_ROUTE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerId)
                    .HasColumnName("CONTROLLER_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });

            modelBuilder.Entity<SysUserRouteCondition>(entity =>
            {
                entity.ToTable("SYS_USER_ROUTE_CONDITION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_USER_ROUTE_CONDITION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NVARCHAR2(36)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("CONDITION_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.PropertyId)
                    .HasColumnName("PROPERTY_ID")
                    .HasColumnType("NVARCHAR2(36)");

                entity.Property(e => e.UserRouteId)
                    .HasColumnName("USER_ROUTE_ID")
                    .HasColumnType("NVARCHAR2(36)");
            });
            modelBuilder.Entity<DcsDataCatalog>(entity =>
            {
                entity.HasKey(e => e.DataCatalogId);
                entity.ToTable("DCS_DATA_CATALOG");
                entity.HasIndex(e => e.DataCatalogId)
                    .HasName("PK_DCS_DATA_CATALOG")
                    .IsUnique();

                entity.Property(e => e.DataCatalogId)
                    .HasColumnName("DATA_CATALOG_ID")
                    .HasColumnType("NVARCHAR2(40)")
                    .ValueGeneratedNever();

                entity.Property(e => e.DataCatalogCode)
                    .HasColumnName("DATA_CATALOG_CODE")
                    .HasColumnType("NVARCHAR2(60)");

                entity.Property(e => e.DataCatalogName)
                    .HasColumnName("DATA_CATALOG_NAME")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.ParentIdTree)
                    .HasColumnName("PARENT_ID_TREE")
                    .HasColumnType("NVARCHAR2(40)");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG")
                ;

                entity.Property(e => e.DataCountSelf)
                    .HasColumnName("DATA_COUNT_SELF");

                entity.Property(e => e.DataCountTree)
                    .HasColumnName("DATA_COUNT_TREE");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasColumnType("NVARCHAR2(100)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasColumnType("NVARCHAR2(40)");
                entity.Property(e => e.LastUpdateDate)
                   .HasColumnName("LAST_UPDATE_DATE")
                   .HasColumnType("DATE");
                entity.Property(e => e.LastUpdatedBy)
                   .HasColumnName("LAST_UPDATED_BY")
                   .HasColumnType("NVARCHAR2(40)");
                entity.Property(e => e.DeleteDate)
                   .HasColumnName("DELETE_DATE")
                   .HasColumnType("DATE");
                entity.Property(e => e.DeleteBy)
                   .HasColumnName("DELETED_BY")
                   .HasColumnType("NVARCHAR2(40)");

            });
            modelBuilder.Entity<ApdFctOrgIndexV>(entity => {
                entity.HasKey(e => e.RecordId);
                entity.HasIndex(e => e.RecordId)
                  .HasName("PK_APD_FCT_ORG_INDEX_V")
                  .IsUnique();
                entity.ToTable("APD_FCT_ORG_INDEX_V");
                entity.Property(e => e.PerIodYear).HasColumnName("PERIOD_YEAR");
                entity.Property(e => e.CompositeScore).HasColumnName("COMPOSITE_SCORE");
                entity.Property(e => e.TaxPerMu).HasColumnName("TAX_PER_MU");
                entity.Property(e => e.AddValuePerMu).HasColumnName("ADD_VALUE_PER_MU");
                entity.Property(e => e.Productivity).HasColumnName("PRODUCTIVITY");
                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG")
               ;
                entity.Property(e => e.OrgCode).HasColumnName("ORG_CODE")
               .HasColumnType("NVARCHAR2(30)");
                entity.Property(e => e.OrgName).HasColumnName("ORG_NAME")
             .HasColumnType("NVARCHAR2(30)");
                entity.Property(e => e.Industry).HasColumnName("INDUSTRY")
             .HasColumnType("NVARCHAR2(30)");
            });
            modelBuilder.Entity<ApdDimRatio>(entity =>
            {
                entity.HasKey(e => e.PeriodYear);
                entity.HasIndex(e => e.Procuctivity)
                .HasName("PERIOD_YEAR")
                .IsUnique()
                ;
                entity.ToTable("APD_DIM_RATIO");
                entity.Property(e => e.PeriodYear).HasColumnName("PERIOD_YEAR");
                entity.Property(e => e.TaxPerMu).HasColumnName("TAX_PER_MU");
                entity.Property(e => e.AddValuePerMu).HasColumnName("ADD_VALUE_PER_MU");
                entity.Property(e => e.Procuctivity).HasColumnName("PRODUCTIVITY");
                entity.Property(e => e.PollutantDischarge).HasColumnName("POLLUTANT_DISCHARGE");
                entity.Property(e => e.EnergyConsumption).HasColumnName("ENERGY_CONSUMPTION");
                entity.Property(e => e.NetAssesProfit).HasColumnName("NET_ASSETS_PROFIT");
                entity.Property(e => e.RDExpenditureRatio).HasColumnName("R_D_EXPENDITURE_RATIO");
                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");
               
            });

            modelBuilder.Entity<VisLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id)
                .HasName("PRIMARY_KEY")
                .IsUnique();

                entity.ToTable("VISLOG");
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Method).HasColumnName("METHOD");
                entity.Property(e => e.UserId).HasColumnName("USERID");
                entity.Property(e => e.VisTime).HasColumnName("VISTIME");
                entity.Property(e => e.RequestUrl).HasColumnName("REQUESTURL");
                entity.Property(e => e.RequestMethod).HasColumnName("REQUESTMETHOD");
                entity.Property(e => e.RequestBody).HasColumnName("REQUESTBODY");
                entity.Property(e => e.Params).HasColumnName("PARAMS");
                entity.Property(e => e.Result).HasColumnName("RESULT");
                entity.Property(e => e.TakeUpTime).HasColumnName("TAKEUPTIME");
                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

            });
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
               //.Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType))
               )
            {
                //foreach (var property in entityType.GetProperties()) {
                //    property.Relational().ColumnName = property.Name.ToUpper();
                //}

                modelBuilder.Entity(entityType.ClrType).Property<int>("DeleteFlag");
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(int) }, parameter, Expression.Constant("DeleteFlag")),
                Expression.Constant(0));


                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }


            base.OnModelCreating(modelBuilder);
        }
    }
}
