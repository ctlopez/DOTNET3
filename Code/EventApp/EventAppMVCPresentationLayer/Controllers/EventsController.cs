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
    public class EventsController : Controller
    {
        private IEventManager _eventManager;
        private IGuestManager _guestManager;

        public EventsController(IEventManager eventManager, IGuestManager guestManager)
        {
            //_eventManager = new EventManager();
            _eventManager = eventManager;
            _guestManager = guestManager;
        }

        // GET: Events
        public ActionResult Index()
        {
            _eventManager.ClearOldEvents();
            if (User != null)
            {
                if (User.IsInRole("Manager"))
                {
                    return View(_eventManager.GetAllEvents());
                }
                else
                {
                    return View(_eventManager.GetActiveEvents());
                }
                
            }
            else
            {
                return View(_eventManager.GetUpcomingEvents());
            }
            
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventWithEmployee @event = _eventManager.GetEventWithEmployeeById((int)id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            if (User != null)
            {
                if (User.IsInRole("Guest"))
                {
                    //Guest current = null;
                    //try
                    //{
                    //    current = _guestManager.GetGuestByRoomID(User.Identity.Name);
                    //}
                    //catch (Exception)
                    //{
                        
                    //    throw;
                    //}
                    PurchaseModel purchaseModel = new PurchaseModel()
                    {
                        Name = @event.Name,
                        EventID = @event.EventID,
                        Date = @event.Date,
                        Active = @event.Active,
                        AddedBy = @event.AddedBy,
                        Description = @event.Description,
                        EmployeeCreater = @event.EmployeeCreater,
                        Location = @event.Location,
                        MaxSeats = @event.MaxSeats,
                        Price = @event.Price,
                        Quantity = 0,
                        Time = @event.Time,
                        RoomId = User.Identity.Name
                    };
                    return View("DetailsWithPurchase", purchaseModel);
                }
                return View(@event);
            }
            else
            {
                return View(@event);
            }
            
        }

        [HttpPost, ActionName("Details")]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles="Guest")]
        public ActionResult DetailsWithPurchase(int id, int quantity)
        {

            if (ModelState.IsValid)
            {
                EventAppDataObjects.Event @event = null;
                try
                {
                    @event = _eventManager.GetEventByID(id);
                }
                catch (Exception)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                }
            }
            return View();
        }

        [Authorize(Roles="Manager")]
        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Events/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles="Manager")]
        //public ActionResult Create([Bind(Include = "EventID,Name,Description,Date,Time,Location,MaxSeats,Price,AddedBy,Active")] Event @event)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Events.Add(@event);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(@event);
        //}

        [Authorize(Roles = "Manager")]
        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = _eventManager.GetEventByID((int)id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit([Bind(Include = "EventID,Name,Description,Date,Time,Location,MaxSeats,Price,Active")] Event newEvent)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(@event).State = EntityState.Modified;
                //db.SaveChanges();
                
                try
                {
                    var oldEvent = _eventManager.GetEventByID(newEvent.EventID);
                    //Change with a user
                    newEvent.AddedBy = oldEvent.AddedBy;
                    if (_eventManager.EditEvent(oldEvent, newEvent))
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                    }
                }
                catch (Exception)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                }
                
            }
            return View(newEvent);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventWithEmployee @event = _eventManager.GetEventWithEmployeeById((int)id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Event @event = db.Events.Find(id);
            //db.Events.Remove(@event);
            //db.SaveChanges();
            try
            {
                if (_eventManager.DeactivateEventById(id))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                }
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }
            
        }

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
