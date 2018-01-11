using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace FUNAPI.Middlewares
{
    public class ReturnJsonOnErrorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly Dictionary<int, string> ResponseStringDictionary = new Dictionary<int, string>()
        { { 400, "400_bad_request" }, { 404, "404_not_found" }, { 405, "405_method_not_allowed" }, { 411, "411_length_required" }, { 500, "500_internal_server_error" }
        };
        public ReturnJsonOnErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            int statuscode = context.Response.StatusCode;
            if (statuscode / 200 != 1)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { code = statuscode, message = ResponseStringDictionary[statuscode] }));
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}