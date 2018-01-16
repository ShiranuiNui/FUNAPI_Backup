using System;
using System.Collections.Generic;
using FUNAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace FUNAPI.Context
{
    public class LecturesContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public LecturesContext(DbContextOptions<LecturesContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LectureTeacher>()
                .HasKey(key => new { key.LectureId, key.TeacherId });
            modelBuilder.Entity<LectureRoom>()
                .HasKey(key => new { key.LectureId, key.RoomId });
            modelBuilder.Entity<LectureClass>()
                .HasKey(key => new { key.LectureId, key.ClassId });

            modelBuilder.Entity<LectureTeacher>()
                .HasOne(x => x.Lecture)
                .WithMany(x => x.LectureTeachers)
                .HasForeignKey(x => x.LectureId);
            modelBuilder.Entity<LectureTeacher>()
                .HasOne(x => x.Teacher)
                .WithMany(x => x.LectureTeachers)
                .HasForeignKey(x => x.TeacherId);

            modelBuilder.Entity<LectureRoom>()
                .HasOne(x => x.Lecture)
                .WithMany(x => x.LectureRooms)
                .HasForeignKey(x => x.LectureId);
            modelBuilder.Entity<LectureRoom>()
                .HasOne(x => x.Room)
                .WithMany(x => x.LectureRooms)
                .HasForeignKey(x => x.RoomId);

            modelBuilder.Entity<LectureClass>()
                .HasOne(x => x.Lecture)
                .WithMany(x => x.LectureClasses)
                .HasForeignKey(x => x.LectureId);
            modelBuilder.Entity<LectureClass>()
                .HasOne(x => x.Class)
                .WithMany(x => x.LectureClasses)
                .HasForeignKey(x => x.ClassId);
        }
    }
}