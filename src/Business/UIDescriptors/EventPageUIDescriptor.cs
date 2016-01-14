using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVNetwork.Attend.Models.Pages;
using EPiServer.Shell;

namespace BVNetwork.Attend.Business.UIDescriptors
{
    [UIDescriptorRegistration]
    public class EventPageBaseUIDescriptor : UIDescriptor<EventPageBase>
    {
        public EventPageBaseUIDescriptor()
                : base("EventPageBase")
            {
                DefaultView = CmsViewNames.OnPageEditView;
            }

    }
}