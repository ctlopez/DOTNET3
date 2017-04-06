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
    /// Interaction logic for frmVerifyPIN.xaml
    /// </summary>
    public partial class frmVerifyPIN : Window
    {
        Guest _guest;
        IGuestManager _gm;

        public frmVerifyPIN(Guest guest, IGuestManager gm)
        {
            InitializeComponent();
            _guest = guest;
            _gm = gm;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // check if the PIN matches the room
                if (txtPIN.Password.Length == 0)
                {
                    throw new ApplicationException("Please enter the PIN");
                }
                else
                {
                    if (_gm.VerifyPIN(_guest, txtPIN.Password))
                    {
                        this.DialogResult = true;
                    }
                    else
                    {
                        txtPIN.Clear();
                        txtPIN.Focus();
                        throw new ApplicationException("Incorrect PIN. Please try again");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtPIN.Focus();
        }
    }
}
