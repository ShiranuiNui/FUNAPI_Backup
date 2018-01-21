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
    public class LectureInMemoryRepository : IReadOnlyRepository<LectureJson>
    {
        private List<Lecture> context { get; set; } = new List<Lecture>();
        public bool IsReady { get; set; } = false;
        //TODO:FIX PERFORMANCE
        public LectureInMemoryRepository(IHostingEnvironment environment)
        {
            string tsvPath = environment.ContentRootPath.Substring(0, environment.ContentRootPath.IndexOf("/FUNAPI_Backup/") + 15) + "MainData/";
            this.IsReady = this.Initialize(tsvPath);
        }
        public LectureInMemoryRepository(IConfiguration configuration)
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
            var lectures = File.ReadAllLines(tsvPath + "/Lectures.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => new Lecture() { LectureId = int.Parse(x[1]), disp_lecture = x[2], week = int.Parse(x[4]), jigen = int.Parse(x[5]) }).ToList();
            var lectures_rooms = File.ReadAllLines(tsvPath + "/Lectures_Rooms.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Where((data, i) => i % 2 == 1).Select(x => x.Skip(1)).ToList();
            var lectures_teachers = File.ReadAllLines(tsvPath + "/Lectures_Teachers.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Where((data, i) => i % 2 == 1).Select(x => x.Skip(1)).ToList();
            var lectures_classes = File.ReadAllLines(tsvPath + "/Lectures_Classes.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => x.Skip(2)).ToList();
            foreach (var lecture in lectures.Select((data, i) => new { data, i }))
            {
                var rooms = lectures_rooms[lecture.i].Where(x => int.TryParse(x, out int y)).Select(x => int.Parse(x)).ToList();
                var teachers = lectures_teachers[lecture.i].Where(x => int.TryParse(x, out int y)).Select(x => int.Parse(x)).ToList();
                var classes = lectures_classes[lecture.i].Where(x => int.TryParse(x, out int y)).Select(x => int.Parse(x)).ToList();

                var lecturerooms = rooms.Select(x => new LectureRoom() { LectureId = lecture.i, RoomId = x });
                var lectureteachers = teachers.Select(x => new LectureTeacher() { LectureId = lecture.i, TeacherId = x });
                var lectureclasses = classes.Select(x => new LectureClass() { LectureId = lecture.i, ClassId = x });

                lecture.data.LectureRooms.AddRange(lecturerooms);
                lecture.data.LectureTeachers.AddRange(lectureteachers);
                lecture.data.LectureClasses.AddRange(lectureclasses);
                this.context.Add(lecture.data);
            }
            return this.context.Any();
        }

        public async Task<IEnumerable<LectureJson>> GetAllAsync()
        {
            return await Task.Run(() => this.context.Select(x => new LectureJson(x)).ToList());
        }
        public async Task<LectureJson> GetSingleAsync(int id)
        {
            if (this.context.Any(x => x.LectureId == id))
            {
                return await Task.Run(() => new LectureJson(this.context.SingleOrDefault(x => x.LectureId == id)));
            }
            else
            {
                return null;
            }
        }
    }
}