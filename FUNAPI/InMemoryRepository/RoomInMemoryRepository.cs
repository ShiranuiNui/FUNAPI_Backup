using System;
using FUNAPI.Context;
using FUNAPI.Models;
using FUNAPI.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace FUNAPI.Repository
{
    public class RoomInMemoryRepository : SuperInMemoryRepository, IReadOnlyRepository<Room>
    {
        private List<Room> context { get; set; } = new List<Room>();
        public RoomInMemoryRepository(IConfiguration configuration) : base(configuration) { }
        private bool Initialize(string tsvPath)
        {
            this.context = File.ReadAllLines(tsvPath + "/Rooms.tsv").Select(x => x.Split("\t")).SkipWhile(x => x[0] != "BEGIN DATA").Skip(1)
                .Select(x => new Room() { RoomId = int.Parse(x[1]), disp_room = x[2] }).ToList();
            return this.context.Any();
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await Task.Run(() => this.context);
        }
        public async Task<Room> GetSingleAsync(int id)
        {
            if (this.context.Any(x => x.RoomId == id))
            {
                return await Task.Run(() => this.context.SingleOrDefault(x => x.RoomId == id));
            }
            else
            {
                return null;
            }
        }
    }
}