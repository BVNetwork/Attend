using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;

namespace BVNetwork.Attend.Admin.MasterPages
{
    public partial class AttendAdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((System.Web.Security.Roles.IsUserInRole("WebAdmins") || System.Web.Security.Roles.IsUserInRole("Administrators") || System.Web.Security.Roles.IsUserInRole("CmsAdmins") || System.Web.Security.Roles.IsUserInRole("AttendAdmins")) == false)
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
        }
    }
}