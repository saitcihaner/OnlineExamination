using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class RealExam
    {
        [Key]
        public int ID { get; set; }
        public string Question { get; set; }
        public int QuestionNumber { get; set; }
        public string WrongAnswer_1 { get; set; }
        public string WrongAnswer_2 { get; set; }
        public string WrongAnswer_3 { get; set; }
        public string RightAnswer { get; set; }

        public DateTime ExamEnteredDate { get; set; }
        public int TeacherNumber { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime FinishDate { get; set; }
        //public TimeSpan ExamSecond { get; set; }
        //public int ClassNumber { get; set; }
        //public string ClassCode { get; set; }
    }
}
