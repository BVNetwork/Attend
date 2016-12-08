using BVNetwork.Attend.Business.Participant;
using EPiServer.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Forms.Models.Forms
{
    [ContentType(
     DisplayName = "Attend Submit Button",
     GroupName = "Attend",
     GUID = "3dc75d57-3e34-4bda-b9be-21500303cd24")]
    public class AttendSubmitButton : EPiServer.Forms.Implementation.Elements.SubmitButtonElementBlock
    {
        [Ignore]
        public virtual string AttendPage { get {
                string requestedEvent = System.Web.HttpContext.Current.Request.QueryString["eventPageID"];
                if (!string.IsNullOrEmpty(requestedEvent))
                    return requestedEvent;
                return this.FormElement.Form.RedirectUrl;
            } }

        [Ignore]
        public virtual Dictionary<string, string> PredefinedValues { get; set; }

        [Ignore]
        public virtual string ParticipantCode {get; set;}
        [Ignore]
        public virtual string ParticipantEmail { get; set; }
    }
}