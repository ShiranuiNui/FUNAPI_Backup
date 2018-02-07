using System;
using FUNAPI.Repository;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FUNAPI.Tests
{
    public class LecturesInMemoryParseTest
    {
        private string path = "/tmp/" + Guid.NewGuid().ToString();
        public LecturesInMemoryParseTest()
        {
            Directory.CreateDirectory(path);
            var lecturestestString =
                "BEGIN DATA\n" +
                "\t1\t教科名1\t\t0\t1\n" +
                "\t2\t教科名2\t\t0\t2\n";
            File.WriteAllText(path + "/Lectures.tsv", lecturestestString);
            var lecture_classes_testString =
                "BEGIN DATA\n" +
                "1\t1-AB\t101\t102\n" +
                "1\tM1\t#N/A\t#N/A";
            File.WriteAllText(path + "/Lectures_Classes.tsv", lecture_classes_testString);
            var lecture_must_testString =
                "BEGIN DATA\n" +
                "1\t教科名1\t0\t0\t0\t0\t0\t2\t0\n" +
                "2\t教科名2\t#N/A\t#N/A\t#N/A\t#N/A\t#N/A\t#N/A\t#N/A";
            File.WriteAllText(path + "/Lectures_Must.tsv", lecture_must_testString);
            var lecture_rooms_testString =
                "BEGIN DATA\n" +
                "1\t教室1\n" +
                "1\t101\n" +
                "2\t\n" +
                "2\t\n";
            File.WriteAllText(path + "/Lectures_Rooms.tsv", lecture_rooms_testString);
            var lecture_teachers_testString =
                "BEGIN DATA\n" +
                "1\t教員1\n" +
                "1\t101\n" +
                "2\t\n" +
                "2\t\n";
            File.WriteAllText(path + "/Lectures_Teachers.tsv", lecture_teachers_testString);
        }

        [Fact]
        public async Task IntializeTest()
        {
            var mockConfiguration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>() { { "TSVPATH", path } }).Build();

            var classInMemoryRepository = new LectureInMemoryRepository(mockConfiguration);

            Assert.True(classInMemoryRepository.IsReady);
            var parseResult = await classInMemoryRepository.GetAllAsync();
            Assert.InRange(parseResult.Count(), 2, 2);
        }
    }
}