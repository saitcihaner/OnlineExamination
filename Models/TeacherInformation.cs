using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class TeacherInformation
    {
        [Key]
        public int ID { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string TeacherFullName { get; set; }
        public int TeacherNumber { get; set; }
        public string TeacherBranch { get; set; }
        public int ClassRoomNumber_1 { get; set; }
        public int ClassRoomNumber_2 { get; set; }
        public int ClassRoomNumber_3 { get; set; }
        public int ClassRoomNumber_4 { get; set; }
        public string ClassRoomCode_1 { get; set; }
        public string ClassRoomCode_2 { get; set; }
        public string ClassRoomCode_3 { get; set; }
        public string ClassRoomCode_4 { get; set; }
        public string EmailAddress { get; set; }
    }
}
