using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class TeacherMessageTable
    {
        [Key]
        public int ID { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public int TeacherNumber { get; set; }
        public DateTime MessageSentTime { get; set; }
        public bool IsRead { get; set; }

        public string Message { get; set; }
        public string ToWhichStudent { get; set; }
    }
}
