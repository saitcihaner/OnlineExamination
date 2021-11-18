using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineExamination.Models
{
    public class ExampleExamTable
    {
        [Key]
        public int ID { get; set; }
        public string Question { get; set; }
        public int QuestionNumber { get; set; }
        public string WrongAnswer_1 { get; set; }
        public string WrongAnswer_2 { get; set; }
        public string WrongAnswer_3 { get; set; }
        public string RightAnswer { get; set; }
    }
}
