using System;
using System.Web.Security;
using System.Web.UI;
using EPiServer.Security;

namespace BVNetwork.Attend.Admin.MasterPages
{
    public partial class AttendAdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((PrincipalInfo.CurrentPrincipal.IsInRole("WebAdmins") || PrincipalInfo.CurrentPrincipal.IsInRole("Administrators") || PrincipalInfo.CurrentPrincipal.IsInRole("CmsAdmins") || PrincipalInfo.CurrentPrincipal.IsInRole("AttendAdmins")) == false)
                FormsAuthentication.RedirectToLoginPage();
        }
    }
}