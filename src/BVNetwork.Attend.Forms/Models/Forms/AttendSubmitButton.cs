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
                return this.FormElement.Form.RedirectUrl;
            } }
    }
}