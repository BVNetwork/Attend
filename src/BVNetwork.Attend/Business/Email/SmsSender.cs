using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace BVNetwork.Attend.Business.Email
{
    public class SmsSender
    {
        public static string SendSMS(string to, string from, string message)
        {
            using (var client = new HttpClient())
            {
                //https://sveve.no/SMS/SendMessage?user=bvnetwor&passwd=dffdq&to={0}&from={1}&msg={2}
                client.BaseAddress = new Uri(string.Format(Settings.Settings.GetSetting("smsurl"),to, from, message));
                var result = client.GetAsync("").Result;
                return result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}