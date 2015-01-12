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
using System.Threading.Tasks;
using System.Threading;

namespace konto
{
    public partial class LoggedInPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        // Data context for the local database
        private DbDataContext userDB;
        
        private ObservableCollection<User> _userDetail;
        private ObservableCollection<Cookies> _cookieDetail;
        private ObservableCollection<Notification> _notification;
        private ObservableCollection<RealData> _realdata;
        private ObservableCollection<RealDataLocal> _realdatalocal;

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
                    NotifyPropertyChanging("users");
                    _userDetail = value;
                    NotifyPropertyChanged("users");
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
                    NotifyPropertyChanging("cookies");
                    _cookieDetail = value;
                    NotifyPropertyChanged("cookies");
                }
            }
        }

        public ObservableCollection<RealData> realdata
        {
            get
            {
                return _realdata;
            }
            set
            {
                if (_realdata != value)
                {
                    NotifyPropertyChanging("realdata");
                    _realdata = value;
                    NotifyPropertyChanged("realdata");
                }
            }
        }

        public ObservableCollection<RealDataLocal> realdatalocal
        {
            get
            {
                return _realdatalocal;
            }
            set
            {
                if (_realdatalocal != value)
                {
                    NotifyPropertyChanging("realdatalocal");
                    _realdatalocal = value;
                    NotifyPropertyChanged("realdatalocal");
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
                    NotifyPropertyChanging("notification");
                    _notification = value;
                    NotifyPropertyChanged("notification");
                }
            }
        }

        public LoggedInPage()
        {
            try
            {
                InitializeComponent();
                userDB = new DbDataContext(DbDataContext.DBConnectionString);
                getAllNotices();
                getAllData();
                this.DataContext = this;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            
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

        public List<RealData> getAllData()
        {
            var dataInDb = from RealData _realdata_ in userDB.realdata select _realdata_;
            realdata = new ObservableCollection<RealData>(dataInDb);
            return realdata.ToList();
        }

        public List<Cookies> getAllCookieFromDB()
        {
            var cookieInDB = from Cookies _cookie_ in userDB.cookies select _cookie_;
            List<Cookies> L;

            try
            {
                cookies = new ObservableCollection<Cookies>(cookieInDB);
                L = new List<Cookies>(cookies);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                L = new List<Cookies>();
            }

            return L;
        }

        public void LogoutUserDelDB()
        {
            foreach (Cookies c in getAllCookieFromDB())
            {
                cookies.Remove(c);
                userDB.cookies.DeleteOnSubmit(c);
                userDB.SubmitChanges();
            }

            List<List<Notification>> n = getAllNotices();
            foreach (List<Notification> _n in n)
            {
                foreach(Notification __n in _n){
                    notification.Remove(__n);
                    userDB.notification.DeleteOnSubmit(__n);
                    userDB.SubmitChanges();
                }
            }

            List<RealData> _R = getAllData();
            foreach (RealData __R in _R)
            {
                realdata.Remove(__R);
                userDB.realdata.DeleteOnSubmit(__R);
                userDB.SubmitChanges();
            }
        }

        public void delFromNotification(Notification n)
        {
            int breaker = 0;
            List<List<Notification>> _n = getAllNotices();
            foreach (List<Notification> __n in _n)
            {
                foreach (Notification ___n in __n)
                {
                    if (___n.Notice_id == n.Notice_id)
                    {
                        Notification ____n = ___n;
                        NotifyPropertyChanging("notification");
                        notification.Remove(____n);
                        userDB.notification.DeleteOnSubmit(____n);
                        userDB.SubmitChanges();
                        NotifyPropertyChanged("notification");
                        breaker = 1;
                        break;
                    }
                    if (breaker == 1)
                        break;
                }
            }

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
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
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
                NotifyPropertyChanging("users");
                users.Add(newUser);
                userDB.users.InsertOnSubmit(newUser);
                userDB.SubmitChanges();
                NotifyPropertyChanged("users");
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
            int breakit = 0;
            try
            {
                notification = new ObservableCollection<Notification>(noticeInDb);
                System.Diagnostics.Debug.WriteLine(notification.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            foreach (Notification n in notification)
            {
                if (n.Notice_id == result.Notice_id)
                {
                    System.Diagnostics.Debug.WriteLine("Already in Database");
                    breakit = 1;
                    break;
                }
                    
            }

            if(breakit == 0)
            try
            {
                NotifyPropertyChanging("notification");
                notification.Add(result);
                userDB.notification.InsertOnSubmit(result);
                System.Diagnostics.Debug.WriteLine("Adding in Database");
                userDB.SubmitChanges();
                NotifyPropertyChanged("notification");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public void RealDataSaveInDb(RealData result)
        {
            var dataInDb = from RealData _realdata_ in userDB.realdata select _realdata_;
            //System.Diagnostics.Debug.WriteLine(cookieInDB);
            int breakit = 0;
            try
            {
                realdata = new ObservableCollection<RealData>(dataInDb);
                System.Diagnostics.Debug.WriteLine(realdata.Count);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            foreach (RealData R in realdata)
            {
                if (R.Notice_id == result.Notice_id)
                {
                    breakit = 1;
                    break;
                }
            }

            if (breakit == 0)
            {
                try
                {
                    NotifyPropertyChanging("realdata");
                    realdata.Add(result);
                    userDB.realdata.InsertOnSubmit(result);
                    System.Diagnostics.Debug.WriteLine("Adding in Database");
                    userDB.SubmitChanges();
                    NotifyPropertyChanged("realdata");
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
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

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        private void ShowCredits(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Credits.xaml", UriKind.Relative));
        }

        private void AddADeal(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddADeal.xaml", UriKind.Relative));
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            LogoutUserDelDB();
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }

        private void butt1_click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var myValue = button.Tag;
            httpHelper.noticeAcceptDeclineDelete n = new httpHelper.noticeAcceptDeclineDelete
            {
                account_id = (string)myValue,
                decision = "Accept"
            };
            notification.Remove(sender as Notification);
            httpHelper.RequestSender(n, 2);
            System.Diagnostics.Debug.WriteLine(n);
            NavigationService.Navigate(new Uri("/LoggedInPage.xaml?Refresh=true", UriKind.Relative));
        }

        private void butt2_click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var myValue = button.Tag;
            httpHelper.noticeAcceptDeclineDelete n = new httpHelper.noticeAcceptDeclineDelete
            {
                account_id = (string)myValue,
                decision = "Decline"
            };
            notification.Remove(sender as Notification);
            httpHelper.RequestSender(n, 3);
            NavigationService.Navigate(new Uri("/LoggedInPage.xaml?Refresh=true", UriKind.Relative));
            System.Diagnostics.Debug.WriteLine(n);
        }

        private void SyncNotice(object sender, EventArgs e)
        {
            var p = new List<httpHelper.noticeGet>
            {
                new httpHelper.noticeGet {
                    unread = 1
                }
            };

            try
            {
                httpHelper.RequestSender(p[0], 0);
                getAllNotices();
                getAllData();
                
                NavigationService.Navigate(new Uri("/LoggedInPage.xaml?Refresh=true", UriKind.Relative));
            }
            catch (Exception er)
            {
                System.Diagnostics.Debug.WriteLine(er.ToString());
            }
            
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

        public void realdatapopulator(RealData result)
        {
            UIThread.Invoke(() => _loggedInPage = new LoggedInPage());
            UIThread.Invoke(() => _loggedInPage.RealDataSaveInDb(result));
        }

        public int CookieCount()
        {
            int c;
            //UIThread.Invoke(() => _loggedInPage = new LoggedInPage());
            //UIThread.Invoke(() => c = _loggedInPage.getAllCookieFromDB().Count);
            _loggedInPage = new LoggedInPage();
            c = _loggedInPage.getAllCookieFromDB().Count;
            //UIThread.Invoke(() => return c);
            return c;
        }

        public void delFromNotificationHelper(Notification n)
        {
            UIThread.Invoke(() => _loggedInPage = new LoggedInPage());
            UIThread.Invoke(() => _loggedInPage.delFromNotification(n));
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

    /*
     * Main data classes
     * */

    public class RealDataYo
    {
        public List<string> negetive_name { get; set; }
        public int status { get; set; }
        public List<List<object>> negetive { get; set; }
        public List<List<object>> positive { get; set; }
        public List<string> positive_name { get; set; }
        public int current_balance { get; set; }
    }
}