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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Guest _guest = null;
        private User _user = null;
        private GuestManager _guestManager = new GuestManager();
        private EventAppLogicLayer.EventManager _eventManager = new EventAppLogicLayer.EventManager();
        private RoomManager _roomManger = new RoomManager();
        private List<Event> _events = new List<Event>();
        private List<RoomEvent> _rmevnts = new List<RoomEvent>();
        private List<Guest> _activeGuests = new List<Guest>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void mnuQuit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            resetTabs();
            txtRoomNumber.Focus();
            resetLogOut();
            try
            {
                // Because I want any one to look at upcoming events, need to populate the list before any one logs in
                // The following lines deactivate events with a date in the past, then gets a list of active events within
                // the next ten days.
                _eventManager.ClearOldEvents();
                _events = _eventManager.GetUpcomingEvents();
                tabEvents.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }

        private void resetTabs()
        {
            tabAccount.Visibility = Visibility.Collapsed;
            tabRooms.Visibility = Visibility.Collapsed;
        }


        // The btnLogin is only for guest log in; employees use a different method.
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (null == _guest && null == _user)
            {

                try
                {
                    _guest = _guestManager.VerifyGuest(txtRoomNumber.Text, txtPin.Password);
                    loggedIn(_guest.FirstName);
                    statusMessage.Content = "Welcome " + _guest.FirstName + ", Room " + _guest.RoomID;
                    tabAccount.Visibility = Visibility.Visible;
                    _rmevnts = _roomManger.GetEventsForRoom(_guest.RoomID);
                }
                catch (Exception ex)
                {
                    txtPin.Clear();
                    txtRoomNumber.Focus();
                    txtRoomNumber.SelectAll();
                    MessageBox.Show("There was an error: " + ex.Message);
                }
            }
            else
            {
                resetLogOut();
                resetTabs();
            }
        }

        private void loggedIn(string firstName)
        {
            // set up window for when a guest or user is logged in
            txtRoomNumber.Clear();
            txtPin.Clear();
            txtRoomNumber.IsEnabled = false;
            txtPin.IsEnabled = false;
            btnLogin.Content = "Log Out";
            btnLogin.IsDefault = false;
            lblWelcome.Content = "Welcome, " + firstName;
            lblDescription.Content = "";
            lblDate.Visibility = Visibility.Visible;
            pkrDate.Visibility = Visibility.Visible;
            btnDateSearch.Visibility = Visibility.Visible;
            hdrUserSettings.Visibility = Visibility.Visible;
            if (_guest != null)
            {
                // if a guest is logged in, change the header option
                mnuChangePassword.Header = "Change PIN";
            }
            try
            {
                _events = _eventManager.GetActiveEvents();

                tabEvents.Focus();
                refreshEventTable();
            }
            catch (Exception ex)
            {

                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void resetLogOut()
        {
            // reset window for when there is no user or guest logged in
            // rebind the focus event
            tabEvents.GotFocus -= tabEvents_GotFocus;
            tabEvents.GotFocus += tabEvents_GotFocus;
            tabEvents.Focus();
            txtRoomNumber.IsEnabled = true;
            txtPin.IsEnabled = true;
            btnLogin.Content = "Log In";
            btnLogin.IsDefault = true;
            _guest = null;
            _user = null;
            _rmevnts = null;
            _activeGuests = null;
            lblWelcome.Content = "Welcome, Guest";
            statusMessage.Content = "Please log in to continue...";
            lblDescription.Content = "Upcoming Events";
            lblDate.Visibility = Visibility.Hidden;
            pkrDate.Text = "";
            pkrDate.Visibility = Visibility.Hidden;
            txtSearchRoom.Text = "";
            btnDateSearch.Visibility = Visibility.Hidden;
            btnClearDateSearch.Visibility = Visibility.Hidden;
            btnClearRoomSearch.Visibility = Visibility.Hidden;
            hdrUserSettings.Visibility = Visibility.Hidden;
            mnuChangePassword.Header = "Change Password";
            resetTabs();
            lblStatus.Visibility = Visibility.Collapsed;
            cboStatus.Visibility = Visibility.Collapsed;
            cboStatus.SelectedValue = "All";
            btnEditEvent.Visibility = Visibility.Collapsed;
            btnAddEvent.Visibility = Visibility.Collapsed;
            eventActive.Visibility = Visibility.Collapsed;
            try
            {
                _events = _eventManager.GetUpcomingEvents();
                dgEvents.ItemsSource = _events;
                refreshEventTable();
            }
            catch (Exception ex)
            {

                MessageBox.Show("ERROR: " + ex.Message);
            }
        }

        private void tabEvents_GotFocus(object sender, RoutedEventArgs e)
        {
            dgEvents.ItemsSource = _events;
        }

        private void dgEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // check if an event was actually selected
            if (dgEvents.SelectedIndex != -1)
            {
                var selectedEvent = (Event)dgEvents.SelectedItem;
                // check if a user is logged in, not a guest, and the user has a role of
                // "Clerk"
                if (_guest == null && _user != null &&
                    _user.Roles.Contains(_user.Roles.Find(r => r.RoleID.Contains("Clerk"))))
                {
                    var guestForm = new frmEmpLogin(_roomManger, _guestManager, "rooms");
                    var guestResult = guestForm.ShowDialog();
                    if (guestResult == true)
                    {
                        _guest = guestForm.guest;
                    }
                    else
                    {
                        return;
                    }
                }
                var detailForm = new frmEventInfo(selectedEvent, _guest, _eventManager, _roomManger);
                var result = detailForm.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    refreshEventTable();
                    refreshRoomEvents();
                }
                if (_user != null && _guest != null)
                {
                    _guest = null;
                }
            }
        }

        private void refreshEventTable()
        {
            dgEvents.Items.Refresh();
            dgEvents.Focus();
        }

        private void refreshRoomEvents()
        {
            _rmevnts = _roomManger.GetEventsForRoom(_guest.RoomID);
            dgRoomEvents.Items.Refresh();
            dgRoomEvents.Focus();
        }

        private void refreshClerkRooms()
        {
            _activeGuests = _guestManager.GetGuests();
            dgClerkRooms.Items.Refresh();
            dgClerkRooms.Focus();

        }

        private void imgBed_MouseUp(object sender, MouseButtonEventArgs e) // Emp login
        {
            // check that no one is currently logged in. If so, do nothing.
            if (_guest == null && _user == null)
            {
                var loginForm = new frmEmpLogin(_roomManger, _guestManager);
                var result = loginForm.ShowDialog();
                if (result == true)
                {
                    _user = loginForm.user;
                    _activeGuests = _guestManager.GetGuests();
                    dgClerkRooms.ItemsSource = _activeGuests;
                    loggedIn(_user.FirstName);
                    showTabs();
                    statusMessage.Content = "Welcome " + _user.FirstName;
                }
            }
        }

        private void showTabs()
        {
            // show the required tabs depending on the rules of the user.
            foreach (var role in _user.Roles)
            {
                switch (role.RoleID)
                {
                    case "Clerk":
                        tabRooms.Visibility = Visibility.Visible;
                        break;
                    case "Manager":
                        lblStatus.Visibility = Visibility.Visible;
                        cboStatus.SelectedIndex = (int)DropDownEnum.All;
                        cboStatus.Visibility = Visibility.Visible;
                        btnAddEvent.Visibility = Visibility.Visible;
                        btnEditEvent.Visibility = Visibility.Visible;

                        // make a new column to include the active state
                        //var col = new DataGridCheckBoxColumn();
                        //col.Header = "Active";
                        //col.Binding = new Binding("Active");
                        //dgEvents.Columns.Add(col);

                        // Show the hidden active col
                        eventActive.Visibility = Visibility.Visible;

                        try
                        {
                            _events = _eventManager.GetAllEvents();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                        dgEvents.ItemsSource = _events;
                        refreshEventTable();
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var addEventForm = new frmAddEditEvent(_eventManager, _user);
            var result = addEventForm.ShowDialog();
            if (result == true)
            {
                // the data was stored, so get new event list
                try
                {
                    //_events = _eventManager.GetActiveEvents();
                    refreshEventsByComboBox();
                }
                catch (Exception)
                {

                    MessageBox.Show("Unable to refresh table data");
                }
                refreshEventTable();
            }
        }

        private void btnEditEvent_Click(object sender, RoutedEventArgs e)
        {
            var selectedEvent = (Event)dgEvents.SelectedItem;
            // check if we actually have a selected event
            if (selectedEvent == null)
            {
                MessageBox.Show("Please select an event to edit");
                return;
            }
            var editEventForm = new frmAddEditEvent(_eventManager, _user, selectedEvent);
            var result = editEventForm.ShowDialog();
            if (result == true)
            {
                // the data was stored, so get new event list
                try
                {
                    refreshEventsByComboBox();
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to refresh table data");
                }
                refreshEventTable();
            }
        }

        private void tabAccount_GotFocus(object sender, RoutedEventArgs e)
        {
            // populate the fields with the proper information
            lblGuestRoomNumber.Content = "Room Number: " + _guest.RoomID;

            dgRoomEvents.ItemsSource = _rmevnts;
            decimal total = _rmevnts.Select(c => c.TotalPrice).Sum();
            lblRoomTotal.Content = "Total: $" + Math.Round(total, 2, MidpointRounding.AwayFromZero);
        }

        private void btnDateSearch_Click(object sender, RoutedEventArgs e)
        {
            refreshByDate();
        }

        private void refreshByDate()
        {

            // see if anything was chosen yet.
            if (pkrDate.Text != "")
            {
                tabEvents.GotFocus -= tabEvents_GotFocus;
                // a date was picked
                try
                {
                    //_events = _eventManager.GetEventsByDate(pkrDate.Text);
                    dgEvents.ItemsSource = _events.Where(ev => ev.Date.Equals(pkrDate.Text)).OrderBy(ev => ev.Date).Select(ev => ev);
                    btnClearDateSearch.Visibility = Visibility.Visible;

                    // we don't want to call the combo box change event here, so take it off and add it back on later.
                    //cboStatus.SelectionChanged -= cboStatus_SelectionChanged;
                    //cboStatus.SelectedIndex = (int)DropDownEnum.Active; //if someone is a manager, they will know that these are only active events
                    //cboStatus.SelectionChanged += cboStatus_SelectionChanged;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " + ex.Message);
                }
                //refreshEventTable();
                dgEvents.Items.Refresh();
            }
        }

        private void mnuChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var passwordWindow = new frmChangePassword(_user, _guest);
            passwordWindow.ShowDialog();
        }

        private void btnClearDateSearch_Click(object sender, RoutedEventArgs e)
        {
            tabEvents.GotFocus += tabEvents_GotFocus;
            try
            {
                //_events = _eventManager.GetActiveEvents();
                dgEvents.ItemsSource = _events;
                pkrDate.Text = "";
                btnClearDateSearch.Visibility = Visibility.Hidden;
                //cboStatus.SelectedIndex = (int)DropDownEnum.Active; //if someone is a manager, they will know that these are only active events
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }
            refreshEventTable();
        }

        private void dgClerkRooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // make sure something is actually selected
            if (dgClerkRooms.SelectedIndex != -1)
            {
                var selectedGuest = (Guest)dgClerkRooms.SelectedItem;
                _rmevnts = _roomManger.GetEventsForRoom(selectedGuest.RoomID.ToString());

                var roomDetails = new RoomDetails(_rmevnts, selectedGuest, _eventManager, _guestManager, _roomManger);
                roomDetails.ShowDialog();
            }
        }

        private void btnSearchRoom_Click(object sender, RoutedEventArgs e)
        {
            // check that the room has something that is 3 characters long
            if (txtSearchRoom.Text != "" && txtSearchRoom.Text.Length == 3)
            {
                // see if it is all numbers
                try
                {
                    Int32.Parse(txtSearchRoom.Text);
                }
                catch (Exception)
                {

                    MessageBox.Show("Invalid Room Number");
                    return;
                }

                // try to get the guest associated with that room number
                Guest guest;
                try
                {
                    guest = _guestManager.GetGuestByRoomID(txtSearchRoom.Text);
                    if (guest != null)
                    {
                        _activeGuests.Clear();
                        _activeGuests.Add(guest);
                        btnClearRoomSearch.Visibility = Visibility.Visible;
                        dgClerkRooms.ItemsSource = _activeGuests;
                        refreshClerkRooms();
                    }
                    else
                    {
                        MessageBox.Show("Could not find Guest Associated with that room");
                        txtSearchRoom.SelectAll();
                        txtSearchRoom.Focus();
                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Invalid Room Number");
            }
            //refreshClerkRooms();
        }

        private void btnCancelRoomSearch_Click(object sender, RoutedEventArgs e)
        {
            // clear the room search and get the list of active guests
            try
            {
                txtSearchRoom.Text = "";
                _activeGuests.Clear();
                _activeGuests = _guestManager.GetGuests();
                btnClearRoomSearch.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {

                MessageBox.Show("ERROR: " + ex.Message);
            }

            dgClerkRooms.ItemsSource = _activeGuests;
        }

        private void dgRoomEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // make sure something is actually selected
            if (dgRoomEvents.SelectedIndex != -1)
            {
                // populate the frmRegisteredEvent form
                var roomEvent = (RoomEvent)dgRoomEvents.SelectedItem;
                var frmRegisteredEvent = new frmRegisteredEvent(roomEvent, _guest, _eventManager, _guestManager, _roomManger);
                var result = frmRegisteredEvent.ShowDialog();

                if (result == true)
                {
                    _rmevnts = _roomManger.GetEventsForRoom(_guest.RoomID);

                    dgRoomEvents.ItemsSource = _rmevnts;
                    decimal total = _rmevnts.Select(c => c.TotalPrice).Sum();
                    lblRoomTotal.Content = "Total: $" + Math.Round(total, 2, MidpointRounding.AwayFromZero);
                }

            }
        }

        private void cboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // only run when someone is logged in (avoids running when window load or 
            // user loggout)
            if (_user != null)
            {

                try
                {
                    refreshEventsByComboBox();
                    //refreshEventTable();
                    //pkrDate.Text = "";
                    //btnClearDateSearch.Visibility = Visibility.Hidden;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " + ex.Message);
                }

            }

        }

        private void refreshEventsByComboBox()
        {
            // check the dropdown and display events associated with the proper setting
            try
            {
                if (cboStatus.SelectedIndex == (int)DropDownEnum.All)
                {
                    _events = _eventManager.GetAllEvents();
                }
                else if (cboStatus.SelectedIndex == (int)DropDownEnum.Active)
                {
                    _events = _eventManager.GetActiveEvents();
                }
                else if (cboStatus.SelectedIndex == (int)DropDownEnum.Inactive)
                {
                    _events = _eventManager.GetInactiveEvents();
                }
                refreshByDate();
            }
            catch (Exception)
            {

                throw;
            }

        }

        //only allow numbers in the room number field
        private void txtRoomNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }



        private void cboStatus_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //tabEvents.GotFocus -= tabEvents_GotFocus;
        }

        private void txtSearchRoom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }




    }
}
