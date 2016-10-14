using BVNetwork.Attend.Forms.Models.Forms;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Forms.Business.Rendering
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class AttendTemplateCoordinator : IViewTemplateModelRegistrator
    {
        public const string BlockFolder = "~/modules/BVNetwork.Attend.Forms/Views/Forms/";

        public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {
        }

        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
            viewTemplateModelRegistrator.Add(typeof(AttendSubmitButton), new TemplateModel
            {
                Path = FormPath("AttendSubmitButton.ascx")
            });

            viewTemplateModelRegistrator.Add(typeof(AttendSessionForm), new TemplateModel
            {
                Path = FormPath("AttendSessionForm.ascx")
            });

        }

        private static string FormPath(string fileName)
        {
            return string.Format("{0}{1}", BlockFolder, fileName);
        }

    }
}