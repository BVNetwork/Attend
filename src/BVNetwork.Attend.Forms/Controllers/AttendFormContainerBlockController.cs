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
using EPiServer.DataAbstraction;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Forms.Implementation.Elements.BaseClasses;

namespace BVNetwork.Attend.Forms.Controllers
{

    [TemplateDescriptor(AvailableWithoutTag = true, Default = true, ModelType = typeof(FormContainerBlock), TagString = "AttendContentArea", TemplateTypeCategory = TemplateTypeCategories.MvcPartialController)]
    public class AttendFormContainerBlockController : FormContainerBlockController
    {
        public override ActionResult Index(FormContainerBlock currentBlock)
        {
            var rep = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
            string email = Request.QueryString["email"];
            string code = Request.QueryString["code"];


            currentBlock = currentBlock.CreateWritableClone() as FormContainerBlock;
            string eventPage = "";
            if (this.ControllerContext.ParentActionViewContext.ViewData["EventPage"] != null)
            {
                eventPage = this.ControllerContext.ParentActionViewContext.ViewData["EventPage"].ToString();
            }
            if(string.IsNullOrEmpty(eventPage) == false)
            { 
                foreach (var element in currentBlock.ElementsArea.Items)
                {
                    var elementData = rep.Get<IContent>(element.ContentLink);
                    if((elementData as AttendSessionForm) != null)
                    {
                        (elementData as AttendSessionForm).Sessions = BVNetwork.Attend.Business.API.AttendSessionEngine.GetSessionsList(new ContentReference(eventPage));
                        (elementData as AttendSessionForm).EventName = "Temp";
                    }
                }
                currentBlock.RedirectToPage = new EPiServer.Url(eventPage);
            }

            // Edit existing participation?
            var currentParticipant = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetParticipant(email, code);
            if (currentParticipant != null)
            {
                AttendSubmitButton submitButton = null;
                Dictionary<string, string> predefinedValues = new Dictionary<string, string>();
                foreach (var element in currentBlock.ElementsArea.Items)
                {
                    var elementData = rep.Get<IContent>(element.ContentLink);
                    string value = BVNetwork.Attend.Business.API.AttendRegistrationEngine.GetParticipantInfo(currentParticipant, elementData.Name);
                    if (!string.IsNullOrEmpty(value)) {
                        predefinedValues.Add(elementData.GetType().Name+";__field_"+element.ContentLink.ID, value); 
                    }
                    if ((elementData as AttendSubmitButton) != null)
                    {
                        submitButton = elementData as AttendSubmitButton;
                    }
                }
                if(submitButton != null) { 
                    submitButton.PredefinedValues = predefinedValues;
                    submitButton.ParticipantCode = currentParticipant.Code;
                    submitButton.ParticipantEmail = currentParticipant.Email;
                }
            }


            var baseActionResult = base.Index(currentBlock);
            return baseActionResult;
        }
        

    }
}
