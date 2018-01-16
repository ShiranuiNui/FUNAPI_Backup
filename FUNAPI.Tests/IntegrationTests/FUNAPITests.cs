using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FUNAPI.Models;
using FUNAPI.Tests;
using Newtonsoft.Json;
using Xunit;

namespace FUNAPI.Tests
{
    public class FUNAPITests
    {
        [Fact]
        public async Task ExistData_GetAll_200()
        {
            using(var server = new FUNAPITestServer())
            {
                //Arrange
                server.Context.Rooms.Add(new Room { RoomId = 0, disp_room = "教室１" });
                server.Context.Classes.Add(new Class { ClassId = 101, disp_class = "1A" });
                server.Context.Teachers.Add(new Teacher { TeacherId = 0, disp_teacher = "教員１", roman_name = "Kyouin1", position = "教授", research_area = "研究分野", role = "所属学科" });
                server.Context.SaveChanges();
                var rooms = server.Context.Rooms.Where(x => x.RoomId == 0).Select(x => x).ToList();
                var classes = server.Context.Classes.Where(x => x.ClassId == 101).Select(x => x).ToList();
                var teachers = server.Context.Teachers.Where(x => x.TeacherId == 0).Select(x => x).ToList();
                var lecture = new Lecture { LectureId = 0, disp_lecture = "教科A", must = "", week = 1, jigen = 1 };
                var lectureroom = rooms.Select(x => new LectureRoom() { Lecture = lecture, Room = x });
                var lectureclass = classes.Select(x => new LectureClass() { Lecture = lecture, Class = x });
                var lectureteacher = teachers.Select(x => new LectureTeacher() { Lecture = lecture, Teacher = x });
                lecture.LectureRooms.AddRange(lectureroom);
                lecture.LectureClasses.AddRange(lectureclass);
                lecture.LectureTeachers.AddRange(lectureteacher);
                server.Context.Lectures.Add(lecture);
                server.Context.SaveChanges();

                Assert.Equal(1, server.Context.Lectures.Count());
                Assert.Single(server.Context.Lectures.FirstOrDefault(x => x.LectureId == 0).LectureRooms);

                var request = server.CreateRequest("/api/lectures");

                //Act
                var response = await request.GetAsync();
                var responseContent = await response.Content.ReadAsStringAsync();
                var lectures = JsonConvert.DeserializeObject<List<string>>(responseContent);

                //Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Single(lectures);
            }
        }
    }
}