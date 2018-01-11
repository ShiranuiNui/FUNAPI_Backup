using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Controllers;
using FUNAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FUNAPI.Tests
{
    public class LecturesControllerTest
    {
        private readonly List<Lecture> TestData;
        private readonly ILectureRepository NomalMockRepository;
        private readonly ILectureRepository EmptyMockRepository;

        public LecturesControllerTest()
        {
            var random = new Random();
            this.TestData = new List<Lecture>()
            {
                new Lecture
                {
                LectureId = random.Next(0, int.MaxValue),
                disp_lecture = $"教科{random.Next(0, int.MaxValue)}",
                must = "",
                week = 0,
                jigen = 1,
                LectureTeachers = new List<LectureTeacher>
                {
                new LectureTeacher { TeacherId = random.Next(0, int.MaxValue) }
                },
                LectureRooms = new List<LectureRoom>
                {
                new LectureRoom { RoomId = random.Next(0, int.MaxValue) }
                },
                LectureClasses = new List<LectureClass>
                {
                new LectureClass { ClassId = 11 }
                }
                },
                new Lecture
                {
                LectureId = random.Next(0, int.MaxValue),
                disp_lecture = $"教科{random.Next(0, int.MaxValue)}",
                must = "",
                week = 2,
                jigen = 4,
                LectureTeachers = new List<LectureTeacher>
                {
                new LectureTeacher { TeacherId = random.Next(0, int.MaxValue) }
                },
                LectureRooms = new List<LectureRoom>
                {
                new LectureRoom { RoomId = random.Next(0, int.MaxValue) }
                },
                LectureClasses = new List<LectureClass>
                {
                new LectureClass { ClassId = 11 }
                }
                }
            };
            this.NomalMockRepository = new LectureMockRepository(this.TestData);
            this.EmptyMockRepository = new LectureMockRepository(new List<Lecture>());
        }

        [Fact]
        public async Task GetAll_200()
        {
            var controller = new LecturesController(this.NomalMockRepository);
            var actionResult = await controller.Get();
            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<List<LectureJson>>(modelResult.Value);
            Assert.True(model.Any());
            Assert.Equal(2, model.Count());
            var mockData = await this.NomalMockRepository.GetAllAsync();
            Assert.Equal(mockData.First().LectureId, model.First().LectureId);
        }

        [Fact]
        public async Task GetAll_404()
        {
            var controller = new LecturesController(this.EmptyMockRepository);
            var actionResult = await controller.Get();
            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task GetSingle_200()
        {
            var controller = new LecturesController(this.NomalMockRepository);
            var actionResult = await controller.Get(this.TestData.First().LectureId);
            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<LectureJson>(modelResult.Value);
            var mockData = await this.NomalMockRepository.GetAllAsync();
            Assert.Equal(mockData.First().LectureId, model.LectureId);
        }

        [Fact]
        public async Task GetSingle_404()
        {
            var controller = new LecturesController(this.EmptyMockRepository);
            var actionResult = await controller.Get(new Random().Next(10, int.MaxValue));
            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}