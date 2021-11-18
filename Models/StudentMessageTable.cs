using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class StudentMessageTable
    {
        [Key]
        public int ID { get; set; }
        public string StudentName { get; set; }
        public string StudentClass { get; set; }
        public int StudentNumber { get; set; }
        public DateTime MessageSentTime { get; set; }
        public bool IsRead { get; set; }

        public string Message { get; set; }
        public string ToWhichTeacher { get; set; }

    }
}
