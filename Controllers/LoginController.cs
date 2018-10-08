using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    public class LoginController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            var username = fc["inputId"];
            var password = fc["inputPassword"];
            ViewBag.Username = username;
            ViewBag.Password = password;
            if (String.IsNullOrEmpty(username))
            {
                ViewBag.Error = "Chưa nhập tên Đăng nhập";
            }
            else
            {
                if (String.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Chưa nhập Password";
                }
                else
                {
                    String user = ur.Login(username, password);

                    if (user == null)
                    {
                        var bss = ur.Loginbs(username, password);
                        if (bss != null)
                        {
                            Session.Add("user", bss);
                            return RedirectToAction("Index", "BacSi");
                        }
                        else
                        {
                            ViewBag.Error = "Sai tên đăng nhập  hoặc mật khẩu";
                        }

                    }

                    else
                    {
                        if (user.Equals("NhanVienNhanBenh"))
                        {
                            // Session["user"] = user;
                            Session.Add("user", user);
                            return RedirectToAction("Create", "NhanVienNhanBenh");
                        }
                        else if (user.Equals("QuanLy"))
                        {
                            // Session["user"] = user;
                            Session.Add("user", user);
                            return RedirectToAction("Index", "QLBenhNhan");
                        }
                        else if (user.Equals("NhanVienPhatThuoc"))
                        {
                            // Session["user"] = user;
                            Session.Add("user", user);
                            return RedirectToAction("Index", "PhatThuoc");
                        }

                    }

                }
            }

            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }
    }

}