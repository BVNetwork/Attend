using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using BVNetwork.Attend.Models.Pages;
using System.Collections.Generic;
using BVNetwork.Attend.Business.API;

namespace BVNetwork.Attend.Forms.Models.Forms
{
    [ContentType(DisplayName = "Sessions", GUID = "d7f34cd6-11ea-48f4-ad1d-46bda70e05de", Description = "", GroupName ="Attend")]
    public class AttendSessionForm : EPiServer.Forms.Implementation.Elements.TextboxElementBlock
    {
        [Ignore]
        public List<AttendSessionEngine.Session> Sessions { get; set; }

        [Ignore]
        public string EventName { get; set; }
    }
}