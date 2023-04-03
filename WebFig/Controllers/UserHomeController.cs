using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebFig.Models;
using WebFig.Models.Momo;

namespace WebFig.Controllers
{
    public class UserHomeController : Controller
    {
        // GET: UserHome
        UserData data = new UserData();
        // GET: UserHome


        public ActionResult Index(string SearchString, int idCategory = 0, int idNSX = 0)
        {
            ViewBag.Find = SearchString;
            var all_product = (from ele in data.Products select ele).OrderBy(p => p.idProduct);
            if (!String.IsNullOrEmpty(SearchString))
            {
                all_product = (IOrderedQueryable<Product>)all_product.Where(a => a.ten.Contains(SearchString));
                return View(all_product.ToList());
            }
            else if (idCategory == 0 && idNSX == 0)
            {
                var products = data.Products.ToList();
                return View(products.ToList());
            }
            else if (idCategory != 0)
            {
                var products = data.Products.Where(x => x.idCategory == idCategory);
                return View(products.ToList());

            }
            else if (idNSX != 0)
            {
                var sanPhams = data.Products.Where(x => x.idNSX == idNSX);
                return View(sanPhams.ToList());
            }
            return View();

        }

        public ActionResult Detail(int id)
        {
            var D_fig = data.Products.Where(m => m.idProduct == id).FirstOrDefault();
            ViewBag.ProductId = id;
            ViewBag.SoLgTon = D_fig.soluongton;
            return View(D_fig);
        }

        public ActionResult AccountDetail(int idAccount)
        {
            var D_fig = data.Accounts.Where(m => m.idAccount == idAccount).First();
            return View(D_fig);
        }


        [HttpPost]
        public ActionResult AccountDetail(int idAccount, Account account)
        {

            var check = data.Accounts.Where(n => n.idAccount == idAccount).FirstOrDefault();
            if (check != null)
            {
                check.username = check.username;
                check.password = check.password;
                check.password_verify = check.password;
                check.Email = account.Email;
                check.IsValid1 = check.IsValid1;
                check.Hoten = check.Hoten;
                check.Diachi = account.Diachi;
                check.SoDT = account.SoDT;
                data.SaveChanges();
                return RedirectToAction("DatHang", "Giohang");
            }
            else
            {
                return View();
            }
        }



        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient()
        {
            //hiển thị thông báo cho người dùng
            return View();
        }

     

    }
}
    