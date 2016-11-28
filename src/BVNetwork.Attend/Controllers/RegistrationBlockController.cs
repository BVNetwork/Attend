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

    public class RegistrationBlockController : BlockController<RegistrationBlock>
    {
        private IContentLoader _contentLoader;
        private IContentRepository _contentRepository;
        private UrlResolver _urlResolver;
        private EventPageBase currentEvent;

        public RegistrationBlockController(IContentLoader contentLoader, UrlResolver urlResolver, IContentRepository contentRepository)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _contentRepository = contentRepository;
        }


        public override ActionResult Index(RegistrationBlock currentBlock)
        {
            var model = new RegistrationBlockViewModel<RegistrationBlock>(currentBlock);

            string eventPageID = Request.QueryString["eventPageID"];
            ContentArea contentArea = new ContentArea();

            if (!string.IsNullOrEmpty(eventPageID) && new ContentReference(eventPageID) != null)
            {
                contentArea.Items.Add(new ContentAreaItem
                {
                    ContentLink = new ContentReference(eventPageID)
                });
            }

            model.CurrentBlock.MainContentArea = contentArea;

            return View("~/Modules/BVNetwork.Attend/Views/Blocks/RegistrationBlock/Index.cshtml", model);
        }



    }
}
