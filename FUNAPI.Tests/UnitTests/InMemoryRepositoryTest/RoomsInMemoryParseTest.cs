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
    public class RoomsInMemoryParseTest
    {
        private string path = "/tmp/" + Guid.NewGuid().ToString();
        public RoomsInMemoryParseTest()
        {
            Directory.CreateDirectory(path);
            var testString =
                "BEGIN DATA\n" +
                "\t1\t教室1";
            File.WriteAllText(path + "/Rooms.tsv", testString);

        }

        [Fact]
        public async Task IntializeTest()
        {
            var mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>() { { "TSVPATH", path } }).Build();
            //var mockConfiguration = new Mock<IConfiguration>();
            //mockConfiguration.Setup(x => x.GetValue<string>(It.IsAny<string>())).Returns(path);

            var roomInMemoryRepository = new RoomInMemoryRepository(mockConfiguration);

            Assert.True(roomInMemoryRepository.IsReady);
            var parseResult = await roomInMemoryRepository.GetAllAsync();
            Assert.Single(parseResult);
        }
    }
}