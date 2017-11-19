using System;
using System.Collections.Generic;

namespace FUNAPI.Models
{
    public class LectureTeacher : BaseEntity
    {
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}