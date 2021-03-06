using System;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace FUNAPI.Repository
{
    public class TeacherInMemoryRepository : SuperInMemoryRepository<Teacher>, IReadOnlyRepository<Teacher>
    {
        public TeacherInMemoryRepository(IConfiguration configuration) : base(configuration) { }
        protected override bool Initialize(string tsvPath)
        {
            this.context = File.ReadAllLines(tsvPath + "/Teachers.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1)
                .Select(x => new Teacher() { TeacherId = int.Parse(x[1]), disp_teacher = x[2], roman_name = x[3], position = x[4], research_area = x[5], role = x[6] }).ToList();
            return this.context.Any();
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await Task.Run(() => this.context);
        }
        public async Task<Teacher> GetSingleAsync(int id)
        {
            if (this.context.Any(x => x.TeacherId == id))
            {
                return await Task.Run(() => this.context.SingleOrDefault(x => x.TeacherId == id));
            }
            else
            {
                return null;
            }
        }
    }
}