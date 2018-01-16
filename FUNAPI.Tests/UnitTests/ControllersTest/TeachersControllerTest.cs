using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FUNAPI.Controllers;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FUNAPI.Tests
{
    public class TeachersControllerTest
    {
        private readonly List<Teacher> TestData = new List<Teacher>();
        public TeachersControllerTest()
        {
            var random = new Random();
            TestData.Add(new Teacher { TeacherId = random.Next(0, int.MaxValue), disp_teacher = $"教員{random.Next()}" });
            TestData.Add(new Teacher { TeacherId = random.Next(0, int.MaxValue), disp_teacher = $"教員{random.Next()}" });
        }

        [Fact]
        public async Task GetAll_200()
        {
            var mock = new Mock<IReadOnlyRepository<Teacher>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(TestData);

            var controller = new TeachersController(mock.Object);
            var actionResult = await controller.Get();

            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<List<Teacher>>(modelResult.Value);
            Assert.True(model.Any());
            Assert.Equal(2, model.Count());
            Assert.Equal(TestData.First().TeacherId, model.First().TeacherId);
        }

        [Fact]
        public async Task GetAll_404()
        {
            var mock = new Mock<IReadOnlyRepository<Teacher>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Teacher>());

            var controller = new TeachersController(mock.Object);
            var actionResult = await controller.Get();

            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task GetSingle_200()
        {
            var mock = new Mock<IReadOnlyRepository<Teacher>>();
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync(TestData.First());

            var controller = new TeachersController(mock.Object);
            var actionResult = await controller.Get(this.TestData.First().TeacherId);

            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<Teacher>(modelResult.Value);
            Assert.Equal(TestData.First().TeacherId, model.TeacherId);
        }

        [Fact]
        public async Task GetSingle_404()
        {
            var mock = new Mock<IReadOnlyRepository<Teacher>>();
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync((Teacher) null);

            var controller = new TeachersController(mock.Object);
            var actionResult = await controller.Get(new Random().Next(0, int.MaxValue));

            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}