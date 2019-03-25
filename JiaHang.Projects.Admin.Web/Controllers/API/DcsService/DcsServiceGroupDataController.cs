using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.BLL.DcsService;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.DcsServiceGroup.RequestModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JiaHang.Projects.Admin.Web.Controllers.API.DcsServiceGroup
{
    [Route("api/[controller]")]
    public class DcsServiceGroupDataController : Controller
    {
        private readonly DcsServiceGroupBLL DcsserviceBll;
        private readonly IMemoryCache cache;
        private IHostingEnvironment hostingEnv;

        public DcsServiceGroupDataController(DataContext datacontext, IMemoryCache cache,IHostingEnvironment evn)
        {
            DcsserviceBll = new DcsServiceGroupBLL(datacontext);
            this.cache = cache;
            this.hostingEnv = evn;
        }

        [Route("Search")]
        [HttpPost]
        public FuncResult Select([FromBody] SearchDesServiceGroup model)
        {
            model.page--;if (model.page < 0)
                model.page = 0;

            return DcsserviceBll.Select(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<FuncResult> Delete([FromRoute]string id)
        {
            return await DcsserviceBll.Delete(id, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 删除（按批）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Route("BatchDelete")]
        [HttpDelete]
        public async Task<FuncResult> Delete(string[] ids)
        {
            return await DcsserviceBll.Delete(ids, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<FuncResult> Add([FromBody] DcsServiceGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return new FuncResult() { IsSuccess = false, Message = "参数错误" };
            }
            return await DcsserviceBll.Add(model, HttpContext.CurrentUser(cache).Id);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<FuncResult> Update(string id, [FromBody]DcsServiceGroupModel model)
        {
            FuncResult data = await DcsserviceBll.Update(id, model, HttpContext.CurrentUser(cache).Id);
            return data;
        }

        [HttpGet]
        [Route("Export")]
        public async Task<IActionResult> Export()
        {
            var result = await DcsserviceBll.GetDcsServiceGroupListBytes();
            return File(result, "application/ms-excel", $"目录分类.xlsx");

        }


        /// <summary>
        /// 图片上传（上传路径为~/wwwroot/images/catelog/..）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        public FuncResult UploadImage()
        {
            try
            {
                #region 图片上传
                var imgFile = Request.Form.Files[0];
                if (imgFile != null && !string.IsNullOrEmpty(imgFile.FileName))
                {
                    long size = 0;
                    string tempname = "";
                    var filename = ContentDispositionHeaderValue
                                    .Parse(imgFile.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    var extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));
                    var filename1 = Guid.NewGuid().ToString().Substring(0, 6) + extname;
                    tempname = filename1;
                    var path = hostingEnv.WebRootPath;
                    string dir = DateTime.Now.ToString("yyyyMMdd");
                    if (!Directory.Exists(hostingEnv.WebRootPath + $@"\images\catelog\{dir}"))
                    {
                        Directory.CreateDirectory(hostingEnv.WebRootPath + $@"\images\catelog\{dir}");
                    }
                    filename = hostingEnv.WebRootPath + $@"\images\catelog\{dir}\{filename1}";
                    size += imgFile.Length;
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        imgFile.CopyTo(fs);
                        fs.Flush();
                    }
                    return new FuncResult() { IsSuccess = true, Message = "上传成功", Content = new { src = $"/images/catelog/{dir}/{filename1}" } };
                }
          
                #endregion
            }
            catch (Exception ex)
            {
                return new FuncResult() { IsSuccess = false, Message = ex.Message };
            }
            return new FuncResult() { IsSuccess = false, Message = "上传失败" };
        }
    }
}
