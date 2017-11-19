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
            var roomsdata = File.ReadAllLines("./Database/data/rooms.json").Select(x => x.Replace("\"", "")).Select(x => new Room() { disp_room = x });
            context.Rooms.AddRange(roomsdata);
            var teachersdata = File.ReadAllLines("./Database/data/teachers.json").Select(x => x.Replace("\"", "")).Select(x => new Teacher() { disp_teacher = x });
            context.Teachers.AddRange(teachersdata);
            context.SaveChanges();
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
        }
    }
}