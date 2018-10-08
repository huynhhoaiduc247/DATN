using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class QLTaiKhoanController : Controller
    {
        ServiceReference1.MyServiceClient sv = new ServiceReference1.MyServiceClient();
        // GET: QLTaiKhoan
        public ActionResult Index()
        {
            var l = sv.getalltaikhoan();
            return View(l);
            
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(ServiceReference1.taikhoanEntity tk)
        {
            var l = sv.getalltaikhoan();
            ServiceReference1.Taikhoan t = new ServiceReference1.Taikhoan();
            t.ID = tk.Id;
            t.LoaiTaiKhoan = tk.LoaiTaiKhoan;
            t.MatKhau = tk.MatKhau;
            foreach (var it in l)
            {
                if(it.ID == tk.Id)
                {
                    ViewBag.thongbao = "Trùng ID";
                   
                    return View(t);
                    
                }
            }
            if (ModelState.IsValid)
            {
                ViewBag.thongbao = "";
                sv.addTaiKhoan(tk);
                return RedirectToAction("Index");
            }
            
            return View(t);
        }

        public ActionResult Edit(string id)
        {
            ServiceReference1.Taikhoan a = sv.getTaiKhoan(id);
            
            return View(a);

        }
        [HttpPost]
        public ActionResult Edit(string id , ServiceReference1.taikhoanEntity tk)
        {
            ServiceReference1.Taikhoan a = sv.getTaiKhoan(id);
            if (ModelState.IsValid)
            {
                sv.updateTaiKhoan(tk);
                return RedirectToAction("Index");
            }
            return View(a);

        }

        public ActionResult Delete(string id)
        {
            sv.DeleteTaiKhoan(id);
            return RedirectToAction("Index");
        }
    }
}