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
using konto;
using System.IO;
using System.Text;

namespace konto
{
    public partial class Signup : PhoneApplicationPage
    {
        public static string json;
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

        class dataFromSignUpURL
        {
            public string status;
            public string message;
        }

        public void SignupUser(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Sign up");
            string _firstname = firstname.Text;
            string _lastname = lastname.Text;
            string _username = username.Text;
            string _password = password.Password;

            var config = new urlConfig();
            var dataToSend = new DataObject();
            dataToSend.data = new List<Item>{
                new Item {
                     username = _username,
                     firstname = _firstname,
                     lastname = _lastname,
                     password = _password
                }
            };

            json = JsonConvert.SerializeObject(dataToSend.data[0]);
            
            string a = config.signupUrl();
            System.Diagnostics.Debug.WriteLine(a);
            PostJsonRequest();
        }


        private void PostJsonRequest()
        {
            var config = new urlConfig();
            string AuthServiceUri = config.signupUrl(); 
            HttpWebRequest spAuthReq = HttpWebRequest.Create(AuthServiceUri) as HttpWebRequest;
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

        void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {

            try
            {
                //System.Diagnostics.Debug.WriteLine(json);
                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                string responseString = "";
                Stream streamResponse = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse);
                responseString = reader.ReadToEnd();
                streamResponse.Close();
                reader.Close();
                response.Close();
                dataFromSignUpURL result = JsonConvert.DeserializeObject <dataFromSignUpURL>(responseString);

                if (result.status == "1")
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("You are signed up !!", "Konto", MessageBoxButton.OK)));
                    //MessageBox.Show("You are signed up !!", "Konto", MessageBoxButton.OKCancel);
                    Dispatcher.BeginInvoke(new Action(() => NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative))));
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(result.message);
                    //MessageBox.Show(result.message, "Konto", MessageBoxButton.OKCancel);
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        public void Q(){
            
        }
    }

    //Contents of json
}