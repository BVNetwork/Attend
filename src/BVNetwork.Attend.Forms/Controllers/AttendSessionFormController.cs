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
using EPiServer.Forms.Implementation.Elements;
using EPiServer.Framework.Web;
using EPiServer.Forms.Controllers;
using EPiServer.Data.Dynamic;
using BVNetwork.Attend.Forms.Models.Forms;

namespace BVNetwork.Attend.Forms.Controllers
{

    [TemplateDescriptor(AvailableWithoutTag = true, Default = true, ModelType = typeof(AttendSessionForm), TemplateTypeCategory = TemplateTypeCategories.MvcPartialController)]
    public class AttendSessionFormController
    {
        public ActionResult Index(FormContainerBlock currentBlock)
        {
            currentBlock = currentBlock.CreateWritableClone() as FormContainerBlock;
            string eventPage = "";
            if (this.ControllerContext.ParentActionViewContext.ViewData["EventPage"] != null)
            {
                eventPage = this.ControllerContext.ParentActionViewContext.ViewData["EventPage"].ToString();
            }
            currentBlock.RedirectToPage = new EPiServer.Url(eventPage);
            var baseActionResult = base.Index(currentBlock);
            return baseActionResult;
        }
    }
}
