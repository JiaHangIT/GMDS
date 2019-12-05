using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.GasImport
{
    [Route("api/[controller]")]
    //[ApiController]
    public class GasImportController : ControllerBase
    {
        public readonly DataContext context;
        private readonly IMemoryCache cache;
        private readonly IHostingEnvironment hosting;

        public GasImportController(DataContext _context, IHostingEnvironment _hosting, IMemoryCache _cache) { this.context = _context; this.hosting = _hosting; this.cache = _cache; }

    }
}