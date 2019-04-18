using JiaHang.Projects.Admin.BLL.DcsCustomerBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsCustomerServiceParams.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Controllers.API
{

    [Route("api/[controller]")]
    public class DcsCustomerServiceDataController : ControllerBase
    {

        private readonly DcsCustomerServiceBLL  dcsCustomerServiceBLL;
        private readonly IMemoryCache cache;
        public DcsCustomerServiceDataController(DAL.EntityFramework.DataContext dataContext, IMemoryCache cache)
        {
            dcsCustomerServiceBLL = new DcsCustomerServiceBLL(dataContext);
            this.cache = cache;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize, int currentPage, string customerName)
        {
            currentPage--;

            return dcsCustomerServiceBLL.Select(pageSize, currentPage, customerName);

        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>       
        [HttpGet]
        [Route("NotBind/{customerId}")]
        public FuncResult NotBind(string customerId)
        {            
            return dcsCustomerServiceBLL.NotBind(customerId);

        }
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DcsCustomerServiceParamsModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await dcsCustomerServiceBLL.Add( HttpContext.CurrentUser(cache).Id,model);
        }

        [HttpPost]
        [Route("UpdateField")]
        public async Task<FuncResult> UpdateField([FromBody] DcsCustomerServiceParamsModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await dcsCustomerServiceBLL.UpdateField(HttpContext.CurrentUser(cache).Id, model);
        }

        

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}/{serviceId}")]
        public async Task<FuncResult> Delete(string customerId, string serviceId)
        {
            return await dcsCustomerServiceBLL.Delete(customerId,serviceId, HttpContext.CurrentUser(cache).Id);

        }

        
    }
}
