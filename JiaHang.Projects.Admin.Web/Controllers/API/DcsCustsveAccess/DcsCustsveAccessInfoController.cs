
using JiaHang.Projects.Admin.BLL;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DcsCustsveAccessInfoController : ControllerBase
    {
        private readonly DcsCustsveAccessInfoBLL  dcsCustsveAccessInfoBLL;
        private readonly IMemoryCache cache;
        public DcsCustsveAccessInfoController(DataContext context, IMemoryCache cache)
        {
            dcsCustsveAccessInfoBLL = new DcsCustsveAccessInfoBLL(context);
            this.cache = cache;
        }

        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Get(int pageSize, int currentPage)
        {
             currentPage--;
            var data = dcsCustsveAccessInfoBLL.List(pageSize,currentPage,HttpContext.CurrentUser(cache).Id);
            return data;
        }

        [HttpGet("{accessId}")]
        public FuncResult Get( string accessId)
        {
            var data = dcsCustsveAccessInfoBLL.Detail(accessId);
            return data;
        }
    }
}
