using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
namespace FUNAPI.Repository
{
    public class ClassInMemoryRepository : IReadOnlyRepository<Class>
    {
        private List<Class> context { get; set; } = new List<Class>();
        public bool IsReady { get; set; } = false;
        //TODO:FIX PERFORMANCE
        public ClassInMemoryRepository(IHostingEnvironment environment)
        {
            string tsvPath = environment.ContentRootPath.Substring(0, environment.ContentRootPath.IndexOf("/FUNAPI_Backup/") + 15) + "MainData/";
            this.IsReady = this.Initialize(tsvPath);
        }
        public ClassInMemoryRepository(IConfiguration configuration)
        {
            string tsvPath = configuration.GetValue<string>("TSVPATH");
            if (string.IsNullOrEmpty(tsvPath))
            {
                throw new ArgumentNullException("TSVPATH IS EMPTY");
            }
            this.IsReady = this.Initialize(tsvPath);
        }
        private bool Initialize(string tsvPath)
        {
            this.context = File.ReadAllLines(tsvPath + "/Classes.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1)
                .Select(x => new Class() { ClassId = int.Parse(x[1]), disp_class = x[2], course = int.Parse(x[3]) }).ToList();
            return this.context.Any();
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            return await Task.Run(() => this.context);
        }
        public async Task<Class> GetSingleAsync(int id)
        {
            if (this.context.Any(x => x.ClassId == id))
            {
                return await Task.Run(() => this.context.SingleOrDefault(x => x.ClassId == id));
            }
            else
            {
                return null;
            }
        }
    }
}