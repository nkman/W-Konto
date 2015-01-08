using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using konto.Resources;

namespace konto
{
    class httpHelper
    {
        static string json;
        static int variableUrlLocal;

        private static DbDataContext userDB;
        private static ObservableCollection<Cookies> _cookieDetail;

        public static cookieDb cake;

        public static ObservableCollection<Cookies> cookies
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
                }
            }
        }

        public httpHelper()
        {
            
        }

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
            public int unread { get; set; }
        }

        public class noticeAcceptDeclineDelete
        {
            string account_id { get; set; }
            string decision { get; set; }
        }

        
        internal static void RequestSender(dynamic variableData, int variableUrl)
        {
            var config = new urlConfig();
            string AuthServiceUri;

            variableUrlLocal = variableUrl;
            switch (variableUrlLocal)
            {
                case 0:
                    AuthServiceUri = config.notificationUrl();
                    json = JsonConvert.SerializeObject((noticeGet)variableData);
                    break;
                case 1:
                    AuthServiceUri = config.notificationReadUrl();
                    json = JsonConvert.SerializeObject((noticeRead)variableData);
                    break;
                case 2:
                    AuthServiceUri = config.notificationAcceptUrl();
                    json = JsonConvert.SerializeObject((noticeAcceptDeclineDelete)variableData);
                    break;
                case 3:
                    AuthServiceUri = config.notificationDeclineUrl();
                    json = JsonConvert.SerializeObject((noticeAcceptDeclineDelete)variableData);
                    break;
                case 4:
                    AuthServiceUri = config.notificationDeleteUrl();
                    json = JsonConvert.SerializeObject((noticeAcceptDeclineDelete)variableData);
                    break;
                default:
                    AuthServiceUri = config.notificationUrl();
                    json = JsonConvert.SerializeObject((noticeGet)variableData);
                    break;
            }
            
            HttpWebRequest spAuthReq = HttpWebRequest.Create(AuthServiceUri) as HttpWebRequest;
            spAuthReq.CookieContainer = new CookieContainer();
            addCookieInReq();
            spAuthReq.CookieContainer.Add(new Uri(config.homeUrl()), new Cookie("tea", cake.tea));
            spAuthReq.CookieContainer.Add(new Uri(config.homeUrl()), new Cookie("user", cake.user));

            spAuthReq.ContentType = "application/json";
            spAuthReq.Method = "POST";
            spAuthReq.Headers["Authorization"] = config.apiKey();
            spAuthReq.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), spAuthReq);
        }

        static void GetRequestStreamCallback(IAsyncResult callbackResult)
        {
            HttpWebRequest myRequest = (HttpWebRequest)callbackResult.AsyncState;
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);
            string postData = json;
            System.Diagnostics.Debug.WriteLine(postData);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }

        static void GetResponsetStreamCallback(IAsyncResult callbackResult)
        {
            dynamic result;
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

                System.Diagnostics.Debug.WriteLine(responseString);
                switch (variableUrlLocal)
                {
                    case 0:
                        result = JsonConvert.DeserializeObject<RootObject>(responseString);
                        System.Diagnostics.Debug.WriteLine(((RootObject)result).negetive.negetive[0][0]);
                        
                        break;
                    case 1:
                        result = JsonConvert.DeserializeObject<noticeRead>(responseString);
                        break;
                    case 2:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDelete>(responseString);
                        break;
                    case 3:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDelete>(responseString);
                        break;
                    case 4:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDelete>(responseString);
                        break;
                    default:
                        result = JsonConvert.DeserializeObject<RootObject>(responseString);
                        break;
                }

                //System.Diagnostics.Debug.WriteLine(result.ToString());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        static public void addCookieInReq()
        {
            cake = new cookieDb();
            userDB = new DbDataContext(DbDataContext.DBConnectionString);
            var cookieInDB = from Cookies _cookie_ in userDB.cookies select _cookie_;

            try
            {
                cookies = new ObservableCollection<Cookies>(cookieInDB);
                //System.Diagnostics.Debug.WriteLine(cookies);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }


            var dbData = cookies.ToList();
            cake.tea = dbData[0].tea;
            cake.user = dbData[0].user;

        }

        public class cookieDb
        {
            public string tea { set; get; }
            public string user { set; get; }
        }
    }
}
