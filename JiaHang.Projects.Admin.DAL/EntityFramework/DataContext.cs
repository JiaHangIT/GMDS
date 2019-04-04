using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using System.Linq.Expressions;
using System.Linq;

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

        public virtual DbSet<DcsCustomerInfo> DcsCustomerInfo { get; set; }
        public virtual DbSet<DcsCustomerLogInfo> DcsCustomerLogInfo { get; set; }
        public virtual DbSet<DcsCustomerServices> DcsCustomerServices { get; set; }
        public virtual DbSet<DcsCustsveAccessInfo> DcsCustsveAccessInfo { get; set; }
        public virtual DbSet<DcsCustsveAccessResult> DcsCustsveAccessResult { get; set; }
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
        public virtual DbSet<SysModule> SysModule { get; set; }
        public virtual DbSet<SysModuleRouteRelation> SysModuleRouteRelation { get; set; }
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
            //    .HasAnnotation("Relational:DefaultSchema", "DCSP_DATA");

            modelBuilder.Entity<DcsCustomerInfo>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.ToTable("DCS_CUSTOMER_INFO");

                entity.HasIndex(e => e.CustomerId)
                    .HasName("PK_DCS_CUSTOMER_INFO")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConcurrentLimit).HasColumnName("CONCURRENT_LIMIT");

                entity.Property(e => e.ContactEmail)
                    .HasColumnName("CONTACT_EMAIL")
                    .HasMaxLength(120);

                entity.Property(e => e.ContactMobile)
                    .HasColumnName("CONTACT_MOBILE")
                    .HasMaxLength(60);

                entity.Property(e => e.ContactName)
                    .HasColumnName("CONTACT_NAME")
                    .HasMaxLength(60);

                entity.Property(e => e.ContactTel)
                    .HasColumnName("CONTACT_TEL")
                    .HasMaxLength(60);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CustomerName)
                    .HasColumnName("CUSTOMER_NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.CustomerStatus)
                    .HasColumnName("CUSTOMER_STATUS")
                    .HasMaxLength(60);

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.EffEndDate)
                    .HasColumnName("EFF_END_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.IpLimitFlag).HasColumnName("IP_LIMIT_FLAG");

                entity.Property(e => e.IpLimitList).HasColumnName("IP_LIMIT_LIST");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.LoginAccount)
                    .HasColumnName("LOGIN_ACCOUNT")
                    .HasMaxLength(60);

                entity.Property(e => e.LoginPassword)
                    .HasColumnName("LOGIN_PASSWORD")
                    .HasMaxLength(200);

                entity.Property(e => e.ServerIp)
                    .HasColumnName("SERVER_IP")
                    .HasMaxLength(60);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LogDate)
                    .HasColumnName("LOG_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LogInfo)
                    .HasColumnName("LOG_INFO")
                    .HasColumnType("CLOB(4000)");

                entity.Property(e => e.LogType)
                    .HasColumnName("LOG_TYPE")
                    .HasMaxLength(60);
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
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatarightFlag).HasColumnName("DATARIGHT_FLAG");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastAccessDate)
                    .HasColumnName("LAST_ACCESS_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.LimitDay).HasColumnName("LIMIT_DAY");

                entity.Property(e => e.LimitMonth).HasColumnName("LIMIT_MONTH");

                entity.Property(e => e.Param1)
                    .HasColumnName("PARAM_1")
                    .HasMaxLength(120);

                entity.Property(e => e.Param2)
                    .HasColumnName("PARAM_2")
                    .HasMaxLength(120);

                entity.Property(e => e.Param3)
                    .HasColumnName("PARAM_3")
                    .HasMaxLength(120);

                entity.Property(e => e.Param4)
                    .HasColumnName("PARAM_4")
                    .HasMaxLength(120);

                entity.Property(e => e.Param5)
                    .HasColumnName("PARAM_5")
                    .HasMaxLength(120);

                entity.Property(e => e.Param6)
                    .HasColumnName("PARAM_6")
                    .HasMaxLength(120);

                entity.Property(e => e.Param7)
                    .HasColumnName("PARAM_7")
                    .HasMaxLength(120);

                entity.Property(e => e.Param8)
                    .HasColumnName("PARAM_8")
                    .HasMaxLength(120);

                entity.Property(e => e.Param9)
                    .HasColumnName("PARAM_9")
                    .HasMaxLength(120);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessDate)
                    .HasColumnName("ACCESS_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.AccessExeTime).HasColumnName("ACCESS_EXE_TIME");

                entity.Property(e => e.AccessIp)
                    .HasColumnName("ACCESS_IP")
                    .HasMaxLength(60);

                entity.Property(e => e.AccessResultFlag).HasColumnName("ACCESS_RESULT_FLAG");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ReturnDataNum).HasColumnName("RETURN_DATA_NUM");

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<DcsCustsveAccessResult>(entity =>
            {
                entity.HasKey(e => e.AccessId);

                entity.ToTable("DCS_CUSTSVE_ACCESS_RESULT");

                entity.HasIndex(e => e.AccessId)
                    .HasName("PK_DCS_CUSTSVE_ACCESS_RESULT")
                    .IsUnique();

                entity.Property(e => e.AccessId)
                    .HasColumnName("ACCESS_ID")
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessParams)
                    .HasColumnName("ACCESS_PARAMS")
                    .HasMaxLength(1000);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.ReturnResult)
                    .HasColumnName("RETURN_RESULT")
                    .HasColumnType("CLOB(4000)");
            });

            modelBuilder.Entity<DcsCustsveDatarightInfo>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.DatarightTypeId, e.ServiceId });

                entity.ToTable("DCS_CUSTSVE_DATARIGHT_INFO");

                entity.HasIndex(e => new { e.CustomerId, e.ServiceId, e.DatarightTypeId })
                    .HasName("PK_DCS_CUSTSVE_DATARIGHT_INFO")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.DatarightTypeId)
                    .HasColumnName("DATARIGHT_TYPE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.RightValue1)
                    .HasColumnName("RIGHT_VALUE1")
                    .HasMaxLength(120);

                entity.Property(e => e.RightValue2)
                    .HasColumnName("RIGHT_VALUE2")
                    .HasMaxLength(120);

                entity.Property(e => e.RightValue3)
                    .HasColumnName("RIGHT_VALUE3")
                    .HasMaxLength(120);

                entity.Property(e => e.UseChildrenLevel).HasColumnName("USE_CHILDREN_LEVEL");

                entity.Property(e => e.ValueRelativePath).HasColumnName("VALUE_RELATIVE_PATH");
            });

            modelBuilder.Entity<DcsCustsveDatarightType>(entity =>
            {
                entity.HasKey(e => new { e.DataRightId, e.CustomerId, e.ServiceId });

                entity.ToTable("DCS_CUSTSVE_DATARIGHT_TYPE");

                entity.HasIndex(e => new { e.CustomerId, e.ServiceId, e.DataRightId })
                    .HasName("PK_DCS_CUSTSVE_DATARIGHT_TYPE")
                    .IsUnique();

                entity.Property(e => e.DataRightId)
                    .HasColumnName("DATA_RIGHT_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataLevel).HasColumnName("DATA_LEVEL");

                entity.Property(e => e.DataRightColumn1)
                    .HasColumnName("DATA_RIGHT_COLUMN1")
                    .HasMaxLength(120);

                entity.Property(e => e.DataRightColumn2)
                    .HasColumnName("DATA_RIGHT_COLUMN2")
                    .HasMaxLength(120);

                entity.Property(e => e.DataRightColumn3)
                    .HasColumnName("DATA_RIGHT_COLUMN3")
                    .HasMaxLength(120);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80);

                entity.Property(e => e.FieldId)
                    .HasColumnName("FIELD_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CustomerId)
                    .HasColumnName("CUSTOMER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DisplayName)
                    .HasColumnName("DISPLAY_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<DcsServiceCResults>(entity =>
            {
                entity.HasKey(e => new { e.ReFieldName, e.ServiceId })
                    .HasName("PK_DCS_SERVICE_COLLECT_RESULTS");

                entity.ToTable("DCS_SERVICE_C_RESULTS");

                entity.HasIndex(e => new { e.ServiceId, e.ReFieldName })
                    .HasName("PK_DCS_SERVICE_COLLECT_RESULTS")
                    .IsUnique();

                entity.Property(e => e.ReFieldName)
                    .HasColumnName("RE_FIELD_NAME")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DimTransFlag).HasColumnName("DIM_TRANS_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ToFieldId)
                    .IsRequired()
                    .HasColumnName("TO_FIELD_ID")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceGroupCode)
                    .HasColumnName("SERVICE_GROUP_CODE")
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceGroupName)
                    .HasColumnName("SERVICE_GROUP_NAME")
                    .HasMaxLength(120);
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
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataMultiFlag).HasColumnName("DATA_MULTI_FLAG");

                entity.Property(e => e.DataPageFlag).HasColumnName("DATA_PAGE_FLAG");

                entity.Property(e => e.DatasourceId)
                    .HasColumnName("DATASOURCE_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceCode)
                    .IsRequired()
                    .HasColumnName("SERVICE_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.ServiceDesc)
                    .HasColumnName("SERVICE_DESC")
                    .HasMaxLength(600);

                entity.Property(e => e.ServiceGroupId)
                    .IsRequired()
                    .HasColumnName("SERVICE_GROUP_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasColumnName("SERVICE_NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.ServiceNo)
                    .HasColumnName("SERVICE_NO")
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceReturn)
                    .IsRequired()
                    .HasColumnName("SERVICE_RETURN")
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceStatus)
                    .HasColumnName("SERVICE_STATUS")
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceTech)
                    .IsRequired()
                    .HasColumnName("SERVICE_TECH")
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceType)
                    .IsRequired()
                    .HasColumnName("SERVICE_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.ServiceVersion)
                    .HasColumnName("SERVICE_VERSION")
                    .HasMaxLength(60);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ParamCode)
                    .IsRequired()
                    .HasColumnName("PARAM_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.ParamDesc)
                    .HasColumnName("PARAM_DESC")
                    .HasColumnType("CLOB(4000)");

                entity.Property(e => e.ParamName)
                    .IsRequired()
                    .HasColumnName("PARAM_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.ParamNullable).HasColumnName("PARAM_NULLABLE");

                entity.Property(e => e.ParamTypeId).HasColumnName("PARAM_TYPE_ID");

                entity.Property(e => e.RelaFieldId)
                    .HasColumnName("RELA_FIELD_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceId)
                    .IsRequired()
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.TimestampFlag).HasColumnName("TIMESTAMP_FLAG");
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
                    .HasMaxLength(80);

                entity.Property(e => e.ServiceId)
                    .HasColumnName("SERVICE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<SysAreaRoute>(entity =>
            {
                entity.ToTable("SYS_AREA_ROUTE");

                entity.HasIndex(e => e.SysAreaRouteId)
                    .HasName("PK_SYS_AREA_ROUTE")
                    .IsUnique();

                entity.Property(e => e.SysAreaRouteId)
                    .HasColumnName("SYS_AREA_ROUTE_ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaAlias)
                    .HasColumnName("AREA_ALIAS")
                    .HasMaxLength(72);

                entity.Property(e => e.AreaPath)
                    .HasColumnName("AREA_PATH")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy).HasColumnName("CREATED_BY");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasColumnType("NVARCHAR2(4000)");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConnectionName)
                    .HasColumnName("CONNECTION_NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.ConnectionString)
                    .HasColumnName("CONNECTION_STRING")
                    .HasMaxLength(400);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatabaseTypeId).HasColumnName("DATABASE_TYPE_ID");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
            });

            modelBuilder.Entity<SysControllerRoute>(entity =>
            {
                entity.ToTable("SYS_CONTROLLER_ROUTE");

                entity.HasIndex(e => e.SysControllerRouteId)
                    .HasName("PK_SYS_CONTROLLER_ROUTE")
                    .IsUnique();

                entity.Property(e => e.SysControllerRouteId)
                    .HasColumnName("SYS_CONTROLLER_ROUTE_ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.AreaId)
                    .HasColumnName("AREA_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.ControllerAlias)
                    .HasColumnName("CONTROLLER_ALIAS")
                    .HasMaxLength(72);

                entity.Property(e => e.ControllerPath)
                    .HasColumnName("CONTROLLER_PATH")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

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
                    .HasMaxLength(72);

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
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ChildColumn)
                    .HasColumnName("CHILD_COLUMN")
                    .HasMaxLength(72);

                entity.Property(e => e.ConditionDesc)
                    .HasColumnName("CONDITION_DESC")
                    .HasMaxLength(72);

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasMaxLength(72);

                entity.Property(e => e.ConditionValue)
                    .HasColumnName("CONDITION_VALUE")
                    .HasMaxLength(72);

                entity.Property(e => e.ConditionValueDesc)
                    .HasColumnName("CONDITION_VALUE_DESC")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.MasterSlaveFlag).HasColumnName("MASTER_SLAVE_FLAG");

                entity.Property(e => e.ParentColumn)
                    .HasColumnName("PARENT_COLUMN")
                    .HasMaxLength(72);

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");

                entity.Property(e => e.TableName)
                    .HasColumnName("TABLE_NAME")
                    .HasMaxLength(72);
            });

            modelBuilder.Entity<SysDataRightInfo>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ModelId, e.UserGroupId, e.DatarightTypeId });

                entity.ToTable("SYS_DATA_RIGHT_INFO");

                entity.HasIndex(e => new { e.UserId, e.UserGroupId, e.ModelId, e.DatarightTypeId })
                    .HasName("PK_SYS_DATA_RIGHT_INFO")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelId)
                    .HasColumnName("MODEL_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.DatarightTypeId)
                    .HasColumnName("DATARIGHT_TYPE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DisplayName)
                    .HasColumnName("DISPLAY_NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.RightValue1)
                    .HasColumnName("RIGHT_VALUE1")
                    .HasMaxLength(120);

                entity.Property(e => e.RightValue2)
                    .HasColumnName("RIGHT_VALUE2")
                    .HasMaxLength(120);

                entity.Property(e => e.RightValue3)
                    .HasColumnName("RIGHT_VALUE3")
                    .HasMaxLength(120);

                entity.Property(e => e.UseChildrenLevel).HasColumnName("USE_CHILDREN_LEVEL");

                entity.Property(e => e.ValueRelativePath).HasColumnName("VALUE_RELATIVE_PATH");
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
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatabaseTypeCode)
                    .HasColumnName("DATABASE_TYPE_CODE")
                    .HasMaxLength(60);

                entity.Property(e => e.DatabaseTypeName)
                    .HasColumnName("DATABASE_TYPE _NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataLevelColumn)
                    .HasColumnName("DATA_LEVEL_COLUMN")
                    .HasMaxLength(120);

                entity.Property(e => e.DatarightTypeCode)
                    .HasColumnName("DATARIGHT_TYPE_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.DatarightTypeName)
                    .HasColumnName("DATARIGHT_TYPE_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.DatasourceCode)
                    .HasColumnName("DATASOURCE_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.EnableFlag).HasColumnName("ENABLE_FLAG");

                entity.Property(e => e.HaveDataLevel).HasColumnName("HAVE_DATA_LEVEL");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.RightValueColumnCode1)
                    .HasColumnName("RIGHT_VALUE_COLUMN_CODE1")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnCode2)
                    .HasColumnName("RIGHT_VALUE_COLUMN_CODE2")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnCode3)
                    .HasColumnName("RIGHT_VALUE_COLUMN_CODE3")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnId1)
                    .HasColumnName("RIGHT_VALUE_COLUMN_ID1")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnId2)
                    .HasColumnName("RIGHT_VALUE_COLUMN_ID2")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnId3)
                    .HasColumnName("RIGHT_VALUE_COLUMN_ID3")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnName1)
                    .HasColumnName("RIGHT_VALUE_COLUMN_NAME1")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnName2)
                    .HasColumnName("RIGHT_VALUE_COLUMN_NAME2")
                    .HasMaxLength(60);

                entity.Property(e => e.RightValueColumnName3)
                    .HasColumnName("RIGHT_VALUE_COLUMN_NAME3")
                    .HasMaxLength(60);

                entity.Property(e => e.RootLevelValue)
                    .HasColumnName("ROOT_LEVEL_VALUE")
                    .HasMaxLength(60);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatasourceId)
                    .HasColumnName("DATASOURCE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DimFieldCode)
                    .HasColumnName("DIM_FIELD_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.DimFieldName)
                    .HasColumnName("DIM_FIELD_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.DimFlag).HasColumnName("DIM_FLAG");

                entity.Property(e => e.DimTableName)
                    .HasColumnName("DIM_TABLE_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.FieldCode)
                    .HasColumnName("FIELD_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.FieldIndexFlag).HasColumnName("FIELD_INDEX_FLAG");

                entity.Property(e => e.FieldKeyFlag).HasColumnName("FIELD_KEY_FLAG");

                entity.Property(e => e.FieldLength).HasColumnName("FIELD_LENGTH");

                entity.Property(e => e.FieldName)
                    .HasColumnName("FIELD_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.FieldNullable).HasColumnName("FIELD_NULLABLE");

                entity.Property(e => e.FieldTypeId).HasColumnName("FIELD_TYPE_ID");

                entity.Property(e => e.FieldValue)
                    .HasColumnName("FIELD_VALUE")
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.OraSequenceCode)
                    .HasColumnName("ORA_SEQUENCE_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.TimestampFlag).HasColumnName("TIMESTAMP_FLAG");
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConnectionId)
                    .HasColumnName("CONNECTION_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DatasourceCode)
                    .HasColumnName("DATASOURCE_CODE")
                    .HasMaxLength(120);

                entity.Property(e => e.DatasourceName)
                    .HasColumnName("DATASOURCE_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.DatasourceType)
                    .HasColumnName("DATASOURCE_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.DatasourceUse)
                    .HasColumnName("DATASOURCE_USE")
                    .HasMaxLength(60);

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DimName)
                    .HasColumnName("DIM_NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.DimTypeCode)
                    .HasColumnName("DIM_TYPE_CODE")
                    .HasMaxLength(80);

                entity.Property(e => e.DimValue)
                    .HasColumnName("DIM_VALUE")
                    .HasMaxLength(400);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DimTypeName)
                    .HasColumnName("DIM_TYPE_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag).HasColumnName("AUDIT_FLAG");

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.ErrorCodeCode)
                    .IsRequired()
                    .HasColumnName("ERROR_CODE_CODE")
                    .HasMaxLength(60);

                entity.Property(e => e.ErrorCodeDesc)
                    .HasColumnName("ERROR_CODE_DESC")
                    .HasColumnType("CLOB(4000)");

                entity.Property(e => e.ErrorCodeName)
                    .IsRequired()
                    .HasColumnName("ERROR_CODE_NAME")
                    .HasMaxLength(200);

                entity.Property(e => e.ImportantFlag).HasColumnName("IMPORTANT_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.FieldTypeCode)
                    .HasColumnName("FIELD_TYPE_CODE")
                    .HasMaxLength(60);

                entity.Property(e => e.FieldTypeName)
                    .HasColumnName("FIELD_TYPE_NAME")
                    .HasMaxLength(60);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag).HasColumnName("AUDIT_FLAG");

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.HelpContent)
                    .HasColumnName("HELP_CONTENT")
                    .HasColumnType("CLOB(4000)");

                entity.Property(e => e.HelpTitle)
                    .HasColumnName("HELP_TITLE")
                    .HasMaxLength(400);

                entity.Property(e => e.HelpTypeId)
                    .IsRequired()
                    .HasColumnName("HELP_TYPE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ImportantFlag).HasColumnName("IMPORTANT_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .IsRequired()
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.HelpTypeName)
                    .IsRequired()
                    .HasColumnName("HELP_TYPE_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CycleDayFrequeceType)
                    .HasColumnName("CYCLE_DAY_FREQUECE_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleDayIntervalNumber).HasColumnName("CYCLE_DAY_INTERVAL_NUMBER");

                entity.Property(e => e.CycleDayIntervalType)
                    .HasColumnName("CYCLE_DAY_INTERVAL_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleDayOnetimesTime)
                    .HasColumnName("CYCLE_DAY_ONETIMES_TIME")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleEndDate)
                    .HasColumnName("CYCLE_END_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CycleFrequeceType)
                    .HasColumnName("CYCLE_FREQUECE_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleMonthDaytimes)
                    .HasColumnName("CYCLE_MONTH_DAYTIMES")
                    .HasMaxLength(200);

                entity.Property(e => e.CycleMonthFrequeceType)
                    .HasColumnName("CYCLE_MONTH_FREQUECE_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleMonthIntervalNumber).HasColumnName("CYCLE_MONTH_INTERVAL_NUMBER");

                entity.Property(e => e.CycleMonthIntervalType)
                    .HasColumnName("CYCLE_MONTH_INTERVAL_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleMonthOnetimesTime)
                    .HasColumnName("CYCLE_MONTH_ONETIMES_TIME")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleMonthType)
                    .HasColumnName("CYCLE_MONTH_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleMonthWeekNumber).HasColumnName("CYCLE_MONTH_WEEK_NUMBER");

                entity.Property(e => e.CycleMonthWeekType)
                    .HasColumnName("CYCLE_MONTH_WEEK_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleStartDate)
                    .HasColumnName("CYCLE_START_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CycleWeekEnabledFri)
                    .HasColumnName("CYCLE_WEEK_ENABLED_FRI")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekEnabledMon)
                    .HasColumnName("CYCLE_WEEK_ENABLED_MON")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekEnabledSat)
                    .HasColumnName("CYCLE_WEEK_ENABLED_SAT")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekEnabledSun)
                    .HasColumnName("CYCLE_WEEK_ENABLED_SUN")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekEnabledThu)
                    .HasColumnName("CYCLE_WEEK_ENABLED_THU")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekEnabledTue)
                    .HasColumnName("CYCLE_WEEK_ENABLED_TUE")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekEnabledWed)
                    .HasColumnName("CYCLE_WEEK_ENABLED_WED")
                    .HasMaxLength(2);

                entity.Property(e => e.CycleWeekFrequeceType)
                    .HasColumnName("CYCLE_WEEK_FREQUECE_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleWeekIntervalNumber).HasColumnName("CYCLE_WEEK_INTERVAL_NUMBER");

                entity.Property(e => e.CycleWeekIntervalType)
                    .HasColumnName("CYCLE_WEEK_INTERVAL_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.CycleWeekOnetimesTime)
                    .HasColumnName("CYCLE_WEEK_ONETIMES_TIME")
                    .HasMaxLength(60);

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.EnableFlag).HasColumnName("ENABLE_FLAG");

                entity.Property(e => e.JobCode)
                    .IsRequired()
                    .HasColumnName("JOB_CODE")
                    .HasMaxLength(80);

                entity.Property(e => e.JobDesc)
                    .HasColumnName("JOB_DESC")
                    .HasMaxLength(400);

                entity.Property(e => e.JobLastRuntime)
                    .HasColumnName("JOB_LAST_RUNTIME")
                    .HasColumnType("DATE");

                entity.Property(e => e.JobName)
                    .IsRequired()
                    .HasColumnName("JOB_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.JobType)
                    .HasColumnName("JOB_TYPE")
                    .HasMaxLength(60);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag).HasColumnName("AUDIT_FLAG");

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.ImportantFlag).HasColumnName("IMPORTANT_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.MessageContent)
                    .HasColumnName("MESSAGE_CONTENT")
                    .HasColumnType("CLOB(4000)");

                entity.Property(e => e.MessageTitle)
                    .HasColumnName("MESSAGE_TITLE")
                    .HasMaxLength(400);
            });

            modelBuilder.Entity<SysMethodConditions>(entity =>
            {
                entity.ToTable("SYS_METHOD_CONDITIONS");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_METHOD_CONDITIONS")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("CONDITION_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasMaxLength(72);
            });

            modelBuilder.Entity<SysMethodRoute>(entity =>
            {
                entity.ToTable("SYS_METHOD_ROUTE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_METHOD_ROUTE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerId)
                    .HasColumnName("CONTROLLER_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.MethodAlias)
                    .HasColumnName("METHOD_ALIAS")
                    .HasMaxLength(72);

                entity.Property(e => e.MethodPath)
                    .HasColumnName("METHOD_PATH")
                    .HasMaxLength(72);

                entity.Property(e => e.MethodType)
                    .HasColumnName("METHOD_TYPE")
                    .HasMaxLength(72);

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");
            });

            modelBuilder.Entity<SysModelDatarightType>(entity =>
            {
                entity.HasKey(e => new { e.DataRightTypeId, e.ModelId });

                entity.ToTable("SYS_MODEL_DATARIGHT_TYPE");

                entity.HasIndex(e => new { e.ModelId, e.DataRightTypeId })
                    .HasName("PK_SYS_MODEL_DATARIGHT_TYPE")
                    .IsUnique();

                entity.Property(e => e.DataRightTypeId)
                    .HasColumnName("DATA_RIGHT_TYPE_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelId)
                    .HasColumnName("MODEL_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DataLevel).HasColumnName("DATA_LEVEL");

                entity.Property(e => e.DataRightColumn1)
                    .HasColumnName("DATA_RIGHT_COLUMN1")
                    .HasMaxLength(80);

                entity.Property(e => e.DataRightColumn2)
                    .HasColumnName("DATA_RIGHT_COLUMN2")
                    .HasMaxLength(80);

                entity.Property(e => e.DataRightColumn3)
                    .HasColumnName("DATA_RIGHT_COLUMN3")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .IsRequired()
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.EnableFlag).HasColumnName("ENABLE_FLAG");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelGroupCode)
                    .HasColumnName("MODEL_GROUP_CODE")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelGroupName)
                    .HasColumnName("MODEL_GROUP_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.ParentId)
                    .IsRequired()
                    .HasColumnName("PARENT_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ParentIdTree).HasColumnName("PARENT_ID_TREE");

                entity.Property(e => e.SortKey).HasColumnName("SORT_KEY");
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.EnableFlag).HasColumnName("ENABLE_FLAG");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("IMAGE_URL")
                    .HasMaxLength(200);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelCode)
                    .HasColumnName("MODEL_CODE")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelGroupId)
                    .IsRequired()
                    .HasColumnName("MODEL_GROUP_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelName)
                    .HasColumnName("MODEL_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.ModelUrl)
                    .HasColumnName("MODEL_URL")
                    .HasMaxLength(200);

                entity.Property(e => e.SortKey).HasColumnName("SORT_KEY");
            });

            modelBuilder.Entity<SysModule>(entity =>
            {
                entity.ToTable("SYS_MODULE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_MODULE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.ModuleName)
                    .HasColumnName("MODULE_NAME")
                    .HasMaxLength(60);

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");
            });

            modelBuilder.Entity<SysModuleRouteRelation>(entity =>
            {
                entity.ToTable("SYS_MODULE_ROUTE_RELATION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_MODULE_ROUTE_RELATION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerRouteId)
                    .HasColumnName("CONTROLLER_ROUTE_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasMaxLength(72);
            });

            modelBuilder.Entity<SysModuleUserRelation>(entity =>
            {
                entity.ToTable("SYS_MODULE_USER_RELATION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_MODULE_USER_RELATION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql(@"0
");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.ModuleId)
                    .HasColumnName("MODULE_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.ModuleUserRelation).HasColumnName("MODULE_USER_RELATION");

                entity.Property(e => e.PermissionType).HasColumnName("PERMISSION_TYPE");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(72);
            });

            modelBuilder.Entity<SysOperRightInfo>(entity =>
            {
                entity.HasKey(e => new { e.FunctionCode, e.UserId, e.ModelGroupId, e.ModelId, e.UserGroupId });

                entity.ToTable("SYS_OPER_RIGHT_INFO");

                entity.HasIndex(e => new { e.UserId, e.UserGroupId, e.ModelId, e.ModelGroupId, e.FunctionCode })
                    .HasName("PK_SYS_OPER_RIGHT_INFO")
                    .IsUnique();

                entity.Property(e => e.FunctionCode)
                    .HasColumnName("FUNCTION_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelGroupId)
                    .HasColumnName("MODEL_GROUP_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ModelId)
                    .HasColumnName("MODEL_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.AuditFlag).HasColumnName("AUDIT_FLAG");

                entity.Property(e => e.AuditedBy)
                    .HasColumnName("AUDITED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.AuditedDate)
                    .HasColumnName("AUDITED_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ProblemContent)
                    .HasColumnName("PROBLEM_CONTENT")
                    .HasColumnType("CLOB(4000)");

                entity.Property(e => e.ProblemTitle)
                    .HasColumnName("PROBLEM_TITLE")
                    .HasMaxLength(400);

                entity.Property(e => e.ProblemTypeId)
                    .HasColumnName("PROBLEM_TYPE_ID")
                    .HasMaxLength(80);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ProblemTypeName)
                    .IsRequired()
                    .HasColumnName("PROBLEM_TYPE_NAME")
                    .HasMaxLength(200);
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
                    .ValueGeneratedNever();

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.SystemCode)
                    .HasColumnName("SYSTEM_CODE")
                    .HasMaxLength(60);

                entity.Property(e => e.SystemName)
                    .HasColumnName("SYSTEM_NAME")
                    .HasMaxLength(120);

                entity.Property(e => e.SystemUrl)
                    .HasColumnName("SYSTEM_URL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<SysUserDataCondition>(entity =>
            {
                entity.ToTable("SYS_USER_DATA_CONDITION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_USER_DATA_CONDITION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("CONDITION_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.ConditionName)
                    .HasColumnName("CONDITION_NAME")
                    .HasMaxLength(72);

                entity.Property(e => e.ConditionValue)
                    .HasColumnName("CONDITION_VALUE")
                    .HasMaxLength(72);

                entity.Property(e => e.ControllerId)
                    .HasColumnName("CONTROLLER_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

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
                    .HasMaxLength(72);

                entity.Property(e => e.SortValue).HasColumnName("SORT_VALUE");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(72);
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
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.ParentId)
                    .HasColumnName("PARENT_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.ParentIdTree).HasColumnName("PARENT_ID_TREE");

                entity.Property(e => e.UserGroupLevel).HasColumnName("USER_GROUP_LEVEL");

                entity.Property(e => e.UserGroupName)
                    .HasColumnName("USER_GROUP_NAME")
                    .HasMaxLength(120);
            });

            modelBuilder.Entity<SysUserGroupRelation>(entity =>
            {
                entity.ToTable("SYS_USER_GROUP_RELATION");

                entity.HasIndex(e => e.SysUserGroupRelationId)
                    .HasName("PK_SYS_USER_GROUP_RELATION")
                    .IsUnique();

                entity.Property(e => e.SysUserGroupRelationId)
                    .HasColumnName("SYS_USER_GROUP_RELATION_ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(72);
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
                    .HasMaxLength(80);

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("USER_GROUP_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteFlag)
                    .HasColumnName("DELETE_FLAG")
                    .HasDefaultValueSql("0 ");
            });

            modelBuilder.Entity<SysUserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("SYS_USER_INFO");

                entity.HasIndex(e => e.UserAccount)
                    .HasName("UX_SYS_USER_INFO")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("PK_SYS_USER_INFO")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(80)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("DELETE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.EffEndDate)
                    .HasColumnName("EFF_END_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.EffStartDate)
                    .HasColumnName("EFF_START_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LanguageCode)
                    .HasColumnName("LANGUAGE_CODE")
                    .HasMaxLength(60);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("DATE");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(80);

                entity.Property(e => e.UserAccount)
                    .IsRequired()
                    .HasColumnName("USER_ACCOUNT")
                    .HasMaxLength(60);

                entity.Property(e => e.UserEmail)
                    .HasColumnName("USER_EMAIL")
                    .HasMaxLength(60);

                entity.Property(e => e.UserGroupNames)
                    .HasColumnName("USER_GROUP_NAMES")
                    .HasMaxLength(1000);

                entity.Property(e => e.UserIsLdap).HasColumnName("USER_IS_LDAP");

                entity.Property(e => e.UserIsLock).HasColumnName("USER_IS_LOCK");

                entity.Property(e => e.UserMobile)
                    .HasColumnName("USER_MOBILE")
                    .HasMaxLength(60);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(60);

                entity.Property(e => e.UserOrgId)
                    .HasColumnName("USER_ORG_ID")
                    .HasMaxLength(80);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasColumnName("USER_PASSWORD")
                    .HasMaxLength(200);

                entity.Property(e => e.UserTel)
                    .HasColumnName("USER_TEL")
                    .HasMaxLength(60);
            });

            modelBuilder.Entity<SysUserRoute>(entity =>
            {
                entity.ToTable("SYS_USER_ROUTE");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_USER_ROUTE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ControllerId)
                    .HasColumnName("CONTROLLER_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.DeleteFlag).HasColumnName("DELETE_FLAG");

                entity.Property(e => e.DeleteTime)
                    .HasColumnName("DELETE_TIME")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.LastUpdateDate)
                    .HasColumnName("LAST_UPDATE_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.LastUpdatedBy)
                    .IsRequired()
                    .HasColumnName("LAST_UPDATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(72);
            });

            modelBuilder.Entity<SysUserRouteCondition>(entity =>
            {
                entity.ToTable("SYS_USER_ROUTE_CONDITION");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_SYS_USER_ROUTE_CONDITION")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(72)
                    .ValueGeneratedNever();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("CONDITION_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("CREATED_BY")
                    .HasMaxLength(72);

                entity.Property(e => e.CreationDate)
                    .HasColumnName("CREATION_DATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.DeleteBy)
                    .HasColumnName("DELETE_BY")
                    .HasMaxLength(72);

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
                    .HasMaxLength(72);

                entity.Property(e => e.PropertyId)
                    .HasColumnName("PROPERTY_ID")
                    .HasMaxLength(72);

                entity.Property(e => e.UserRouteId)
                    .HasColumnName("USER_ROUTE_ID")
                    .HasMaxLength(72);
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
