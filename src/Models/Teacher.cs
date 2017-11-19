using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FUNAPI.Models
{
    public class Teacher : BaseEntity
    {
        public int TeacherId { get; set; }
        public string disp_teacher { get; set; }
        public string research_area { get; set; }
        public int course { get; set; }
        [JsonIgnore]
        public List<LectureTeacher> LectureTeachers { get; set; } = new List<LectureTeacher>();
        public override string ToString() =>
            $"<Teacher Class> id={this.TeacherId}, disp={this.disp_teacher}, area={this.research_area}, course={this.course}, LectureCount={this.LectureTeachers.Count}";
    }
}