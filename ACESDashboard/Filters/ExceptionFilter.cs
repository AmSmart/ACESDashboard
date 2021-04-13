using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExceptionFilter(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                return;
            }

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = 500;
            var file = File.ReadAllBytes(Path.Combine(_webHostEnvironment.WebRootPath, "error.html"));

            context.Result = new FileContentResult(file, "text/html");
        }
    }
}
