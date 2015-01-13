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
    public partial class AddLocalDeal : PhoneApplicationPage
    {

        private ObservableCollection<RealDataLocal> _realdatalocal;
        private DbDataContext userDB;

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

        public AddLocalDeal()
        {
            InitializeComponent();
            userDB = new DbDataContext(DbDataContext.DBConnectionString);
            getAllLocalData();
            DataContext = this.DataContext;
        }

        public void AddATransaction_Button_click_Local(object sender, EventArgs e)
        {

            string _fellow_username = fellow_username.Text;
            string _amount = amount.Text;
            var _mod = (string)((ListPickerItem)mod.SelectedItem).Content;
            RealDataLocal R;
            if (_mod == "He'll give me")
            {
                R = new RealDataLocal
                {
                    Amount = Int32.Parse(_amount),
                    Name = _fellow_username + " owes you",
                    IsPositive = true,
                    IsNegetive = false,
                    Notice_id = _fellow_username
                };
            }
            else
            {
                R = new RealDataLocal
                {
                    Amount = Int32.Parse(_amount),
                    Name = "You owe " + _fellow_username,
                    IsPositive = false,
                    IsNegetive = true,
                    Notice_id = _fellow_username
                };
            }
            addInLocalDb(R);
            NavigationService.Navigate(new Uri("/LocalPage.xaml", UriKind.Relative));
        }

        private void addInLocalDb(RealDataLocal result)
        {
            NotifyPropertyChanging("realdata");
            realdatalocal.Add(result);
            userDB.realdatalocal.InsertOnSubmit(result);
            System.Diagnostics.Debug.WriteLine("Adding in Database");
            userDB.SubmitChanges();
            NotifyPropertyChanged("realdata");
        }

        private List<RealDataLocal> getAllLocalData()
        {
            var dataInDb = from RealDataLocal _realdatalocal_ in userDB.realdatalocal select _realdatalocal_;
            realdatalocal = new ObservableCollection<RealDataLocal>(dataInDb);
            return realdatalocal.ToList();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            getAllLocalData();
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
    }
}