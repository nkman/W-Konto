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
        private ObservableCollection<Notification> _notification;
        public static List<List<Notification>> Notices;

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

        public ObservableCollection<Notification> notification
        {
            get
            {
                return _notification;
            }
            set
            {
                if (_notification != value)
                {
                    _notification = value;
                    NotifyPropertyChanged("Notification");
                }
            }
        }

        public LoggedInPage()
        {
            InitializeComponent();

            userDB = new DbDataContext(DbDataContext.DBConnectionString);
            Notices = getAllNotices();
            System.Diagnostics.Debug.WriteLine(Notices.ToString());
            this.DataContext = this;

            notificationDataBinding.ItemsSource = Notices;
        }

        public List<List<Notification>> getAllNotices()
        {
            var noticeInDb = from Notification _notice_ in userDB.notification select _notice_;
            //System.Diagnostics.Debug.WriteLine(cookieInDB);

            try
            {
                notification = new ObservableCollection<Notification>(noticeInDb);
                //System.Diagnostics.Debug.WriteLine(cookies);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            List<Notification> positive_n = new List<Notification>();
            List<Notification> negetive_n = new List<Notification>();
            List<Notification> tracking_n = new List<Notification>();

            foreach (Notification n in notification)
            {
                if (n.IsPositive)
                {
                    positive_n.Add(n);
                }
                else if (n.IsNegetive)
                {
                    negetive_n.Add(n);
                }
                else
                {
                    tracking_n.Add(n);
                }
            }
            List<List<Notification>> _n = new List<List<Notification>>();
            _n.Add(positive_n);
            _n.Add(negetive_n);
            _n.Add(tracking_n);

            return _n;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
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
            
            Cookies newCookie = new Cookies {
                tea = result[0].Value,
                user = result[1].Value
            };

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

        public void NotificationSaveInDb(Notification result)
        {
            var noticeInDb = from Notification _notice_ in userDB.notification select _notice_;
            //System.Diagnostics.Debug.WriteLine(cookieInDB);

            try
            {
                notification = new ObservableCollection<Notification>(noticeInDb);
                //System.Diagnostics.Debug.WriteLine(cookies);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            try
            {
                notification.Add(result);
                userDB.notification.InsertOnSubmit(result);
                System.Diagnostics.Debug.WriteLine("Adding in Database");
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

        public void notificationPopulator(Notification result)
        {
            UIThread.Invoke(() => _loggedInPage = new LoggedInPage());
            UIThread.Invoke(() => _loggedInPage.NotificationSaveInDb(result));
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



    /*
     * Notification classes
     * */

    public class Negetive
    {
        public List<List<object>> negetive { get; set; }
        public int status { get; set; }
        public List<string> name { get; set; }
    }

    public class Track
    {
        public int status { get; set; }
        public List<List<string>> unread { get; set; }
    }

    public class Positive
    {
        public List<List<object>> positive { get; set; }
        public List<string> name { get; set; }
        public int status { get; set; }
    }

    public class RootObject
    {
        public Negetive negetive { get; set; }
        public int status { get; set; }
        public Track track { get; set; }
        public Positive positive { get; set; }
    }
}