using EventAppDataAccess;
using EventAppDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppLogicLayer
{
    public class EventManager : IEventManager
    {



        public int ClearOldEvents()
        {
            // deactivate any events in the past.
            int rows = 0;

            int today = DateTime.Today.Day; // current day
            int month = DateTime.Today.Month; // current month
            int year = DateTime.Today.Year; // current year

            // format it for Sql
            string currentDate = month + "-" + today + "-" + year;

            try
            {
                EventAccessor.DeactivateOldEvents(currentDate);
            }
            catch (Exception)
            {

                throw;
            }

            return rows;
        }

        public List<Event> GetUpcomingEvents(bool active = true)
        {
            List<Event> events = null;

            // limit upcoming events to those within the next 10 days.
            DateTime limit = DateTime.Today.AddDays(10.0);
            int limitDay = limit.Day;
            int limitMonth = limit.Month;
            int limitYear = limit.Year;

            // make sure it is formatted for Sql
            string limitDate = limitMonth + "-" + limitDay + "-" + limitYear;

            try
            {
                events = EventAccessor.RetrieveUpcomingEvents(limitDate, active);
                formatTimeForEvents(events);
            }
            catch (Exception)
            {

                throw;
            }

            return events;
        }

        private List<Event> formatTimeForEvents(List<Event> unformattedEvents)
        {
            // This calls the calculateTime helper method on each event in a list
            // to make it easier for humans to read.
            List<Event> events = unformattedEvents;

            if (null != events)
            {
                foreach (Event e in events)
                {
                    try
                    {
                        calculateTime(e);
                    }
                    catch (Exception ex)
                    {

                        throw new ApplicationException("Error formatting data." + ex);
                    }
                }
            }

            return events;
        }

        private void formatTimeForEvent(Event unformattedEvent)
        {
            // This calls the calculateTime helper method on one particular event
            // to make it easier for humans to read.
            try
            {
                calculateTime(unformattedEvent);
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error formatting data." + ex);
            }
        }

        private void calculateTime(Event e)
        {
            // Reformats Sql's time (stored as integers for minutes past midnight)
            // for human readability.
            int minutes = Int32.Parse(e.Time);
            string period;
            if (minutes >= 720) // 720 minutes is 12 Noon
            {
                period = "PM";
            }
            else
            {
                period = "AM";
            }

            int hours = minutes / 60; // integer division ignores remainder
            minutes -= (hours * 60); // get the minutes left after the hour
            string displayMinutes = minutes.ToString();
            if (minutes < 10)
            {
                displayMinutes = "0" + minutes; // force two character minutes
            }
            if (hours > 12)
            {
                hours %= 12; // 12 hr format, not 24
            }
            if (hours == 0) // Midnight will yeild hours of 0, need to change to 12
            {
                hours = 12;
            }

            // combine the separate parts
            e.Time = MakeFullTime(hours.ToString(), displayMinutes, period);
        }

        public List<Event> GetActiveEvents()
        {
            // Retrieves and returns a list of active events
            List<Event> events = null;

            try
            {
                events = EventAccessor.RetrieveEventsByStatus();
                formatTimeForEvents(events);
            }
            catch (Exception)
            {
                
                throw;
            }

            return events;
        }

        public List<Event> GetInactiveEvents()
        {
            // Retrieves and returns a list of inactive events
            List<Event> events = null;

            try
            {
                events = EventAccessor.RetrieveEventsByStatus(false);
                formatTimeForEvents(events);
            }
            catch (Exception)
            {

                throw;
            }

            return events;
        }

        public List<Event> GetAllEvents()
        {
            // Retrieves and returns a list of all events in DB
            List<Event> events = null;

            try
            {
                events = EventAccessor.RetrieveAllEvents();
                formatTimeForEvents(events);
            }
            catch (Exception)
            {

                throw;
            }

            return events;
        }

        public int CalculateAvailableTickets(Event selectedEvent)
        {
            // returns a count of available tickets
            int tickets = 0;

            int maxTickets = selectedEvent.MaxSeats;

            try
            {
                // check if there are any tickets reserved already
                if (0 != EventAccessor.RetrieveCountEventIDInRoomEvents(selectedEvent.EventID))
                {
                    // if there is, subtract that amount from the maximum amount
                    tickets = maxTickets - EventAccessor.RetrieveCountOfPurchasedTickets(selectedEvent.EventID);
                }
                else
                {
                    // none have been purchased yet, all the tickets remain
                    tickets = maxTickets;
                }
                
            }
            catch (Exception)
            {
                
                throw;
            }

            return tickets;
        }

        public bool AddNewEvent(Event ev)
        {
            // add a new event to the system.
            bool wasAdded = false;
            int time = stringTimeToInt(ev);

            try
            {
                // check if it actually was added
                if (1 == EventAccessor.InsertNewEvent(ev.Name, ev.Description, ev.Date, time, ev.Location, ev.MaxSeats, ev.Price, ev.AddedBy))
                {
                    wasAdded = true;
                }
            }
            catch (Exception)
            {
                
                throw;
            }

            return wasAdded;
        }

        private int stringTimeToInt(Event e)
        {
            // get the integer representation for minutes after midnight from the typical
            // 12 hour format. This is how it is stored in DB. Because this is used only
            // with existing, constrained data, no need to surround in try block.
            int minutesFromMidnight;
            string time = e.Time;
            int colonIndex = findColon(time); // this allows us to separate the hours and minutes

            //int hours = Int32.Parse(time.Substring(0, colonIndex));
            //int minutes = Int32.Parse(time.Substring(colonIndex + 1, 2));
            int hours = Int32.Parse(PullHoursFromFull(time));
            int minutes = Int32.Parse(PullMinutesFromFull(time));

            //string period = time.Substring(time.Length - 2);
            string period = PullPeriodFromFull(time);
            if (period == "PM" && hours != 12) // in afternoon, need to add 12
            {
                hours += 12;
            }
            if (period == "AM" && hours == 12) // midnight
            {
                hours = 0;
            }

            minutesFromMidnight = (hours * 60) + minutes;

            return minutesFromMidnight;

        }

        private static int findColon(string time)
        {
            return time.IndexOf(":");
        }

        public string PullHoursFromFull(string time)
        {
            int colonIndex = findColon(time);
            return (time.Substring(0, colonIndex));
        }

        public string PullMinutesFromFull(string time)
        {
            int colonIndex = findColon(time);
            return (time.Substring(colonIndex + 1, 2));
        }

        public string PullPeriodFromFull(string time)
        {
            return time.Substring(time.Length - 2);
        }

        public int EventActive(bool isActive)
        {
            return isActive ? 0 : 1;
        }

        public bool ValidData(string name, string description, string hours, string minutes, 
            string period, string date, string location, string price, string maxSeats, string active)
        {
            bool isValid = true;

            // Primarily just see if there is data present.
            if (name == "")
            {
                throw new ApplicationException("Invalid name");
            }
            if (description == "")
            {
                throw new ApplicationException("Invalid description");
            }
            if (date == "")
            {
                throw new ApplicationException("Invalid date");
            }
            if (location == "")
            {
                throw new ApplicationException("Invalid location");
            }
            if (price == "")
            {
                throw new ApplicationException("Invalid price");
            }
            try
            {
                Decimal.Parse(price); //see if it is a decimal.
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Invalid price, " + ex.Message);
            }
            if (maxSeats == "")
            {
                throw new ApplicationException("Invalid max seats");
            }
            try //see if it is an integer.
            {
                Int32.Parse(maxSeats);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Invalid max seats, " + ex.Message);
            }
            if (hours == "" || minutes == "" || period == "")
            {
                throw new ApplicationException("Invalid time");
            }
            if (active == "")
            {
                throw new ApplicationException("Invalid active field");
            }
            try // see if user tried to make an event active for today's date or prior.
            {
                //DateTime dateTime = DateTime.ParseExact(date, "mm/dd/yyyy", null);
                DateTime dateTime = DateTime.Parse(date);
                if ((DateTime.Compare(dateTime, DateTime.Now) <= 0) && active == "Yes")
                {
                    throw new ApplicationException("Cannot set an event as active on or prior to today's date.");
                }
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("Invalid Date: " + ex.Message);
            }

            return isValid;
        }

        public string MakeFullTime(string hours, string minutes, string period)
        {
            // combines the seperate parts
            return hours + ":" + minutes + " " + period;
        }

        public Event MakeEvent(string name, string description, string date, string time,
            string location, int maxSeats, decimal price, int addedBy, bool active = true)
        {
            // build an event for the presentaion layer
            return new Event()
            {
                Name = name,
                Description = description,
                Date = date,
                Time = time,
                Location = location,
                MaxSeats = maxSeats,
                Price = price,
                AddedBy = addedBy,
                Active = active
            };
        }

        public bool EditEvent(Event oldEvent, Event newEvent)
        {
            // update an event in the system
            bool edited = false;
            int oldTime = stringTimeToInt(oldEvent);
            int newTime = stringTimeToInt(newEvent);

            try
            {
                if (1 == EventAccessor.UpdateEvent(oldEvent.EventID, oldEvent.Name, oldEvent.Description, oldEvent.Date, oldTime, 
                    oldEvent.Location, oldEvent.MaxSeats, oldEvent.Price, oldEvent.AddedBy, oldEvent.Active, 
                    newEvent.Name, newEvent.Description, newEvent.Date, newTime, newEvent.Location, newEvent.MaxSeats,
                    newEvent.Price, newEvent.AddedBy, newEvent.Active))
                {
                    edited = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return edited;
        }

        public List<Event> GetEventsByDate(string date)
        {
            // Get events by a given date
            List<Event> events;

            try
            {
                events = EventAccessor.RetrieveEventsByDate(date);
            }
            catch (Exception)
            {

                throw;
            }

            return events;
        }

        public Event GetEventByID(int eventID)
        {
            // Find a specific event by the given ID
            Event selectedEvent = null;

            try
            {
                selectedEvent = EventAccessor.RetrieveEventByID(eventID);
            }
            catch (Exception)
            {

                throw;
            }

            formatTimeForEvent(selectedEvent);

            return selectedEvent;
        }

    }
}
