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

        public EventsController(IEventManager eventManager)
        {
            //_eventManager = new EventManager();//Will change once we get Ninject involved
            _eventManager = eventManager;
        }

        // GET: Events
        public ActionResult Index()
        {
            _eventManager.ClearOldEvents();
            return View(_eventManager.GetUpcomingEvents());
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
            return View(@event);
        }

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
