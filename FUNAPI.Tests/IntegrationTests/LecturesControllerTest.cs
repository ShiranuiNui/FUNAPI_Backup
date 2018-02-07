using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
using Xunit;
using Xunit.Abstractions;

namespace FUNAPI.Tests.Integration
{
    public class LecturesControllerTest : IntegrationTestsBase<Startup>
    {
        private readonly ITestOutputHelper output;
        public LecturesControllerTest(ITestOutputHelper _output)
        {
            this.output = _output;
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LecturesContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IReadOnlyRepository<LectureJson>, LecturesTestRepository>();
        }

        [Fact]
        public async Task ExistData_GetAll_200()
        {
            var response = await this.Client.GetAsync("api/lectures/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<LectureJson>>(responseString);
            Assert.NotEmpty(responseModel);
        }

        [Fact]
        public async Task RequestPost_405()
        {
            var response = await this.Client.PostAsync("api/lectures/", new StringContent(""));

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
        }

        [Fact]
        public async Task ReturnErrorOnJson()
        {
            Func<string, bool> validateJSON = (string s) =>
            {
                try
                {
                    JToken.Parse(s);
                    return true;
                }
                catch (JsonReaderException ex)
                {
                    return false;
                }
            };

            var response = await this.Client.PostAsync("api/lectures/", new StringContent(""));

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.True(validateJSON(responseString));
        }
    }
}