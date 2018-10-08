using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;

namespace MvcApp.Controllers
{
    public class BacSiController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
        // GET: BacSi
        public ActionResult Index()
        {
            ServiceReference1.BacSi bs= (ServiceReference1.BacSi)Session["user"];
            ViewBag.BenhNhan = ur.GetAllUser().ToList();
            ViewBag.BacSi = ur.GetAllBacSi().ToList();
           
            var lk = ur.DSkham(bs.MaBacSi).Where(x => x.NgayKham == DateTime.Today);
            ViewBag.mabs = bs.MaBacSi;
            return View(lk);            
        }
        public ActionResult DSBenhNhanDaKham()
        {
            ServiceReference1.BacSi bs = (ServiceReference1.BacSi)Session["user"];
            ViewBag.BenhNhan = ur.GetAllUser().ToList();
            ViewBag.BacSi = ur.GetAllBacSi().ToList();

            var lk = ur.DSkham(bs.MaBacSi).Where(x => x.NgayKham == DateTime.Today);

            return View(lk);
        }
        public ActionResult dsthuocjson()
        {
            var lt = ur.dsthuoc().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult kham(int id)
        {
            ViewBag.trung = TempData["trung"];
            var p = ur.GetAllUserById((Int32)ur.getphieukham(id).MaBenhNhan);
            ViewBag.bn = p;
            return View();
        }
        [HttpPost]
        public ActionResult kham(int id, string [] dsthuoc, string [] soluong,string []lieudung, string ketquakham)
        {
            
            ServiceReference1.toathuocEntity t = new ServiceReference1.toathuocEntity();

            t.KetQuaKhamBenh = ketquakham;
            t.MaPhieuKham = id;
            ur.AddToaThuoc(t);
            var a =  ur.getthelasttoa();
            if (dsthuoc == null)
            {
                ur.trangthaiphieu(id);
                return RedirectToAction("Index");
            }
            for (int i= 0;i<dsthuoc.Length;i++)
            {
                for(int j =0;j<dsthuoc.Length;j++)
                {
                    if(dsthuoc[i] == dsthuoc[j] && i!=j)
                    {
                        TempData["trung"] = "Thêm không đcược vì trùng mã thuốc" ;
                        return RedirectToAction("kham/"+id.ToString());
                    }
                }
                ViewBag.trung = "";
                ServiceReference1.chitiettoathuocEntity ct = new ServiceReference1.chitiettoathuocEntity();
                ct.LieuDung = lieudung[i];
                ct.MaThuoc = dsthuoc[i];
                ct.SoLuong = float.Parse( soluong[i]);
                ct.MaToaThuoc =(Int32) a;
                ur.AddChiTietToaThuoc(ct);
                
            }
            ur.trangthaiphieu(id);
            return RedirectToAction("Index");
        }
        
        public ActionResult boqua(int id)
        {
            ur.boquaphieu((Int32)id);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
            var a = ur.getthelasttoabyid(id);
            
            ServiceReference1.ToaThuoc l = ur.chitiettoa(a);
            ViewBag.bacsi = ur.getbacsi((Int32)ur.getphieukham((Int32)l.MaPhieuKham).MaBacSi);
            ViewBag.benhnhan = ur.GetAllUserById((Int32)ur.getphieukham((Int32)l.MaPhieuKham).MaBenhNhan);
            ViewBag.chitiet = ur.getallchitiettoathuocbytoa(l.MaToaThuoc).ToList();
            ViewBag.toa = l;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, string[] dsthuoc, string[] soluong, string[] lieudung, string ketquakham)
        {
            ServiceReference1.toathuocEntity t = new ServiceReference1.toathuocEntity();
            var a = ur.getthelasttoabyid(id);
            t.MaToaThuoc = a;
            t.KetQuaKhamBenh = ketquakham;
            if (ModelState.IsValid)
            {
                ur.Updatetoathuoc(t);
                ur.Deletechitiettoathuoc(a);
                if(dsthuoc == null)
                {
                    return RedirectToAction("Index");
                }
                for (int i = 0; i < dsthuoc.Length; i++)
                {
                    ServiceReference1.chitiettoathuocEntity ct = new ServiceReference1.chitiettoathuocEntity();
                    ct.LieuDung = lieudung[i];
                    ct.MaThuoc = dsthuoc[i];
                    ct.SoLuong = float.Parse(soluong[i]);
                    ct.MaToaThuoc = (Int32)a;
                    ur.AddChiTietToaThuoc(ct);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        //Xem lịch xử khám bệnh
       public ActionResult DsPhieuKham()
        {
            var lt = ur.GetAllPhieuKham().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DsPhieuKhambn(int id)
        {

            ViewBag.phieukhambn = ur.GetAllPhieuKham().ToList();
            ViewBag.bn = ur.GetAllUser().ToList();
            ViewBag.bs = ur.GetAllBacSi().ToList();
            ViewBag.ToaThuoc = ur.GetAllToaThuoc().ToList();
            ViewBag.cttt = ur.getallchitiettoathuoc().ToList();
            ViewBag.thuoc = ur.dsthuoc().ToList();
            List<ServiceReference1.thuocEntity> thuoc = new List<ServiceReference1.thuocEntity>();
            var pk = ur.GetAllPhieuKham().ToList();
            var lstToaThuoc = ur.GetAllToaThuoc().ToList();           
            var lstCTToaThuoc = ur.getallchitiettoathuoc().ToList();
            var lstThuoc = ur.dsthuoc().ToList();
            foreach (var item in pk)
            {
                if (item.MaBenhNhan == id)
                {

               
                foreach (var item1 in lstToaThuoc)
                {
                        if (item1.MaPhieuKham == item.MaPhieuKham)
                        {
                            foreach (var item3 in lstCTToaThuoc)
                            {
                                if (item3.MaToaThuoc == item1.MaToaThuoc)
                                {
                                    ViewBag.cttoathuoc1 = ur.getallchitiettoathuocbytoa(item3.MaToaThuoc).ToList();
                                    foreach (var item2 in lstThuoc)
                                    {
                                        if (item3.MaThuoc == item2.MaThuoc)
                                        {
                                            ServiceReference1.thuocEntity t = new ServiceReference1.thuocEntity();
                                            t.maThuoc = item2.MaThuoc;
                                            t.tenThuoc = item2.TenThuoc;
                                            t.donViTinh = item2.DonVitinh;
                                            t.nhaSanXuat = item2.NhaSanXuat;
                                            t.ngaySanXuat = (DateTime)item2.NgaySanXuat;
                                            t.soLuongThuoc = (int)item2.SoLuongThuoc;
                                            t.giaThuoc = (int)item2.GiaThuoc;
                                            t.hanSuDung = item2.HanSuDung;
                                            thuoc.Add(t);
                                        }
                                    }
                                }
                            }

                        }
                       
                    }
                }
            }            
            ViewBag.thuocct = thuoc;
            var a = ur.GetAllUserById(id);
            return View(a);
        }
        [HttpPost]
        public ActionResult DsPhieuKhambn(int id,benhnhanEntity bn)
        {
           
            return View();
        }
        public ActionResult DsUser()
        {
            var lt = ur.GetAllUser().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DsToaThuoc()
        {
            var lstToaThuoc = ur.GetAllToaThuoc().ToList();
            return Json(lstToaThuoc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChiTietToaThuoc(int id)
        {
            var lstThuoc = ur.getallchitiettoathuocbytoa(id).ToList();
            // ViewBag.thuocct = thuoc;
            return Json(lstThuoc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DsThuoc()
        {
            var lstThuoc = ur.dsthuoc().ToList();
            return Json(lstThuoc, JsonRequestBehavior.AllowGet);
        }
        public int kiemtra(int id)
        {

            var kq = ur.KiemTraLichSu(id);
            return kq;

        }
      
    }

}