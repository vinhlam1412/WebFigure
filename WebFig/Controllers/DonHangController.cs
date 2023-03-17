using System;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFig.Models;
namespace WebFig.Controllers
{
    public class DonHangController : Controller
    {
        // GET: DonHang
        UserData data = new UserData();
        public ActionResult DSDonHang(int ? page)
        {
            if(Session["TaiKhoan"]!= null)
            {
                if (page == null) page = 1;
                int pageSize = 6;
                int pageNum = page ?? 1;
                Account kh = (Account)Session["TaiKhoan"];
                var list_donhang = data.Orders.Where(n => n.idAccount == kh.idAccount).ToList();
                return View(list_donhang.ToPagedList(pageNum,pageSize));
            }
            else
            {
                return View("DangNhap","NguoiDung");
            }
        }
        public ActionResult ChiTietDonHang(int ? page, int id)
        {
            if (page == null) page = 1;
            int pageSize = 6;
            int pageNum = page ?? 1;
            var chitietdonhang = data.OrderDetails.Where(n=>n.IdOrder == id).ToList();
            return View(chitietdonhang.ToPagedList(pageNum,pageSize));
        }
    }
}