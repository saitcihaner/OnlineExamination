using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using OnlineExamination.DAL;
using OnlineExamination.Models;

namespace OnlineExamination.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly OnlineExaminationContext _onlineExaminationContext;
       
        public AccountManagementController(OnlineExaminationContext onlineExaminationContext)
        {
            _onlineExaminationContext = onlineExaminationContext;
           
        }
      public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Login(Login model)
        {
            Response response = new Response();
            if (string.IsNullOrEmpty(model.Email))
            {
                response.Message = "EmailAddress can not be empty";
                response.Status = false;
                return Json(response);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                response.Message = "Password can not be empty";
                response.Status = false;
                return Json(response);
            }
            var emailAndPasswordMatch = _onlineExaminationContext.Register.FirstOrDefault(x => x.EmailAdress == model.Email);
            if (emailAndPasswordMatch == null)
            {
                response.Message = "You Dont have account";
                response.Status = false;
                return Json(response);
            }
            if (emailAndPasswordMatch.Password == model.Password)
            {
                var User = _onlineExaminationContext.Register.First(x => x.EmailAdress == model.Email);

                List<Claim> claimList = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, User.FirstName + " " + User.LastName),
                    new Claim(ClaimTypes.Email,User.EmailAdress),
                    new Claim(ClaimTypes.Role,User.Position),
                    new Claim(ClaimTypes.SerialNumber,User.SchoolNumber.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claimList, CookieAuthenticationDefaults.AuthenticationScheme);

                var properties = new AuthenticationProperties() { ExpiresUtc = DateTime.UtcNow.AddMinutes(30) };
             
               
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    properties);

                response.Status = true;
                var users = new { Status = response.Status, Message = response.Message, UserType= emailAndPasswordMatch.Position };

                return Json(users);
            }
            else
            {
                response.Status = false;
                response.Message = "The Email and Password dont match.Try Again...";
                
                return Json(response);
            }
        }
        public IActionResult Register() 
        {
            return View();

        }
        [HttpPost]
        public JsonResult Register(Register model,string Position)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(model.FirstName))
            {
                response.Message = "FirstName can not be empty";
                response.Status = false;
                return Json(response);
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                response.Message = "LastName can not be empty";
                response.Status = false;
                return Json(response);
            }
            if (string.IsNullOrEmpty(model.EmailAdress))
            {
                response.Message = "EmailAddress can not be empty";
                response.Status = false;
                return Json(response);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                response.Message = "Password can not be empty";
                response.Status = false;
                return Json(response);
            }
            if (model.SchoolNumber < 1)
            {
                response.Message = "StudentNumber must be valid number";
                response.Status = false;
                return Json(response);
            }
            var isLoginBefore = _onlineExaminationContext.Login.Where(x => x.Email == model.EmailAdress).Count();
            
            if (isLoginBefore != 0)
            {
                response.Message = "This email have been used by different student.Try another one";
                response.Status = false;
                return Json(response);
            }

         var fullName = model.FirstName.Trim().ToUpper() + model.LastName.Trim().ToUpper();
            if (Position == "Teacher")
            {
                var existTeacher = _onlineExaminationContext.TeacherInformation.Where(x => x.TeacherNumber == model.SchoolNumber && x.EmailAddress == model.EmailAdress).ToList();
                if (existTeacher!=null)
                {
                    Login studentLogin = new Login() { Email = model.EmailAdress, Password = model.Password, ID = model.ID };
                    try
                    {
                        _onlineExaminationContext.Register.Add(model);
                        _onlineExaminationContext.Login.Add(studentLogin);
                        _onlineExaminationContext.SaveChanges();
                    }
                    catch (Exception e)
                    {

                        response.Message = "System is not available right now";
                        response.Status = false;
                        return Json(response);
                    }

                    response.Message = "The Teacher registered successfully";
                    response.Status = true;
                    return Json(response);


                }
                response.Message = "Your information are incorrect.Please check your information";
                response.Status = false;
                return Json(response);
            }
            else if (Position == "Student"){
                var existStudent = _onlineExaminationContext.StudentInformation.Where(x => x.StudentNumber == model.SchoolNumber && x.EmailAdress==model.EmailAdress).ToList();
                if (existStudent !=null )
                {
                    Login studentLogin = new Login() { Email = model.EmailAdress, Password = model.Password, ID = model.ID };
                    try
                    {
                        _onlineExaminationContext.Register.Add(model);
                        _onlineExaminationContext.Login.Add(studentLogin);
                        _onlineExaminationContext.SaveChanges();
                    }
                    catch (Exception e)
                    {

                        response.Message = "System is not available right now";
                        response.Status = false;
                        return Json(response);
                    }

                    response.Message = "The student registered successfully";
                    response.Status = true;
                    return Json(response);

                }

                response.Message = "Your information are incorrect.Please check your information";
                response.Status = false;
                return Json(response);

            }
            else
            {
                response.Message = "System is incorrect";
                response.Status = false;
                return Json(response);
                
            }
            
       
          
            
        }
        public IActionResult ForgotPassword()
        {
            return View();

        }
        [HttpPost]
        public JsonResult ForgotPassword(string Email)
        {
           
            
            

            List<string> list = new List<string>();
          
            Response response = new Response();
            var userIsLogined = _onlineExaminationContext.Login.FirstOrDefault(x => x.Email ==Email);
           

            if (userIsLogined == null)
            {
                response.Message = "EmailAdress is not exist in system.Please check your emailAdress";
                response.Status = false;
                return Json(response);
            }
            else
            {


                if (_onlineExaminationContext.TeacherInformation.FirstOrDefault(x => x.EmailAddress ==Email) != null)
                {
                    //  var userName = (from f in _onlineExaminationContext.TeacherInformation
                    //                  where f.EmailAddress == Email
                    //                  select new { f.TeacherFirstName, f.TeacherLastName, userIsLogined.Password });
                    //var k=  userName.FirstOrDefault(x => x.TeacherFirstName != null);


                    var userName = _onlineExaminationContext.TeacherInformation.First(x => x.EmailAddress == Email);
                    list.Add(userName.TeacherFirstName);
                    list.Add(userName.TeacherLastName);
                    list.Add(userIsLogined.Password);



                }
                else if (_onlineExaminationContext.StudentInformation.FirstOrDefault(x => x.EmailAdress ==Email) != null)
                {

                    var userName = _onlineExaminationContext.StudentInformation.First(x => x.EmailAdress == Email);
                    list.Add(userName.StudentFirstName);
                    list.Add(userName.StudentLastName);
                    list.Add(userIsLogined.Password);
                }

                else
                {

                }

                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(Email);
                // You can define your school or personal mail to Send email
                  mailMessage.From = new MailAddress("YOUR_EMAIL");
               
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Remember Password";
                mailMessage.Body = "<p> Dear" + list[0] + " " + list[1] + "</p> <br> <p>Your password is" +" "+ list[2] + "</p>";
                mailMessage.BodyEncoding = Encoding.UTF8;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); // for Gmail smtp    
                smtp.UseDefaultCredentials = false;
                 smtp.Credentials = new NetworkCredential("YOUR_EMAIL","YOUR_EMAIL_PASSWORD");
              
                smtp.EnableSsl = true;
                
                try
                {


                    smtp.Send(mailMessage);

                }
                catch(Exception e)
                {
                    response.Message = "Password can not be send";
                    response.Status = false;
                    return Json(response);

                }
                response.Message = "Password has been send successfuly.Please check your email";
                response.Status = true;
                return Json(response);
            }
        }

    }


           
        }

