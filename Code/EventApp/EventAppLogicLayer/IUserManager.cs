using System;
namespace EventAppLogicLayer
{
    public interface IUserManager
    {
        EventAppDataObjects.User AuthenticateUser(string username, string password);
        bool UpdateUserPassword(int employeeID, string oldPassword, string newPassword);
    }
}
