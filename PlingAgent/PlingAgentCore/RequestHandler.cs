using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlingAgentCore
{
    class RequestHandler
    {
        public static ConcurrentQueue<Event> ActiveEvents;
        public static ConcurrentQueue<Account> Accounts = new ConcurrentQueue<Account>();
        public static ConcurrentQueue<ActiveSession> ActiveSessions;

        public static string JSONHandle(String JSON)
        {
            Request rq = JsonConvert.DeserializeObject<Request>(JSON);

            switch (rq.request_type)
            {
                case "login": LoginRequest(rq.content);
                    break;
                case "register": RegisterRequest(rq.content);
                    break;
                case "heartbeat":HeartbeatRequest(rq.content);
                    break;
                case "host_event": HostRequest(rq.content);
                    break;
                case "view_events": ViewRequest(rq.content);
                    break;
                case "remove_event": removeEventRequest(rq.content);
                    break;
                case "add_friend": AddFriendRequest(rq.content);
                    break;
                case "view_friends": ViewFriendsRequest(rq.content);
                    break;
                case "remove_friend": RemoveFriendRequest(rq.content);
                    break;
                default: otherRequest(rq.content);
                    break;
            }

            

            string reply = "";
            return reply;

            
        }


        private static string LoginRequest(String JSON)
        {
            //Handle Login request
            Dictionary<string, string> doc = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);
            bool invalidlogin = true;
            foreach (Account a in Accounts)
            {
                //Login is invalid until proven otherwise
                if (a.name == doc["name"]) invalidlogin = false ;
                if (a.passhash != doc["passhash"]) invalidlogin = true;
            }

            if (invalidlogin)
            {
                Dictionary<string, string> reply2 = new Dictionary<string, string>()
                {
                    //Account exists error code
                    {"reply","1" }
                };
                return JsonConvert.SerializeObject(reply2, Formatting.None);
            }

            ActiveSession session = new ActiveSession();
            session.username = doc["name"];
            session.lastbeat = DateTime.Now;
            ActiveSessions.Enqueue(session);

            Dictionary<string, string> reply = new Dictionary<string, string>()
            { 
                    //Give good reply
                    {"reply","0"}
            };
            return JsonConvert.SerializeObject(reply, Formatting.None);
        }
        


        private static string RegisterRequest(String JSON)
        {
            Dictionary<string, string> doc = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);

            bool accountExists = false;

            foreach (Account a in Accounts)
            {
                //Login is invalid until proven otherwise
                if (a.name == doc["name"]) accountExists = true;
            }

            Account account = new Account();
            account.name     = doc["name"];
            account.passhash = doc["passhash"];




            Dictionary<string, string> reply = new Dictionary<string, string>()
            { 
                    //Give good reply
                    {"reply","0"}
            };
            return JsonConvert.SerializeObject(reply, Formatting.None); ;
        }

        private static string HeartbeatRequest(String JSON)
        {
            //handle heartbeat request

            Dictionary<string, string> doc = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);

            foreach (ActiveSession A in ActiveSessions)
            {
                if (A.username == doc["name"]) A.lastbeat = DateTime.Now;
            }

            Dictionary<string, string> reply = new Dictionary<string, string>()
            { 
                    //Give good reply
                    {"reply","0"}
            };
            return JsonConvert.SerializeObject(reply, Formatting.None); 
        }

        private static string HostRequest(String JSON)
        {
            //handle host request

            Dictionary<string, string> doc = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);

            foreach (Event ev in ActiveEvents)
            {
                if (ev.owner == doc["name"])
                {
                    Dictionary<string, string> reply = new Dictionary<string, string>()
                    { 
                    //Give event exists reply
                    {"reply","1"}
                    };
                    return JsonConvert.SerializeObject(reply, Formatting.None);

                }
            }

            Event e = new Event();
            e.description = doc["desc"];
            e.title = doc["title"];
            e.latlong = doc["latlng"];
            e.owner = doc["name"];

            ActiveEvents.Enqueue(e);

            Dictionary<string, string> reply = new Dictionary<string, string>()
            { 
                //Give event exists reply
                {"reply","0"}
            };
            return JsonConvert.SerializeObject(reply, Formatting.None);


        }

        private static string ViewRequest(String JSON)
        {
            
            
            
            
            
            //handle view request
        }

        private static string AddFriendRequest(String JSON)
        {
            //handle add friend request
        }

        private static string removeEventRequest(String JSON)
        {
            //handle remove event request ehre
        }

        private static string RemoveFriendRequest(String JSON)
        {
            //Handle remove friend request
        }

        private static string ViewFriendsRequest(String JSON)
        {
            //Handle view friend request
        }


        private static void otherRequest(string content)
        {

        }


        private class Request
        {
            public string request_type;
            public string content;
        }
    }
}
