using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FUNAPI.Models
{
    public class Lecture : BaseEntity
    {
        [JsonProperty(Order = 0)]
        public int LectureId { get; protected set; }

        [JsonProperty(Order = 1)]
        public string disp_lecture　 { get; set; }

        [JsonProperty(Order = 2)]
        public string must { get; set; }

        [JsonProperty(Order = 3)]
        public int week { get; set; }

        [JsonProperty(Order = 4)]
        public int jigen { get; set; }

        [JsonIgnore]
        public virtual List<LectureTeacher> LectureTeachers { get; set; } = new List<LectureTeacher>();
        [JsonIgnore]
        public virtual List<LectureRoom> LectureRooms { get; set; } = new List<LectureRoom>();
        [JsonIgnore]
        public virtual List<LectureClass> LectureClasses { get; set; } = new List<LectureClass>();

        public override string ToString() =>
            $"<Lecture Class> id={this.LectureId}, disp={this.disp_lecture}, week={this.week}, jigen={this.jigen}, RoomsCount={this.LectureRooms.Count}";
    }
    public class LectureJson : Lecture
    {
        [JsonProperty(Order = 4)]
        public IEnumerable<int> teachers { get; private set; }

        [JsonProperty(Order = 5)]
        public IEnumerable<int> rooms { get; private set; }

        [JsonProperty(Order = 6)]
        public IEnumerable<int> classes { get; private set; }
        public LectureJson() { }

        public LectureJson(Lecture querydata)
        {
            this.LectureId = querydata.LectureId;
            this.disp_lecture = querydata.disp_lecture;
            this.must = querydata.must;
            this.week = querydata.week;
            this.jigen = querydata.jigen;
            this.teachers = querydata.LectureTeachers.Select(x => x.TeacherId);
            this.rooms = querydata.LectureRooms.Select(x => x.RoomId);
            this.classes = querydata.LectureClasses.Select(x => x.ClassId);
        }
    }
}