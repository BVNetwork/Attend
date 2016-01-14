using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using System;
using BVNetwork.Attend.Models.Pages;
using EPiServer.XForms;
using EPiServer;
using System.Collections.Specialized;
using EPiServer.Core;

namespace BVNetwork.Attend.Business.EditorDescriptors
{
    /// <summary>
    /// Provides a list of options corresponding to ContactPage pages on the site
    /// </summary>
    /// <seealso cref="AttendStatusSelector"/>
    public class AttendXFormFieldsSelectionFactory : ISelectionFactory
    { 
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata) 
        {
            if ((metadata.Model as PropertyData) == null || (metadata.Model as PropertyData).Parent["icontent_contentlink"] == null || string.IsNullOrEmpty((metadata.Model as PropertyData).Parent["icontent_contentlink"].ToString()))
                yield break;

            var ownerPage = new ContentReference((metadata.Model as PropertyData).Parent["icontent_contentlink"].ToString());
            EventPageBase EventPageBase = EPiServer.DataFactory.Instance.Get<EventPageBase>(ownerPage);

            if (EventPageBase.RegistrationForm == null)
                yield break;

            XForm xform = XForm.CreateInstance(new Guid(EventPageBase.RegistrationForm.Id.ToString()));
            NameValueCollection formControls = xform.CreateFormData().GetValues();
            foreach (string data in formControls)
            {
                yield return new SelectItem() { Text = data, Value = data };
            }

        }
    }

}