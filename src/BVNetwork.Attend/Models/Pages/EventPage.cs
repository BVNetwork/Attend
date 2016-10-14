using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.XForms;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Shell.ObjectEditing;
using BVNetwork.Attend.Business.EditorDescriptors;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Models.Pages
{
    [ImageUrl("~/Modules/BVNetwork.Attend/Static/attend_blue.png")]
    [ContentType(GUID = "A0468734-C768-447A-86B3-2DA018C80A54", DisplayName = "Attend", Description = "Event page", GroupName = Groups.Attend)]
    public class EventPage : EventPageBase
    {

    }
}