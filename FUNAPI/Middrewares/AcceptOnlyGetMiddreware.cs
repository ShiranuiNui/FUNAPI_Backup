using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace FUNAPI.Middlewares
{
    public class AcceptOnlyGetMiddleware
    {
        private readonly RequestDelegate next;
        public AcceptOnlyGetMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method != "GET")
            {
                var response = context.Response;
                response.StatusCode = 405;
                await response.WriteAsync("");
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}