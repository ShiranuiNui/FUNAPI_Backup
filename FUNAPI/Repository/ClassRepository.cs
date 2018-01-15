using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.EntityFrameworkCore;
namespace FUNAPI.Repository
{
    public class ClassRepository : IReadOnlyRepository<Class>
    {
        private readonly LecturesContext context;
        public ClassRepository(LecturesContext _context)
        {
            this.context = _context;
        }
        public async Task<IEnumerable<Class>> GetAllAsync() =>
        await this.context.Classes.ToListAsync();

        public async Task<Class> GetSingleAsync(int id) =>
        await this.context.Classes.SingleOrDefaultAsync(x => x.ClassId == id);
    }
}