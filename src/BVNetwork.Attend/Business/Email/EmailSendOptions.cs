using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Email
{
        public enum SendOptions
        {
            Specific = 1, Relative = 2, Action = 3 
        }

        public enum RelativeUnit
        {
            Months, Days, Hours, Minutes
        }

        public enum RelativeTo
        {
            BeforeStartPublish, AfterStartPublish, BeforeEventStart, AfterEventStart, 
        }

}