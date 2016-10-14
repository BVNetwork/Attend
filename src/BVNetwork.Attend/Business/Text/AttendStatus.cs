using EPiServer.Framework.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Text
{
    public enum AttendStatus { Undefined = -1, Deleted = 0, Submitted = 1, Standby = 2, Cancelled = 3, Confirmed = 4, Participated = 5 };

    public class AttendStatusText
    {
        public static string Get(AttendStatus status)
        {
            return Get(status.ToString());
        }

        public static string Get(string status)
        {
            return LocalizationService.Current.GetString("/attend/attendstatus/" + status);
        }
    }

}