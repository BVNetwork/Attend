using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BVNetwork.Attend.Business.Participant
{
    public class ParticipantProviderManager
    {
        private static ParticipantProviderBase defaultProvider;

        public static void Initialize()
        {
            const string fallbackProviderSetting = "BVNetwork.Attend.Business.Participant.BlockProvider.BlockParticipantProvider";
            string defaultProviderSetting = Settings.Settings.GetSetting("DefaultParticipantProviderString");
            if (string.IsNullOrEmpty(defaultProviderSetting))
                defaultProviderSetting = fallbackProviderSetting;

            defaultProvider = GetParticipantProvider(defaultProviderSetting) as ParticipantProviderBase ??
                GetParticipantProvider(fallbackProviderSetting) as ParticipantProviderBase;
        }

        public static Type[] GetParticipantProviders()
        {

            var type = typeof(ParticipantProviderBase);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            System.Collections.Generic.List<Type> providerTypes = new System.Collections.Generic.List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    foreach (var assemblyType in assembly.GetTypes())
                    {
                        try
                        {
                            if (type.IsAssignableFrom(assemblyType) && type != assemblyType)
                                providerTypes.Add(assemblyType);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
                catch (Exception e)
                {

                }
            }

            return providerTypes.ToArray<Type>();

        }

        public static ParticipantProviderBase Provider
        {
            get
            {
                if (defaultProvider == null)
                    Initialize();
                return defaultProvider;
            }
        }

        private static ProviderBase GetParticipantProvider(string providerType)
        {
            try
            {
                ParticipantProviderBase base2 = null;
                if (string.IsNullOrEmpty(providerType))
                {
                    throw new ArgumentException("Provider type is invalid");
                }
                Type c = Type.GetType(providerType, true, true);
                if (!typeof(ParticipantProviderBase).IsAssignableFrom(c))
                {
                    throw new ArgumentException(String.Format("Provider must implement type {0}.",
                        typeof(ParticipantProviderBase).ToString()));
                }
                base2 = (ParticipantProviderBase)Activator.CreateInstance(c);
                var config = new NameValueCollection();
                config.Add("name", providerType);
                base2.ProviderName = providerType;

                base2.Initialize(providerType, config);
                return base2;
            }
            catch (Exception e)
            {

                return null;
            }

        }


    }
}