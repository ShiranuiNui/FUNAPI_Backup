using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FUNAPI.Repository
{
    public abstract class SuperInMemoryRepository
    {
        public bool IsReady { get; set; } = false;
        public SuperInMemoryRepository()
        {
            throw new NotImplementedException();
        }
        public SuperInMemoryRepository(IHostingEnvironment environment)
        {
            if (environment.IsProduction())
            {
                string tsvPath = environment.ContentRootPath.Substring(0, environment.ContentRootPath.IndexOf("/FUNAPI_Backup/") + 15) + "MainData/";
                this.IsReady = this.Initialize(tsvPath);
            }
        }
        public SuperInMemoryRepository(IConfiguration configuration)
        {
            string tsvPath = configuration.GetValue<string>("TSVPATH");
            if (string.IsNullOrEmpty(tsvPath))
            {
                throw new ArgumentNullException("TSVPATH IS EMPTY");
            }
            this.IsReady = this.Initialize(tsvPath);
        }
        private bool Initialize(string tsvPath) { throw new MethodAccessException(); }
    }
}