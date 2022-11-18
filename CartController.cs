using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualMallProject.Data;
using VirtualMallProject.Models;
using VirtualMallProject.Models.ViewModels;
using static System.Net.WebRequestMethods;

namespace VirtualMallProject.Controllers
{
    public class CartController : Controller
    {


        public AppDbContext db;

        public CartController(AppDbContext _db)
        {

            db = _db;
        }

        [HttpGet]
        public IActionResult Cart()
        {
            var data2 = db.Carts;
            var data3 = db.Items;
            var data4 = db.ItemDetails;



            var data = Tuple.Create<IEnumerable<Cart>, IEnumerable<Item>, IEnumerable<ItemDetails>>(data2, data3,data4);


            return View(data);
        }

        
       



        [HttpPost]
        public IActionResult Delete(int id)
        {

            var data = db.Carts.Find(id);
            db.Carts.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }



       
        [HttpGet]
        public IActionResult Checkout(double Price)
        {
            if (Price == 0)
            {
                return RedirectToAction("Cart", "Cart");
            }

                var p = Price;
                ViewBag.price = p;

            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Bill model, string Name)
        {
            Bill bill = new Bill
            {
                BillDate=DateTime.Now,
                Username=Name,
                City=model.City,
                Address=model.Address,
                Number=model.Number,
                Status="Sent"
            };

            db.Bills.Add(bill);
            db.SaveChanges();


            var data = db.Bills.Where(a => a.Username == bill.Username).Select(a=>a.BillId);
            
            var data2 = db.Carts.Where(a => a.UserId == bill.Username);
            
           
            foreach (var item in data2.Where(a => a.UserId == bill.Username))
            {
                BillDetails Bill = new BillDetails
                {
                    BillId = bill.BillId,
                    ItemId = item.ItemId,
                    Qty = item.Qty,
                    Size=item.Size
                };

                db.BillDetails.Add(Bill);

                var delete = db.Carts.Find(item.CartId);
                db.Carts.Remove(delete);
             
            }
            db.SaveChanges();
            return RedirectToAction("OrderTracking", "OrderTracking");

        }

    }
}
