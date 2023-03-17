using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFig.Models;
namespace WebFig.Controllers
{
    public class GioiThieuController : Controller
    {
        // GET: GioiThieu
        UserData data = new UserData();
        public ActionResult Gioithieu(int idBaiviet = 1)
        {
            var D_fig = data.Gioithieux.Where(m => m.idBaiviet == idBaiviet).First();
            ViewBag.idBaiviet = idBaiviet;
            return View(D_fig);
        }


    }
}