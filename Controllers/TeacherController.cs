using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using OnlineExamination.Models;
using OnlineExamination.DAL;

namespace OnlineExamination.Controllers
{
    [Authorize("Teacher")]
    // [AllowAnonymous]
    public class TeacherController : Controller
    {
        public readonly OnlineExaminationContext _db;
        public TeacherController(OnlineExaminationContext onlineExaminationContext)
        {
            _db = onlineExaminationContext;
        }
        public IActionResult MainPage()
        {
            var classRoomList = _db.ClassRoomList.ToList();
            return View(classRoomList);
            
        }


        public IActionResult UploadExam()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveExam(ExamInformation model)
        {

            Response response = new Response();
            var LastUploadTime = _db.RealExam.OrderByDescending(x => x.ExamEnteredDate).ToList();
            var deger = LastUploadTime[0].ExamEnteredDate;
            model.ExamEnteredTime = deger;
          
            try
            {
                _db.ExamInformation.Add(model);
                _db.SaveChanges();
            }
            catch (Exception e)
            {

                throw e.InnerException;
            }


            return Json("");
        }
        public IActionResult StudentList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetStudentList()
        {
            var studentList = _db.StudentInformation.ToList();

            return Json(studentList);
        }

        public IActionResult UnReadedMessages()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UnReadedMessages(string TeacherEmail)
        {
            var messageTable = _db.StudentMessageTable.Where(x => x.ToWhichTeacher == TeacherEmail && x.IsRead == false).OrderByDescending(x => x.MessageSentTime).ToList();

            return Json(messageTable);
        }
        [HttpPost]
        public IActionResult MessageReadCheck(int studentNumber, DateTime messageEnteredTime)
        {


            var res = _db.StudentMessageTable.First(x => x.StudentNumber == studentNumber && x.MessageSentTime == messageEnteredTime);
            res.IsRead = true;
            _db.SaveChanges();
            return Json(res);
        }
        public IActionResult ReadedMessages()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ReadedMessages(string TeacherEmail)
        {
            var messageTable = _db.StudentMessageTable.Where(x => x.ToWhichTeacher == TeacherEmail && x.IsRead == true).OrderByDescending(x => x.MessageSentTime).ToList();

            return Json(messageTable);
        }
        [HttpPost]
        public JsonResult BasicInformation(string UserEmail,string EnteredTime)
        {
            var LoginTime = _db.Login.First(x => x.Email == UserEmail).EnteredTime.ToString("dd.MM.yyyy");
            var UnreadedMessageCount = _db.StudentMessageTable.Where(x => x.IsRead == false && x.ToWhichTeacher == UserEmail).ToList().Count;
            var response = new { loginTime = LoginTime, unreadedMessageCount = UnreadedMessageCount };
            var loginUser = _db.Login.First(x => x.Email == UserEmail);
            loginUser.EnteredTime = Convert.ToDateTime(EnteredTime);
            

            _db.SaveChanges();
            return Json(response);
        }

      public JsonResult GetStudentList(int ClassRoomNumber,string ClassRoomCode)
        {
            var students = _db.StudentInformation.Where(x => x.ClassRoomCode == ClassRoomCode && x.ClassRoomNumber == ClassRoomNumber).ToList();
            foreach (var item in students)
            {
                item.ClassRoomCode = item.ClassRoomNumber + "/" + item.ClassRoomCode;
            }
            return Json(students);
        }
        [HttpPost]
        public JsonResult GetStudentInformationList(int StudentNumber)
        {
            var student = _db.StudentInformation.First(x => x.StudentNumber == StudentNumber);
            student.ClassRoomCode = student.ClassRoomNumber + "/" + student.ClassRoomCode;

            return Json(student);
        }
        public IActionResult OneStudentMessage()
        {
            return View();
        }
        public JsonResult FillTeacherMessageTable(TeacherMessageTable model)
        {
            Response response = new Response();
            var TeacherInformation = _db.TeacherInformation.First(x => x.EmailAddress == model.TeacherEmail);
            model.TeacherName = TeacherInformation.TeacherFullName;
            model.TeacherNumber = TeacherInformation.TeacherNumber;
            try
            {
                _db.TeacherMessageTable.Add(model);
                 _db.SaveChanges();
            }
            catch (Exception)
            {
                response.Status = false;
                response.Message = "Your message has not been sent.Try Again please..";
              
            }
            response.Message = "The message has been sent Successfully";
            response.Status = true;
            return Json(response);
        }

        public IActionResult ExamResult()
        {
            var classRoom = _db.ClassRoomList.ToList();
            return View(classRoom);
        }
        [HttpPost]
        public JsonResult Geto(string Mi,DateTime ki)
        {
            return Json("");
        }

        [HttpPost]
        public JsonResult ExamResult(string ExamName, string ClassRoom, string StartDate, string FinishDate,string Branch)
        {
            List<ExamInformation> examList = new List<ExamInformation>();
            List<ExamResult> examResult = new List<ExamResult>();
           var  aa = _db.ExamInformation.ToList();
            if (string.IsNullOrEmpty(ExamName))
            {

               examList = _db.ExamInformation.Where(x =>  x.ExamStartDate == Convert.ToDateTime(StartDate) && x.ExamBranch==Branch && x.ExamFinishDate == Convert.ToDateTime(FinishDate)).ToList();

            }
            else
            {
                examList = _db.ExamInformation.Where(x => x.ExamName == ExamName && x.ExamBranch == Branch && x.ExamStartDate ==Convert.ToDateTime(StartDate) && x.ExamFinishDate == Convert.ToDateTime(FinishDate)).ToList();

            }

            if (examList.Count == 1)
            {
                foreach (var item in examList)
                {
                    examResult = _db.ExamResult.Where(x => x.ExamEnteredDate == item.ExamEnteredTime).ToList();

                }
                return Json(examResult);
            }
            else if (examList.Count >= 1)
            {
                return Json(examList);
            }
            else
            {
                return Json("There is no any exam for your filter values.Try Different values");
            }
        }
    }
}

