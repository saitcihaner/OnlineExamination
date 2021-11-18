using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class ClassRoomList
    {
        [Key]
        public int ID { get; set; }
        public int ClassRoomNumber { get; set; }
        public string ClassRoomCode { get; set; }
    }
}
