using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.OrgBLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.Org;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.Org
{
    [Route("api/[controller]")]
    //[ApiController]
    public class DimOrgController : ControllerBase
    {
        private readonly DataContext context;
        private readonly DimOrgBll orgBll;
        private readonly IMemoryCache cache;

        public DimOrgController(DataContext _context, IMemoryCache _cache)
        {
            this.context = _context;
            this.cache = _cache;
            this.orgBll = new DimOrgBll(_context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetListPagination")]
        public FuncResult GetListPagination([FromBody] SearchOrgModel model)
        {
            try
            {
                model.page--; if (model.page < 0)
                {
                    model.page = 0;
                }
                return orgBll.GetListPagination(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public FuncResult Add([FromBody]PostOrgModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return orgBll.Add(model,HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update/{recordid}")]
        public FuncResult Update(string recordid,[FromBody]PostOrgModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return orgBll.Update(recordid,model, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <returns></returns>
        [HttpGet("delete/{recordid}")]
        public FuncResult Delete(string recordid)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return orgBll.Delete(recordid, HttpContext.CurrentUser(cache).Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}