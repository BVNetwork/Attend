using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;
using BVNetwork.Attend.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAccess;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Web;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.XForms;
using EPiServer.Web.Routing;
using EPiServer.XForms.Util;

namespace BVNetwork.Attend.Controllers
{
    [TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialController, Inherited = true)]
    public class EventPageBasePartialController : PageController<EventPageBase>
    {
        private XFormPageUnknownActionHandler _xformHandler;
        private IContentRepository _contentRepository;

        public EventPageBasePartialController(XFormPageUnknownActionHandler xformHandler)
        {
            _xformHandler = xformHandler;
            _contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        }

        public ActionResult Index(EventPageBase currentPage, string contentLink)
        {

            if (TempData["ViewData"] != null)
            {
                ViewData = (ViewDataDictionary)TempData["ViewData"];
            }

            var viewModel = CreateEventRegistrationModel(currentPage, contentLink);

            var pageRouteHelper = ServiceLocator.Current.GetInstance<IPageRouteHelper>();
            PageData hostPageData = pageRouteHelper.Page;

            if (currentPage != null && hostPageData != null)
            {


                var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
                var pageUrl = urlResolver.GetUrl(hostPageData.ContentLink);

                var actionUrl = string.Format("{0}/", pageUrl);
                if (BVNetwork.Attend.Business.API.AttendRegistrationEngine.UseForms == false)
                    actionUrl = UriSupport.AddQueryString(actionUrl, "XFormId", currentPage.RegistrationForm.Id.ToString());
                actionUrl = UriSupport.AddQueryString(actionUrl, "failedAction", "Failed");
                actionUrl = UriSupport.AddQueryString(actionUrl, "successAction", "Success");

                viewModel.ActionUrl = actionUrl;
            }

            //return PartialView(viewModel);
            return PartialView("~/modules/BVNetwork.Attend/Views/Pages/Partials/EventPagePartial.cshtml", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DoSubmit(XFormPostedData xFormpostedData)
        {
            return _xformHandler.HandleAction(this);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Index(EventPageBase currentPage, XFormPostedData xFormPostedData, string contentLink)
        {
            var model = CreateEventRegistrationModel(currentPage, contentLink);

            var id = (currentPage as IContent).ContentLink.ID;

            var viewDataKey = string.Format("TempViewData_{0}", id);
            this.ViewData["XFormFragments"] = (object)xFormPostedData.Fragments;
            this.ControllerContext.Controller.ViewData["XFormFragments"] = (object)xFormPostedData.Fragments;
            this.TempData[viewDataKey] = this.ViewData;
            model.ViewData = this.ViewData;
            model.ViewDataKey = viewDataKey;

            string participantEmail = "";
            foreach (var fragment in xFormPostedData.Fragments)
            {
                if (fragment as EPiServer.XForms.Parsing.InputFragment != null)
                {
                    string fragmentReference = (fragment as EPiServer.XForms.Parsing.InputFragment).Reference.ToLower();
                    if (fragmentReference == "epost" || fragmentReference == "email" || fragmentReference == BVNetwork.Attend.Business.Settings.Settings.GetSetting("emailFieldName"))
                    {
                        participantEmail = (fragment as EPiServer.XForms.Parsing.InputFragment).Value;
                    }
                }
            }

            string xformdata = new EPiServer.Web.Mvc.XForms.XFormPageHelper().GetXFormData(this, xFormPostedData).Data.InnerXml;

            var metadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(XFormPostedData));

            var validationResult = new XFormValidator(metadata, this.ControllerContext).Validate(xFormPostedData);
            model.Messages = new List<string>();
            foreach (var a in validationResult)
            {
                if (!string.IsNullOrEmpty(a.Message))
                    model.Messages.Add(a.Message);
            }
            if (Business.Email.Validation.IsEmail(participantEmail) == false)
                model.Messages.Add(EPiServer.Framework.Localization.LocalizationService.Current.GetString("/attend/edit/emailmissing"));

            if (Business.Email.Validation.IsEmail(participantEmail) == false || model.Messages.Count > 0)
            {
                model.PostedData = xFormPostedData;
                _xformHandler.HandleAction(this);
                return PartialView("~/modules/BVNetwork.Attend/Views/Pages/Partials/EventPagePartial.cshtml", model);
            }

            IParticipant participant = AttendRegistrationEngine.GenerateParticipation(model.EventPageBase.ContentLink, participantEmail, true, xformdata, "Participant submitted form");
            participant.XForm = xformdata;

            //Add sessions to participant

            participant.Sessions = new ContentArea();
            foreach (var key in Request.Form.AllKeys)
            {
                if (key.StartsWith("Session"))
                {
                    var sessionContentReference = new ContentReference(Request[key]);
                    participant.Sessions.Items.Add(new ContentAreaItem() { ContentLink = sessionContentReference });
                }
            }
            _contentRepository.Save(participant as IContent, SaveAction.Publish, AccessLevel.NoAccess);

            model.Submitted = participant.AttendStatus.ToLower() == "submitted";

            ViewBag.Participant = participant;

            return PartialView("~/modules/BVNetwork.Attend/Views/Pages/Partials/EventPagePartialSuccess.cshtml", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Failed(EventPageBase currentPage, XFormPostedData xFormPostedData, string contentLink)
        {
            var model = CreateEventRegistrationModel(currentPage, contentLink);
            return View("Index", model);
        }


        private EventRegistrationModel CreateEventRegistrationModel(EventPageBase currentPage, string contentLink)
        {
            var model = new EventRegistrationModel(currentPage);
            var repository = ServiceLocator.Current.GetInstance<IContentLoader>();
            var localizationService = ServiceLocator.Current.GetInstance<LocalizationService>();

            var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IPageRouteHelper>();
            PageReference currentPageLink = pageRouteHelper.PageLink;
            model.HostPageData = pageRouteHelper.Page;
            model.EventPageBase = currentPage;
            model.Sessions = BVNetwork.Attend.Business.API.AttendSessionEngine.GetSessionsList(model.EventPageBase.PageLink);
            model.AvailableSeats = AttendRegistrationEngine.GetAvailableSeats(model.EventPageBase.PageLink);
            model.PriceText = model.EventPageBase.EventDetails.Price > 0 ? model.EventPageBase.EventDetails.Price + " " + localizationService.GetString("/eventRegistrationTemplate/norwegianCurrencey") : localizationService.GetString("/eventRegistrationTemplate/freeOfCharge");
            return model;
        }

    }
}