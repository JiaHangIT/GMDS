using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiaHang.Projects.Admin.BLL.ExcelImportTax
{
    public class ExcelTaxImportBLL
    {
        private readonly DataContext _context;
        public ExcelTaxImportBLL(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// elementUI列表查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNum"></param>
        /// <param name="title"></param>
        /// <param name="creationby"></param>
        /// <param name="auditflag"></param>
        /// <returns></returns>
        public FuncResult Select(int pageSize, int pageNum, string ORG_CODE)
        {
            try
            {
                var query = _context.ApdFctTAx.
                        Where(a =>
                        (
                         (string.IsNullOrWhiteSpace(ORG_CODE) || a.ORG_CODE.Contains(ORG_CODE))
                        //&& (string.IsNullOrWhiteSpace(BUSINESS_TYPE_STATUS.ToString()) || a.BUSINESS_TYPE_STATUS == (BUSINESS_TYPE_STATUS))
                        //&& (a.DELETE_FLAG != 1)
                        )
                        ).ToList().OrderByDescending(e => e.LAST_UPDATE_DATE);
                int total = query.Count();
                var data = query.Skip(pageSize * pageNum).Take(pageSize).ToList();

                return new FuncResult() { IsSuccess = true, Content = new { data, total } };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Content = null, Message = ex.Message };
            }
        }

        //public FuncResult SelectORG()
        //{
        //    try
        //    {
        //        var query = _context.ApdFctTAx.
        //                Where(a =>
        //                (
        //                 (string.IsNullOrWhiteSpace(ORG_CODE) || a.ORG_CODE.Contains(ORG_CODE))
        //                //&& (string.IsNullOrWhiteSpace(BUSINESS_TYPE_STATUS.ToString()) || a.BUSINESS_TYPE_STATUS == (BUSINESS_TYPE_STATUS))
        //                //&& (a.DELETE_FLAG != 1)
        //                )
        //                ).ToList().OrderByDescending(e => e.LAST_UPDATE_DATE);
        //        int total = query.Count();
        //        var data = query.Skip(pageSize * pageNum).Take(pageSize).ToList();

        //        return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FuncResult() { IsSuccess = false, Content = null, Message = ex.Message };
        //    }
        //}
    }
}
