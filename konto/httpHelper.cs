using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace konto
{
    class httpHelper
    {
        public class noticeRead
        {
            string notice_id { get; set; }
        }

        public class transactionAdd
        {
            string fellow_username { get; set; }
            int amount { get; set; }
            string decision { get; set; }
        }

        public class noticeGet
        {
            int unread { get; set; }
        }

        public class noticeAcceptDeclineDelete
        {
            string account_id { get; set; }
            string decision { get; set; }
        }

        /*
        private void PostJsonRequest(dynamic variableData, int variableUrl)
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
                HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
                string responseString = "";
                Stream streamResponse = response.GetResponseStream();
                StreamReader reader = new StreamReader(streamResponse);
                responseString = reader.ReadToEnd();
                streamResponse.Close();
                reader.Close();
                response.Close();
                konto.Signup.dataFromSignUpURL result = JsonConvert.DeserializeObject<konto.Signup.dataFromSignUpURL>(responseString);

                if (result.status == "1")
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("You are signed up !!", "Konto", MessageBoxButton.OK)));
                    Dispatcher.BeginInvoke(new Action(() => NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative))));
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show(result.message, "Konto", MessageBoxButton.OK)));
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
         * */
    }
}
