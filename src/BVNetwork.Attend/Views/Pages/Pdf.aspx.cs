using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Pdf;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Data;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Hosting;
using EPiServer.Web.Routing;

namespace BVNetwork.Attend.Views.Pages
{
    public partial class Pdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected IParticipant registration;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));

            string idstring = Request.QueryString["participant"];
            string email = Request.QueryString["email"];
            string code = Request.QueryString["code"];

            int id = 0;
            int.TryParse(idstring, out id);

            registration = AttendRegistrationEngine.GetParticipant(id);

            if (null == registration)
            {
                Response.Status = "404 Not Found";
                Response.StatusCode = 404;
                Response.End();
            }
            else
            {
                EventPageBase EventPageBaseData = EPiServer.DataFactory.Instance.GetPage(registration.EventPage) as EventPageBase;

                var file = ServiceLocator.Current.GetInstance<IContentRepository>().Get<MediaData>(EventPageBaseData.EventDetails.PdfTemplate);
                string pdfTemplateUrl = UrlResolver.Current.GetUrl(file.ContentLink);
                
                //string pdfTemplateUrl = EventPageBaseData.EventDetails.PdfTemplate;
                var filestream = file.BinaryData.OpenRead();
                

                VirtualFile vf = HostingEnvironment.VirtualPathProvider.GetFile(pdfTemplateUrl);

                string physicalPath = "";
                /*
                UnifiedFile uf = vf as UnifiedFile;
                if (null != uf)
                    physicalPath = uf.LocalPath;
                */
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=EventDiploma_{0}.pdf", code));

                PdfGenerator.Create(filestream, registration, EventPageBaseData, Response.OutputStream);

                Response.Flush();
                Response.End();
            }
        }
    }
}