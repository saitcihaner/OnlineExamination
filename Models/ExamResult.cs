using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class ExamResult
    {
        [Key]
        public int ID { get; set; }
        public string ExamName { get; set; }
        public int StudentNumber { get; set; }
        public string StudentName { get; set; }
        public DateTime ExamEnteredDate { get; set; }
        public int StudentResult { get; set; }
        public int TeacherNumber { get; set; }
        public string StudentClassroom { get; set; }
    }
}
