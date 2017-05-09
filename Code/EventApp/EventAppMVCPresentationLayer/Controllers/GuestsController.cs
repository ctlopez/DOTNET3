using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EventAppDataObjects;
using EventAppMVCPresentationLayer.Models;
using EventAppLogicLayer;

namespace EventAppMVCPresentationLayer.Controllers
{
    public class GuestsController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private IGuestManager _guestManager;

        public GuestsController(IGuestManager guestManager)
        {
            _guestManager = guestManager;
        }

        // GET: Guests
        [Authorize(Roles="Clerk")]
        public ActionResult Index()
        {
            List<Guest> guests = null;
            try
            {
                guests = _guestManager.GetGuests();
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }

            if (null == guests)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(guests);
        }

        // GET: Guests/Details/5
        [Authorize(Roles="Clerk")]
        public ActionResult Details(string id)
        {
            if (id == null || id.Equals(""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = null;
            try
            {
                guest = _guestManager.GetGuestByRoomID(id);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }
            if (guest == null)
            {
                return HttpNotFound();
            }

            return View(guest);
        }

        public PartialViewResult FullList()
        {
            IEnumerable<Guest> fullGuestList = new List<Guest>();
            try
            {
                fullGuestList = _guestManager.GetGuests();
            }
            catch (Exception)
            {


            }

            return PartialView("GuestListPartial", fullGuestList);
        }

        public PartialViewResult FirstFloor()
        {
            IEnumerable<Guest> fullGuestList = new List<Guest>();
            try
            {
                fullGuestList = _guestManager.GetGuests();
            }
            catch (Exception)
            {


            }
            List<Guest> firstFloorGuestList = new List<Guest>();

            foreach (Guest g in fullGuestList)
            {
                try
                {
                    int roomNum = Int32.Parse(g.RoomID);
                    if (roomNum >= 100 && roomNum <= 120)
                    {
                        firstFloorGuestList.Add(g);
                    }
                }
                catch (Exception)
                {
                    
                    
                }
            }

            return PartialView("GuestListPartial", firstFloorGuestList);
        }

        public PartialViewResult SecondFloor()
        {
            IEnumerable<Guest> fullGuestList = new List<Guest>();
            try
            {
                fullGuestList = _guestManager.GetGuests();
            }
            catch (Exception)
            {


            }
            List<Guest> secondFloorGuestList = new List<Guest>();

            foreach (Guest g in fullGuestList)
            {
                try
                {
                    int roomNum = Int32.Parse(g.RoomID);
                    if (roomNum >= 200 && roomNum <= 220)
                    {
                        secondFloorGuestList.Add(g);
                    }
                }
                catch (Exception)
                {


                }
            }

            return PartialView("GuestListPartial", secondFloorGuestList);
        }

        public PartialViewResult ThirdFloor()
        {
            IEnumerable<Guest> fullGuestList = new List<Guest>();
            try
            {
                fullGuestList = _guestManager.GetGuests();
            }
            catch (Exception)
            {


            }
            List<Guest> thirdFloorGuestList = new List<Guest>();

            foreach (Guest g in fullGuestList)
            {
                try
                {
                    int roomNum = Int32.Parse(g.RoomID);
                    if (roomNum >= 300 && roomNum <= 320)
                    {
                        thirdFloorGuestList.Add(g);
                    }
                }
                catch (Exception)
                {


                }
            }

            return PartialView("GuestListPartial", thirdFloorGuestList);
        }

        //// GET: Guests/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Guests/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "GuestID,RoomID,FirstName,LastName,Phone,Email,Active")] Guest guest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Guests.Add(guest);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(guest);
        //}

        //// GET: Guests/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Guest guest = db.Guests.Find(id);
        //    if (guest == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(guest);
        //}

        //// POST: Guests/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "GuestID,RoomID,FirstName,LastName,Phone,Email,Active")] Guest guest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(guest).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(guest);
        //}

        //// GET: Guests/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Guest guest = db.Guests.Find(id);
        //    if (guest == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(guest);
        //}

        //// POST: Guests/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Guest guest = db.Guests.Find(id);
        //    db.Guests.Remove(guest);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
