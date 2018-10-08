using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp.ServiceReference1;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace MvcApp.Controllers
{
    public class QLToaThuocController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
        // GET: QLToaThuoc
        public ActionResult Index()
        {
            ViewBag.benhnhan = ur.GetAllUser().ToList();
            ViewBag.phieu = ur.GetAllPhieu().ToList();
            var l = ur.GetAllToaThuoc();
            return View(l);
        }

        public ActionResult Edit(int id)
        {
            ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
            ToaThuoc l = ur.chitiettoa(id);
            ViewBag.phieu = ur.getphieukham((Int32)l.MaPhieuKham);
            ViewBag.bacsi = ur.getbacsi((Int32)ur.getphieukham((Int32)l.MaPhieuKham).MaBacSi);
            ViewBag.benhnhan = ur.GetAllUserById((Int32)ur.getphieukham((Int32)l.MaPhieuKham).MaBenhNhan);
            ViewBag.chitiet = ur.getallchitiettoathuocbytoa(l.MaToaThuoc).ToList();
            ViewBag.toa = l;
           
            return View(l);
        }
        [HttpPost]
        public ActionResult Edit(int id , string[] dsthuoc, string[] soluong, string[] lieudung, string ketquakham)
        {
            ServiceReference1.toathuocEntity t = new ServiceReference1.toathuocEntity();
            t.MaToaThuoc = id;
            t.KetQuaKhamBenh = ketquakham;
            if (ModelState.IsValid)
            {
                ur.Updatetoathuoc(t);
                ur.Deletechitiettoathuoc(id);
                if (dsthuoc == null)
                {
                    return RedirectToAction("Index");
                }
                for (int i = 0; i < dsthuoc.Length; i++)
                {
                    ServiceReference1.chitiettoathuocEntity ct = new ServiceReference1.chitiettoathuocEntity();
                    ct.LieuDung = lieudung[i];
                    ct.MaThuoc = dsthuoc[i];
                    ct.SoLuong = float.Parse(soluong[i]);
                    ct.MaToaThuoc = (Int32)id;
                    ur.AddChiTietToaThuoc(ct);
                }
                return RedirectToAction("Index");
            }
            return View();            
        }

        public ActionResult Delete(int id)
        {
            ur.DeleteToaThuoc(id);
            return RedirectToAction("Index");
        }
        public ActionResult XuatToaThuocExcel()
        {
            string t=null,i =null,h=null;
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[6] {
                    new DataColumn("Mã Toa thuốc", typeof(string)),
                new DataColumn("Mã Phiếu Khám", typeof(string)),
                 new DataColumn("Ngày Khám bệnh",typeof(string)),
            new DataColumn("Kết quả khám",typeof(string)),
            new DataColumn(" Mã Bác Sĩ khám",typeof(string)),
           new DataColumn("Trạng thái",typeof(string)),
            

            });

           
            List<ToaThuoc> lstToaThuoc = ur.GetAllToaThuoc().ToList();
            List<BenhNhan> lstBenhNhan = ur.GetAllUser().ToList();
            List<PhieuKham> lstPhieuKham = ur.GetAllPhieuKham().ToList();
            List<ChiTietToaThuoc> lstChiTiet = ur.getallchitiettoathuoc().ToList();
            foreach (var item in lstToaThuoc)
            {
                foreach (var it in lstPhieuKham)
                {
                    if (item.MaPhieuKham == it.MaPhieuKham)
                    {
                        t =it.NgayKham.Value.ToShortDateString();
                        i = it.MaBacSi.ToString();
                        h = it.TrangThai.ToString();
                    }
                    
                }
                


                dt.Rows.Add(item.MaToaThuoc, item.MaPhieuKham, t, item.KetQuaKhamBenh,i,h);
            }
            //Exporting to Excel
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "ToaThuoc");

                //wb.SaveAs(folderPath + "DataGridViewExport.xlsx");
                string myName = Server.UrlEncode("Test" + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
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