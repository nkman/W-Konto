using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace konto
{
    public partial class Signup : PhoneApplicationPage
    {
        public Signup()
        {
            InitializeComponent();
        }

        public class Item
        {

            public string firstname { get; set; }
            public string lastname { get; set; }
            public string username { get; set; }
            public string password { get; set; }
        }

        public class DataObject
        {
            public List<Item> data { get; set; }
        }

        public void SignupUser(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Sign up");
            string _firstname = firstname.Text;
            string _lastname = lastname.Text;
            string _username = username.Text;
            string _password = password.Password;

            var dataToSend = new DataObject();
            dataToSend.data = new List<Item>{
                new Item {
                     username = _username,
                     firstname = _firstname,
                     lastname = _lastname,
                     password = _password
                }
            };

            string json = JsonConvert.SerializeObject(dataToSend.data);
            
            //HttpWebRequest 
            //commits after this will implement signup
            System.Diagnostics.Debug.WriteLine(json);
        }
    }

    //Contents of json
}