using System;
using FUNAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace FUNAPI.Tests
{
    public class LecturesTestContext : LecturesContext
    {
        public LecturesTestContext(DbContextOptions<LecturesContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}