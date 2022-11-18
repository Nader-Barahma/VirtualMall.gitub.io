using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualMallProject.Data;
using VirtualMallProject.Models;

namespace VirtualMallProject.Controllers
{
    public class SearchController : Controller
    {
        public AppDbContext db;

        public SearchController(AppDbContext _db)
        {

            db = _db;

        }

        [HttpGet]
        public IActionResult Index(string Searching)
        {
            
            var data2 = db.Items.Where(a => a.ItemName.StartsWith(Searching) || Searching == null).ToList();
            var data3 = db.Brands;
            var data4 = db.Categories;
            var data5 = db.ItemDetails;


            var data = Tuple.Create<IEnumerable<Item>, IEnumerable<Brand>, IEnumerable<Category>, IEnumerable<ItemDetails>>(data2, data3, data4, data5);
            return View(data);
        }



    }
}
