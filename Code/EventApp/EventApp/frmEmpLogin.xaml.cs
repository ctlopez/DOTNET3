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
    /// Interaction logic for frmEmpLogin.xaml
    /// </summary>
    public partial class frmEmpLogin : Window
    {
        public User user { get; set; }
        public Guest guest { get; set; }
        private string type;
        private IRoomManager _roomManager;
        private IGuestManager _guestManager;
        private IUserManager _userManager;

        public frmEmpLogin(IRoomManager rm, IGuestManager gm, IUserManager userManager, string usage = "default")
        {
            InitializeComponent();
            type = usage;
            _roomManager = rm;
            _guestManager = gm;
            _userManager = userManager;

            // If an employee is entering guest credentials to perform tasks
            // on behalf of the guest
            if (type == "rooms")
            {
                this.lblEmpUsername.Content = "Room Number:";
                this.lblEmpPassword.Content = "Room PIN:";
                this.btnEmpLogin.Content = "Confirm";
                this.Title = "Validate Room";
            }
            else // it is the employee logging on
            {
                this.lblEmpUsername.Content = "Username:";
                this.lblEmpPassword.Content = "Password:";
                this.btnEmpLogin.Content = "Log In";
                this.Title = "Employee Login";
            }
        }

        private void btnEmpLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text; // could also be room number
            var password = txtPassword.Password; // could also be room PIN
            
            
            if (type == "rooms")
            {
                if (_roomManager.CheckRoomNumberLength(username))
                {
                    try
                    {
                        if (_roomManager.CheckDefault(username, password))
                        {
                            //Use default loggin (UN:000 PW:0000) to only see details
                            guest = null;
                            this.DialogResult = true;
                        }
                        else
                        {
                            //logging in for a guest
                            guest = _guestManager.VerifyGuest(username, password);
                            this.DialogResult = true;
                        }
                        
                    }
                    catch (Exception ex)
                    {

                        errorSteps(ex);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Room Number");
                    txtUsername.Focus();
                    txtUsername.SelectAll();
                    return;
                }
            }
            else
            {
                try
                {
                    user = _userManager.AuthenticateUser(username, password);
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {

                    errorSteps(ex);
                }
            }


        }

        private void errorSteps(Exception ex)
        {
            MessageBox.Show("ERROR: " + ex.Message);
            txtPassword.Clear();
            txtUsername.SelectAll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }
    }
}
