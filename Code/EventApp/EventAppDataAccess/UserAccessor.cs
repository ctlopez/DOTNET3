using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventAppDataObjects;

namespace EventAppDataAccess
{
    public class UserAccessor
    {
        public static int VerifyUsernameAndPassword(string username, string passwordHash)
        {
            // Check to see if emp login successful.
            var result = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_authenticate_user";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Username"].Value = username;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                conn.Open();
                result = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {

                throw new ApplicationException("Unable to access employee data!");
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static User RetrieveUserByUsername(string username)
        {
            // Get the user from DB after validated.
            User user = null;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_employee_by_username";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 20);
            cmd.Parameters["@Username"].Value = username;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    user = new User()
                    {
                        EmployeeID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Phone = reader.GetString(3),
                        Email = reader.GetString(4),
                        UserName = reader.GetString(5),
                        Active = reader.GetBoolean(6)
                    };
                }
                reader.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }

            return user;
        }

        public static List<Role> RetrieveEmployeeRoles(int employeeID)
        {
            // Get a list of roles for the employee.
            var roles = new List<Role>();

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_employee_roles";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters["@EmployeeID"].Value = employeeID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var role = new Role()
                        {
                            RoleID = reader.GetString(0),
                            Description = reader.GetString(1)
                        };
                        roles.Add(role);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                
                throw;
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        public static int UpdateEmployeePassword(int employeeID, string oldPasswordHash, string newPasswordHash)
        {
            // Allow employees to change their password. Validation in Logic Layer.
            int rows = 0;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_update_user_password";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@EmployeeID"].Value = employeeID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw new ApplicationException("Unable to save data");
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    }
}
