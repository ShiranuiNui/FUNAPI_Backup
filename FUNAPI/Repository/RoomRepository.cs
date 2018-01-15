using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.EntityFrameworkCore;
namespace FUNAPI.Repository
{
    public class RoomRepository : IReadOnlyRepository<Room>
    {
        private readonly LecturesContext context;
        public RoomRepository(LecturesContext _context)
        {
            this.context = _context;
        }
        public async Task<IEnumerable<Room>> GetAllAsync() =>
        await this.context.Rooms.ToListAsync();

        public async Task<Room> GetSingleAsync(int id) =>
        await this.context.Rooms.SingleOrDefaultAsync(x => x.RoomId == id);
    }
}