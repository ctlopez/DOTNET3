using EventAppDataAccess;
using EventAppDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppLogicLayer
{
    public class GuestManager
    {
        

        public Guest VerifyGuest(string roomID, string PIN)
        {
            // check if the guest entered a proper room number and PIN
            Guest guest = null;


            try
            {
                if (1 == RoomAccessor.VerifyRoomAndPIN(roomID, UserManager.HashSHA256(PIN)))
                {
                    // the login was successfull
                    PIN = null;

                    guest = GuestAccessor.RetrieveGuestByRoomID(roomID);

                    if (null == guest)
                    {
                        throw new ApplicationException("Could not find guest associated with that room!");
                    }

                }
                else
                {
                    throw new ApplicationException("Incorrect Login, please try again.");
                }

            }
            catch (Exception)
            {
                
                throw;
            }

            return guest;
        }

        public List<Guest> GetGuests()
        {
            // Generate a list of guests in the hotel for the clerk
            List<Guest> guests;

            try
            {
                guests = GuestAccessor.RetriveAllGuests();
            }
            catch (Exception)
            {

                throw;
            }

            return guests;
        }

        public bool VerifyPIN(Guest guest, string PIN)
        {
            // lets the user check the guest PIN without returning a Guest
            bool isValid = false;

            try
            {
                if(1 == RoomAccessor.VerifyRoomAndPIN(guest.RoomID, UserManager.HashSHA256(PIN)))
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

        public Guest GetGuestByRoomID(string roomID)
        {
            // Get a specific guest by the room
            Guest guest = null;

            try
            {
                guest = GuestAccessor.RetrieveGuestByRoomID(roomID);
            }
            catch (Exception)
            {

                throw;
            }

            return guest;
        }

    }
}
