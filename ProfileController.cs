using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
    public class ProfileController : Controller
    {
        public AppDbContext db;
        public ProfileController(AppDbContext _db)
        {

            db = _db;

        }



        [HttpGet]
        public IActionResult Profile(string name)
        {
            var data1 = db.Bills;
            var data2 = db.BillDetails;
            var data3 = db.Items;





            var data = Tuple.Create<IEnumerable<Bill>, IEnumerable<BillDetails>, IEnumerable<Item>>(data1, data2, data3);





            return View(data);
        }



        [HttpGet]
        public IActionResult Search()
        {
            var data = db.Items;

            return View(data);

        }
    }
}
