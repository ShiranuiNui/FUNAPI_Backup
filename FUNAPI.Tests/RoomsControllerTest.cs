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
    public class RoomsControllerTest
    {
        private readonly List<Room> TestData = new List<Room>();
        public RoomsControllerTest()
        {
            var random = new Random();
            TestData.Add(new Room { RoomId = random.Next(0, int.MaxValue), disp_room = $"教室{random.Next()}" });
            TestData.Add(new Room { RoomId = random.Next(0, int.MaxValue), disp_room = $"教室{random.Next()}" });
        }

        [Fact]
        public async Task GetAll_200()
        {
            var mock = new Mock<IReadOnlyRepository<Room>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(TestData);

            var controller = new RoomsController(mock.Object);
            var actionResult = await controller.Get();

            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<List<Room>>(modelResult.Value);
            Assert.True(model.Any());
            Assert.Equal(2, model.Count());
            Assert.Equal(TestData.First().RoomId, model.First().RoomId);
        }

        [Fact]
        public async Task GetAll_404()
        {
            var mock = new Mock<IReadOnlyRepository<Room>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Room>());

            var controller = new RoomsController(mock.Object);
            var actionResult = await controller.Get();

            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task GetSingle_200()
        {
            var mock = new Mock<IReadOnlyRepository<Room>>();
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync(TestData.First());

            var controller = new RoomsController(mock.Object);
            var actionResult = await controller.Get(this.TestData.First().RoomId);

            var modelResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsType<Room>(modelResult.Value);
            Assert.Equal(TestData.First().RoomId, model.RoomId);
        }

        [Fact]
        public async Task GetSingle_404()
        {
            var mock = new Mock<IReadOnlyRepository<Room>>();
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync((Room) null);

            var controller = new RoomsController(mock.Object);
            var actionResult = await controller.Get(new Random().Next(0, int.MaxValue));

            var modelResult = Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}