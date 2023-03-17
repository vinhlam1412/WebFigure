using Common;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFig.helper;
using WebFig.Models;
using Order = WebFig.Models.Order;
using Payment = PayPal.Api.Payment;

namespace WebFig.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Paypal
        UserData data = new UserData();
        private PayPal.Api.Payment payment;
        public double TyGiaUSD = 23300;
        public ActionResult Index()
        {
            return View();
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string failedUrl)
        {

            Account tk = (Account)Session["TaiKhoan"];
            var kh = data.Accounts.Where(x => x.idAccount == tk.idAccount).FirstOrDefault();
            var itemList = new ItemList() { items = new List<Item>(), shipping_address = new ShippingAddress() { recipient_name = kh.Hoten.ToString(), country_code = "VN", city = kh.Diachi.ToString(), line1 = kh.Diachi.ToString(), postal_code = "700000" } };
            //recipient_name: tên người đặt hàng
            //country_code: code quốc gia, tham khảo thêm tại: https://developer.paypal.com/docs/api/reference/country-codes/
            //city: thành phố shipping
            //line1: địa chỉ giao hàng
            //postal_code: code postal (ví dụ code ở Việt Nam: https://www.google.com/search?q=postal+code+vietnam)


            List<Cart> lstCart = data.Carts.Where(n => n.idAccount == kh.idAccount).ToList();
            if (lstCart.Count > 0)
            {
                foreach (var item in lstCart)
                {
                    string price_ = Math.Round((double)item.gia / TyGiaUSD, 2).ToString();
                    var soluong = Math.Round((double)item.soluong);
                    itemList.items.Add(new Item()
                    {
                        //Thông tin đơn hàng

                        //name = item.Product.ten.ToString(),
                        //currency = "USD",
                        //price = item.gia.ToString(),
                        //quantity = item.soluong.ToString(),
                        //sku = item.ToString(),
                        name = item.Product.ten,
                        currency = "USD",
                        price = price_,
                        quantity = soluong.ToString(),
                        sku = "sku"
                    });
                }
            }
            var total = Math.Round((double)lstCart.Sum(p => p.gia * p.soluong) / TyGiaUSD, 2);

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = failedUrl, //url cancel
                return_url = redirectUrl //url return
            };

            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = total.ToString()
            };


            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details,
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction description.", //nội dung thanh toán
                invoice_number = DateTime.Now.ToString(), //mã hóa đơn
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            return this.payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult PaymentWithPaypal(int? mavan)
        {
            APIContext apiContext = helper.Configuration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority
                    + "/Paypal/PaymentWithPaypal?";

                    //link trả về khi người dùng hủy thanh toán
                    string failedURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/FailureView";

                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, failedURI);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }
            return RedirectToAction("SuccessView");
        }
        public double TongTien()
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
        public void SendEmail()
        {
            Account kh = (Account)Session["TaiKhoan"];
            var order = data.Orders.Where(n => n.idAccount == kh.idAccount).First();
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
        public void LuuDonHangPaypal()
        {
            Order order = new Order();
            Account kh = (Account)Session["TaiKhoan"];
            Product product = new Product();
            List<Cart> gh = Laygiohang();
            var ngaygiao = DateTime.Now;

            //var E_Deli = collection["idDelivery"];

            order.idAccount = kh.idAccount;
            order.idPayment = 3;
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

        [HttpGet]
        public ActionResult SuccessView()
        {
            LuuDonHangPaypal();
            return View();

        }
        public ActionResult FailureView()
        {
            return View();
        }
    }
}