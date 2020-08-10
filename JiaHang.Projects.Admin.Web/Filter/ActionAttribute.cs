using JiaHang.Projects.Admin.DAL.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;

namespace JiaHang.Projects.Admin.Web.Filter
{
    public class ActionAttribute :  IActionFilter
    {
        private string ActionInfo { get; set; }

        private string ActionFlag { get; set; }

        private string ActionArguments { get; set; }

        public string RequestBody;

        private object ActionAttributeLog { get; set; }

        private string UserId { get; set; }

        public string Ip { get; set; }

        private readonly IMemoryCache cache;
        private readonly DataContext datacontext;
        private readonly IHttpContextAccessor accessor;

        public ActionAttribute(IMemoryCache _cache,DataContext _context, IHttpContextAccessor _accessor)
        {
            this.cache = _cache;
            this.datacontext = _context;
            this.accessor = _accessor;
        }

        Stopwatch Stopwatch;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                Stopwatch = new Stopwatch();
                long contenxtLen = context.HttpContext.Request.ContentLength == null ? 0 : context.HttpContext.Request.ContentLength.Value;

                if (contenxtLen > 0)
                {
                    //读取请求体的值
                    Stream fs = context.HttpContext.Request.Body;
                    if (context.HttpContext.Request.Method.ToUpper() == "POST")
                    {
                        fs.Position = 0;
                    }
                    byte[] buffer = new byte[fs.Length];
                    int r = fs.Read(buffer, 0, buffer.Length);
                    string requestbody = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                    RequestBody = requestbody;
                }
               
                Stopwatch.Start();
            }
            catch (Exception)
            {
                
            }
         
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                Stopwatch.Stop();
                string url = context.HttpContext.Request.Host + context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                url = url.ToLower();
                string method = context.HttpContext.Request.Method;

                string qs = RequestBody;

                dynamic result = context.Result.GetType().Name == "EmptyResult" ? new { Value = "无返回结果" } : context.Result as dynamic;

                string res = "在返回结果前发生了异常";
                try
                {
                    if (result != null)
                    {
                        res = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
                    }
                }
                catch (System.Exception)
                {
                    res = "日志未获取到结果，返回的数据无法序列化";
                }
                //如果是访问了api才写入日志
                if (url.Contains("/api/"))
                {
                    ActionFlag = url.Split('/')[2];
                    var ip = context.HttpContext.Request.Host;
                    var ips = accessor.HttpContext.Request.Host.Value;
                    var ipss = context.HttpContext.Request.Headers["remote"].ToString();

                    //获取ip地址
                    string ipaddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                    var Ip = ipaddress;
                    var Vdate = DateTime.Now;
                    //请求方式get,post,put等
                    var RequestType = context.HttpContext.Request.Method;
                    //获取请求地址
                    var Url = context.HttpContext.Request.Path;
                    //获取UserAgent
                    var UserAgent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();

                    if (!url.Contains("login"))
                    {
                        var t = context.HttpContext.Request.Cookies;
                        string str = $"\n 方法：{ActionFlag} \n " +
                                     $"用户id：{ context.HttpContext.CurrentUser(cache).Id} \n " +
                                     $"操作时间：{DateTime.Now} \n " +
                                     $"请求地址：{url} \n " +
                                     $"方式：{method} \n " +
                                     $"请求体：{RequestBody} \n " +
                                     $"参数：{qs}\n " +
                                     $"结果：{res}\n " +
                                     $"耗时：{Stopwatch.Elapsed.TotalMilliseconds} 毫秒（指控制器内对应方法执行完毕的时间）";
                        VisLog visLog = new VisLog()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Method = ActionFlag,
                            UserId = context.HttpContext.CurrentUser(cache).Id,
                            VisTime = DateTime.Now,
                            RequestUrl = url,
                            RequestBody = RequestBody,
                            RequestMethod = method,
                            Params = qs,
                            Result = res,
                            TakeUpTime = (decimal)Stopwatch.Elapsed.TotalMilliseconds,
                            DeleteFlag = 0
                        };
                        datacontext.VisLog.Add(visLog);
                        try
                        {
                            datacontext.SaveChanges();
                        }
                        catch (Exception)
                        {
                            
                        }

                    }

                    //LogService.WriteInfo(str); 
                    //将日志写入到日志表
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}
