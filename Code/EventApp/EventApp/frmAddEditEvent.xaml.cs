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
using EventAppLogicLayer;
using EventAppDataObjects;

namespace EventApp
{
    /// <summary>
    /// Interaction logic for frmAddEditEvent.xaml
    /// </summary>
    public partial class frmAddEditEvent : Window
    {
        private IEventManager _eventManager;
        private User _user;
        private Event _event;

        public frmAddEditEvent(IEventManager em, User user, Event e = null)
        {
            InitializeComponent();
            _eventManager = em;
            _user = user;
            _event = e; // if null, we are adding. If not, we are editing.
            this.cboActive.Text = "Yes";

           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // add the hours to the combo box
            for (int i = 1; i <= 12; i++)
            {
                cboHour.Items.Add(i.ToString());
            }

            // add the minutes to the combo box
            for (int i = 0; i < 60; i++)
            {
                string minutes = i.ToString();
                if (i < 10)
                {
                    minutes = "0" + minutes;
                }
                cboMinute.Items.Add(minutes);
            }

            // add the period to the combo box
            cboAmPm.Items.Add("AM");
            cboAmPm.Items.Add("PM");

            // add the active state to the combo box. Removed because I put it in XAML
            //cboActive.Items.Add("Yes");
            //cboActive.Items.Add("No");

            //if it is not null, then we are editing
            if (_event != null)
            {
                this.Title = "Edit Event";
                this.txtName.Text = _event.Name;
                this.txtLocation.Text = _event.Location;
                this.txtDate.Text = _event.Date;
                this.txtDescription.Text = _event.Description;
                this.txtPrice.Text = _event.Price.ToString();
                this.txtMaxSeats.Text = _event.MaxSeats.ToString();
                this.cboHour.SelectedItem = _eventManager.PullHoursFromFull(_event.Time);
                this.cboMinute.SelectedItem = _eventManager.PullMinutesFromFull(_event.Time);
                this.cboAmPm.SelectedItem = _eventManager.PullPeriodFromFull(_event.Time);
                this.lblActive.Visibility = Visibility.Visible;
                this.cboActive.Visibility = Visibility.Visible;
                this.cboActive.SelectedIndex = _eventManager.EventActive(_event.Active);
                this.btnAdd.Content = "Save Changes";
            }
            else
            {
                // if we are adding, do not let user choose a date for the event that is
                // on or before the current date.
                txtDate.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now));
            }


        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Event newEvent = null;
            
            // combine the values in the combo boxes
            string time = _eventManager.MakeFullTime(cboHour.Text, cboMinute.Text, cboAmPm.Text);


            try
            {
                // see if the data is valid
                _eventManager.ValidData(txtName.Text, txtDescription.Text, cboHour.Text,
                    cboMinute.Text, cboAmPm.Text, txtDate.Text, txtLocation.Text,
                    txtPrice.Text, txtMaxSeats.Text, cboActive.Text);

                // the data is valid, so try to make a new event
                newEvent = _eventManager.MakeEvent(txtName.Text, txtDescription.Text, txtDate.Text,
                    time, txtLocation.Text, Int32.Parse(txtMaxSeats.Text), Decimal.Parse(txtPrice.Text),
                    _user.EmployeeID, (cboActive.Text == "Yes"? true : false));
                //newEvent = new Event()
                //{
                //    Name = txtName.Text,
                //    Description = txtDescription.Text,
                //    Date = txtDate.Text,
                //    Time = time,
                //    Location = txtLocation.Text,
                //    MaxSeats = Int32.Parse(txtMaxSeats.Text),
                //    Price = Decimal.Parse(txtPrice.Text),
                //    AddedBy = _user.EmployeeID

                //};
            }
            catch (Exception ex)
            {

                MessageBox.Show("Invalid data. Please try again." + Environment.NewLine +
                    ex.Message);
                return;
            }

            if (_event == null) // we are adding an event
            {
                try
                {
                    if (_eventManager.AddNewEvent(newEvent))
                    {
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " +ex.Message);
                }
            }
            else // we are editing an event
            {
                try
                {
                    if (_eventManager.EditEvent(_event, newEvent))
                    {
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ERROR: " + ex.Message);
                }
            }
        }

        

        private void txtPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            // check if the inputted price is valid as the user leaves the field
            if (txtPrice.Text != "")
            {
                decimal price;
                try
                {
                    // convert to decimal
                    price = Decimal.Parse(txtPrice.Text);

                    // round to 2 decimal places, rounding up
                    price = Decimal.Round(price, 2, MidpointRounding.AwayFromZero);

                    // use comma notation and force two decimals
                    txtPrice.Text = price.ToString("0,0.00");
                }
                catch (Exception)
                {
                    txtPrice.Text = "";
                    MessageBox.Show("Invalid Price");
                }
                
            }
        }
    }
}
