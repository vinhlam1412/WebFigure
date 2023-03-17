using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFig.Models;
namespace WebFig.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            UserData data = new UserData();

            var loaifig = data.Categorys.ToList();
            Hashtable tenloaifig = new Hashtable();
            foreach (var item in loaifig)
            {
                tenloaifig.Add(item.idCategory, item.tenCategory);
            }
            ViewBag.tenCategory = tenloaifig;
            return PartialView("Index");
        }
    }
}