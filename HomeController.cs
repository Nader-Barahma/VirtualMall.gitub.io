using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualMallProject.Data;
using VirtualMallProject.Models;
using VirtualMallProject.Models.ViewModels;

namespace VirtualMallProject.Controllers
{
    public class HomeController : Controller
    {
        public AppDbContext db;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private readonly DNTCaptchaOptions _catchaOptions;
        public HomeController(AppDbContext _db, IDNTCaptchaValidatorService validatorService, IOptions<DNTCaptchaOptions> catchaOptions)
        {

            db = _db;
            _validatorService = validatorService;
            _catchaOptions = catchaOptions==null?throw new ArgumentNullException(nameof(catchaOptions)):catchaOptions.Value;
        }



        public IActionResult Index()
        {
        
            return View();
      
        }



       

              [HttpGet]
              public IActionResult ShopItem(int id)
              {
              
                  var data1 = db.Shops.Find(id);
                  var data2 = db.Items.Where(c => c.ShopId == data1.ShopId);
                  var data3 = db.Brands.Where(c=>c.ShopId==data1.ShopId);
                  var data4 = db.Categories.Where(c => c.ShopId == data1.ShopId);
                   var data5 = db.ItemDetails;


        
                  
                 var name = data1.ShopName;
                  ViewBag.Name = name;
            
              var data = Tuple.Create<IEnumerable<Item>, IEnumerable<Brand>, IEnumerable<Category>,IEnumerable<ItemDetails>>(data2,data3,data4,data5);



                  return View(data);

              }




        [HttpPost]
        public async Task<IActionResult> ShopItem(string Size, int Qty, int id, string Session)
        {
            var data2 = db.Items.Find(id);

            var data = (from p in db.Carts select p).FirstOrDefault();

            // user.SessionID is an integer


            Cart cart1 = new Cart
            {
                UserId = Session,
                ItemId = id,
                Qty = Qty,
                Size = Size,
                CreationDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false

            };

            if (cart1.UserId == null)
            {
                return RedirectToAction("LoginUser", "Accounts");
            }

            db.Carts.Add(cart1);
            db.SaveChanges();

            return RedirectToAction("ShopItem", "Home", new { ID = data2.ShopId });







            return View();
        }








        public IActionResult OTPAutherization()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Comment(Message model, string name)
        {
            if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
            {
                this.ModelState.AddModelError(_catchaOptions.CaptchaComponent.CaptchaInputName, "Please enter the security code as number . ");


                return View("Index");
            }
            else
            {
                Message message = new Message
                {
                    TheMessage = model.TheMessage,
                    Name = name,
                    Status = "False"

                };
                db.Messages.Add(message);
                db.SaveChanges();
            }
            return View("Index");
        }






    }

}
