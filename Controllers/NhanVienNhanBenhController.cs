using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    public class NhanVienNhanBenhController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
        // GET: NhanVienNhanBenh
        public ActionResult Index()
        {
            ViewBag.PhieuKham = ur.GetAllPhieuKham();
            ViewBag.BenhNhan = ur.GetAllUser();
            ViewBag.ListBacSi = ur.GetAllBacSi();
            return View();
        }
        public ActionResult Details(int id)
        {
            var a = ur.GetAllUserById(id);
            return View(a);
        }
        public ActionResult Delete(int id)
        {
            ur.DeleteUserById(id);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            ViewBag.PhieuKham = ur.GetAllPhieuKham();
            ViewBag.BenhNhan = ur.GetAllUser();
            ViewBag.ListBacSi = ur.GetAllBacSi();
            var lstbs = ur.GetAllBacSi().ToList();
            SelectList lstbacsi = new SelectList(lstbs, "MaBacSi", "TenBacSi");
            ViewBag.BacSi = lstbacsi;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(benhnhanEntity a, FormCollection fc)
        {
            var bs = fc["bacsi"];
            var ax = new ServiceReference1.benhnhanEntity();
            ax.HoTenBenhNhan = a.HoTenBenhNhan;
            ax.SDT = a.SDT;
            ax.Tuoi = a.Tuoi;
            ax.DiaChi = a.DiaChi;
            if (ModelState.IsValid)
            {


                var pk = new ServiceReference1.phieuKhamEntity();
                //pk.maBenhNhan = ax.mabenhnhan;
                pk.maBacSi = int.Parse(bs);
                pk.trangThai = "Đang Chờ";
                //ur.AddPhieuKham(pk);
                ur.AddUserPhieuKham(ax, pk);
                return RedirectToAction("Create", "NhanVienNhanBenh");
            }
            return RedirectToAction("Create", "NhanVienNhanBenh");
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ListBacSi = ur.GetAllBacSi();
            ViewBag.PhieuKham = ur.GetAllPhieuKham();
            ViewBag.BenhNhan = ur.GetAllUser();
            var a = ur.GetAllUserById(id);
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(int id, benhnhanEntity a)
        {

            var axx = new ServiceReference1.benhnhanEntity();
            axx.mabenhnhan = id;
            axx.HoTenBenhNhan = a.HoTenBenhNhan;
            axx.SDT = a.SDT;

            axx.Tuoi = (int)a.Tuoi;
            axx.DiaChi = a.DiaChi;
            if (ModelState.IsValid)
            {
                ur.UpdateUser(axx);
                return RedirectToAction("Create", "NhanVienNhanBenh");
            }
            return RedirectToAction("Create", "NhanVienNhanBenh");
        }
        public ActionResult CapSoKhamBenh(int id)
        {
            ViewBag.ListBacSi = ur.GetAllBacSi();
            ViewBag.PhieuKham = ur.GetAllPhieuKham();
            ViewBag.BenhNhan = ur.GetAllUser();
            var lstbs = ur.GetAllBacSi().ToList();
            SelectList lstbacsi = new SelectList(lstbs, "MaBacSi", "TenBacSi");
            ViewBag.BacSi = lstbacsi;
            var a = ur.GetAllUserById(id);
            return View(a);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapSoKhamBenh(int id, benhnhanEntity a, FormCollection fc)
        {
            var bs = fc["bacsi"];
            var axx = new ServiceReference1.benhnhanEntity();
            axx.mabenhnhan = id;
            axx.HoTenBenhNhan = a.HoTenBenhNhan;
            axx.SDT = a.SDT;
            axx.Tuoi = (int)a.Tuoi;
            axx.DiaChi = a.DiaChi;
            if (ModelState.IsValid)
            {
                ur.UpdateUser(axx);
                var pk = new ServiceReference1.phieuKhamEntity();
                pk.maBenhNhan = id;
                pk.maBacSi = int.Parse(bs);
                pk.trangThai = "Đang Chờ";
                if (ur.AddPhieuKham(pk) == 2)
                {


                    return null;
                }
                else
                {
                    ur.AddPhieuKham(pk);

                    return RedirectToAction("Create", "NhanVienNhanBenh");
                }
                // return RedirectToAction("Create", "NhanVienNhanBenh");
            }

            return RedirectToAction("Create", "NhanVienNhanBenh");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult XemChiTietBacSi(FormCollection fc1)
        {

            var maBacSi = fc1["bacsi"];
            var bs = new ServiceReference1.bacSiEntity();

            if (ModelState.IsValid)
            {
                int id;
                id = int.Parse(maBacSi);
                var bs1 = ur.GetBacSiById(id);
                bs.maBacSi = bs1.MaBacSi;
                bs.tenBacSi = bs1.TenBacSi;
                bs.trinhDo = bs1.TrinhDo;
            }
            return Json(bs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DsPhieuKham()
        {
            var lt = ur.GetAllPhieuKham().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DsUser()
        {
            var lt = ur.GetAllUser().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DsBacSi()
        {
            var lt = ur.GetAllBacSi().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
        public int kiemtra(int id)
        {

            var kq = ur.kiemtratrongngay(id);
            return kq;
        }
    }
}