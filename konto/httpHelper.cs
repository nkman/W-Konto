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
        static string json;
        static int variableUrlLocal;

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

        
        internal static void RequestSender(dynamic variableData, int variableUrl)
        {
            var config = new urlConfig();
            string AuthServiceUri;

            variableUrlLocal = variableData;
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
                
                switch (variableUrlLocal)
                {
                    case 0:
                        result = JsonConvert.DeserializeObject<noticeGet>(responseString);
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
                        result = JsonConvert.DeserializeObject<noticeGet>(responseString);
                        break;
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
        

    }
}
