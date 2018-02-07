using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FUNAPI.Models
{
    public class Teacher : BaseEntity
    {
        public int TeacherId { get; set; }
        public string disp_teacher { get; set; }
        public string roman_name { get; set; }
        public string position { get; set; }
        public string research_area { get; set; }
        public string role { get; set; }

        [JsonIgnore]
        public List<LectureTeacher> LectureTeachers { get; set; } = new List<LectureTeacher>();
        public override string ToString() =>
            $"<Teacher Class> id={this.TeacherId}, disp={this.disp_teacher}, roman={this.roman_name}, potision={this.position}, research_area={this.research_area}, role={this.role}, LectureCount={this.LectureTeachers.Count}";
    }
}