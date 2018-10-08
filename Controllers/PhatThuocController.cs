using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class PhatThuocController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
        // GET: QLBaciSi
        // GET: PhatThuoc
        public ActionResult Index()
        {
            
            ViewBag.BenhNhan = ur.GetAllUser().ToList();
            var lk = ur.DSPhatThuoc().Where(x => x.NgayKham.Value.ToShortDateString() == DateTime.Today.ToShortDateString());

            return View(lk);
        }


        public ActionResult Edit(int id)
        {
            ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
            var a = ur.getthelasttoabyid(id);
            ViewBag.phieu = ur.getphieukham(id);
            ServiceReference1.ToaThuoc l = ur.chitiettoa(a);
            ViewBag.bacsi = ur.getbacsi((Int32)ur.getphieukham((Int32)l.MaPhieuKham).MaBacSi);
            ViewBag.benhnhan = ur.GetAllUserById((Int32)ur.getphieukham((Int32)l.MaPhieuKham).MaBenhNhan);
            ViewBag.chitiet = ur.getallchitiettoathuocbytoa(l.MaToaThuoc).ToList();
            ViewBag.toa = l;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection f)
        {
            ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
            ur.thanhtoanphieu(id);
            return RedirectToAction("Index");
        }

    }
}