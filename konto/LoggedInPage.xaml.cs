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

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace konto
{
    public partial class LoggedInPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        // Data context for the local database
        private DbDataContext userDB;

        // Define an observable collection property that controls can bind to.
        private ObservableCollection<User> _userDetail;
        public ObservableCollection<User> users
        {
            get
            {
                return _userDetail;
            }
            set
            {
                if (_userDetail != value)
                {
                    _userDetail = value;
                    NotifyPropertyChanged("UserDetails");
                }
            }
        }

        public LoggedInPage()
        {
            InitializeComponent();
            userDB = new DbDataContext(DbDataContext.DBConnectionString);

            this.DataContext = this;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Define the query to gather all of the to-do items.
            var userInDB = from User todo in userDB.users select todo;

            // Execute the query and place the results into a collection.
            users = new ObservableCollection<User>(userInDB);

            // Call the base method.
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            userDB.SubmitChanges();
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}