using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarFuel.DataAccess;
using CarFuel.Models;
using CarFuel.Service;
using Microsoft.AspNet.Identity;

namespace CarFuel.Controllers
{
    public class CarController : Controller
    {
        private ICarDb db;
        private CarService carService;

        public CarController()
        {
            db = new CarDb();
            carService = new CarService(db);
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = new Guid(User.Identity.GetUserId());
            IEnumerable<Car> cars = carService.GetCarsByMember(userId);
            return View(cars);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(Car item)
        {
            var userId = new Guid(User.Identity.GetUserId());
            try
            {
                carService.AddCar(item, userId);
            }
            catch (OverQuotaException ex)
            {
                TempData["error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid id)
        {
            var userId = new Guid(User.Identity.GetUserId());
            var c = carService.GetCarsByMember(userId).SingleOrDefault(x => x.Id == id);

            return View(c);
        }
    }
}