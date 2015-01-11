using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace konto
{
    class dbCollection
    {
    }

    [Table]
    public class Cookies : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _cookieId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int CookieId
        {
            get
            {
                return _cookieId;
            }
            set
            {
                if (_cookieId != value)
                {
                    NotifyPropertyChanging("CookieId");
                    _cookieId = value;
                    NotifyPropertyChanged("CookieId");
                }
            }
        }

        private string _tea;
        [Column]
        public string tea
        {
            get
            {
                return _tea;
            }
            set
            {
                if (_tea != value)
                {
                    NotifyPropertyChanging("tea");
                    _tea = value;
                    NotifyPropertyChanged("tea");
                }
            }
        }

        private string _user;
        [Column]
        public string user
        {
            get 
            {
                return _user;
            }
            set 
            {
                if (_user != value)
                {
                    NotifyPropertyChanging("user");
                    _user = value;
                    NotifyPropertyChanged("user");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
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

    [Table]
    public class Notification : INotifyPropertyChanged, INotifyPropertyChanging
    {

        /*
         * NoticeId - int,
         * Name - string(can be NUll),
         * Notice_id - string,
         * IsPositive - bool (can be Null),
         * IsNegetive - bool (can be Null),
         * IsTracking - bool (can be Null),
         * Amount - int (can be Null)
         * */
        private int _noticeId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int NoticeId
        {
            get
            {
                return _noticeId;
            }
            set
            {
                if (_noticeId != value)
                {
                    NotifyPropertyChanging("NoticeId");
                    _noticeId = value;
                    NotifyPropertyChanged("NoticeId");
                }
            }
        }

        private string _name;
        [Column(CanBeNull = true)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _notice_id;
        [Column(CanBeNull = false)]
        public string Notice_id
        {
            get
            {
                return _notice_id;
            }
            set
            {
                if (_notice_id != value)
                {
                    NotifyPropertyChanging("Notice_id");
                    _notice_id = value;
                    NotifyPropertyChanged("Notice_id");
                }
            }
        }

        private bool _isPositive;
        [Column(CanBeNull = true)]
        public bool IsPositive
        {
            get
            {
                return _isPositive;
            }
            set
            {
                if (_isPositive != value)
                {
                    NotifyPropertyChanging("IsPositive");
                    _isPositive = value;
                    NotifyPropertyChanged("IsPositive");
                }
            }
        }

        private bool _isNegetive;
        [Column(CanBeNull = true)]
        public bool IsNegetive
        {
            get
            {
                return _isNegetive;
            }
            set
            {
                if (_isNegetive != value)
                {
                    NotifyPropertyChanging("IsNegetive");
                    _isNegetive = value;
                    NotifyPropertyChanged("IsNegetive");
                }
            }
        }

        private bool _isTracking;
        [Column(CanBeNull = true)]
        public bool IsTracking
        {
            get
            {
                return _isTracking;
            }
            set
            {
                if (_isTracking != value)
                {
                    NotifyPropertyChanging("IsTracking");
                    _isTracking = value;
                    NotifyPropertyChanged("IsTracking");
                }
            }
        }

        private int _amount;
        [Column(CanBeNull = true)]
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (_amount != value)
                {
                    NotifyPropertyChanging("Amount");
                    _amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
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

    [Table]
    public class User : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Define ID: private field, public property and database column.
        private int _userId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                if (_userId != value)
                {
                    NotifyPropertyChanging("UserId");
                    _userId = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }

        // Define username: private field, public property and database column.
        private string _username;

        [Column]
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username != value)
                {
                    NotifyPropertyChanging("UserName");
                    _username = value;
                    NotifyPropertyChanged("UserName");
                }
            }
        }


        private string _firstname;

        [Column]
        public string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                if (_firstname != value)
                {
                    NotifyPropertyChanging("FirstName");
                    _firstname = value;
                    NotifyPropertyChanged("FirstName");
                }
            }
        }


        private string _lastname;

        [Column]
        public string LastName
        {
            get
            {
                return _lastname;
            }
            set
            {
                if (_lastname != value)
                {
                    NotifyPropertyChanging("LastName");
                    _lastname = value;
                    NotifyPropertyChanged("LastName");
                }
            }
        }


        private string _userid;

        [Column]
        public string User_Id
        {
            get
            {
                return _userid;
            }
            set
            {
                if (_userid != value)
                {
                    NotifyPropertyChanging("UserId");
                    _userid = value;
                    NotifyPropertyChanged("UserId");
                }
            }
        }
        // Define completion value: private field, public property and database column.
        private bool _isLoggedIn;

        [Column]
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                if (_isLoggedIn != value)
                {
                    NotifyPropertyChanging("IsLoggedIn");
                    _isLoggedIn = value;
                    NotifyPropertyChanged("IsLoggedIn");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
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

    [Table]
    public class RealData : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /*
         * Name
         * Amount
         * Notice_id
         * IsNegetive
         * IsPositive
         * */

        private int _dataId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int DataId
        {
            get
            {
                return _dataId;
            }
            set
            {
                if (_dataId != value)
                {
                    NotifyPropertyChanging("DataId");
                    _dataId = value;
                    NotifyPropertyChanged("DataId");
                }
            }
        }

        private string _name;
        [Column(CanBeNull = true)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _notice_id;
        [Column(CanBeNull = false)]
        public string Notice_id
        {
            get
            {
                return _notice_id;
            }
            set
            {
                if (_notice_id != value)
                {
                    NotifyPropertyChanging("Notice_id");
                    _notice_id = value;
                    NotifyPropertyChanged("Notice_id");
                }
            }
        }

        private int _amount;
        [Column(CanBeNull = true)]
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (_amount != value)
                {
                    NotifyPropertyChanging("Amount");
                    _amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        private bool _isPositive;
        [Column(CanBeNull = true)]
        public bool IsPositive
        {
            get
            {
                return _isPositive;
            }
            set
            {
                if (_isPositive != value)
                {
                    NotifyPropertyChanging("IsPositive");
                    _isPositive = value;
                    NotifyPropertyChanged("IsPositive");
                }
            }
        }

        private bool _isNegetive;
        [Column(CanBeNull = true)]
        public bool IsNegetive
        {
            get
            {
                return _isNegetive;
            }
            set
            {
                if (_isNegetive != value)
                {
                    NotifyPropertyChanging("IsNegetive");
                    _isNegetive = value;
                    NotifyPropertyChanged("IsNegetive");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
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


    [Table]
    public class RealDataLocal : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /*
         * Name
         * Amount
         * Notice_id
         * IsNegetive
         * IsPositive
         * */

        private int _dataId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int DataId
        {
            get
            {
                return _dataId;
            }
            set
            {
                if (_dataId != value)
                {
                    NotifyPropertyChanging("DataId");
                    _dataId = value;
                    NotifyPropertyChanged("DataId");
                }
            }
        }

        private string _name;
        [Column(CanBeNull = true)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    NotifyPropertyChanging("Name");
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _notice_id;
        [Column(CanBeNull = false)]
        public string Notice_id
        {
            get
            {
                return _notice_id;
            }
            set
            {
                if (_notice_id != value)
                {
                    NotifyPropertyChanging("Notice_id");
                    _notice_id = value;
                    NotifyPropertyChanged("Notice_id");
                }
            }
        }

        private int _amount;
        [Column(CanBeNull = true)]
        public int Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if (_amount != value)
                {
                    NotifyPropertyChanging("Amount");
                    _amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        private bool _isPositive;
        [Column(CanBeNull = true)]
        public bool IsPositive
        {
            get
            {
                return _isPositive;
            }
            set
            {
                if (_isPositive != value)
                {
                    NotifyPropertyChanging("IsPositive");
                    _isPositive = value;
                    NotifyPropertyChanged("IsPositive");
                }
            }
        }

        private bool _isNegetive;
        [Column(CanBeNull = true)]
        public bool IsNegetive
        {
            get
            {
                return _isNegetive;
            }
            set
            {
                if (_isNegetive != value)
                {
                    NotifyPropertyChanging("IsNegetive");
                    _isNegetive = value;
                    NotifyPropertyChanged("IsNegetive");
                }
            }
        }
        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
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
    public class DbDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/Konto.sdf";

        // Pass the connection string to the base class.
        public DbDataContext(string connectionString)
            : base(connectionString)
        { }


        public Table<User> users;
        public Table<Cookies> cookies;
        public Table<Notification> notification;
    }
}
