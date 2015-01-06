﻿using System;
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
    public class user : INotifyPropertyChanged, INotifyPropertyChanging
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
        public string UserId
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
}
