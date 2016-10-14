using System.Collections.Generic;
using System.Linq;
using EPiServer.Shell.ObjectEditing;
using System;
using BVNetwork.Attend.Business.Text;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;

namespace BVNetwork.Attend.Business.EditorDescriptors
{
    /// <summary>
    /// Provides a list of options corresponding to ContactPage pages on the site
    /// </summary>
    /// <seealso cref="AttendStatusSelector"/>
    public class AttendStatusSelectionFactory : ISelectionFactory
    { 
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata) 
        { 
            Array items = Enum.GetValues(typeof(AttendStatus));

            foreach (var item in items) {
                yield return new SelectItem() { Text = GetValueName(item), Value=Enum.GetName(typeof(AttendStatus), item) };
            } 
        }

        private string GetValueName(object value)
        {
            var staticName = Enum.GetName(typeof(AttendStatus), value);

            string localizationPath = string.Format(
                "/attend/{0}/{1}",
                typeof(AttendStatus).Name.ToLowerInvariant(),
                staticName.ToLowerInvariant());

            return ServiceLocator.Current.GetInstance<LocalizationService>().GetString(localizationPath);
        }

    }

}