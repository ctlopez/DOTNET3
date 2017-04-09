using System;
using EventAppDataObjects;
namespace EventAppLogicLayer
{
    public interface IEventManager
    {
        bool AddNewEvent(EventAppDataObjects.Event ev);
        int CalculateAvailableTickets(EventAppDataObjects.Event selectedEvent);
        int ClearOldEvents();
        bool EditEvent(EventAppDataObjects.Event oldEvent, EventAppDataObjects.Event newEvent);
        int EventActive(bool isActive);
        System.Collections.Generic.List<EventAppDataObjects.Event> GetActiveEvents();
        System.Collections.Generic.List<EventAppDataObjects.Event> GetAllEvents();
        EventAppDataObjects.Event GetEventByID(int eventID);
        System.Collections.Generic.List<EventAppDataObjects.Event> GetEventsByDate(string date);
        System.Collections.Generic.List<EventAppDataObjects.Event> GetInactiveEvents();
        System.Collections.Generic.List<EventAppDataObjects.Event> GetUpcomingEvents(bool active = true);
        EventAppDataObjects.Event MakeEvent(string name, string description, string date, string time, string location, int maxSeats, decimal price, int addedBy, bool active = true);
        string MakeFullTime(string hours, string minutes, string period);
        string PullHoursFromFull(string time);
        string PullMinutesFromFull(string time);
        string PullPeriodFromFull(string time);
        bool ValidData(string name, string description, string hours, string minutes, string period, string date, string location, string price, string maxSeats, string active);
        EventWithEmployee GetEventWithEmployeeById(int eventId);
        bool DeactivateEventById(int eventId);
    }
}
