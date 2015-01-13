using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace konto
{
    public partial class LocalPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private static ObservableCollection<RealDataLocal> _realdatalocal;
        private DbDataContext userDB;

        private ObservableCollection<RealDataLocal> realdatalocal
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

        public LocalPage()
        {
            InitializeComponent();
            userDB = new DbDataContext(DbDataContext.DBConnectionString);
            //getAllLocalData();
            realDataBinding.ItemsSource = getAllLocalData();
            DataContext = this.DataContext;
        }

        private List<RealDataLocal> getAllLocalData()
        {
            var dataInDb = from RealDataLocal _realdatalocal_ in userDB.realdatalocal select _realdatalocal_;
            realdatalocal = new ObservableCollection<RealDataLocal>(dataInDb);
            System.Diagnostics.Debug.WriteLine(realdatalocal.Count);
            return realdatalocal.ToList();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            getAllLocalData();
            while (NavigationService.CanGoBack)
            {
                NavigationService.RemoveBackEntry();
            }
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            getAllLocalData();
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

        public void butt3_click_local(object sender, EventArgs e)
        {
            var button = sender as Button;
            int myValue = Convert.ToInt32(button.Tag);
            DelRealDataWithId(myValue);
            realDataBinding.ItemsSource = getAllLocalData();
            NavigationService.Navigate(new Uri("/LocalPage.xaml?Refresh=true", UriKind.Relative));
        }

        private void DelRealDataWithId(int id)
        {
            List<RealDataLocal> faggot = getAllLocalData();
            foreach (RealDataLocal _faggot in faggot)
            {
                if (_faggot.DataId == id)
                {
                    RealDataLocal __faggot = _faggot;
                    realdatalocal.Remove(__faggot);
                    userDB.realdatalocal.DeleteOnSubmit(__faggot);
                    userDB.SubmitChanges();
                    break;
                }
            }
        }

        private void ShowCredits(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Credits.xaml", UriKind.Relative));
        }

        private void AddADeal(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AddLocalDeal.xaml", UriKind.Relative));
        }
    }
}