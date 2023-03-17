using Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFig.Models;
using WebFig.Models.Momo;

namespace WebFig.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang

        UserData data = new UserData();

        // GET: Giohang
        public List<Cart> Laygiohang()
        {
            Account kh = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.Where(n => n.idAccount == kh.idAccount).ToList();
            if (lstGiohang == null)
            {
                ViewBag.ThongBao1 = "Gio Hang Rong";
            }
            else if (lstGiohang != null)
            {
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        int slton = 0;
        public ActionResult ThemGioHang(int id, string strURL, FormCollection collection)
        {

            if (Session["TaiKhoan"] != null)
            {
                Account kh = (Account)Session["TaiKhoan"];
                List<Cart> lstGiohang = Laygiohang(); // lấy ra danh sách giỏ hàng
                Cart sanpham = lstGiohang.Find(n => n.idProduct == id); // tìm trong giỏ hàng có idproduct nào bằng id product vừa bấm

                //Nếu sản phẩm đã có trong giỏ hàng
                var lstCart1 = data.Carts.Where(n => n.idAccount == kh.idAccount).ToList(); //lấy ra giỏ hàng theo id account                
                Cart sanpham1 = lstCart1.SingleOrDefault(n => n.idProduct == id); // tim đúng 1 idproduct trong giỏ hàng của account
                var sl = data.Products.Where(n => n.idProduct == id).First();
                slton = (int)sl.soluongton;
                //Nếu sản phẩm chưa có trong giỏ hàng
                if (sanpham == null)
                {
                    lstGiohang.Add(sanpham);

                    Cart list = lstGiohang.First();
                    Cart dh = new Cart();
                    Product product = new Product();

                    dh.idProduct = id;
                    dh.gia = (int?)sl.gia;
                    dh.soluong = 1;
                    dh.idAccount = kh.idAccount;

                    data.Carts.Add(dh);
                    data.SaveChanges();
                }

                else if (sanpham != null && sanpham1.soluong < sl.soluongton)
                {
                    Account account = (Account)Session["TaiKhoan"];
                    List<Cart> lst = data.Carts.ToList();
                    Cart lstCart = data.Carts.Where(n => n.idAccount == account.idAccount && n.idProduct == id).First();
                    sanpham.soluong++;
                    lstCart.soluong = sanpham.soluong;
                    UpdateModel(lstCart);
                    data.SaveChanges();

                }
            }
            else
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return Redirect(strURL);
        }



        private int TongSoLuongSanPham()

        {
            int tsl = 0;
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.ToList();
            if (account == null) { return 0; }
            else
            {
                var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();
                if (lstCart != null)
                {
                    tsl = lstCart.Count;

                }
                return tsl;
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.ToList();

            if (account == null) { return 0; }
            else
            {
                var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();
                if (lstCart != null)
                {
                    foreach (var item in lstCart)
                    {
                        tsl = (int)(tsl + item.soluong);

                    }
                }
                return tsl;

            }
        }
        private double TongTien()
        {
            double tt = 0;
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.ToList();
            if (account == null) { return 0; }
            else
            {
                var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();
                if (lstCart != null)
                {
                    foreach (var item in lstCart)
                    {
                        tt = tt + (double)(item.gia * item.soluong);
                    }
                }
                return tt;
            }        
        }


        public ActionResult GioHang(string thongbao)
        {
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = new List<Cart>();
            if (Session["TaiKhoan"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            else
            {     
                    var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();

                    foreach (var i in lstCart)
                    {
                        Cart cart = data.Carts.Where(n => n.idCart == i.idCart).First();
                        lstGiohang.Add(cart);

                    }
                    ViewBag.Tongsoluong = TongSoLuong();
                    ViewBag.Tongtien = TongTien();
                    ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
                    ViewBag.SoLuongTon = slton;                   

            }
            Session["GiohangAccount"] = lstGiohang;
            return View(lstGiohang);
        }


        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.ToList();
            var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();
            Cart sanpham = lstCart.SingleOrDefault(n => n.idProduct == id);
            if (sanpham != null)
            {
                data.Carts.Remove(sanpham);
                data.SaveChanges();
                return RedirectToAction("GioHang");
            }
            Session["Giohang"] = null;
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int id, FormCollection collection)

        {
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.ToList();
            var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();
            Cart sanpham = lstCart.SingleOrDefault(n => n.idProduct == id);
            var sl = data.Products.Where(n => n.idProduct == id).First();
            if (sanpham != null && int.Parse(collection["txtSoLg"]) <= sl.soluongton)
            {   
                    sanpham.soluong = int.Parse(collection["txtSoLg"].ToString());
                    UpdateModel(sanpham);
                    data.SaveChanges();            
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            Account account = (Account)Session["TaiKhoan"];
            List<Cart> lstGiohang = data.Carts.ToList();
            var lstCart = data.Carts.Where(n => n.idAccount == account.idAccount).ToList();
            foreach (var ele in lstCart)
            {
                data.Carts.Remove(ele);
            }
            data.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("GioHang");

        }
        int i = 0;
        [HttpGet]
        public ActionResult DatHang()
        {
            Account kh = (Account)Session["TaiKhoan"];
            List<Cart> dsGiohang = data.Carts.Where(n => n.idAccount == kh.idAccount).ToList();

            Session["Giohang"] = dsGiohang;


            // Lấy thông tin tài khoản
            var TTTaiKhoan = data.Accounts.Where(n => n.idAccount == kh.idAccount).First();          
            ViewBag.DiaChi = TTTaiKhoan.Diachi;
            ViewBag.SoDT = TTTaiKhoan.SoDT;
            ViewBag.HoTen = TTTaiKhoan.Hoten;
            ViewBag.Email = TTTaiKhoan.Email;

            var listdelivery = data.Deliveries.ToList();
            ViewBag.Deliveries = new SelectList(listdelivery, "idDelivery", "tenDelivery");

            var listpay = data.Payments.ToList();
            ViewBag.Payments = new SelectList(listpay, "idPayment", "tenPayment");

            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "UserHome");
            }
            List<Cart> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
           
            var E_payment = collection["idPayment"];
            var E_Deli = collection["idDelivery"];
            Session["id_deli"] = int.Parse(E_Deli);
            if (int.Parse(E_payment) == 2)
            {
                int i = int.Parse(E_Deli);
                return RedirectToAction("Payment", "Giohang", new { @id = int.Parse(E_Deli) });
            }
            else if (int.Parse(E_payment) == 3)
            {
                return RedirectToAction("PaymentWithPaypal", "Paypal");
            }
            LuuDonHang(collection);
            return RedirectToAction("XacnhanDonhang", "Giohang");
        }
        public ActionResult XacnhanDonhang()
        {
            return View();
        }
        public ActionResult Payment()
        {
            //request params need to request to MoMo system

            string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
            string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();
            string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
            string serectkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
            string notifyurl = ConfigurationManager.AppSettings["notifyurl"].ToString(); //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = TongTien().ToString();
            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());         
            JObject jmessage = JObject.Parse(responseFromMomo);         
            return Redirect(jmessage.GetValue("payUrl").ToString());

        }

        [HttpGet]
        public ActionResult ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = "Thông tin request không hợp lệ";
                return View();
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "Thanh toán thất bại";
            }
            else
            {
                ViewBag.message = "Thanh toán thành công";
                LuuDonHangMoMo();
            }
            return View();
        }

        [HttpPost]
        public JsonResult NotifyUrl(FormCollection collection, int id)
        {
            string param = "";
            param = "partner_code=" + Request["partner_code"] +
                "&access_key=" + Request["access_key"] +
                "&amount=" + Request["amount"] +
                "&order_id=" + Request["order_id"] +
                "&order_type=" + Request["order_type"] +
                "&transaction_id=" + Request["transaction_id"] +
                "&message=" + Request["message"] +
                "&response_time=" + Request["response_time"] +
                "&status_code=" + Request["status_code"];
            param =Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = "Thong tin request khong hop le";
            }
            string status_code = Request["status_code"].ToString();
            if (status_code != "0")
            {
                ViewBag.message = "Thanh toán thất bại";
            }else 
            {
                
            }

            return Json("",JsonRequestBehavior.AllowGet);
        }

        public void LuuDonHang(FormCollection collection)
        {
            Order order = new Order();
            Account kh = (Account)Session["TaiKhoan"];
            Product product = new Product();
            List<Cart> gh = Laygiohang();
  
            var E_payment = collection["idPayment"];
            var E_Deli = collection["idDelivery"];

            order.idAccount = kh.idAccount;
            order.idPayment = int.Parse(E_payment);
            order.idDelivery = (int)Session["id_deli"];
            order.NgayDat = DateTime.Now;
            order.IdStatus = 0;
            data.Orders.Add(order);
            data.SaveChanges();

            foreach (var item in gh)
            {
                OrderDetail detail = new OrderDetail();
                detail.IdOrder = order.IdOrder;
                detail.idProduct = (int)item.idProduct;
                detail.Quantity = item.soluong;
                detail.Price = (int?)item.gia;
                product = data.Products.Single(n => n.idProduct == item.idProduct);
                product.soluongton -= detail.Quantity;
                data.SaveChanges();

                data.OrderDetails.Add(detail);

            }
            SendEmail();
            List<Cart> lstGiohang = data.Carts.ToList();
            var lstCart = data.Carts.Where(n => n.idAccount == kh.idAccount).ToList();
            foreach (var ele in lstCart)
            {
                data.Carts.Remove(ele);
            }

            data.SaveChanges();          
            Session["Giohang"] = null;  
        }

        public void LuuDonHangMoMo()
        {
            Order order = new Order();
            Account kh = (Account)Session["TaiKhoan"];
            Product product = new Product();
            List<Cart> gh = Laygiohang();

            order.idAccount = kh.idAccount;
            order.idPayment = 2;
            order.idDelivery = (int)Session["id_deli"];
            order.NgayDat = DateTime.Now;
            order.IdStatus = 0;
            data.Orders.Add(order);
            data.SaveChanges();

            foreach (var item in gh)
            {
                OrderDetail detail = new OrderDetail();
                detail.IdOrder = order.IdOrder;
                detail.idProduct = (int)item.idProduct;
                detail.Quantity = item.soluong;
                detail.Price = (int?)item.gia;
                product = data.Products.Single(n => n.idProduct == item.idProduct);
                product.soluongton -= detail.Quantity;
                data.SaveChanges();

                data.OrderDetails.Add(detail);
            }
            SendEmail();
            List<Cart> lstGiohang = data.Carts.ToList();
            var lstCart = data.Carts.Where(n => n.idAccount == kh.idAccount).ToList();
            foreach (var ele in lstCart)
            {
                data.Carts.Remove(ele);
            }

            data.SaveChanges();
            Session["Giohang"] = null;
        }


        public void SendEmail()
        {       
            Account kh = (Account)Session["TaiKhoan"];
            var order = data.Orders.Where(n=>n.idAccount == kh.idAccount).First();
            var TTTaiKhoan = data.Accounts.Where(n => n.idAccount == kh.idAccount).First();
            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));
            content = content.Replace("{{CustomerName}}", kh.Hoten);
            content = content.Replace("{{Phone}}", TTTaiKhoan.SoDT);
            content = content.Replace("{{Email}}", TTTaiKhoan.Email);
            content = content.Replace("{{Address}}", TTTaiKhoan.Diachi);
            content = content.Replace("{{Total}}", TongTien().ToString("N0"));
            content = content.Replace("{{DateBuy}}", order.NgayDat.ToString());

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(TTTaiKhoan.Email, "Đơn hàng mới từ Figure Shop", content); ;
            new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Figure Shop", content);       
        }
    }       
}
