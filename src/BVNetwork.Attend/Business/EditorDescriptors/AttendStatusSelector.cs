using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = EventRenderTags.AttendStatus)]
    public class AttendStatusSelector : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            SelectionFactoryType = typeof(AttendStatusSelectionFactory);

            ClientEditingClass = "epi-cms/contentediting/editors/SelectionEditor";

            metadata.CustomEditorSettings["uiWrapperType"] = "flyout";

            base.ModifyMetadata(metadata, attributes);
        }


    }
}