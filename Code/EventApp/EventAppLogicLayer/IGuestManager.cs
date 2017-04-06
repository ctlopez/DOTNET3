using System;
namespace EventAppLogicLayer
{
    public interface IGuestManager
    {
        EventAppDataObjects.Guest GetGuestByRoomID(string roomID);
        System.Collections.Generic.List<EventAppDataObjects.Guest> GetGuests();
        EventAppDataObjects.Guest VerifyGuest(string roomID, string PIN);
        bool VerifyPIN(EventAppDataObjects.Guest guest, string PIN);
    }
}
