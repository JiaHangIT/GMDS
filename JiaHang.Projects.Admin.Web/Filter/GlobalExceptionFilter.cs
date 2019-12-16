using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiaHang.Projects.Admin.Web.Filter
{
    public class GlobalExceptionFilter : Attribute, IExceptionFilter
    {

        private readonly IHostingEnvironment environment = null;


        public GlobalExceptionFilter(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        //实现
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            string error = string.Empty;

            void ReadException(Exception ex)
            {
                error += string.Format("{0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException);
                if (ex.InnerException != null)
                {
                    ReadException(ex.InnerException);
                }
            }

            ReadException(context.Exception);
            LogService.WriteError(error);

            //ContentResult result = new ContentResult
            //{
            //    StatusCode = 500,
            //    ContentType = "text/json;charset=utf-8;"
            //};

            //if (environment.IsDevelopment())
            //{
            //    var json = new { message = exception.Message, detail = error };
            //    result.Content = JsonConvert.SerializeObject(json);
            //}
            //else
            //{
            //    result.Content = "抱歉，出错了";
            //}
            //context.Result = result;
            //context.ExceptionHandled = true;
        }
    }
}
