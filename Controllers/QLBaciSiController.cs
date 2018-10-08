using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class QLBaciSiController : Controller
    {
        ServiceReference1.MyServiceClient ur = new ServiceReference1.MyServiceClient();
        // GET: QLBaciSi
        public ActionResult Index()
        {
            ViewBag.khoa = ur.getallkhoa().ToList();
            var l = ur.GetAllBacSi().ToList();
            return View(l);
        }

        public ActionResult Edit(int id)
        {
            var bs = ur.getbacsi(id);
            return View(bs);
        }

        [HttpPost]
        public ActionResult Edit(int id, ServiceReference1.bacSiEntity bs, string dskhoa)
        {
            bs.makhoa = dskhoa;
            if(ModelState.IsValid)
            {
                ur.updatebacsi(bs);
                return RedirectToAction("Index");
            }
            return View(bs);
        }
        public ActionResult dstkhoa()
        {
            var lt = ur.getallkhoa().ToList();
            return Json(lt, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(ServiceReference1.bacSiEntity bs, string dskhoa)
        {
            bs.makhoa = dskhoa;
            if (ModelState.IsValid)
            {
                ur.addBacSi(bs);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            ur.Deletebacsi(id);
            return RedirectToAction("Index");
        }

    }
}