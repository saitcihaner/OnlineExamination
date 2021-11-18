using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OnlineExamination.DAL;
using OnlineExamination.Models;

namespace OnlineExamination.Controllers
{
    public class FileController : Controller
    {
        private readonly OnlineExaminationContext _db;
        public FileController(OnlineExaminationContext db)
        {
            _db = db;
        }
        [HttpPost]
        public FileResult ExportExcel()
        {
            DataTable dt = new DataTable("ExampleExam");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Question"), new DataColumn("QuestionNumber"), new DataColumn("WrongAnswer1"), new DataColumn("WrongAnswer2"), new DataColumn("WrongAnswer3"), new DataColumn("RightAnswer") });
            var questions = _db.ExampleExamTable.ToList();
            foreach (var item in questions)
            {
                dt.Rows.Add(item.Question, item.QuestionNumber, item.WrongAnswer_1, item.WrongAnswer_2, item.WrongAnswer_3, item.RightAnswer)
;
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExampleExam.xlsx");
                }
            }

        }
        [HttpPost]
        public async Task<IActionResult> ImportExcel()

        {

            Response response = new Response();
            IFormFile files = Request.Form.Files[0];
            using (var stream = new MemoryStream())
            {
                await files.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    try
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowNumbers = worksheet.Dimension.Rows;
                        for (int i = 2; i <= rowNumbers; i++)
                        {
                            RealExam exam = new RealExam();
                            if (worksheet.Cells[i, 1].Value==null || string.IsNullOrEmpty(worksheet.Cells[i, 1].Value.ToString())) { response.Status = false; response.Message = "Question can not be null"; return Json(response); }
                            exam.Question = worksheet.Cells[i, 1].Value.ToString().Trim();
                            if (worksheet.Cells[i, 2].Value == null || string.IsNullOrEmpty(worksheet.Cells[i, 2].Value.ToString())) { response.Status = false; response.Message = "QuestionNumber can not be null"; return Json(response); }

                            exam.QuestionNumber = Convert.ToInt32(worksheet.Cells[i, 2].Value);
                            if (worksheet.Cells[i, 3].Value == null || string.IsNullOrEmpty(worksheet.Cells[i, 3].Value.ToString())) { response.Status = false; response.Message = "WrongAnswer_1 can not be null"; return Json(response); }

                            exam.WrongAnswer_1 = worksheet.Cells[i, 3].Value.ToString().Trim();
                            if (worksheet.Cells[i, 4].Value == null || string.IsNullOrEmpty(worksheet.Cells[i, 4].Value.ToString())) { response.Status = false; response.Message = "WrongAnswer_2 can not be null"; return Json(response); }

                            exam.WrongAnswer_2 = worksheet.Cells[i, 4].Value.ToString().Trim();
                            if (worksheet.Cells[i, 5].Value == null || string.IsNullOrEmpty(worksheet.Cells[i, 5].Value.ToString())) { response.Status = false; response.Message = "WrongAnswer_3 can not be null"; return Json(response); }

                            exam.WrongAnswer_3 = worksheet.Cells[i, 5].Value.ToString().Trim();
                            if (worksheet.Cells[i, 6].Value == null || string.IsNullOrEmpty(worksheet.Cells[i, 6].Value.ToString())) { response.Status = false; response.Message = "RightAnswer can not be null"; return Json(response); }

                            exam.RightAnswer = worksheet.Cells[i, 6].Value.ToString().Trim();
                            DateTime now = DateTime.Now;
                            DateTime endDate = now - new TimeSpan(0, 0, 0, 0, now.TimeOfDay.Milliseconds);
                            exam.ExamEnteredDate = endDate;
                            exam.TeacherNumber = Convert.ToInt32(files.Name);


                            _db.RealExam.Add(exam);

                        }

                        _db.SaveChanges();



                    }

                    catch (Exception e)
                    {

                        response.Message = "System not available not now.";
                        response.Status = false;
                        return Json(response);
                    }

                    response.Message = "Exam is successfully saved.";
                    response.Status = true;
                    return Json(response);

                }
            }
        }
    }
}
          
        
    

