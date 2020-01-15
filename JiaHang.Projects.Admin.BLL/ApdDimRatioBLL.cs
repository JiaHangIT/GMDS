using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
namespace JiaHang.Projects.Admin.BLL
{
   public  class ApdDimRatioBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public ApdDimRatioBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        //根据年份查询
        public FuncResult Select(int pageSize, int currentPage, int year) {
            var data =  from e in _context.ApdDimRatio where (year==0|| e.PeriodYear== year) 
                        select new 
                         {
                PeriodYear= e.PeriodYear,
                PollutantDischarge=e.PollutantDischarge,
                Procuctivity=e.Procuctivity,
                RDExpenditureRatio= e.RDExpenditureRatio,
                TaxPerMu= e.TaxPerMu,
                NetAssesProfit=  e.NetAssesProfit,
                EnergyConsumption= e.EnergyConsumption,
                AddValuePerMu=e.AddValuePerMu,
            };
            int total = data.Count();
            var datas = data.Skip(pageSize * currentPage).Take(pageSize).ToList();
            return new FuncResult() { IsSuccess=true,Content=new { total,datas} };
        }
        //修改
        public async Task<FuncResult> Update(int year,Param model) {
            ApdDimRatio entity = await _context.ApdDimRatio.FindAsync(year);
            if (entity == null) {
                return new FuncResult() {IsSuccess=false,Message="未找到当前年份ID" };
            }
            entity.NetAssesProfit = model.NetAssesProfit;
            entity.PollutantDischarge = model.PollutantDischarge;
            entity.Procuctivity = model.Procuctivity;
            entity.RDExpenditureRatio = model.RDExpenditureRatio;
            entity.TaxPerMu = model.TaxPerMu;
            entity.AddValuePerMu = model.TaxPerMu;
            entity.EnergyConsumption = model.EnergyConsumption;
            _context.ApdDimRatio.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(int year)
        {
            ApdDimRatio entity = await _context.ApdDimRatio.FindAsync(year);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "ID不存在!" };
            }
            _context.ApdDimRatio.Remove(entity);
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LogService.WriteError(ex);
                    return new FuncResult() { IsSuccess = false, Message = "删除时发生了意料之外的错误" };
                }
            }
            ;
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Deletes(string[] years)
        {
            IQueryable<ApdDimRatio> entitys = _context.ApdDimRatio.Where(e => years.Contains(Convert.ToString( e.PeriodYear)));
            if (entitys.Count() != years.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (ApdDimRatio obj in entitys)
            {
                _context.ApdDimRatio.Remove(obj);
            }
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LogService.WriteError(ex);
                    return new FuncResult() { IsSuccess = false, Message = "删除时发生了意料之外的错误" };
                }
            }
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{years.Length}条记录" };

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(Params model)
        {
            ApdDimRatio entity = new ApdDimRatio
            {
                PeriodYear = model.PeriodYear,
                TaxPerMu = model.TaxPerMu,
                AddValuePerMu = model.AddValuePerMu,
                Procuctivity = model.Procuctivity,
                PollutantDischarge = model.PollutantDischarge,
                EnergyConsumption = model.EnergyConsumption,
                NetAssesProfit = model.NetAssesProfit,
                RDExpenditureRatio=model.RDExpenditureRatio
            };
            await _context.ApdDimRatio.AddAsync(entity);
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction trans = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return new FuncResult() { IsSuccess = false, Content = ex.Message };
                }
            }


            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }
    }
    public class Param {
    
        public decimal? TaxPerMu { get; set; }
        public decimal? AddValuePerMu { get; set; }
        public decimal? Procuctivity { get; set; }
        public decimal? PollutantDischarge { get; set; }
        public decimal? EnergyConsumption { get; set; }
        public decimal? NetAssesProfit { get; set; }
        public decimal? RDExpenditureRatio { get; set; }
    }
     public class Params{
        public int PeriodYear { get; set; }
        public decimal? TaxPerMu { get; set; }
        public decimal? AddValuePerMu { get; set; }
        public decimal? Procuctivity { get; set; }
        public decimal? PollutantDischarge { get; set; }
        public decimal? EnergyConsumption { get; set; }
        public decimal? NetAssesProfit { get; set; }
        public decimal? RDExpenditureRatio { get; set; }
    }
}
