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
    /// Interaction logic for frmEventInfo.xaml
    /// </summary>
    public partial class frmEventInfo : Window
    {
        private Event _event;
        private Guest _guest;
        private EventAppLogicLayer.EventManager _eventManager;
        private RoomManager _roomManager;
        private string thanks = "Thank you! Your purchase has been charged to your room.";

        public frmEventInfo(Event selectedEvent, Guest guest, EventAppLogicLayer.EventManager em, RoomManager rm)
        {
            InitializeComponent();
            _event = selectedEvent;
            _guest = guest;
            _eventManager = em;
            _roomManager = rm;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // populate fields with the event information
            this.lblEventName.Content = _event.Name;
            this.lblEventDate.Content = _event.Date;
            this.lblEventLocation.Content = _event.Location;
            this.lblEventMaxSeats.Content = _event.MaxSeats;
            this.lblEventPrice.Content = _event.Price;
            this.lblEventTime.Content = _event.Time;
            this.txtEventDesc.Text = _event.Description;
            this.lblNum.Content = "Event Number: " + _event.EventID;
            if (null == _guest)
            {
                // if no one is logged in, do not allow purchasing
                this.btnPurchase.IsEnabled = false;
                this.lblNotLoggedIn.Visibility = Visibility.Visible;
            }

            int ticketsAvail = 0;

            try
            {
                // get the tickets that ar available
                ticketsAvail = _eventManager.CalculateAvailableTickets(_event);
                this.lblEventSeatsAvail.Content = ticketsAvail.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show("ERROR: " + ex.Message);
            }

                lblCalcCost.Content = "$" + (_event.Price);

            if (0 == ticketsAvail)
            {
                noAvailableTickets();
            }

        }

        private void noAvailableTickets()
        {
            // if there are no available tickets, do not allow purchasing and
            // notify users
            btnPurchase.IsEnabled = false;
            txtQuantity.IsEnabled = false;
            lblNotLoggedIn.Content = "No tickets available";
            lblNotLoggedIn.Visibility = Visibility.Visible;
        }

        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            int quantity = 0;

            int ticketsAvail = 0;

            // check if anyone else had purchased tickets between loading the window
            // and trying to purchase
            try
            {
                ticketsAvail = _eventManager.CalculateAvailableTickets(_event);
            }
            catch (Exception)
            {

                throw;
            }
            if (0 == ticketsAvail)
            {
                noAvailableTickets();
                return;
            }

            // basic checks on user quantity input: if it is an integer greater than
            // 0 and less than or equal to the amount of tickets available.
            try
            {
                quantity = Int32.Parse(txtQuantity.Text);
                if (quantity <= 0)
                {
                    throw new ApplicationException("Quantity must be positive");
                }
                if (quantity > (ticketsAvail))
                {
                    throw new ApplicationException("Cannot purchase more tickets than available");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Input"))
                {
                    MessageBox.Show("Invalid Quantity. Quantity must be a positive number");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return;

            }

            //See if they already have tickets for this event. If so, add the new amount to it!
            if (_roomManager.CheckIfPurchasedAlready(_guest.RoomID, _event.EventID))
            {
                try
                {
                    if (_roomManager.PurchaseMoreTickets(_guest.RoomID, _event.EventID, quantity) == 1)
                    {
                        MessageBox.Show(thanks);
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("There was a problem saving your purchase. Please try again later!");
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
            else // They don't already have tickets, so make a new record
            {
                try
                {
                    if (1 == _roomManager.PurchaseTickets(_guest.RoomID, _event.EventID, quantity))
                    {
                        MessageBox.Show(thanks);
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }

        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Check the user's inputs, namely that it is an positive integer
            if (this.IsLoaded)
            {
                int quantity = 0;

                if (txtQuantity.Text != "")
                {
                    
                    try
                    {
                        quantity = Int32.Parse(txtQuantity.Text);
                        if (quantity <= 0)
                        {
                            throw new ApplicationException();
                        }
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Quantity must be a positive number");
                        txtQuantity.Text = "1";
                        lblCalcCost.Content = "$" + (_event.Price);
                        txtQuantity.SelectAll();
                    }


                    // Calculate the cost as the user modifies teh quanitity.
                    if (quantity != 0)
                    {
                        lblCalcCost.Content = "$" + (quantity * _event.Price);
                    }
                    else
                    {
                        lblCalcCost.Content = "$" + (_event.Price);
                    }
                    
                }
            }
        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
    }
}
