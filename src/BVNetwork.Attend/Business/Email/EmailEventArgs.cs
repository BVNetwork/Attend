using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Email
{
    public class EmailEventArgs : EventArgs
    {
        public bool CancelEmail;
        public bool Success;
        public EmailTemplate Email;
    }
}