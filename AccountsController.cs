using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VirtualMallProject.Data;
using VirtualMallProject.Models;
using VirtualMallProject.Models.ViewModels;

namespace VirtualMallProject.Controllers
{
    public class AccountsController : Controller
    {

        public AppDbContext db;
      
        public AccountsController(AppDbContext _db)
        {

            db = _db;
         
        }

        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Gender = model.Gender,
                    Address = model.Address,
                    City=model.City,
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    

                };
                db.Users.Add(user);
                db.SaveChanges();
                HttpContext.Session.SetString("Mobile",user.Mobile);
                HttpContext.Session.SetString("Email", model.Email);
                HttpContext.Session.SetString("Address", model.Address);
                HttpContext.Session.SetString("City", model.City);
                HttpContext.Session.SetString("name", model.UserName);


                return RedirectToAction("OTPAutherization","Home");

            }


            return View(model);
        }


        
        [HttpGet]


        public IActionResult LoginUser()
        {
            return View();
        }



        [HttpPost]

        public IActionResult LoginUser(LoginViewModel model) // object from RegisterViewModel
        {

            if (ModelState.IsValid)
            {
                var data = db.Users.Where(a => a.UserName.Equals(model.UserName) && a.Password.Equals(model.Password)); // encode
                var data2 = data.Select(a => a.Email);
               
                if (data.Any())  // if succes
                {
                    foreach(var item in data2)
                    { 
                    HttpContext.Session.SetString("Email",item);

                    }
                    HttpContext.Session.SetString("name", model.UserName);
              
                    return RedirectToAction("Index", "Home");

                }
                ViewBag.err = "Invalid Username Or Password";


                return View(model);

            }


            return View(model);

        }




        


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("LoginUser");
        }





        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model,string Name)
        {
            Contact contact = new Contact 
            {
            Email=model.Email,
            Subject=model.Subject,
            Message=model.Message,
            Username=Name
            
            };
            
            db.Contacts.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Contact");

        }


    }



}
