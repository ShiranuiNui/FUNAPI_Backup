using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FUNAPI.Database
{
    class CourseJson
    {
        public int WeekNumber { get; set; }
        public int TimeNumber { get; set; }

        public string CourseName { get; set; }
        public string TargetClass { get; set; }
        public string[] Rooms { get; set; }
        public string[] Teachers { get; set; }
    }
    public class DatabaseInitializer
    {
        public static void Invoke(LecturesContext context, IHostingEnvironment environment)
        {
            //context.Database.Migrate();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string tsvPath = environment.ContentRootPath.Substring(0, environment.ContentRootPath.IndexOf("/FUNAPI_Backup/") + 15) + "MainData/";

            var roomsdata = File.ReadAllLines(tsvPath + "/Rooms.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => new Room() { disp_room = x[2] });
            context.Rooms.AddRange(roomsdata);

            var teachersdata = File.ReadAllLines(tsvPath + "/Teachers.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => new Teacher() { disp_teacher = x[2], roman_name = x[3], position = x[4], research_area = x[5], role = x[6] });
            context.Teachers.AddRange(teachersdata);

            var classesdata = File.ReadAllLines(tsvPath + "/Classes.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => new Class() { ClassId = int.Parse(x[1]), disp_class = x[2], course = int.Parse(x[3]) });
            context.Classes.AddRange(classesdata);
            context.SaveChanges();

            var lecturesdata = File.ReadAllLines(tsvPath + "/Lectures.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => new Lecture() { disp_lecture = x[2], week = int.Parse(x[4]), jigen = int.Parse(x[5]) }).ToList();
            var lectures_rooms = File.ReadAllLines(tsvPath + "/Lectures_Rooms.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Where((data, i) => i % 2 == 1).Select(x => x.Skip(1)).ToList();
            var lectures_teachers = File.ReadAllLines(tsvPath + "/Lectures_Teachers.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Where((data, i) => i % 2 == 1).Select(x => x.Skip(1)).ToList();
            var lectures_classes = File.ReadAllLines(tsvPath + "/Lectures_Classes.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1).Select(x => x.Skip(2)).ToList();

            var lectures = new List<Lecture>();

            foreach (var lecture in lecturesdata.Select((data, i) => new { data, i }))
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
                lectures.Add(lecture.data);
            }
            context.AddRange(lectures);
            context.SaveChanges();
            /*
            var roomsdata = File.ReadAllLines("./Database/data/rooms.json").Select(x => x.Replace("\"", "")).Select(x => new Room() { disp_room = x });
            Console.WriteLine("Room and Teachers Initialize Complete");
            var DBRooms = context.Rooms.AsEnumerable().ToList();
            var DBTeachers = context.Teachers.AsEnumerable().ToList();
            var timetables = JsonConvert.DeserializeObject<IEnumerable<CourseJson>>(File.ReadAllText("./Database/data/timetable.json"))
                .Select(
                    timetableobj =>
                    {
                        var rooms = new List<Room>();
                        foreach (string room in timetableobj.Rooms)
                        {
                            rooms.Add(DBRooms.FirstOrDefault(x => x.disp_room == room));
                        }
                        var teachers = new List<Teacher>();
                        foreach (string teacher in timetableobj.Teachers)
                        {
                            teachers.Add(DBTeachers.FirstOrDefault(x => x.disp_teacher == teacher));
                        }
                        var lecture = new Lecture() { disp_lecture = timetableobj.CourseName, week = timetableobj.WeekNumber, jigen = timetableobj.TimeNumber };
                        var lectureroom = rooms.Select(x => new LectureRoom() { Lecture = lecture, Room = x });
                        var lectureteacher = teachers.Select(x => new LectureTeacher() { Lecture = lecture, Teacher = x });

                        lecture.LectureRooms.AddRange(lectureroom);
                        lecture.LectureTeachers.AddRange(lectureteacher);
                        return lecture;
                    }).ToList();
            context.Lectures.AddRange(timetables);
            context.SaveChanges();
            context.Lectures.AsEnumerable().ToList().ForEach(x => Console.WriteLine(x));
            */
        }
    }
}