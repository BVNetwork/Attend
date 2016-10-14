using System;
using System.Linq;
using System.Web.Mvc;
using BVNetwork.Attend.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace BVNetwork.Attend.Business.InitializableModules
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ViewEngineInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ViewEngineRegistration.Register(ViewEngines.Engines);
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}