using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebFig.Models;
using WebFig.Models.Momo;
using Facebook;
namespace WebFig.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        UserData data = new UserData();



        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult DangNhapFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public int InsertForFacebook(Account user)
        {
            var check = data.Accounts.SingleOrDefault(p => p.username == user.username);
            if (check == null)
            {
                data.Accounts.Add(user);
                try
                {
                    data.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    Console.WriteLine(e);
                }
                return user.idAccount;
            }
            else
                return check.idAccount;
        }
        public ActionResult FacebookCallback(string code)
        {

            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            string Numrd_str;
            Random rd = new Random();
            int Numrd = rd.Next(35450, 1000000);//biến Numrd sẽ nhận có giá trị ngẫu nhiên trong khoảng 1 đến 100
            Numrd_str = rd.Next(35450, 1000000).ToString();//Chuyển giá trị ramdon về kiểu string
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var user = new Account();
                user.username = firstname;
                user.password = Numrd_str;
                user.password_verify = Numrd_str;
                user.Hoten = firstname + " " + middlename + " " + lastname;
                user.SoDT = "1111111111";
                user.Diachi = "10Facebook";
                user.Email = email;
                user.IsValid1 = true;
                int check = InsertForFacebook(user);
                if (check > 0)
                {
                    var taikhoan = data.Accounts.Where(p => p.idAccount == check).FirstOrDefault();
                    Session["TaiKhoan"] = taikhoan;
                    Session["Fullname"] = taikhoan.Hoten;
                }
            }
            return RedirectToAction("Index", "UserHome");
        }
        /*
                public ActionResult DangKy()
                {
                    return View();
                }

                [HttpPost]
                public ActionResult DangKy(Account acc)
                {
                    if (data.Accounts.Any(b => b.idAccount == acc.idAccount))
                        return View(); 
                    else
                    {
                        data.Accounts.Add(acc);
                        data.SaveChanges();
                        return RedirectToAction("Index", "UserHome");
                    }

                }*/
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, Account account)
        {

            var tendangnhap = collection["username"];
            var matkhau = collection["password"];
            var password_verify = collection["password_verify"];
            var hoten = collection["Hoten"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["SoDT"];
            var email = collection["Email"];
            if (String.IsNullOrEmpty(password_verify))
            {
                ViewData["NhapMKXN"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (!matkhau.Equals(password_verify))
                {
                    ViewData["MatKhauGiongNhau"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau ";
                }
                else
                {
                    var check = data.Accounts.FirstOrDefault(s => s.Email == account.Email);
                    var check1 = data.Accounts.FirstOrDefault(s => s.username == account.username);
                    if (check == null && check1 == null)
                    {
                        account.Hoten = hoten;
                        account.username = tendangnhap;
                        account.password = matkhau;
                        account.Diachi = diachi;
                        account.SoDT = dienthoai;
                        account.Email = email;
                        account.IsValid1 = false;
                        data.Accounts.Add(account);
                        data.SaveChanges();
                        var timtk = data.Accounts.FirstOrDefault(s => s.username == tendangnhap);
                        BuildEmailTemplate(timtk.idAccount);
                    }
                    else
                    {
                        ViewBag.error = "Usernam đã tồn tại";
                        return View();
                    }

                }
            }
            return this.DangKy();
        }

        private void BuildEmailTemplate(int idAccount)
        {

            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/Views/NguoiDung/") + "BuildEmailTemplate" + ".cshtml");
            var regInfo = data.Accounts.Where(x => x.idAccount == idAccount).FirstOrDefault();
            var url = "https://localhost:44397/" + "NguoiDung/Confirm?idAccount=" + idAccount;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmailTemplate("Your account is successfully created", body, regInfo.Email);
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendangnhap = collection["username"];
            var matkhau = collection["password"];
            Account kh = data.Accounts.SingleOrDefault(n => n.username == tendangnhap && n.password == matkhau);
            var account = data.Accounts.ToList();
            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = kh;
                Session["Fullname"] = kh.Hoten;           
                return RedirectToAction("Index", "UserHome");
            }
            else
            {
                ViewBag.ThongBao = "Tài khoản không tồn tại";
            }
            if (String.IsNullOrEmpty(tendangnhap))
            {
                ViewData["Loi1"] = "Tài khoản không được để trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
           
            return View();
        }

        public ActionResult DangXuat()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index", "UserHome");
        }

        public ActionResult Confirm(int idAccount)
        {
            ViewBag.idAccount = idAccount;
            return View();
        }

        public JsonResult RegisterConfirm(int idAccount,Account Data)
        {
            try
            {
                Data = data.Accounts.Where(x => x.idAccount == idAccount).FirstOrDefault();
                Data.idAccount = idAccount;
                Data.username = Data.username;
                Data.password_verify = Data.password;
                Data.password = Data.password;
                Data.Hoten = Data.Hoten;
                Data.SoDT = Data.SoDT;
                Data.Diachi = Data.Diachi;
                Data.Email = Data.Email;
                Data.IsValid1 = true;
                data.SaveChanges();
                string msg = "Email của bạn đã được xác nhận";
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }


        public static void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            from = "cylaxjohn46@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress((bcc)));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress((cc)));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendMail(mail);

        }

        private static void SendMail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryFormat = (SmtpDeliveryFormat)SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("wibustore170@gmail.com", "uzbvagzhsmwzjfbp");

            /*"123456Vinh@"*/
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
        public ActionResult changePassword(int id)
        {
            var check = data.Accounts.Where(n=>n.idAccount ==  id).FirstOrDefault();
            return  View(check);
        }
        
        [HttpPost]
        public ActionResult changePassword(int id, Account account)
        {
            var check = data.Accounts.Where(n => n.idAccount == id).FirstOrDefault();
           if(check != null)
            {
                check.idAccount = check.idAccount;
                check.username = check.username;
                check.password = account.password;
                check.password_verify = account.password;
                check.password_verify = account.password;
                check.Hoten = check.Hoten;
                check.SoDT = check.SoDT;
                check.Diachi = check.Diachi;
                check.Email = check.Email;
                check.IsValid1 = check.IsValid1;
                
                
                
                data.SaveChanges();
                return RedirectToAction("DangNhap");
            }else
            {
                return View();
            }
        }

        public ActionResult NhapTaiKhoan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NhapTaiKhoan(Account kh)
        {
            var check = data.Accounts.FirstOrDefault(x => x.Email == kh.Email);
            if (check != null)
            {

                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/changepass.html"));
                content = content.Replace("{{CustomerName}}", check.Hoten);
                content = content.Replace("{{Phone}}", check.SoDT);
                content = content.Replace("{{Email}}", check.Email);
                content = content.Replace("{{Address}}", check.Diachi);
                content = content.Replace("{{Url}}", "https://localhost:44397/NguoiDung/changePassword/" + check.idAccount);
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMailPass(check.Email, "Thay đổi mật khẩu", content);
                new MailHelper().SendMailPass(toEmail, "Thay đổi mật khẩu", content);
                ViewBag.Email2 = "Gửi thành công! Vui lòng kiểm tra email!" + " " + check.Email.ToString();
            }
            else
            {
                ViewBag.Email = "Email không tồn tại";
            }
            return View();
        }
        public void SendEmailchangePass()
        {
            Account kh = new Account();
            var check = data.Accounts.Where(n => n.idAccount == kh.idAccount).FirstOrDefault();
            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/changepass.html"));
            content = content.Replace("{{CustomerName}}", kh.Hoten);
            content = content.Replace("{{Phone}}", kh.SoDT);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{Address}}", kh.Diachi);
            content = content.Replace("{{Url}}", "https://localhost:44397/NguoiDung/changePassword"+check.idAccount);   

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new MailHelper().SendMail(kh.Email, "Đơn hàng mới từ Wibu Shop", content);
            new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Wibu Shop", content);
        }

    }
}
   
