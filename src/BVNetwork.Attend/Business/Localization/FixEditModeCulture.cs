using System.Globalization;


namespace BVNetwork.Attend.Business.Localization
{

    public static class FixEditModeCulture
    {

        public static void TryToFix()
        {

            var prefCulture = new CultureInfo(EPiServer.Personalization.EPiServerProfile.Current.Language);
            if (prefCulture != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = prefCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = prefCulture;
            }
        }

      

    }

}