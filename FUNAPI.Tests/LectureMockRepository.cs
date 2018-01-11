using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;

public class LectureMockRepository : ILectureRepository
{
    private readonly List<Lecture> context;
    public LectureMockRepository(List<Lecture> _context)
    {
        this.context = _context;
    }
    public async Task<IEnumerable<LectureJson>> GetAllAsync()
    {
        return await Task.Run(() => this.context.Select(x => new LectureJson(x)).ToList());
    }
    public async Task<LectureJson> GetSingleAsync(int id)
    {
        return await Task.Run(() =>
        {
            var joinedEntity = this.context.SingleOrDefault(x => x.LectureId == id);
            if (joinedEntity == null)
            {
                return null;
            }
            else
            {
                return new LectureJson(joinedEntity);
            }
        });
    }
}