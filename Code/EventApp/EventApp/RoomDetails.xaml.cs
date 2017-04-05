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
    /// Interaction logic for RoomDetails.xaml
    /// </summary>
    public partial class RoomDetails : Window
    {
        private List<RoomEvent> _roomEvents;
        private Guest _guest;
        private EventAppLogicLayer.EventManager _em;
        private GuestManager _gm;
        private RoomManager _rm;

        public RoomDetails(List<RoomEvent> rmEvents, Guest guest, EventAppLogicLayer.EventManager em, GuestManager gm, RoomManager rm)
        {
            InitializeComponent();
            _roomEvents = rmEvents;
            _guest = guest;
            _em = em;
            _gm = gm;
            _rm = rm;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // get a list of events associated with the room
            dgRoomEvents.ItemsSource = _roomEvents;
            lblGuestName.Content = _guest.FirstName + " " + _guest.LastName;
            lblRoomNum.Content = _guest.RoomID.ToString();
            getTotal();
        }

        private void dgRoomEvents_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedEvent = (RoomEvent)dgRoomEvents.SelectedItem;
            var registeredEventForm = new frmRegisteredEvent(selectedEvent, _guest, _em, _gm, _rm);
            var result = registeredEventForm.ShowDialog();
            if (result == true)
            {
                // if the data changed, refresh the room event information.
                _roomEvents = _rm.GetEventsForRoom(_guest.RoomID);
                //dgRoomEvents.Items.Refresh();
                //dgRoomEvents.Focus();
                dgRoomEvents.ItemsSource = _roomEvents;
                getTotal();
            }
        }

        private void getTotal()
        {
            decimal total = _roomEvents.Select(c => c.TotalPrice).Sum();
            lblRoomTotal.Content = "Total: $" + Math.Round(total, 2, MidpointRounding.AwayFromZero);
        }
    }
}
