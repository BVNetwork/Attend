using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Business.Log;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Localization.XmlResources;
using EPiServer.Web.Hosting;
using System.IO;
using System.Collections.Specialized;

namespace BVNetwork.Attend.Business.InitializableModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class AttendLocalizationProviderInitializer : IInitializableModule
    {
        private const string ProviderName = "AttendProvider";

        public void Initialize(InitializationEngine context)
        {

            ProviderBasedLocalizationService localizationService = context.Locate.Advanced.GetInstance<LocalizationService>() as ProviderBasedLocalizationService;
            if (localizationService != null)
            {
                string langFolder = System.Web.HttpContext.Current.Server.MapPath(@"~/Modules/BVNetwork.Attend/Resources/LanguageFiles/");
                if (Directory.Exists(langFolder))
                {
                    NameValueCollection configValues = new NameValueCollection();
                    configValues.Add(FileXmlLocalizationProvider.PhysicalPathKey, langFolder);
                    FileXmlLocalizationProvider localizationProvider = new FileXmlLocalizationProvider();
                    localizationProvider.Initialize(ProviderName, configValues);
                    localizationService.AddProvider(localizationProvider);
                }
            }

        }

        public void Uninitialize(InitializationEngine context)
        {
            ProviderBasedLocalizationService localizationService = context.Locate.Advanced.GetInstance<LocalizationService>() as ProviderBasedLocalizationService;
            if (localizationService != null)
            {
                LocalizationProvider localizationProvider = localizationService.ProviderList.FirstOrDefault(p => p.Name.Equals(ProviderName, StringComparison.Ordinal));
                if (localizationProvider != null)
                {
                    localizationService.RemoveProvider(localizationProvider.Name);
                }
            }
        }

        public void Preload(string[] parameters)
        {
        }
    }
}