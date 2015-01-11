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
using System.IO;
using System.Text;
using konto.Resources;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace konto
{
    public partial class Login : PhoneApplicationPage
    {
        private DbDataContext userDB;
        public static string json;
        
        public Login()
        {
            InitializeComponent();
            userDB = new DbDataContext(DbDataContext.DBConnectionString);
            this.DataContext = this;
            
        }

        public class Item
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class DataObject
        {
            public List<Item> data { get; set; }
        }

        public class dataFromLoginUrl
        {
            public string firstname { get; set; }
            public string username { get; set; }
            public int status { get; set; }
            public string user_id { get; set; }
            public string lastname { get; set; }
            public string message { get; set; }
        }

        public class cookieFromLoginUrl
        {
            public string Comment { get; set; }
            public string CommentUri { get; set; }
            public bool HttpOnly { get; set; }
            public bool Discard { get; set; }
            public string Domain { get; set; }
            public string Expired { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Port { get; set; }
            public bool Secure { get; set; }
            public string TimeStamp { get; set; }
            public string Value { get; set; }
            public int Version { get; set; }
        }

        public void LoginUser(object sender, RoutedEventArgs e)
        {
            string _username = username.Text;
            string _password = password.Password;

            var dataToSend = new DataObject();
            dataToSend.data = new List<Item>{
                new Item {
                     username = _username,
                     password = _password
                }
            };

            json = JsonConvert.SerializeObject(dataToSend.data[0]);
            PostJsonRequest();
        }



        private void PostJsonRequest()
        {
            var config = new urlConfig();
            string AuthServiceUri = config.loginUrl();
            HttpWebRequest spAuthReq = HttpWebRequest.Create(AuthServiceUri) as HttpWebRequest;
            spAuthReq.CookieContainer = new CookieContainer();
            spAuthReq.ContentType = "application/json";
            spAuthReq.Method = "POST";

            spAuthReq.Headers["Authorization"] = config.apiKey();
            spAuthReq.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), spAuthReq);
        }

        void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);
            string postData = json;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }

        public void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                string cookies = JsonConvert.SerializeObject(response.Cookies);

                System.Diagnostics.Debug.WriteLine(cookies);
                
                string responseString = "";
                Stream streamResponse = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse);
                responseString = reader.ReadToEnd();
                
                streamResponse.Close();
                reader.Close();
                response.Close();
                responseString = responseString.Replace("\"{", "{");
                responseString = responseString.Replace("}\"", "}");
                dataFromLoginUrl result = JsonConvert.DeserializeObject<dataFromLoginUrl>(responseString.Replace("\\", ""));
                konto.loggedInPageHelper _loggedInPage = new konto.loggedInPageHelper();
                List<cookieFromLoginUrl> _cookie = JsonConvert.DeserializeObject<List<cookieFromLoginUrl>>(cookies);
                //cookieFromLoginUrl _cookie = JsonConvert.DeserializeObject<cookieFromLoginUrl>(cookies);
                //System.Diagnostics.Debug.WriteLine(_cookie);

                if (result.status == 1)
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("You are Logged in !!", "Konto", MessageBoxButton.OK)));
                    _loggedInPage.helperFunc(result, _cookie);
                    var p = new List<httpHelper.noticeGet>
                    {
                        new httpHelper.noticeGet {
                            unread = 1
                        }
                    };

                    httpHelper.RequestSender(p[0], 0);
                    Dispatcher.BeginInvoke(new Action(() => NavigationService.Navigate(new Uri("/LoggedInPage.xaml", UriKind.Relative))));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(result.message, "Konto", MessageBoxButton.OK)));
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("You do not have working internet", "Konto", MessageBoxButton.OK)));
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        
    }
}