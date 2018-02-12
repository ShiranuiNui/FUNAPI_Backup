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
    public class TeacherControllerTest : IntegrationTestsBase<Startup>
    {
        private readonly ITestOutputHelper output;
        public TeacherControllerTest(ITestOutputHelper _output)
        {
            this.output = _output;
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            //throw new NotImplementedException();
            var mock = new Mock<IReadOnlyRepository<Teacher>>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Teacher> { new Teacher() { TeacherId = 0, disp_teacher = "教員1", roman_name = "KYOUIN1", position = "教授", research_area = "情報工学", role = "情報学科" }, new Teacher() { TeacherId = 1, disp_teacher = "教員2", roman_name = "KYOUIN1", position = "教授", research_area = "情報工学", role = "情報学科" } });
            mock.Setup(x => x.GetSingleAsync(It.IsAny<int>())).ReturnsAsync(new Teacher { TeacherId = 0, disp_teacher = "教員1", roman_name = "KYOUIN1", position = "教授", research_area = "情報工学", role = "情報学科" });

            services.AddDbContext<LecturesContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddSingleton<IReadOnlyRepository<Teacher>>(mock.Object);
        }

        [Fact]
        public async Task ExistData_GetAll_200()
        {
            var response = await this.Client.GetAsync("api/teachers/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<Teacher>>(responseString);
            Assert.NotEmpty(responseModel);
        }
    }
}