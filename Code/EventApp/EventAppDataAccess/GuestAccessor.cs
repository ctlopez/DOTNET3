using EventAppDataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataAccess
{
    public class GuestAccessor
    {
        public static Guest RetrieveGuestByRoomID(string RoomID)
        {
            // Get a guest associated with a room when he/she logs in.
            Guest guest = null;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_guest_by_roomID";

            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"RoomID", SqlDbType.Char, 3);

            cmd.Parameters[@"RoomID"].Value = RoomID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    
                    guest = new Guest()
                    {
                        GuestID = reader.GetInt32(0),
                        RoomID = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Phone = reader.GetString(4),
                        Email = reader.GetString(5),
                        Active = reader.GetBoolean(6)
                    };

                    
                }

                
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }


            return guest;
        }

        public static List<Guest> RetriveAllGuests()
        {
            // Get all guests for a Clerk
            List<Guest> guests = new List<Guest>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_all_guests";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Guest guest = new Guest()
                        {
                            GuestID = reader.GetInt32(0),
                            RoomID = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            Phone = reader.GetString(4),
                            Email = reader.GetString(5),
                            Active = reader.GetBoolean(6)
                        };
                        guests.Add(guest);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw new ApplicationException("Unable to get guest list");
            }
            finally
            {
                conn.Close();
            }

            return guests;
        }
    }
}
