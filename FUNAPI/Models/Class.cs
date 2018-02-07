using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FUNAPI.Models
{
    public class Class : BaseEntity
    {
        public int ClassId { get; set; }
        public string disp_class { get; set; }
        public int course { get; set; }

        [JsonIgnore]
        public List<LectureClass> LectureClasses { get; set; } = new List<LectureClass>();
        public override string ToString() =>
            $"<Class Class> id={this.ClassId}, disp={this.disp_class}, LectureCount={this.LectureClasses.Count}";
    }
}