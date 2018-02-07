using System;
using FUNAPI.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FUNAPI.Tests
{
    public class TeachersInMemoryParseTest
    {
        private string path = "/tmp/" + Guid.NewGuid().ToString();
        public TeachersInMemoryParseTest()
        {
            Directory.CreateDirectory(path);
            var testString =
                "BEGIN DATA\n" +
                "\t1\t先生1\tSENSEI1\t教授\t分野\t情報学科";
            File.WriteAllText(path + "/Teachers.tsv", testString);

        }

        [Fact]
        public async Task IntializeTest()
        {
            var mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>() { { "TSVPATH", path } }).Build();
            //var mockConfiguration = new Mock<IConfiguration>();
            //mockConfiguration.Setup(x => x.GetValue<string>(It.IsAny<string>())).Returns(path);

            var teacherInMemoryRepository = new TeacherInMemoryRepository(mockConfiguration);

            Assert.True(teacherInMemoryRepository.IsReady);
            var parseResult = await teacherInMemoryRepository.GetAllAsync();
            Assert.Single(parseResult);
        }
    }
}