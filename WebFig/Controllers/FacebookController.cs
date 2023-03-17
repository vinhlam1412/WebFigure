using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using Facebook;

using Newtonsoft.Json;

using System.Web.Security;
using WebFig.Models;
using System.Configuration;
using System.Data.Entity.Validation;

namespace WebFig.Controllers
{
    public class FacebookController : Controller
    {
        UserData context = new UserData();
        // GET: Facebook
        public ActionResult Index()
        {
            return View();
        }

/*
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
            var check = context.Accounts.SingleOrDefault(p => p.Email == user.Email);
            if (check == null)
            {
                context.Accounts.Add(user);
                try
                {
                    context.SaveChanges();
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
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var user = new Account();
                user.Hoten= firstname + " " + middlename + " " + lastname;
                user.Email= email;
                user.username = firstname + " " + middlename + " " + lastname;
                user.password = email;
                user.password_verify = email;
                user.SoDT = "1111";
                user.Diachi = "Facebook";
                user.IsValid1 = true;
                int check = InsertForFacebook(user);
                if (check > 0)
                {
                    Session["TaiKhoan_maSS"] = check;
                    Session["TaiKhoan_tenDNSS"] = firstname + " " + middlename + " " + lastname;
                }
            }
            return RedirectToAction("Index", "UserHome");
        }*/
        /*
                [AllowAnonymous]

                public ActionResult Facebook()

                {

                    var fb = new FacebookClient();

                    var loginUrl = fb.GetLoginUrl(new

                    {




                        client_id = "383277240435869",

                        client_secret = "56a39d9a34845c32ee47f26da316d240",

                        redirect_uri = RediredtUri.AbsoluteUri,

                        response_type = "code",

                        scope = "email"



                    });

                    return Redirect(loginUrl.AbsoluteUri);

                }


                public ActionResult FacebookCallback(string code)
                {

                    var fb = new FacebookClient();

                    dynamic result = fb.Post("oauth/access_token", new

                    {

                        client_id = "Your App ID",

                        client_secret = "Your App Secret key",

                        redirect_uri = RediredtUri.AbsoluteUri,

                        code = code




                    });

                    var accessToken = result.access_token;

                    Session["AccessToken"] = accessToken;

                    fb.AccessToken = accessToken;

                    dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

                    string email = me.email;

                    TempData["email"] = me.email;

                    TempData["first_name"] = me.first_name;

                    TempData["lastname"] = me.last_name;

                    TempData["picture"] = me.picture.data.url;

                    FormsAuthentication.SetAuthCookie(email, false);

                    return RedirectToAction("Index", "UserHome");

                }
        */


    }
}