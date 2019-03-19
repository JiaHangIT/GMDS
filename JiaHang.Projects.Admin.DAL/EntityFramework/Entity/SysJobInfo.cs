using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysJobInfo
    {
        public string JobId { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public int EnableFlag { get; set; }
        public string JobType { get; set; }
        public string JobDesc { get; set; }
        public DateTime? JobLastRuntime { get; set; }
        public DateTime? OnetimesDate { get; set; }
        public string CycleFrequeceType { get; set; }
        public DateTime? CycleStartDate { get; set; }
        public DateTime? CycleEndDate { get; set; }
        public string CycleDayFrequeceType { get; set; }
        public string CycleDayOnetimesTime { get; set; }
        public string CycleDayIntervalType { get; set; }
        public int? CycleDayIntervalNumber { get; set; }
        public string CycleWeekEnabledMon { get; set; }
        public string CycleWeekEnabledTue { get; set; }
        public string CycleWeekEnabledWed { get; set; }
        public string CycleWeekEnabledThu { get; set; }
        public string CycleWeekEnabledFri { get; set; }
        public string CycleWeekEnabledSat { get; set; }
        public string CycleWeekEnabledSun { get; set; }
        public string CycleWeekFrequeceType { get; set; }
        public string CycleWeekOnetimesTime { get; set; }
        public string CycleWeekIntervalType { get; set; }
        public int? CycleWeekIntervalNumber { get; set; }
        public string CycleMonthType { get; set; }
        public string CycleMonthDaytimes { get; set; }
        public string CycleMonthWeekType { get; set; }
        public int? CycleMonthWeekNumber { get; set; }
        public string CycleMonthFrequeceType { get; set; }
        public string CycleMonthOnetimesTime { get; set; }
        public string CycleMonthIntervalType { get; set; }
        public int? CycleMonthIntervalNumber { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public int? DeleteFlag { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string DeletedBy { get; set; }
    }
}
