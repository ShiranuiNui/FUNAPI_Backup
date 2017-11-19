using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FUNAPI.Models
{
    public class Class : BaseEntity
    {
        public int ClassId { get; set; }
        public int year { get; set; }
        public string kumi { get; set; }
        public int course { get; set; }
        [JsonIgnore]
        public List<LectureClass> LectureClasses { get; set; } = new List<LectureClass>();
        public override string ToString() =>
            $"<Class Class> id={this.ClassId}, year={this.year}, kumi={this.kumi}, course={this.course}, LectureCount={this.LectureClasses.Count}";
    }
}