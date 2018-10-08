using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using MvcApp.ServiceReference1;

namespace MvcApp.Controllers
{
    public class QLThuocController : Controller
    {
        ServiceReference1.MyServiceClient sv = new ServiceReference1.MyServiceClient();
        // GET: QLThuoc
        public ActionResult Index()
        {
            var l = sv.dsthuoc();
            return View(l);
        }
        public ActionResult getthuocjson(string id)
        {
            var a = sv.getthuoctheoma(id);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(string id)
        {
            var a = sv.getthuoc(id);
            return View(a);
        }
        public ActionResult Delete(string id)
        {
            sv.DeleteThuoc(id);
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            ViewBag.listthuoc = sv.dsthuoc().ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string mathuoc, string tenthuoc, string donvitinh,int gia,string hansudung,string nhasanxuat,string ngaysanxuat,int soluong)
        {
            var ax = new ServiceReference1.thuocEntity();
            ax.maThuoc = mathuoc;
      
            ax.tenThuoc = tenthuoc;
            ax.donViTinh = donvitinh;
            ax.nhaSanXuat = nhasanxuat;
            ax.ngaySanXuat = DateTime.Parse(ngaysanxuat);
            ax.soLuongThuoc = soluong;
            ax.giaThuoc = gia;
            ax.hanSuDung = hansudung;
            if (ModelState.IsValid)
            {
                sv.AddThuoc(ax);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(string id)
        {
            ServiceReference1.Thuoc a = sv.getthuoc(id);
            ViewBag.thuoc = a;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string mathuoc, string tenthuoc, string donvitinh, int gia, string hansudung, string nhasanxuat, string ngaysanxuat, int soluong)
        {

            var ax = new ServiceReference1.thuocEntity();
            ax.maThuoc = mathuoc;
            ax.tenThuoc = tenthuoc;
            ax.donViTinh = donvitinh;
            ax.nhaSanXuat = nhasanxuat;
            ax.ngaySanXuat = DateTime.Parse(ngaysanxuat);
            ax.soLuongThuoc = soluong;
            ax.giaThuoc = gia;
            ax.hanSuDung = hansudung;
            if (ModelState.IsValid)
            {
                sv.UpdateThuoc(ax);
                return RedirectToAction("Index");
            }
            return View(ax);
        }
        public ActionResult XuatThuocExcel()
        {
           
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[8] {
                    new DataColumn("Mã thuốc", typeof(string)),
                new DataColumn("Tên thuốc", typeof(string)),
                 new DataColumn("Đơn Vị Tính",typeof(string)),
            new DataColumn("Giá Thuốc",typeof(string)),
            new DataColumn("Nhà sản Xuất",typeof(string)),
           new DataColumn("Ngày sản xuất",typeof(string)),
            new DataColumn("Số Lượng",typeof(string)),
           new DataColumn("Hạng Sử dụng",typeof(string)),

            });


            List<Thuoc> lstToaThuoc = sv.dsthuoc().ToList();
            
            foreach (var item in lstToaThuoc)
            {   
                dt.Rows.Add(item.MaThuoc, item.TenThuoc,item.DonVitinh,item.GiaThuoc,item.NhaSanXuat,item.NgaySanXuat,item.SoLuongThuoc,item.HanSuDung);
            }
            //Exporting to Excel
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "thuoc");

                //wb.SaveAs(folderPath + "DataGridViewExport.xlsx");
                string myName = Server.UrlEncode("Thuoc" + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
                MemoryStream stream = GetStream(wb);// The method is defined below
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + myName);
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }

            return RedirectToAction("Index");
        }
        public MemoryStream GetStream(XLWorkbook excelWorkbook)
        {
            MemoryStream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }
    }
}