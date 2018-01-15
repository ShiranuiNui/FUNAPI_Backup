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
    public class ClassesControllerTest
    {
        private readonly List<Class> TestData = new List<Class>();
        public ClassesControllerTest()
        {
            var random = new Random();
            TestData.Add(new Class { ClassId = random.Next(0, int.MaxValue), disp_class = $"クラス{random.Next()}" });
            TestData.Add(new Class { ClassId = random.Next(0, int.MaxValue), disp_class = $"クラス{random.Next()}" });
        }

        [Fact]
        public async Task GetAll_200()
        {
            var mock = new Mock<IReadOnlyRepository<Class>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(TestData);

            var controller = new ClassesController(mock.Object);
            var actionResult = await controller.Get();

            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<List<Class>>(modelResult.Value);
            Assert.True(model.Any());
            Assert.Equal(2, model.Count());
            Assert.Equal(TestData.First().ClassId, model.First().ClassId);
        }

        [Fact]
        public async Task GetAll_404()
        {
            var mock = new Mock<IReadOnlyRepository<Class>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Class>());

            var controller = new ClassesController(mock.Object);
            var actionResult = await controller.Get();

            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task GetSingle_200()
        {
            var mock = new Mock<IReadOnlyRepository<Class>>();
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync(TestData.First());

            var controller = new ClassesController(mock.Object);
            var actionResult = await controller.Get(this.TestData.First().ClassId);

            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<Class>(modelResult.Value);
            Assert.Equal(TestData.First().ClassId, model.ClassId);
        }

        [Fact]
        public async Task GetSingle_404()
        {
            var mock = new Mock<IReadOnlyRepository<Class>>();
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync((Class) null);

            var controller = new ClassesController(mock.Object);
            var actionResult = await controller.Get(new Random().Next(0, int.MaxValue));

            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}