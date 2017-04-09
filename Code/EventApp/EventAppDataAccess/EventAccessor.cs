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
    public class EventAccessor
    {
        private const string noAccess = "Unable to access data";

        public static int DeactivateOldEvents(string currentDate)
        {
            //This looks through the data and deactivates any events that have a date
            // in the past.
            int count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_deactivate_old_events";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"CurrentDate", SqlDbType.Date);
            cmd.Parameters[@"CurrentDate"].Value = currentDate;

            try
            {
                conn.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public static List<Event> RetrieveUpcomingEvents(string limitDate, bool active = true)
        {
            // Because user story wants to display a list of events
            // even if no one is logged in.
            // Limit to 10 days in the future.
            var events = new List<Event>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_approaching_events";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            int activeBit = active ? 1 : 0;


            cmd.Parameters.Add(@"DateLimit", SqlDbType.Date);
            cmd.Parameters.Add(@"Active", SqlDbType.Bit);
            cmd.Parameters[@"DateLimit"].Value = limitDate;
            cmd.Parameters[@"Active"].Value = activeBit;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        var newEvent = new Event()
                        {
                            EventID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Date = reader.GetDateTime(3).ToShortDateString(),
                            Time = reader.GetInt32(4).ToString(),
                            Location = reader.GetString(5),
                            MaxSeats = reader.GetInt32(6),
                            Price = reader.GetDecimal(7),
                            AddedBy = reader.GetInt32(8),
                            Active = reader.GetBoolean(9)
                        };

                        events.Add(newEvent);
                    }
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("There was a problem retrieving the data.", ex);
            }
            finally
            {
                conn.Close();
            }


            return events;
        }

        public static List<Event> RetrieveEventsByStatus(bool active = true)
        {
            // Get a list of events based on whether or not they're active.
            // Primarily used when users log in.
            var events = new List<Event>();


            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_all_events";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            int activeBit = active ? 1 : 0;

            cmd.Parameters.Add(@"Active", SqlDbType.Bit);
            cmd.Parameters[@"Active"].Value = activeBit;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var newEvent = new Event()
                        {
                            EventID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Date = reader.GetDateTime(3).ToShortDateString(),
                            Time = reader.GetInt32(4).ToString(),
                            Location = reader.GetString(5),
                            MaxSeats = reader.GetInt32(6),
                            Price = reader.GetDecimal(7),
                            AddedBy = reader.GetInt32(8),
                            Active = reader.GetBoolean(9)
                        };

                        events.Add(newEvent);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                
                throw new ApplicationException("There was an error retrieving the data");
            }
            finally
            {
                conn.Close();
            }
            return events;
        }

        public static int RetrieveCountOfPurchasedTickets(int eventID)
        {
            // Used to determine if there are seats open for a certain event.
            int tickets = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_ticket_number";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(@"EventID", SqlDbType.Int);
            cmd.Parameters[@"EventID"].Value = eventID;

            try
            {
                conn.Open();
                tickets = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                
                throw new ApplicationException("Error finding Tickets!");
            }
            finally
            {
                conn.Close();
            }

            return tickets;
        }

        public static int InsertNewEvent(string name, string description, string date, int time, string location,
            int maxSeats, decimal price, int addedBy)
        {
            // Makes a new event. Validation in logic layer.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_insert_event";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@Date", SqlDbType.Date);
            cmd.Parameters.Add("@Time", SqlDbType.Int);
            cmd.Parameters.Add("@Location", SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@MaxSeats", SqlDbType.Int);
            cmd.Parameters.Add("@Price", SqlDbType.Decimal, 5);
            cmd.Parameters.Add("@AddedBy", SqlDbType.Int);

            cmd.Parameters["@Name"].Value = name;
            cmd.Parameters["@Description"].Value = description;
            cmd.Parameters["@Date"].Value = date;
            cmd.Parameters["@Time"].Value = time;
            cmd.Parameters["@Location"].Value = location;
            cmd.Parameters["@MaxSeats"].Value = maxSeats;
            cmd.Parameters["@Price"].Value = price;
            cmd.Parameters["@AddedBy"].Value = addedBy;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw new ApplicationException("There was a problem saving the new event!");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public static int UpdateEvent(int eventID, string oldName, string oldDescription, string oldDate,
            int oldTime, string oldLocation, int oldMaxSeats, decimal oldPrice, int oldAddedBy, bool oldActive,
            string newName, string newDescription, string newDate,
            int newTime, string newLocation, int newMaxSeats, decimal newPrice, int newAddedBy, bool newActive)
        {
            // Updates existing event. Validation in Logic Layer.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_update_event";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EventID", SqlDbType.Int);
            cmd.Parameters.Add("@OldName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldDescription", SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@OldDate", SqlDbType.Date);
            cmd.Parameters.Add("@OldTime", SqlDbType.Int);
            cmd.Parameters.Add("@OldLocation", SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@OldMaxSeats", SqlDbType.Int);
            cmd.Parameters.Add("@OldPrice", SqlDbType.Decimal, 5);
            cmd.Parameters.Add("@OldAddedBy", SqlDbType.Int);
            cmd.Parameters.Add("@OldActive", SqlDbType.Bit);
            
            cmd.Parameters.Add("@NewName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewDescription", SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@NewDate", SqlDbType.Date);
            cmd.Parameters.Add("@NewTime", SqlDbType.Int);
            cmd.Parameters.Add("@NewLocation", SqlDbType.NVarChar, 300);
            cmd.Parameters.Add("@NewMaxSeats", SqlDbType.Int);
            cmd.Parameters.Add("@NewPrice", SqlDbType.Decimal, 5);
            cmd.Parameters.Add("@NewAddedBy", SqlDbType.Int);
            cmd.Parameters.Add("@NewActive", SqlDbType.Bit);

            cmd.Parameters["@EventID"].Value = eventID;
            cmd.Parameters["@OldName"].Value = oldName;
            cmd.Parameters["@OldDescription"].Value = oldDescription;
            cmd.Parameters["@OldDate"].Value = oldDate;
            cmd.Parameters["@OldTime"].Value = oldTime;
            cmd.Parameters["@OldLocation"].Value = oldLocation;
            cmd.Parameters["@OldMaxSeats"].Value = oldMaxSeats;
            cmd.Parameters["@OldPrice"].Value = oldPrice;
            cmd.Parameters["@OldAddedBy"].Value = oldAddedBy;
            cmd.Parameters["@OldActive"].Value = oldActive;

            cmd.Parameters["@NewName"].Value = newName;
            cmd.Parameters["@NewDescription"].Value = newDescription;
            cmd.Parameters["@NewDate"].Value = newDate;
            cmd.Parameters["@NewTime"].Value = newTime;
            cmd.Parameters["@NewLocation"].Value = newLocation;
            cmd.Parameters["@NewMaxSeats"].Value = newMaxSeats;
            cmd.Parameters["@NewPrice"].Value = newPrice;
            cmd.Parameters["@NewAddedBy"].Value = newAddedBy;
            cmd.Parameters["@NewActive"].Value = newActive;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw new ApplicationException("Could not save data!");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        public static List<RoomEvent> RetrieveEventsByRoomID(string roomID)
        {
            // Find the events associated with a room.
            List<RoomEvent> events = new List<RoomEvent>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_events_by_roomID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@RoomID", SqlDbType.Char, 3);
            cmd.Parameters["@RoomID"].Value = roomID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var roomEvent = new RoomEvent()
                        {
                            EventID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = reader.GetDateTime(2).ToShortDateString(),
                            IndividualPrice = reader.GetDecimal(3),
                            TicketsReserved = reader.GetInt32(4)
                        };
                        events.Add(roomEvent);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw new ApplicationException(noAccess);
            }
            finally
            {
                conn.Close();
            }


            return events;
        }

        public static int RetrieveCountEventIDInRoomEvents(int eventID)
        {
            // To see how many tickets a guest has for an event. Any new tickets
            // will be added on.
            int count = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_check_if_tickets_purchased";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EventID", SqlDbType.Int);
            cmd.Parameters["@EventID"].Value = eventID;

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw new ApplicationException(noAccess);
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public static List<Event> RetrieveEventsByDate(string searchDate, bool active = true)
        {
            // Pulls a list of active events by the date.
            List<Event> events = new List<Event>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_events_by_date";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SearchDate", SqlDbType.Date);
            cmd.Parameters.Add("@Active", SqlDbType.Bit);

            cmd.Parameters["@SearchDate"].Value = searchDate;
            cmd.Parameters["@Active"].Value = active;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Event newEvent = new Event()
                        {
                            EventID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Date = reader.GetDateTime(3).ToShortDateString(),
                            Time = reader.GetInt32(4).ToString(),
                            Location = reader.GetString(5),
                            MaxSeats = reader.GetInt32(6),
                            Price = reader.GetDecimal(7),
                            AddedBy = reader.GetInt32(8),
                            Active = reader.GetBoolean(9)
                        };
                        events.Add(newEvent);
                    }
                }
            }
            catch (Exception)
            {

                throw new ApplicationException(noAccess);
            }
            finally
            {
                conn.Close();
            }

            return events;
        }

        public static Event RetrieveEventByID(int eventID)
        {
            // Get a specific event by the event's ID. Used by system, user would not 
            // know the ID.
            Event selectedEvent = null;
            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_event_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EventID", SqlDbType.Int);
            cmd.Parameters["@EventID"].Value = eventID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    selectedEvent = new Event()
                    {
                        EventID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Date = reader.GetDateTime(3).ToShortDateString(),
                        Time = reader.GetInt32(4).ToString(),
                        Location = reader.GetString(5),
                        MaxSeats = reader.GetInt32(6),
                        Price = reader.GetDecimal(7),
                        AddedBy = reader.GetInt32(8),
                        Active = reader.GetBoolean(9)
                    };
                }
                reader.Close();
            }
            catch (Exception)
            {

                throw new ApplicationException(noAccess);
            }
            finally
            {
                conn.Close();
            }


            return selectedEvent;
        }

        public static EventWithEmployee RetrieveEventWithEmployeeByID(int eventID)
        {
            // Get a specific event by the event's ID. Used by system, user would not 
            // know the ID.
            EventWithEmployee selectedEvent = null;
            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_event_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EventID", SqlDbType.Int);
            cmd.Parameters["@EventID"].Value = eventID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    selectedEvent = new EventWithEmployee()
                    {
                        EventID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Date = reader.GetDateTime(3).ToShortDateString(),
                        Time = reader.GetInt32(4).ToString(),
                        Location = reader.GetString(5),
                        MaxSeats = reader.GetInt32(6),
                        Price = reader.GetDecimal(7),
                        AddedBy = reader.GetInt32(8),
                        Active = reader.GetBoolean(9)
                    };
                }
                reader.Close();

                selectedEvent.EmployeeCreater = UserAccessor.RetrieveEmployeeById(selectedEvent.AddedBy);
            }
            catch (Exception)
            {

                throw new ApplicationException(noAccess);
            }
            finally
            {
                conn.Close();
            }


            return selectedEvent;
        }

        public static List<Event> RetrieveAllEvents()
        {
            // Get all events, both active and inactive.
            List<Event> events = new List<Event>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_get_events_regardless_active";
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
                        events.Add(
                            new Event()
                            {
                                EventID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                Date = reader.GetDateTime(3).ToShortDateString(),
                                Time = reader.GetInt32(4).ToString(),
                                Location = reader.GetString(5),
                                MaxSeats = reader.GetInt32(6),
                                Price = reader.GetDecimal(7),
                                AddedBy = reader.GetInt32(8),
                                Active = reader.GetBoolean(9)

                            });
                    }
                }
            }
            catch (Exception)
            {

                throw new ApplicationException(noAccess);
            }
            finally
            {
                conn.Close();
            }

            return events;
        }

        public static int DeactivateEventById(int eventId)
        {
            var rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_deactivate_event_by_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EventID", eventId);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    }
}
