using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FUNAPI.Models
{
    public class Room : BaseEntity
    {
        public int RoomId { get; set; }
        public string disp_room { get; set; }

        [JsonIgnore]
        public List<LectureRoom> LectureRooms { get; set; } = new List<LectureRoom>();
        public override string ToString() =>
            $"<Room Class> id={this.RoomId}, disp={this.disp_room}, LectureCount={this.LectureRooms.Count}";
    }
}