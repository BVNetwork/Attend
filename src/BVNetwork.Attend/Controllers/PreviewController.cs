using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.ViewModels;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer;

namespace BVNetwork.Attend.Controllers
{
    [TemplateDescriptor(
        Inherited = true,
        TemplateTypeCategory = TemplateTypeCategories.MvcController,
        Tags = new[] { RenderingTags.Preview, RenderingTags.Edit },
        AvailableWithoutTag = false)]
    [VisitorGroupImpersonation]
    public class PreviewController : ActionControllerBase, IRenderTemplate<EmailTemplateBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly TemplateResolver _templateResolver;
        private readonly DisplayOptions _displayOptions;

        public PreviewController(IContentLoader contentLoader, TemplateResolver templateResolver, DisplayOptions displayOptions)
        {
            _contentLoader = contentLoader;
            _templateResolver = templateResolver;
            _displayOptions = displayOptions;
        }

        public ActionResult Index(IContent currentContent)
        {
            //As the layout requires a page for title etc we "borrow" the start page
            var startPage = _contentLoader.Get<PageData>(SiteDefinition.Current.StartPage);

            var model = new AttendPreviewModel(startPage, currentContent);

            var contentArea = new ContentArea();
            contentArea.Items.Add(new ContentAreaItem 
            { 
                ContentLink = currentContent.ContentLink
            });
            var areaModel = new AttendPreviewModel.PreviewArea
                {
                    ContentArea = contentArea
                };
            model.Areas.Add(areaModel);

            return View(model);
        }

        private bool SupportsTag(IContent content, string tag)
        {
            var templateModel = _templateResolver.Resolve(HttpContext,
                                      content.GetOriginalType(),
                                      content,
                                      TemplateTypeCategories.MvcPartial,
                                      tag);

            return templateModel != null;
        }

    }
}
