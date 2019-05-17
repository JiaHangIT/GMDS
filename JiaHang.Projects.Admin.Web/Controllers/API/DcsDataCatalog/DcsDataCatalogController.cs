using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Web.WebApiIdentityAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using JiaHang.Projects.Admin.BLL.DcsDataCatalogBLL;
using JiaHang.Projects.Admin.Model.DcsDataCatalog.RequestModel;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsDataCatalog
{
    [Route("api/[controller]")]
    public class DcsDataCatalogController:ControllerBase
    {
        private readonly DcsDataCatalogBLL dataDcsDataCatalog;
        private readonly IMemoryCache cache;
        public DcsDataCatalogController(DataContext dataContext, IMemoryCache cache)
        {
            dataDcsDataCatalog = new DcsDataCatalogBLL(dataContext);
            this.cache = cache;
        }
        [HttpGet("{pageSize}/{currentPage}")]
        public FuncResult Select(int pageSize, int currentPage, string dataCatalogName)
        {
            currentPage--;
            return dataDcsDataCatalog.Select(pageSize, currentPage, dataCatalogName);
        }
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DcsDataCatalogModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await dataDcsDataCatalog.Add(model, HttpContext.CurrentUser(cache).Id);
        }
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]DcsDataCatalogModel model)
        {
            FuncResult data = await dataDcsDataCatalog.Update(id, model, HttpContext.CurrentUser(cache).Id);
            return data;
        }
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]string id)
        {
            return await dataDcsDataCatalog.Delete(id, HttpContext.CurrentUser(cache).Id);
        }
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> DeleteDataSourceInfos(string[] ids)
        {
            return await dataDcsDataCatalog.Delete(ids, HttpContext.CurrentUser(cache).Id);
        }
    }
}
