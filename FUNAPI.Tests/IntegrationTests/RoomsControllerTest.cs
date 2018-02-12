using System;
using FUNAPI;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using FUNAPI.Tests.Integration.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace FUNAPI.Tests.Integration
{
    public class RoomControllerTest : IntegrationTestsBase<Startup>
    {
        private readonly ITestOutputHelper output;
        public RoomControllerTest(ITestOutputHelper _output)
        {
            this.output = _output;
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            //throw new NotImplementedException();
            var mock = new Mock<IReadOnlyRepository<Room>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Room> { new Room() { RoomId = 0, disp_room = "教室1" }, new Room() { RoomId = 1, disp_room = "教室2" } });
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync(new Room { RoomId = 0, disp_room = "教室1" });

            services.AddDbContext<LecturesContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddSingleton<IReadOnlyRepository<Room>>(mock.Object);
        }

        [Fact]
        public async Task ExistData_GetAll_200()
        {
            var response = await this.Client.GetAsync("api/rooms/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<Room>>(responseString);
            Assert.NotEmpty(responseModel);
        }
    }
}