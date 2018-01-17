using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Models;
using FUNAPI.Repository;
using FUNAPI.Tests.Integration.Context;
using Microsoft.EntityFrameworkCore;
namespace FUNAPI.Tests.Integration.Repository
{
    public class LecturesTestRepository : IReadOnlyRepository<LectureJson>
    {
        private readonly LecturesTestContext context;
        public LecturesTestRepository(LecturesTestContext _context)
        {
            this.context = _context;
            this.SetData();
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
        public void SetData()
        {
            this.context.Rooms.Add(new Room { RoomId = 0, disp_room = "教室１" });
            this.context.Classes.Add(new Class { ClassId = 101, disp_class = "1A" });
            this.context.Teachers.Add(new Teacher { TeacherId = 0, disp_teacher = "教員１", roman_name = "Kyouin1", position = "教授", research_area = "研究分野", role = "所属学科" });
            this.context.SaveChanges();
            var rooms = this.context.Rooms.Where(x => x.RoomId == 0).Select(x => x).ToList();
            var classes = this.context.Classes.Where(x => x.ClassId == 101).Select(x => x).ToList();
            var teachers = this.context.Teachers.Where(x => x.TeacherId == 0).Select(x => x).ToList();
            var lecture = new Lecture { LectureId = 0, disp_lecture = "教科A", must = "", week = 1, jigen = 1 };
            var lectureroom = rooms.Select(x => new LectureRoom() { Lecture = lecture, Room = x });
            var lectureclass = classes.Select(x => new LectureClass() { Lecture = lecture, Class = x });
            var lectureteacher = teachers.Select(x => new LectureTeacher() { Lecture = lecture, Teacher = x });
            lecture.LectureRooms.AddRange(lectureroom);
            lecture.LectureClasses.AddRange(lectureclass);
            lecture.LectureTeachers.AddRange(lectureteacher);
            this.context.Lectures.Add(lecture);
            this.context.SaveChanges();
        }
    }
}