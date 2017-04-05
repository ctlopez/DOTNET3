using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAppDataAccess;
using EventAppDataObjects;

namespace EventAppLogicLayer
{
    public class RoomManager
    {
        public bool CheckIfPurchasedAlready(string RoomID, int EventID)
        {
            // see if the event is already associated with this room
            bool purchased = false;

            try
            {
                if (RoomAccessor.CheckIfAlreadyHaveTicket(RoomID, EventID) == 1)
                {
                    purchased = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return purchased;
        }

        public int PurchaseMoreTickets(string roomID, int eventID, int moreTickets)
        {
            // if the room already has tickets for this event, add more to it
            int rows = 0;
            int currentTicketAmount = 0;

            try
            {
                // get the current amount of tickets there are
                currentTicketAmount = RoomAccessor.FindNumberOfTickets(roomID, eventID);
            }
            catch (Exception)
            {

                throw;
            }

            // the new amount of tickets that needs to be stored
            int newTicketAmount = currentTicketAmount + moreTickets;

            try
            {
                rows = RoomAccessor.UpdateRoomEventWithTicket(roomID, eventID, currentTicketAmount, newTicketAmount);
            }
            catch (Exception)
            {

                throw;
            }


            return rows;
        }

        public int PurchaseTickets(string roomID, int eventID, int tickets)
        {
            // use if the room doesn't already have tickets for the event
            int rows = 0;

            try
            {
                rows = RoomAccessor.InsertNewPurchase(roomID, eventID, tickets);
            }
            catch (Exception)
            {

                throw;
            }

            return rows;
        }

        public bool CheckRoomNumberLength(string roomNumber)
        {
            // just see if the user entered a room ID that is 3 characters
            bool good = false;

            if (roomNumber.Length == 3)
            {
                good = true;
            }

            return good;
        }

        public List<RoomEvent> GetEventsForRoom(string roomID)
        {
            // return a list of rooms associated with an event
            List<RoomEvent> events = null;

            try
            {
                events = EventAccessor.RetrieveEventsByRoomID(roomID);
            }
            catch (Exception)
            {

                throw;
            }

            // get the total cost calculated here. It is not stored.
            foreach (RoomEvent re in events)
            {
                re.TotalPrice = re.TicketsReserved * re.IndividualPrice;
            }

            return events;
        }

        public bool UpdatePIN(string roomID, string oldPassword, string newPassword)
        {
            // Update a guest's PIN for their room
            var result = false;

            try
            {
                if (1 == RoomAccessor.UpdatePIN(roomID, UserManager.HashSHA256(oldPassword), UserManager.HashSHA256(newPassword)))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public bool RemoveReservedTickets(Guest guest, Event selectedEvent)
        {
            // Cancel tickets for the event
            bool isValid = false;

            try
            {
                if (1 == RoomAccessor.DeleteRoomEvent(guest.RoomID, selectedEvent.EventID))
                {
                    isValid = true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return isValid;
        }

        public bool CheckDefault(string roomID, string password)
        {
            // See if user entered in default values for login. This will allow 
            // employees to see event details without having a guest's
            // login credentials. Because there is no way to modify the DB this way,
            // it is not necessary to protect the information.
            bool isValid = false;

            if (roomID == "000" && password == "0000")
            {
                isValid = true;
            }

            return isValid;
        }

    }
}
