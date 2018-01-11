using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.EntityFrameworkCore;

public interface ILectureRepository : IReadOnlyRepository<LectureJson>
{

}
public class LectureRepository : ILectureRepository
{
    private readonly LecturesContext context;
    public LectureRepository(LecturesContext _context)
    {
        this.context = _context;
    }
    public async Task<IEnumerable<LectureJson>> GetAllAsync()
    {
        var joinedEntity = await this.context.Lectures.Include(x => x.LectureRooms).Include(x => x.LectureTeachers).Include(x => x.LectureClasses).ToListAsync();
        return joinedEntity.Select(x => new LectureJson(x));
    }
    public async Task<LectureJson> GetSingleAsync(int id)
    {
        var joinedEntity = await this.context.Lectures.Include(x => x.LectureRooms).Include(x => x.LectureTeachers).Include(x => x.LectureClasses).SingleOrDefaultAsync(x => x.LectureId == id);
        if (joinedEntity == null)
        {
            return null;
        }
        else
        {
            return new LectureJson(joinedEntity);
        }
    }
}