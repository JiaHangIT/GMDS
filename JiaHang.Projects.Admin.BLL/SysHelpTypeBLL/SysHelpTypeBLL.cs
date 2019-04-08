using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysHelpType.RequestModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.BLL.SysHelpTypeBLL
{
    public class SysHelpTypeBLL
    {
        private readonly DAL.EntityFramework.DataContext _context;
        public SysHelpTypeBLL(DAL.EntityFramework.DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FuncResult Select(SearchSysHelpTypeModel model)
        {
            IOrderedQueryable<SysHelpType> query = _context.SysHelpType.
                Where(a =>
                (
                (string.IsNullOrWhiteSpace(model.Help_Type_Name) || a.HelpTypeName.Contains(model.Help_Type_Name))
                )
                ).OrderByDescending(e => e.CreationDate);
            int total = query.Count();
            var data = query.Skip(model.limit * model.page).Take(model.limit).ToList().Select(e => new
            {

                Help_Type_Id = e.HelpTypeId,
                Help_Type_Name = e.HelpTypeName,
                Creation_Date=e.CreationDate
                //e.HelpTypeId,
                //e.HelpTypeName
            });
           return new FuncResult() { IsSuccess = true, Content = new { data, total } };
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FuncResult> Select(string id)
        {
            SysHelpType entity = await _context.SysHelpType.FindAsync(id);

            return new FuncResult() { IsSuccess = true, Content = entity };
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Update(string id, SysHelpTypeModel model, string currentUserId)
        {
            SysHelpType entity = await _context.SysHelpType.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "帮助类型ID错误!" };
            }
           
            entity.HelpTypeName = model.HelpTypeName;
            entity.LastUpdateDate = DateTime.Now;
            entity.LastUpdatedBy = currentUserId;

            _context.SysHelpType.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "修改成功" };
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Delete(string id, string currentUserId)
        {
            SysHelpType entity = await _context.SysHelpType.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "帮助类型ID不存在!" };
            }
            entity.DeleteFlag = 1;
            //entity.DeleteFlag = true;

            entity.DeleteBy = currentUserId;
            entity.DeleteDate = DateTime.Now;
            _context.SysHelpType.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
        public async Task<FuncResult> Delete(string[] ids, string currentUserId)
        {
            IQueryable<SysHelpType> entitys = _context.SysHelpType.Where(e => ids.Contains(e.HelpTypeId));
            if (entitys.Count() != ids.Length)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            foreach (SysHelpType obj in entitys)
            {
                obj.DeleteBy = currentUserId;
                //obj.DeleteFlag = true;
                obj.DeleteFlag = 1;
                obj.DeleteDate = DateTime.Now;
                _context.SysHelpType.Update(obj);
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
            return new FuncResult() { IsSuccess = true, Message = $"已成功删除{ids.Length}条记录" };
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<FuncResult> Add(SysHelpTypeModel model, string currentUserId)
        {
            SysHelpType entity = new SysHelpType
            {
                HelpTypeId = Guid.NewGuid().ToString(),
                HelpTypeName = model.HelpTypeName,
                
                LastUpdatedBy = currentUserId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now,
                CreatedBy = currentUserId,
                DeleteBy="00000000000000000000000000000000"
            };
            await _context.SysHelpType.AddAsync(entity);
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
        //public async Task<byte[]> GetUserListBytes()
        //{
        //    var comlumHeadrs = new[] { "帮助类型ID", "帮助类型名称" };
        //    byte[] result;
        //    var data = _context.SysHelpType.ToList();
        //    var package = new ExcelPackage();
        //    var worksheet = package.Workbook.Worksheets.Add("Sheet1"); //Worksheet name
        //                                                               //First add the headers
        //    for (var i = 0; i < comlumHeadrs.Count(); i++)
        //    {
        //        worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
        //    }
        //    //Add values
        //    var j = 2;
        //    // var chars = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        //    await Task.Run(() =>
        //    {
        //        foreach (var obj in data)
        //        {
        //            var rt = obj.GetType();
        //            var rp = rt.GetProperties();
        //            worksheet.Cells["A" + j].Value = obj.HelpTypeId;
        //            worksheet.Cells["B" + j].Value = obj.HelpTypeName;
        //            j++;
        //        }
        //    });
        //    result = package.GetAsByteArray();
        //    return result;
        //}


    }
}
