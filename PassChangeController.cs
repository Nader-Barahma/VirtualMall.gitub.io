using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualMallProject.Data;
using VirtualMallProject.Models;
using System.Net.Mail;

namespace VirtualMallProject.Controllers
{
    public class PassChangeController : Controller
    {

        public AppDbContext db;

        public PassChangeController(AppDbContext _db)
        {

            db = _db;
        }



        public IActionResult PassChange()
        {
            var data = db.Users;
            return View(data);
        }


        [HttpPost]
        public IActionResult PassChange(string OldPass, string NewPass, string ConNewPass, string name,string email)
        {


            var data = db.Users.Where(a => a.UserName == name);
            var data2 = data.Where(a => a.Password == OldPass);

            if (data2.Any())
            {
                if (NewPass == ConNewPass)
                {

                    var ent = db.Set<User>().SingleOrDefault(o => o.UserName == name);
                    ent.Password = NewPass;
                    db.SaveChanges();

                    string to = email;
                    string subject = "Virtual Mall | Password Changed ";
                    string body = "Done ,Your password has been changed";
                    MailMessage Ma = new MailMessage();
                    Ma.To.Add(to);
                    Ma.Subject = subject;
                    Ma.Body = body;
                    Ma.From = new MailAddress("naderbarahma2001@gmail.com");
                    Ma.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("naderbarahma2001@gmail.com", "tivtjlrphhssebft");
                    smtp.Send(Ma);

                    
                    ViewBag.PassDone = "Done, The Password Changed";
                    return View();
                }
                else
                {
                    ViewBag.Pass = "the Passwords Not Match";

                }

            }
            else
            {
                ViewBag.Pass = "Old Password Not Match";


            }


            return View();
        }




        


    }
}
