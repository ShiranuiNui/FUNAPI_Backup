using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Context;
using FUNAPI.Models;
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
        public static void Invoke(LecturesContext context)
        {
            //context.Database.Migrate();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var roomsdata = File.ReadAllLines("./Database/data/Rooms.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Select(x => new Room() { disp_room = x[1] });
            context.Rooms.AddRange(roomsdata);

            var teachersdata = File.ReadAllLines("./Database/data/Teachers.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Select(x => new Teacher() { disp_teacher = x[1], roman_name = x[2], position = x[3], research_area = x[4], role = x[5] });
            context.Teachers.AddRange(teachersdata);

            var classesdata = File.ReadAllLines("./Database/data/Classes.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Select(x => new Class() { ClassId = int.Parse(x[0]), disp_class = x[1], course = int.Parse(x[2]) });
            context.Rooms.AddRange(roomsdata);
            context.SaveChanges();

            var lecturesdata = File.ReadAllLines("./Database/data/Lectures.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Select(x => new Lecture() { disp_lecture = x[1], week = int.Parse(x[3]), jigen = int.Parse(x[4]) });
            var lectures_rooms = File.ReadAllLines("./Database/data/Lectures_Rooms.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Where((data, index) => index % 2 == 1).Select(x => x.Skip(1));
            var lectures_teachers = File.ReadAllLines("./Database/data/Lectures_Teachers.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Where((data, index) => index % 2 == 1).Select(x => x.Skip(1));
            var lectures_classes = File.ReadAllLines("./Database/data/Lectures_Classes.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Select(x => x.Skip(2));

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