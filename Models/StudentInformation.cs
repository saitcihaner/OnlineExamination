using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class StudentInformation
    {
        [Key]
        public int ID { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFullName { get; set; }
        public int StudentNumber { get; set; }
        public int ClassRoomNumber { get; set; }
        public string ClassRoomCode { get; set; }
        public string EmailAdress { get; set; }
    }
}
