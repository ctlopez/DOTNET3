using EventAppDataObjects;
using EventAppDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EventAppLogicLayer
{
    public class UserManager : IUserManager
    {
        public static string HashSHA256(string source)
        {
            var result = "";

            // this logic is always the same for our purposes
            // we need to create byte array (8 bit unsigned int)
            byte[] data;

            // create a .NET Hash provider object
            // these are computationally expensive,
            // so we get rid of them as soon as we can.
            // Has providers are all created with factory methods.
            using (SHA256 sha256hash = SHA256.Create())
            {
                // hash the input
                data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            // use a stringbuilder to conserve memory
            var s = new StringBuilder();

            // loop through the bytes creating characters
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            result = s.ToString();

            return result;
        }

        public User AuthenticateUser(string username, string password)
        {
            // check if the user entered the correct username and password
            User user = null;

            try
            {
                if (1 == UserAccessor.VerifyUsernameAndPassword(username, HashSHA256(password)))
                {
                    password = null;

                    //get a user
                    user = UserAccessor.RetrieveUserByUsername(username);

                    //get the roles
                    var roles = UserAccessor.RetrieveEmployeeRoles(user.EmployeeID);

                    user.Roles = roles;
                }
                else
                {
                    throw new ApplicationException("Authentication Failed!");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return user;
        }

        public bool UpdateUserPassword(int employeeID, string oldPassword, string newPassword)
        {
            // allow a user to update their password
            bool result = false;

            try
            {
                if (1 == UserAccessor.UpdateEmployeePassword(employeeID, HashSHA256(oldPassword), HashSHA256(newPassword)))
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

    }
}
