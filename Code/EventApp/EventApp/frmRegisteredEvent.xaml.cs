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
    /// Interaction logic for frmRegisteredEvent.xaml
    /// </summary>
    public partial class frmRegisteredEvent : Window
    {
        private RoomEvent _roomEvent;
        private IEventManager _em;
        private Event _event;
        private Guest _guest;
        private IGuestManager _gm;
        private IRoomManager _rm;

        public frmRegisteredEvent(RoomEvent rmEvnt, Guest guest, IEventManager em, IGuestManager gm, IRoomManager rm)
        {
            InitializeComponent();
            _roomEvent = rmEvnt;
            _em = em;
            _guest = guest;
            _gm = gm;
            _rm = rm;

            try
            {
                // get the event information
                _event = _em.GetEventByID(_roomEvent.EventID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // populate the fields with the required information
            lblNum.Content = "Event Number: " + _event.EventID.ToString();
            lblEventName.Content = _event.Name;
            lblEventLocation.Content = _event.Location;
            lblEventDate.Content = _event.Date;
            lblEventPrice.Content = _event.Price;
            lblEventTime.Content = _event.Time;
            lblEventSeatsReserved.Content = _roomEvent.TicketsReserved;
            lblEventTotalPrice.Content = (_roomEvent.TicketsReserved * _event.Price).ToString();
            txtEventDesc.Text = _event.Description;

            try
            {
                // check if the event's date is today or in the past. If so, users
                // cannot cancel tickets.
                DateTime eventDate = DateTime.Parse(_event.Date);
                if (DateTime.Compare(eventDate, DateTime.Now) <= 0 )
                {
                    btnCancelTickets.IsEnabled = false;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Unable to parse date. Cannot Cancel Tickets at this time");
                btnCancelTickets.IsEnabled = false;
            }

            
        }

        private void btnCancelTickets_Click(object sender, RoutedEventArgs e)
        {
            // confirm they want to cancel their tickets by having them enter 
            // their room PIN. This also prevents others from piggybacking off
            // a shared computer when someone forgets to log off.
            var verifyPIN = new frmVerifyPIN(_guest, _gm);
            var result = verifyPIN.ShowDialog();
            if (result == true)
            {
                try
                {
                    _rm.RemoveReservedTickets(_guest, _event);
                    MessageBox.Show("Your tickets for " + _event.Name + " have been cancelled.");
                    this.DialogResult = true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                
            }
        }
    }
}
