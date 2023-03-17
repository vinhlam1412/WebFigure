using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFig.Models;
namespace WebFig.Controllers
{
    public class NSXController : Controller
    {
        // GET: NSX
        public ActionResult Index()
        {
            UserData data = new UserData();

            var NSXfig = data.NSXes.ToList();
            Hashtable tenNSXfig = new Hashtable();
            foreach (var item in NSXfig)
            {
                tenNSXfig.Add(item.idNSX, item.tenNSX);
            }
            ViewBag.tenNSX = tenNSXfig;
            return PartialView("Index");
        }
    }
}