﻿using System;
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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                loggedInPageHelper _CookieCount = new loggedInPageHelper();
                if (_CookieCount.CookieCount() > 0)
                {
                    NavigationService.Navigate(new Uri("/LoggedInPage.xaml", UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception exx)
            {
                System.Diagnostics.Debug.WriteLine(exx.ToString());
            }
        }

        public void Login(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }

        public void Signup(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Signup.xaml", UriKind.Relative));
        }
        public void Local(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LocalPage.xaml", UriKind.Relative));
        }
    }
}