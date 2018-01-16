using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Database;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql;

namespace FUNAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT", "") != "" && Configuration.GetValue<string>("DB_CONNECTIONSTRING", "") == "")
            {
                throw new ArgumentNullException("CONNECTIONSTRING is Null");
            }
            services.AddLogging();
            services.AddCors();
            services.AddDbContext<LecturesContext>(options => options.UseMySql(Configuration.GetValue<string>("DB_CONNECTIONSTRING", "")));
            services.AddScoped<IReadOnlyRepository<LectureJson>, LectureRepository>();
            services.AddMvc().AddJsonOptions(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, LecturesContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole();
            app.UseCors(bulder => bulder.AllowAnyOrigin().WithMethods("GET", "HEAD", "OPTIONS"));
            app.UseMiddleware<FUNAPI.Middlewares.AcceptOnlyGetMiddleware>();
            app.UseMvc();
            app.UseMiddleware<FUNAPI.Middlewares.ReturnJsonOnErrorMiddleware>();
        }
    }
}