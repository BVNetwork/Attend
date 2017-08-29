using System;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Shell;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Business.EditorDescriptors
{
    [EditorDescriptorRegistration(
       TargetType = typeof(ContentArea), UIHint = "DialogOnly")]
    public class AttendFormsOnlyContentAreaEditing : EditorDescriptor
    {
        public override void ModifyMetadata(
           ExtendedMetadata metadata,
           IEnumerable<Attribute> attributes)
        {
            base.ModifyMetadata(metadata, attributes);
            ClientEditingClass = "epi-cms/contentediting/editors/ContentAreaEditor";
            metadata.CustomEditorSettings["uiWrapperType"] = UiWrapperType.Floating;
            AllowedTypes = new List<Type>() { typeof(BlockData), typeof(PageData), typeof(IContent) };
            
        }
    }
}