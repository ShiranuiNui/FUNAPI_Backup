using System;
using System.Collections.Generic;
using FUNAPI.Context;
using FUNAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace FUNAPI.Tests.Integration.Context
{
    public class LecturesTestContext : LecturesContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
    }
}