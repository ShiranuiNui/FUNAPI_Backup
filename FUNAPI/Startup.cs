using System;
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUNAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;
            if (Environment.IsProduction())
            {
                this.lectureInMemoryRepository = new LectureInMemoryRepository(Configuration);
                this.classInMemoryRepository = new ClassInMemoryRepository(Configuration);
                this.roomInMemoryRepository = new RoomInMemoryRepository(Configuration);
                this.teacherInMemoryRepository = new TeacherInMemoryRepository(Configuration);
                if (!this.lectureInMemoryRepository.IsReady || !this.classInMemoryRepository.IsReady ||
                    !this.roomInMemoryRepository.IsReady || !this.teacherInMemoryRepository.IsReady)
                {
                    throw new Exception();
                }
            }
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        private readonly LectureInMemoryRepository lectureInMemoryRepository;
        private readonly ClassInMemoryRepository classInMemoryRepository;
        private readonly RoomInMemoryRepository roomInMemoryRepository;
        private readonly TeacherInMemoryRepository teacherInMemoryRepository;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddCors();
            if (Environment.IsProduction())
            {
                services.TryAddSingleton<IReadOnlyRepository<LectureJson>>(lectureInMemoryRepository);
                services.TryAddSingleton<IReadOnlyRepository<Class>>(classInMemoryRepository);
                services.TryAddSingleton<IReadOnlyRepository<Room>>(roomInMemoryRepository);
                services.TryAddSingleton<IReadOnlyRepository<Teacher>>(teacherInMemoryRepository);
            }
            services.AddMvc().AddJsonOptions(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddConsole();
            app.UseCors(bulder => bulder.AllowAnyOrigin().WithMethods("GET", "HEAD", "OPTIONS"));
            app.UseMiddleware<FUNAPI.Middlewares.ReturnJsonOnErrorMiddleware>();
            app.UseMiddleware<FUNAPI.Middlewares.AcceptOnlyGetMiddleware>();
            app.UseMvc();
            //Database.DatabaseInitializer.Invoke(context, hostingEnvironment);
        }
    }
}