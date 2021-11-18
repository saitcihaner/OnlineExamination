using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using OnlineExamination.DAL;
using OnlineExamination.Models;
namespace OnlineExamination.Controllers
{
    public class StudentController : Controller
    {
        private readonly OnlineExaminationContext _db;
        public StudentController(OnlineExaminationContext db)
        {
            _db = db;
        }
        public IActionResult MainPage()
        {
            return View();
        }
       public IActionResult ExamList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ExamList(int SchoolNumber)
        {
            List<ExamInformation> examInformations = new List<ExamInformation>();
            var whichStudent = _db.StudentInformation.First(x => x.StudentNumber == SchoolNumber);

            //we will get the exam list that based on whichstudent classroomNumber and ClassroomCode
            var studentExam = _db.ExamInformation.Where(x => x.Exam_ClassroomNumber == whichStudent.ClassRoomNumber && (x.Exam_ClassroomCode_1 == whichStudent.ClassRoomCode || x.Exam_ClassroomCode_2 == whichStudent.ClassRoomCode || x.Exam_ClassroomCode_3 == whichStudent.ClassRoomCode || x.Exam_ClassroomCode_4 == whichStudent.ClassRoomCode)).ToList();
            var isExamDone = _db.ExamResult.FirstOrDefault(x => x.StudentNumber == whichStudent.StudentNumber);
          
            if (isExamDone == null)
            {
                foreach (var item in studentExam)
                {
                    if (DateTime.Compare(item.ExamFinishDate,DateTime.Now)>=0) 
                    examInformations.Add(item);
                    
                }
                return Json(examInformations);
            }
            else
                return Json("");
          
        }

        public IActionResult StudentExam(DateTime EnterTime,int TeacherNumber)
        {
            var exam = _db.RealExam.Where(x => x.ExamEnteredDate == EnterTime && x.TeacherNumber == TeacherNumber).ToList();

             return Json(exam);
       
        }
        [HttpPost]
        public JsonResult ExamResult(ExamResult model, List<string> answerList)
        {
            var thisExam = _db.RealExam.Where(x => x.ExamEnteredDate == model.ExamEnteredDate && x.TeacherNumber == model.TeacherNumber).OrderBy(x => x.QuestionNumber).ToList();
            var thisStudent = _db.StudentInformation.First(x => x.StudentNumber == model.StudentNumber);
            model.StudentName = thisStudent.StudentFullName;
            model.StudentClassroom = thisStudent.ClassRoomNumber + "/" + thisStudent.ClassRoomCode;
            var point = 0;
            for (int i = 0; i < thisExam.Count; i++)
            {
                if (thisExam[i].RightAnswer == answerList[i] && i != thisExam.Count - 1)
                {
                    point = point + 3;
                }
                if (thisExam[thisExam.Count - 1].RightAnswer == answerList[thisExam.Count - 1] && i == thisExam.Count - 1)
                {
                    point = point + 4;
                }
            }
            model.StudentResult = point;

            try
            {
                _db.ExamResult.Add(model);
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                throw;
            }
            return Json("");
        }

    }
}
