using System;
namespace EventAppLogicLayer
{
    public interface IRoomManager
    {
        bool CheckDefault(string roomID, string password);
        bool CheckIfPurchasedAlready(string RoomID, int EventID);
        bool CheckRoomNumberLength(string roomNumber);
        System.Collections.Generic.List<EventAppDataObjects.RoomEvent> GetEventsForRoom(string roomID);
        int PurchaseMoreTickets(string roomID, int eventID, int moreTickets);
        int PurchaseTickets(string roomID, int eventID, int tickets);
        bool RemoveReservedTickets(EventAppDataObjects.Guest guest, EventAppDataObjects.Event selectedEvent);
        bool UpdatePIN(string roomID, string oldPassword, string newPassword);
    }
}
