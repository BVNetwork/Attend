using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Settings
{

    public class Settings
    {
        public static DynamicDataStore SettingsDataStore { get { return typeof(DDSSetting).GetStore(); } }
        
        public static string GetSetting(string settingName) {
            var query = (from s in SettingsDataStore.Items<DDSSetting>() where s.Name == settingName select s.Value);
            if (query.Count<string>() > 0)
                return query.First<string>();
            return string.Empty;
        }

        public static void AddSetting(string settingName, string settingValue) {
            var query = (from s in SettingsDataStore.Items<DDSSetting>() where s.Name == settingName select s);
            if (query.Count<DDSSetting>() > 0)
            {
                DDSSetting exisiting = query.First<DDSSetting>();
                exisiting.Value = settingValue;
                SettingsDataStore.Save(exisiting);
            }
            else
            {
                DDSSetting newSetting = new DDSSetting() { Value = settingValue, Name = settingName };
                SettingsDataStore.Save(newSetting);
            }
        }
    }
}