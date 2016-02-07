using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PlingAgent
{
    public class Event
    {
        public string sourcemac;
        public string description;
        public string latlong;


        public Event(string desc, string source, string latlong)
        {
            description = desc;
            sourcemac = source;
            this.latlong = latlong;

        }

        public Event(string json)
        {
            Dictionary<string, string> jsonContents = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            jsonContents.TryGetValue("description", out description);
            jsonContents.TryGetValue("sourcemac", out sourcemac);
            jsonContents.TryGetValue("location", out latlong);

        }

        

        public string toJSONString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}