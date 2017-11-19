using System;
using System.Collections.Generic;

namespace FUNAPI.Models
{
    public class LectureClass : BaseEntity
    {
        public int LectureId { get; set; }
        public Lecture Lecture { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }

}