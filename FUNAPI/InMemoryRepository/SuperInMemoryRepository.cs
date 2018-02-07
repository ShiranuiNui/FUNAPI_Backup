using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FUNAPI.Repository
{
    public abstract class SuperInMemoryRepository<T>
    {
        protected List<T> context { get; set; } = new List<T>();
        public bool IsReady { get; set; } = false;
        public SuperInMemoryRepository(IEnumerable<T> data)
        {
            this.context = data.ToList();
            this.IsReady = true;
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
        protected virtual bool Initialize(string tsvPath) { throw new MethodAccessException(); }
    }
}