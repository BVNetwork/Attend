using System;
using System.Web.Mvc;
using EPiServer.Shell.ObjectEditing;

namespace BVNetwork.Attend.Business.EditorDescriptors
{

    public class EnumAttribute : SelectOneAttribute, IMetadataAware
    {
        public EnumAttribute(Type enumType) 
        { EnumType = enumType; } 
        
        public Type EnumType { get; set; }

        public new void OnMetadataCreated(ModelMetadata metadata)
        {
            var enumType = metadata.ModelType; 
            SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType); 
            base.OnMetadataCreated(metadata);
        }
    }
}