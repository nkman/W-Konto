using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace konto
{
    class urlConfig
    {
        public string homeUrl()
        {
            string homeurl = "http://shyamu.herokuapp.com";
            return homeurl;
        }

        public string signupUrl()
        {
            string signupurl = "/mobile/signup";
            return homeUrl() + signupurl;
        }

        public string loginUrl()
        {
            string loginurl = "/mobile/login";
            return homeUrl() + loginurl;
        }

        public string notificationUrl()
        {
            string notificationurl = "/mobile/notification";
            return homeUrl() + notificationurl;
        }

        public string logOutUrl()
        {
            string logouturl = "/";
            return homeUrl() + logouturl;
        }

        public string addUrl()
        {
            string addurl = "/mobile/add";
            return homeUrl() + addurl;
        }

        public string notificationReadUrl()
        {
            string notificationreadurl = "/mobile/notification/read";
            return homeUrl() + notificationreadurl;
        }

        public string notificationDeleteUrl()
        {
            string notificationdeleteurl = "/mobile/notification/delete";
            return homeUrl() + notificationdeleteurl;
        }

        public string notificationDeclineUrl()
        {
            string notificationdeclineurl = "/mobile/notification/decline";
            return homeUrl() + notificationdeclineurl;
        }

        public string notificationAcceptUrl()
        {
            string notificationaccepturl = "/mobile/notification/accept";
            return homeUrl() + notificationaccepturl;
        }

        public string apiKey()
        {
            return "de0464e0-552e-11e4-8c11-843497188779";
        }
    }
}
