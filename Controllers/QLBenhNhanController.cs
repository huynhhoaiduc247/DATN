using ClosedXML.Excel;
using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class QLBenhNhanController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();

        // GET: Home
        public ActionResult Index()
        {
            var lst = ur.GetAllUser();
            return View(lst);
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
            return View();
        }
        [HttpPost]
        public ActionResult Create(benhnhanEntity a)
        {
            var ax = new ServiceReference1.benhnhanEntity();
            ax.mabenhnhan = a.mabenhnhan;
            ax.HoTenBenhNhan = a.HoTenBenhNhan;
            ax.SDT = a.SDT;
            
            ax.Tuoi = a.Tuoi;
            ax.DiaChi = a.DiaChi;
            if (ModelState.IsValid)
            {
                ur.AddUser(ax);
                return RedirectToAction("Index");
            }
            return View(a);
        }

        public ActionResult Edit(int id)
        {
            var a = ur.GetAllUserById(id);
            return View(a);
        }
        [HttpPost]
        public ActionResult Edit(int id, benhnhanEntity a)
        {
            var ax = ur.GetAllUserById(id);
            var axx = new ServiceReference1.benhnhanEntity();
            axx.mabenhnhan = a.mabenhnhan;
            axx.HoTenBenhNhan = a.HoTenBenhNhan;
            axx.SDT = a.SDT;
           
            axx.Tuoi = (int)a.Tuoi;
            axx.DiaChi = a.DiaChi;
            if (ModelState.IsValid)
            {
                ur.UpdateUser(axx);
                return RedirectToAction("Index");
            }
            return View(ax);
        }
        public ActionResult XuatBenhNhanExcel()
        {

            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                    new DataColumn("Mã Bệnh nhân", typeof(string)),
                new DataColumn("Tên Bệnh Nhân", typeof(string)),
                 new DataColumn("Địa Chỉ",typeof(string)),
            new DataColumn("Tuổi",typeof(string)),
            new DataColumn("Số điện thoại",typeof(string)),
          

            });


            var lstuser = ur.GetAllUser().ToList();

            foreach (var item in lstuser)
            {
                dt.Rows.Add(item.MaBenhNhan, item.HoTenBenhNhan, item.DiaChi, item.Tuoi, item.SDT);
            }
            //Exporting to Excel
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Benhnhan");

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