using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class ExamInformation
    {
        [Key]
        public int ID { get; set; }
        public string ExamName { get; set; }
     
        public string ExamBranch { get; set; }

        public int Exam_ClassroomNumber { get; set; }
        public string Exam_ClassroomCode_1 { get; set; }
        public string Exam_ClassroomCode_2 { get; set; }
        public string Exam_ClassroomCode_3 { get; set; }
        public string Exam_ClassroomCode_4 { get; set; }
        
        public int ExamTeacherNumber { get; set; }
        public DateTime ExamFinishDate { get; set; }
        public DateTime ExamEnteredTime { get; set; }
        public TimeSpan ExamTime { get; set; }

        public DateTime ExamStartDate { get; set; }








    }
}
