using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FUNAPI;
using FUNAPI.Models;
using FUNAPI.Repository;
using FUNAPI.Tests.Integration.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace FUNAPI.Tests.Integration
{
    public class LecturesControllerTest : IntegrationTestsBase<Startup>
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IReadOnlyRepository<LectureJson>, LecturesTestRepository>();
        }

        [Fact]
        public async Task ExistData_GetAll_200()
        {
            var response = await this.Client.GetAsync("/api/lectures/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<LectureJson>>(responseString);
            Assert.NotEmpty(responseModel);
        }
    }
}