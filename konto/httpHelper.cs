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
        public static System.IAsyncResult a;
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
            public string notice_id { get; set; }
        }

        public class transactionAdd
        {
            public string fellow_username { get; set; }
            public int amount { get; set; }
            public string sign { get; set; }
        }

        public class noticeGet
        {
            public int unread { get; set; }
        }

        public class noticeAcceptDeclineDelete
        {
            public string account_id { get; set; }
            public string decision { get; set; }
        }

        public class noticeAcceptDeclineDeleteResponse
        {
            public int status { set; get; }
            public string message { set; get; }
        }

        internal static IAsyncResult RequestSender(dynamic variableData, int variableUrl)
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
                case 5:
                    AuthServiceUri = config.addUrl();
                    json = JsonConvert.SerializeObject((transactionAdd)variableData);
                    break;
                case 6:
                     AuthServiceUri = config.getallUrl();
                     json = "";
                    break;
                default:
                    AuthServiceUri = config.notificationUrl();
                    json = JsonConvert.SerializeObject((noticeGet)variableData);
                    break;
            }
            
            HttpWebRequest spAuthReq = HttpWebRequest.Create(AuthServiceUri) as HttpWebRequest;
            spAuthReq.CookieContainer = new CookieContainer();
            addCookieInReq();

            if (cake.tea != null && cake.user != null)
            {
                spAuthReq.CookieContainer.Add(new Uri(config.homeUrl()), new Cookie("tea", cake.tea));
                spAuthReq.CookieContainer.Add(new Uri(config.homeUrl()), new Cookie("user", cake.user));
            }
            

            spAuthReq.ContentType = "application/json";
            spAuthReq.Method = "POST";
            spAuthReq.Headers["Authorization"] = config.apiKey();
            return spAuthReq.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), spAuthReq);
            //a.AsyncWaitHandle.WaitOne();
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

        public static void GetResponsetStreamCallback(IAsyncResult callbackResult)
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
                        if (result.status == 1)
                        {
                            RootObject R = (RootObject)result;
                            iterateSavingNotification(R);
                        }
                        httpHelper.RequestSender("{}", 6);
                        break;
                    case 1:
                        result = JsonConvert.DeserializeObject<noticeRead>(responseString);
                        break;
                    case 2:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDeleteResponse>(responseString);
                        if (result.status == 1)
                        {
                            loggedInPageHelper _l = new loggedInPageHelper();
                            noticeAcceptDeclineDelete __json = JsonConvert.DeserializeObject<noticeAcceptDeclineDelete>(json);
                            Notification n = new Notification
                            {
                                Notice_id = __json.account_id
                            };
                            _l.delFromNotificationHelper(n);
                        }
                        break;
                    case 3:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDeleteResponse>(responseString);
                        if (result.status == 1)
                        {
                            loggedInPageHelper _l = new loggedInPageHelper();
                            noticeAcceptDeclineDelete __json = JsonConvert.DeserializeObject<noticeAcceptDeclineDelete>(json);
                            Notification n = new Notification
                            {
                                Notice_id = __json.account_id
                            };
                            _l.delFromNotificationHelper(n);
                        }
                        break;
                    case 4:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDeleteResponse>(responseString);
                        break;
                    case 5:
                        result = JsonConvert.DeserializeObject<noticeAcceptDeclineDeleteResponse>(responseString);
                        break;
                    case 6:
                        result = JsonConvert.DeserializeObject<RealDataYo>(responseString);
                        RealDataYo _R = (RealDataYo)result;
                        if (result.status == 1)
                        {
                            iterateSavingRealData(_R);
                        }
                        break;
                    default:
                        result = JsonConvert.DeserializeObject<RootObject>(responseString);
                        break;
                }

                if (result.status == 1)
                {
                    System.Diagnostics.Debug.WriteLine("Done");
                }
                else  if (result.status == 0)
                {
                    System.Diagnostics.Debug.WriteLine(result.message);
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
                var dbData = cookies.ToList();
                cake.tea = dbData[0].tea;
                cake.user = dbData[0].user;
                //System.Diagnostics.Debug.WriteLine(cookies);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

        }

        public class cookieDb
        {
            public string tea { set; get; }
            public string user { set; get; }
        }

        public static void iterateSavingNotification(RootObject _result)
        {
            
            Negetive _negetive = _result.negetive;
            loggedInPageHelper _loggedInPageHelper = new loggedInPageHelper();
            for (int i = 0; i < _negetive.negetive.Count; i++)
            {
                //System.Diagnostics.Debug.WriteLine(_negetive.negetive[i][3]);
                Notification _notificationInDb = new Notification
                {
                    Amount = Convert.ToInt32(_negetive.negetive[i][3]),
                    Notice_id = (string)_negetive.negetive[i][0],
                    IsPositive = false,
                    IsNegetive = true,
                    IsTracking = false,
                    Name = "You owe "+_negetive.name[i]
                };
                _loggedInPageHelper.notificationPopulator(_notificationInDb);
                //NotificationSaveInDb(_notificationInDb);
            }

            Positive _positive = _result.positive;
            for (int i = 0; i < _positive.positive.Count; i++)
            {
                Notification _notificationInDb = new Notification
                {
                    Amount = Convert.ToInt32(_positive.positive[i][3]),
                    Notice_id = (string)_positive.positive[i][0],
                    IsPositive = true,
                    IsNegetive = false,
                    IsTracking = false,
                    Name = _positive.name[i]+" owes you"
                };
                _loggedInPageHelper.notificationPopulator(_notificationInDb);
                //NotificationSaveInDb(_notificationInDb);
            }

            Track _track = _result.track;
            for (int i = 0; i < _track.unread.Count; i++)
            {
                Notification _notificationInDb = new Notification
                {
                    Notice_id = (string)_track.unread[i][0],
                    IsPositive = false,
                    IsNegetive = false,
                    IsTracking = true,
                    Name = (string)_track.unread[i][2]
                };
                //_loggedInPageHelper.notificationPopulator(_notificationInDb);
                //NotificationSaveInDb(_notificationInDb);
            }
        }

        public static void iterateSavingRealData(RealDataYo _result)
        {
            loggedInPageHelper _loggedInPageHelper = new loggedInPageHelper();
            for (int i = 0; i < _result.negetive_name.Count; i++)
            {
                RealData __realdata = new RealData();
                __realdata.Name = "You owe " + (string)_result.negetive_name[i];
                __realdata.Amount = Convert.ToInt32(_result.negetive[i][3]);
                __realdata.Notice_id = (string)_result.negetive[i][0];
                __realdata.IsNegetive = true;
                __realdata.IsPositive = false;
                /*{
                    Name = "You owe " + (string)_result.negetive_name[i],
                    Amount = Int32.Parse((string)_result.negetive[i][3]),
                    Notice_id = (string)_result.negetive[i][0],
                    IsNegetive = true,
                    IsPositive = false
                };
                 * */
                _loggedInPageHelper.realdatapopulator(__realdata);
            }

            for (int i = 0; i < _result.positive_name.Count; i++)
            {
                RealData __realdata = new RealData
                {
                    Name = (string)_result.positive_name[i] + " owes you",
                    Amount = Int32.Parse((string)_result.positive[i][3]),
                    Notice_id = (string)_result.positive[i][0],
                    IsNegetive = true,
                    IsPositive = false
                };
                _loggedInPageHelper.realdatapopulator(__realdata);
            }
        }
    }
}
