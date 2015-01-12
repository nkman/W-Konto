using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace konto
{
    public partial class AddADeal : PhoneApplicationPage
    {
        public AddADeal()
        {
            InitializeComponent();
        }

        public void AddATransaction_Button_click(object sender, EventArgs e)
        {
            string _fellow_username = fellow_username.Text;
            string _amount = amount.Text;
            var _mod = (string)((ListPickerItem)mod.SelectedItem).Content;
            httpHelper.transactionAdd _t;
            if (_mod == "He'll give me")
            {
                _t = new httpHelper.transactionAdd
                {
                    fellow_username = _fellow_username,
                    amount = Int32.Parse(_amount),
                    sign = "positive"
                };
            }
            else
            {
                _t = new httpHelper.transactionAdd
                {
                    fellow_username = _fellow_username,
                    amount = Int32.Parse(_amount),
                    sign = "negetive"
                };
            }
            System.Diagnostics.Debug.WriteLine(_t);
            httpHelper.RequestSender(_t, 5);
            NavigationService.Navigate(new Uri("/LoggedInPage.xaml", UriKind.Relative));
        }
    }
}