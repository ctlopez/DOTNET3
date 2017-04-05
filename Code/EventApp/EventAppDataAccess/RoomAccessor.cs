using EventAppDataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventAppDataAccess
{
    public class RoomAccessor
    {

        private static string errRoom = "Invalid Room Number";

        public static int VerifyRoomAndPIN(string RoomID, string PINHash)
        {
            // see if the user has entered correct credentials
            if (RoomID.Length != 3)
            {
                throw new ApplicationException(errRoom);
            }

            try
            {
                int roomNum = Int32.Parse(RoomID);
                if (roomNum < 100 || (roomNum > 120 && roomNum < 200) || (roomNum > 220 && roomNum < 300) || roomNum > 320)
                {
                    throw new ApplicationException(errRoom);
                }
            }
            catch (Exception)
            {

                throw new ApplicationException(errRoom);
            }

            var result = 0;

            // get a connection
            var conn = DBConnection.GetConnection();

            // get command text ** replace with stored procedure
            //var cmdText = @"SELECT COUNT(EmployeeID) " +
            //              @"FROM Employee " +
            //              @"WHERE Username = @Username COLLATE SQL_Latin1_General_CP1_CS_AS " +
            //              @"AND PasswordHash = @PasswordHash";
            var cmdText = @"sp_authenticate_room";

            // create a command object
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // add paramaters
            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@PINHash", SqlDbType.VarChar, 100);

            // set parameter values

            cmd.Parameters["@RoomID"].Value = RoomID;
            cmd.Parameters["@PINHash"].Value = PINHash;

            try
            {
                // open connection
                conn.Open();

                // execute and cast result
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static int CheckIfAlreadyHaveTicket(string RoomID, int EventID)
        {
            // See if a user already has a ticket to the event.
            int count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_lookup_room_event";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@EventID", SqlDbType.Int);

            cmd.Parameters["@RoomID"].Value = RoomID;
            cmd.Parameters["@EventID"].Value = EventID;

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                
                throw new ApplicationException("There was an error accessing the data!");
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public static int UpdateRoomEventWithTicket(string RoomID, int EventID, int OldAmount, int NewAmount)
        {
            // Used when a user already has tickets for an event purchased.
            // This adds new tickets to the stored amount.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_purchase_more_tickets";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@EventID", SqlDbType.Int);
            cmd.Parameters.Add("@OldAmount", SqlDbType.Int);
            cmd.Parameters.Add("@NewAmount", SqlDbType.Int);

            cmd.Parameters["@RoomID"].Value = RoomID;
            cmd.Parameters["@EventID"].Value = EventID;
            cmd.Parameters["@OldAmount"].Value = OldAmount;
            cmd.Parameters["@NewAmount"].Value = NewAmount;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw new ApplicationException("Could not store data.");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public static int FindNumberOfTickets(string roomID, int eventID)
        {
            // Get the amount of tickets purchased by room and event.
            int tickets = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_current_number_of_tickets";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@EventID", SqlDbType.Int);

            cmd.Parameters["@RoomID"].Value = roomID;
            cmd.Parameters["@EventID"].Value = eventID;

            try
            {
                conn.Open();
                tickets = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                
                throw new ApplicationException("Could not access data");
            }
            finally
            {
                conn.Close();
            }

            return tickets;
        }

        public static int InsertNewPurchase(string roomID, int eventID, int numberTickets)
        {
            // If a room does not have tickets for an event, insert a new record with
            // the necessary data.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_purchase_new_tickets";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@EventID", SqlDbType.Int);
            cmd.Parameters.Add("@NumberReserved", SqlDbType.Int);

            cmd.Parameters["@RoomID"].Value = roomID;
            cmd.Parameters["@EventID"].Value = eventID;
            cmd.Parameters["@NumberReserved"].Value = numberTickets;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw new ApplicationException("Unable to store data");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public static int UpdatePIN(string roomID, string oldPINHash, string newPINHash)
        {
            // Updates a user's PIN. Validaion in Logic Layer.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_update_room_pin";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@OldPinHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPinHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@RoomID"].Value = roomID;
            cmd.Parameters["@OldPinHash"].Value = oldPINHash;
            cmd.Parameters["@NewPinHash"].Value = newPINHash;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw new ApplicationException("Unable to save data.");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public static int DeleteRoomEvent(string roomID, int eventID)
        {
            // Actually remove the record in the DB if a guest wishes to remove
            // ticket reservations.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_delete_roomevent";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters.Add("@EventID", SqlDbType.Int);

            cmd.Parameters["@RoomID"].Value = roomID;
            cmd.Parameters["@EventID"].Value = eventID;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw new ApplicationException("Unable to remove data.");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

    }
}
