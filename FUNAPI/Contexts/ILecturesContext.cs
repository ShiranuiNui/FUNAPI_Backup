using System;
using System.Collections.Generic;
using FUNAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace FUNAPI.Context
{
    public interface ILecturesContext
    {
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Room> Rooms { get; set; }
        DbSet<Class> Classes { get; set; }
        DbSet<Lecture> Lectures { get; set; }
    }
}