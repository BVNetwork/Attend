using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.ViewModels;
using Castle.Core.Internal;
using BVNetwork.Attend.Business.Text;
using EPiServer.Web.Routing;
using BVNetwork.Attend.Business.Log;
using BVNetwork.Attend.Business.API;
using EPiServer.DataAccess;
using EPiServer.Security;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Pages;
using BVNetwork.Attend.Business.Extensions;

namespace BVNetwork.Attend.Controllers
{
    [TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialController, Inherited = true, AvailableWithoutTag =true, Default = true)]

    public class MyPageBlockController : BlockController<MyPageBlock>
    {
        private IContentLoader _contentLoader;
        private IContentRepository _contentRepository;
        private UrlResolver _urlResolver;

        public MyPageBlockController(IContentLoader contentLoader, UrlResolver urlResolver, IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _contentRepository = contentRepository;
        }


        public override ActionResult Index(MyPageBlock currentBlock)
        {
            var model = new MyPageBlockViewModel<MyPageBlock>(currentBlock);
            string email = Request.QueryString["email"];
            string code = Request.QueryString["code"];
            string cancelEvent = Request.QueryString["cancelEvent"];
            if (email.IsNullOrEmpty() || code.IsNullOrEmpty())
                return View("~/Modules/BVNetwork.Attend/Views/Blocks/MyPageBlock/EmptyView.cshtml", model);  //No results found, code not matching email.



            var currentParticipant = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetParticipant(email, code);

            if (currentParticipant == null)
                return View("~/Modules/BVNetwork.Attend/Views/Blocks/MyPageBlock/EmptyView.cshtml", model);

            ////Set name
            if (currentParticipant != null)
            {
                model.ParticipantName = currentParticipant.Username;
            }


            //Cancel event?
            if (cancelEvent == "true" && currentParticipant != null)
            {
                var entry = (currentParticipant as BlockData).CreateWritableClone() as ParticipantBlock;
                if (entry.AttendStatus != AttendStatus.Cancelled.ToString())
                {
                    entry.AttendStatus = AttendStatus.Cancelled.ToString();
                    ParticipantLog.AddLogText("Status change", "Status changed to ", entry);
                    _contentRepository.Save(entry as IContent, SaveAction.Publish, AccessLevel.NoAccess);
                    AttendRegistrationEngine.SendStatusMail(entry);
                }
            }


            model.RegistrationCode = code;
            model.Email = email;

            var entries = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetParticipantByEmail(email);


            foreach (var entry in entries)
            {
                if (entry.EventPage == null || entry.EventPage == PageReference.EmptyReference)
                    continue;
                model.AllEntries.Add(entry as ParticipantBlock);

                var eventPage = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetEventPageBase(entry);
                if ((eventPage).EventDetails.EventEnd > DateTime.Now) //&& (entry.AttendStatus.Contains(AttendStatus.Confirmed.ToString()) || entry.AttendStatus == AttendStatus.Submitted.ToString() || entry.AttendStatus == AttendStatus.Standby.ToString()))
                    model.UpcomingEntries.Add(entry as ParticipantBlock);
                else
                    model.PastEntries.Add(entry as ParticipantBlock);
            }
            model.UpcomingEntries = model.UpcomingEntries.OrderBy(x => x.EventPageData().EventDetails.EventEnd).ToList();

            if (model.UpcomingEntries.Count == 0)
            {
                return View("~/Modules/BVNetwork.Attend/Views/Blocks/MyPageBlock/EmptyView.cshtml", model);  //No results found, code not matching email.
            }

            model.CurrentBlock = currentBlock;
            var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.PageRouteHelper>();
            model.CurrentPage = pageRouteHelper.Page;

            return View("~/Modules/BVNetwork.Attend/Views/Blocks/MyPageBlock/Index.cshtml", model);  //No results found, code not matching email.
        }



    }
}
