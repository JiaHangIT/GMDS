using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.DcsService;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceInfo.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsService
{
    [Route("api/[controller]")]
    [ApiController]
    public class DcsServiceAuditAndPubDataController : ControllerBase
    {
        private readonly DcsServiceInfoBLL DcsServiceInfo;
        private readonly DataContext context;
        private readonly IMemoryCache cache;

        public DcsServiceAuditAndPubDataController(DataContext datacontext, IMemoryCache cache)
        {
            DcsServiceInfo = new DcsServiceInfoBLL(datacontext);
            context = datacontext;
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="serviceno"></param>
        /// <param name="servicecode"></param>
        /// <param name="servicename"></param>
        /// <returns></returns>
        [HttpGet("{pageIndex}/{pageSize}")]
        public FuncResult Select(int pageIndex, int pageSize, string serviceno, string servicecode, string servicename)
        {
            pageIndex--; if (pageIndex < 0)
                pageIndex = 0;

            return DcsServiceInfo.Select(pageIndex, pageSize, serviceno, servicecode, servicename);
        }

        /// <summary>
        /// 接口审核
        /// </summary>
        /// <param name="auditJson"></param>
        /// <returns></returns>
        [HttpPost("Audit")]
        public FuncResult AuditService([FromBody]ServiceAudit model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new FuncResult() { IsSuccess = false, Message = "参数错误" };
                }
                DcsServiceInfo serviceInfo = context.DcsServiceInfo.Find(model.ServiceId);
                if (serviceInfo == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = string.Format("Error:{0}", "接口主键信息异常，或当前审核接口已被删除!") };
                }
                serviceInfo.AuditFlag = model.AuditFlag;
                serviceInfo.AuditDate = DateTime.Now;
                serviceInfo.AuditedBy = HttpContext.CurrentUser(cache).Id;
                context.DcsServiceInfo.Update(serviceInfo);
                context.SaveChanges();
                return new FuncResult() { IsSuccess = true, Message = "Success" };
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}