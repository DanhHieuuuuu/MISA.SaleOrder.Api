using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    public class ValidateExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        public ValidateExceptionMiddleWare(RequestDelegate next) 
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidateException vex)
            {
                context.Response.StatusCode = 400;
                var res = new
                {
                    DevMsg = "Lỗi validate",
                    UserMsg = vex.Message,
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(res));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
