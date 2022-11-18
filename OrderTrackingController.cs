using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualMallProject.Data;

namespace VirtualMallProject.Controllers
{
    public class OrderTrackingController : Controller
    {
        public AppDbContext db;

        public OrderTrackingController(AppDbContext _db)
        {

            db = _db;

        }
        public IActionResult OrderTracking()
        {
            var data = db.Bills;

            return View(data);
        }
    }
}
