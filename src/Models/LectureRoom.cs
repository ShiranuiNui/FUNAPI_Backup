using System;
using System.Collections.Generic;

namespace FUNAPI.Models
{
    public class LectureRoom : BaseEntity
    {
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}