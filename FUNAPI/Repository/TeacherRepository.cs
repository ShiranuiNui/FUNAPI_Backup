using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.EntityFrameworkCore;
namespace FUNAPI.Repository
{
    public class TeacherRepository : IReadOnlyRepository<Teacher>
    {
        private readonly LecturesContext context;
        public TeacherRepository(LecturesContext _context)
        {
            this.context = _context;
        }
        public async Task<IEnumerable<Teacher>> GetAllAsync() =>
        await this.context.Teachers.ToListAsync();

        public async Task<Teacher> GetSingleAsync(int id) =>
        await this.context.Teachers.SingleOrDefaultAsync(x => x.TeacherId == id);
    }
}