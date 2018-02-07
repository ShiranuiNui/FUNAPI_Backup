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
    public class ClassInMemoryParseTest
    {
        private string path = "/tmp/" + Guid.NewGuid().ToString();
        public ClassInMemoryParseTest()
        {
            Directory.CreateDirectory(path);
            var testString =
                "BEGIN DATA\n" +
                "101\t1\tA\t0";
            File.WriteAllText(path + "/Classes.tsv", testString);

        }

        [Fact]
        public async Task IntializeTest()
        {
            var mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>() { { "TSVPATH", path } }).Build();
            //var mockConfiguration = new Mock<IConfiguration>();
            //mockConfiguration.Setup(x => x.GetValue<string>(It.IsAny<string>())).Returns(path);

            var classInMemoryRepository = new ClassInMemoryRepository(mockConfiguration);

            Assert.True(classInMemoryRepository.IsReady);
            var parseResult = await classInMemoryRepository.GetAllAsync();
            Assert.Single(parseResult);
        }
    }
}