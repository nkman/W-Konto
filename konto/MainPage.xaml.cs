using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using konto.Resources;

namespace konto
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void Login(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }

        public void Signup(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Signup.xaml", UriKind.Relative));
        }
    }
}