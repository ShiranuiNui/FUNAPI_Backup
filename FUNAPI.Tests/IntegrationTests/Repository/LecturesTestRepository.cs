using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.EntityFrameworkCore;
namespace FUNAPI.Tests.Integration.Repository
{
    public class LecturesTestRepository : IReadOnlyRepository<LectureJson>
    {
        private readonly LecturesContext context;
        public LecturesTestRepository(LecturesContext _context)
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
            this.context.Rooms.Add(new Room { disp_room = "教室１" });
            this.context.Classes.Add(new Class { ClassId = 101, disp_class = "1A" });
            this.context.Teachers.Add(new Teacher { disp_teacher = "教員１", roman_name = "Kyouin1", position = "教授", research_area = "研究分野", role = "所属学科" });
            this.context.SaveChanges();
            var rooms = this.context.Rooms.ToList();
            var classes = this.context.Classes.ToList();
            var teachers = this.context.Teachers.ToList();
            if (!rooms.Any() || !classes.Any() || !teachers.Any())
            {
                throw new IndexOutOfRangeException();
            }
            var lecture = new Lecture { disp_lecture = "教科A", must = "", week = 1, jigen = 1 };
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