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
        private IRoomManager _roomManager;
        private IUserManager _userManager;

        public EventsController(IEventManager eventManager, IGuestManager guestManager, IRoomManager roomManager, IUserManager userManager)
        {
            //_eventManager = new EventManager();
            _eventManager = eventManager;
            _guestManager = guestManager;
            _roomManager = roomManager;
            _userManager = userManager;
        }

        // GET: Events
        public ActionResult Index()
        {
            _eventManager.ClearOldEvents();
            if (Request.IsAuthenticated)
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

                    try
                    {
                        purchaseModel.CurrentAmount = _eventManager.CalculateAvailableTickets(@event);
                    }
                    catch (Exception)
                    {
                        
                        return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                    }

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
            if (quantity < 1 || quantity % 1 != 0)
            {
                ModelState.AddModelError("Quantity", "To purchase, please enter an integer one (1) or greater.");
            }
            EventAppDataObjects.Event @event = null;
            try
            {
                @event = _eventManager.GetEventByID(id);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }

            if (null == @event)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }

            if (quantity > _eventManager.CalculateAvailableTickets(@event))
            {
                ModelState.AddModelError("Quantity", "Cannot purchase more tickets than available.");
            }

            if (ModelState.IsValid)
            {
                
                Guest guest = null;
                try
                {
                    
                    guest = _guestManager.GetGuestByRoomID(User.Identity.Name);
                }
                catch (Exception)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                }

                if (null == @event || null == guest)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                //See if they already have tickets for this event. If so, add the new amount to it!
                if (_roomManager.CheckIfPurchasedAlready(guest.RoomID, @event.EventID))
                {
                    try
                    {
                        if (_roomManager.PurchaseMoreTickets(guest.RoomID, @event.EventID, quantity) == 1)
                        {
                            //MessageBox.Show(thanks);
                            //this.DialogResult = true;
                            //return View("Index", "Events");
                            //return RedirectToAction("Index");
                            return RedirectToAction("Index", "Manage", new { Message = ManageController.ManageMessageId.AddedEvent });
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                            //MessageBox.Show("There was a problem saving your purchase. Please try again later!");
                        }
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                        //MessageBox.Show("ERROR: " + ex.Message);
                    }
                }
                else // They don't already have tickets, so make a new record
                {
                    try
                    {
                        if (1 == _roomManager.PurchaseTickets(guest.RoomID, @event.EventID, quantity))
                        {
                            //MessageBox.Show(thanks);
                            //this.DialogResult = true;
                            //return Index();
                            //return RedirectToAction("Index");
                            return RedirectToAction("Index", "Manage", new { Message = ManageController.ManageMessageId.AddedEvent });
                            
                        }
                        else
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                        }
                    }
                    catch (Exception)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
                        //MessageBox.Show("ERROR: " + ex.Message);
                    }
                }

            }
            return Details(id);
        }

        [Authorize(Roles="Manager")]
        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Create([Bind(Include = "EventID,Name,Description,Date,Time,Location,MaxSeats,Price,AddedBy,Active")] Event @event)
        {
            if (ModelState.IsValid)
            {
                //db.Events.Add(@event);
                //db.SaveChanges();
                
                try
                {
                    Employee emp = _userManager.RetrieveEmployeeByUsername(User.Identity.Name);
                    @event.AddedBy = emp.EmployeeID;
                    _eventManager.AddNewEvent(@event);
                }
                catch (Exception)
                {
                    
                    throw;
                }

                return RedirectToAction("Index");
            }

            return View(@event);
        }

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

        public PartialViewResult EventsForRoom(string guestRoom)
        {
            List<RoomEvent> events = new List<RoomEvent>();
            try
            {
                events = _roomManager.GetEventsForRoom(guestRoom);
            }
            catch (Exception)
            {
                
                throw;
            }

            var eventsForAccount = new EventsForAccountViewModel()
            {
                RoomNumber = guestRoom,
                EventsWithRoom = events,
                TotalAmount = events.Select(e => e.TotalPrice).Sum()
            };
            return PartialView("~/Views/Manage/EventsForAccount.cshtml", eventsForAccount);
            
        }

        [Authorize(Roles="Guest,Clerk")]
        public ActionResult RemovePurchasedTickets(string guestRoom, int eventId)
        {
            Guest currentGuest;
            EventAppDataObjects.Event evnt;
            try
            {
                currentGuest = _guestManager.GetGuestByRoomID(guestRoom);
                evnt = _eventManager.GetEventByID(eventId);
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }

            if (null == currentGuest || null == evnt)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                if (!_roomManager.RemoveReservedTickets(currentGuest, evnt))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable);
            }

            if (User.IsInRole("Guest"))
            {
                return RedirectToAction("Index", "Manage", new { Message = ManageController.ManageMessageId.RemoveTickets });
            }
            else if (User.IsInRole("Clerk"))
            {
                return RedirectToAction("Details", "Guests", new { id = guestRoom });
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
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
