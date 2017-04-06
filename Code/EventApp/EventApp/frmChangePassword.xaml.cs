using EventAppDataObjects;
using EventAppLogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EventApp
{
    /// <summary>
    /// Interaction logic for frmChangePassword.xaml
    /// </summary>
    public partial class frmChangePassword : Window
    {
        private User _user;
        private Guest _guest;
        private IRoomManager _roomManager;
        private IUserManager _userManager;
        private bool isGuest = false;
        private string samePassErr = "You must make a new password.";
        private string difPassErr = "New Password and Confirmed Password must match.";
        private string err = "Update Failed. Recheck passwords and try again.";

        public frmChangePassword(User user, Guest guest, IRoomManager roomManager, IUserManager userManager)
        {
            InitializeComponent();
            _user = user;
            _guest = guest;
            _roomManager = roomManager;
            _userManager = userManager;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_guest != null) // a guest is using this, not employee. Change labels, etc
            {
                isGuest = true;
            }
            formatForm(isGuest);
            txtOldPassword.Focus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var oldPassword = txtOldPassword.Password;
            var newPassword = txtNewPassword.Password;
            var confirmPassword = txtConfirmPassword.Password;

            if (newPassword == oldPassword) // make sure it is a new password
            {
                MessageBox.Show(samePassErr);
                resetNewAndConfirmPasswords();
                return;
            }
            if (newPassword != confirmPassword) // make sure the user knows what password was chosed
            {
                MessageBox.Show(difPassErr);
                resetNewAndConfirmPasswords();
                return;
            }

            try
            {
                if (isGuest)
                {
                    if(_roomManager.UpdatePIN(_guest.RoomID, oldPassword, newPassword))
                    {
                        MessageBox.Show("PIN updated.");
                        this.Close();
                    }
                    else
                    {
                        throw new ApplicationException(err);
                    }
                }
                else //emp using, not guest
                {
                    
                    if(_userManager.UpdateUserPassword(_user.EmployeeID, oldPassword, newPassword))
                    {
                        MessageBox.Show("Password updated.");
                        this.Close();
                    }
                    else
                    {
                        throw new ApplicationException(err);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to update." + Environment.NewLine + ex.Message);
                resetNewAndConfirmPasswords();
                txtOldPassword.Clear();
                txtOldPassword.Focus();
            }

        }

        private void resetNewAndConfirmPasswords()
        {
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            txtNewPassword.Focus();
        }

        private void formatForm(bool isGuest)
        {
            // format content for a guest to use. Default is employee.
            if (isGuest)
            {
                this.lblNewPassword.Content = "New PIN";
                this.lblOldPassword.Content = "Old PIN";
                this.lblConfirmPassword.Content = "Confirm PIN";
                this.Title = "Change PIN";
                samePassErr = "You must make a new PIN.";
                difPassErr = "New PIN and Confirmed PIN must match.";
                err = "Update Failed. Recheck PINs and try again";
            }
        }
    }
}
