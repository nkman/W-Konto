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
using System.Windows.Threading;

using Newtonsoft.Json;

namespace konto
{
    public partial class LoggedInPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        // Data context for the local database
        private DbDataContext userDB;
        
        private ObservableCollection<User> _userDetail;
        private ObservableCollection<Cookies> _cookieDetail;

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

        public ObservableCollection<Cookies> cookies
        {
            get
            {
                return _cookieDetail;
            }
            set
            {
                if (_cookieDetail != value)
                {
                    _cookieDetail = value;
                    NotifyPropertyChanged("CookieDetail");
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
            //var userInDB = from User _user_ in userDB.users select _user_;
            //users = new ObservableCollection<User>(userInDB);
            //base.OnNavigatedTo(e);
            //Removes Back stack entries.
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            userDB.SubmitChanges();
        }


        public void adduserInDb(Login.dataFromLoginUrl result)
        {
            var userInDB = from User todo in userDB.users select todo;
            users = new ObservableCollection<User>(userInDB);

            var dbData = users.ToList();

            string _t = JsonConvert.SerializeObject(dbData);
            System.Diagnostics.Debug.WriteLine(_t);

            User newUser = new User { 
                UserName = result.username,
                FirstName = result.firstname,
                LastName = result.lastname,
                User_Id = result.user_id,
                UserId = 1,
                IsLoggedIn = true
            };

            //System.Diagnostics.Debug.WriteLine(result.username);

            try
            {
                users.Add(newUser);
                userDB.users.InsertOnSubmit(newUser);
                userDB.SubmitChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            
        }

        public void addCookieInDb(List<Login.cookieFromLoginUrl> result){
            var cookieInDB = from Cookies _cookie_ in userDB.cookies select _cookie_;
            //System.Diagnostics.Debug.WriteLine(cookieInDB);

            try
            {
                cookies = new ObservableCollection<Cookies>(cookieInDB);
                //System.Diagnostics.Debug.WriteLine(cookies);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            

            //var dbData = cookies.ToList();

            //string _t = JsonConvert.SerializeObject(dbData);
            //System.Diagnostics.Debug.WriteLine(_t);

            
            Cookies newCookie = new Cookies {
                tea = result[0].Value,
                user = result[1].Value
            };

            //System.Diagnostics.Debug.WriteLine(result.username);

            try
            {
                cookies.Add(newCookie);
                userDB.cookies.InsertOnSubmit(newCookie);
                userDB.SubmitChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
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

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

    public class loggedInPageHelper
    {
        public LoggedInPage _loggedInPage;
        public void helperFunc(Login.dataFromLoginUrl result, List<Login.cookieFromLoginUrl> _cookie)
        {
            UIThread.Invoke(() => _loggedInPage = new LoggedInPage());
            UIThread.Invoke(() => _loggedInPage.adduserInDb(result));
            UIThread.Invoke(() => _loggedInPage.addCookieInDb(_cookie));
        }

    }


    public static class UIThread
    {
        private static readonly Dispatcher Dispatcher;

        static UIThread()
        {
            Dispatcher = Deployment.Current.Dispatcher;
        }

        public static void Invoke(Action action)
        {
            if (Dispatcher.CheckAccess())
                action.Invoke();
            else
                Dispatcher.BeginInvoke(action);
        }
    }
}