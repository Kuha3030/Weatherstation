using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Sasema_test.Models;
using MVC_Sasema_test.Models3;

namespace MVC_Sasema_test.Controllers
{
    public class weatherdataController : Controller
    {
        private weatherstationEntities  db = new weatherstationEntities();

        // GET: weatherdata
        public ActionResult Index()
        {
            var dataAsia = db.data;
            return View(dataAsia.ToList());
        }

        public ActionResult Results()
        {
            var resultsFromDB = db.data;

            return View(resultsFromDB);
        }

        // GET: weatherdata/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: weatherdata/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: weatherdata/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: weatherdata/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: weatherdata/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: weatherdata/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: weatherdata/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }

    public class CopyOfweatherdataController : Controller
    {
        private weatherstationEntities db = new weatherstationEntities();

        // GET: weatherdata
        public ActionResult Index()
        {
            var dataAsia = db.data;
            return View(dataAsia.ToList());
        }

        public ActionResult Results()
        {
            var resultsFromDB = db.data;

            return View(resultsFromDB);
        }

        // GET: weatherdata/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: weatherdata/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: weatherdata/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: weatherdata/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: weatherdata/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: weatherdata/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: weatherdata/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
