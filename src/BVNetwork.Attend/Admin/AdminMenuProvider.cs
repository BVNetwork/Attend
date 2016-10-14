using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Admin
{
    [MenuProvider]
    public class AdminMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems() {
            var localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();
            return new List<MenuItem> {
                new SectionMenuItem(localizationService.GetString("/attend/admin/attend"), "/global/Attend"),
                new UrlMenuItem(localizationService.GetString("/attend/admin/participants"), "/global/Attend/participants","/Modules/BVNetwork.Attend/Admin/Participants.aspx"),
                new UrlMenuItem(localizationService.GetString("/attend/admin/events"), "/global/Attend/Events","/Modules/BVNetwork.Attend/Admin/Events.aspx"),
                new UrlMenuItem(localizationService.GetString("/attend/admin/settings"), "/global/Attend/esettings","/Modules/BVNetwork.Attend/Admin/SettingsEdit.aspx"),
                new UrlMenuItem(localizationService.GetString("/attend/admin/invoicing"), "/global/Attend/cinvoice","/Modules/BVNetwork.Attend/Admin/Invoicing.aspx"),
                new UrlMenuItem(localizationService.GetString("/attend/admin/import"), "/global/Attend/dimport","/Modules/BVNetwork.Attend/Admin/Import.aspx"),
                new UrlMenuItem(localizationService.GetString("/attend/admin/scheduledemail"), "/global/Attend/dimport","/Modules/BVNetwork.Attend/Admin/ScheduledEmails.aspx")
                    };
        }
    }
}