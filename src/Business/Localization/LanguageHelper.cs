using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;

namespace BVNetwork.Attend.Business.Localization
{
    public class LanguageHelper
    {
        public static string MasterLanguage(ContentReference EventPageBase)
        {
            return
                ServiceLocator.Current.GetInstance<IContentRepository>().Get<PageData>(EventPageBase).MasterLanguageBranch;
        }

        public static string GetHelpText(string key)
        {
            return LocalizationService.Current.GetString(string.Format("/attend/help/{0}/content", key)).Replace("[", "<").Replace("]", ">");
        }

        public static string GetHelpTitle(string key)
        {
            return LocalizationService.Current.GetString(string.Format("/attend/help/{0}/title", key));
        }
    }
}