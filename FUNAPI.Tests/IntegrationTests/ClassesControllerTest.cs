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
    public class ClassControllerTest : IntegrationTestsBase<Startup>
    {
        private readonly ITestOutputHelper output;
        public ClassControllerTest(ITestOutputHelper _output)
        {
            this.output = _output;
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            var mock = new Mock<IReadOnlyRepository<Class>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Class> { new Class() { ClassId = 0, disp_class = "クラス1" }, new Class() { ClassId = 1, disp_class = "クラス2" } });
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync(new Class { ClassId = 0, disp_class = "クラス1" });

            services.AddDbContext<LecturesContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddSingleton<IReadOnlyRepository<Class>>(mock.Object);
        }

        [Fact]
        public async Task ExistData_GetAll_200()
        {
            var response = await this.Client.GetAsync("api/classes/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<Class>>(responseString);
            Assert.NotEmpty(responseModel);
        }
    }
}