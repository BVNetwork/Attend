using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Text;
using EPiServer.Web;

namespace BVNetwork.Attend.Models.Blocks
{
    [ContentType(DisplayName = "EventDetailsBlock", GUID = "603aa6bf-ea2e-4c7a-9261-eaa1897528fc", Description = "", AvailableInEditMode = false, GroupName = Groups.Attend)]
    public class EventDetailsBlock : BlockData
    {
        [Display(
            Name = "Location",
            Description = "Where is the event taking place",
            GroupName = Tabs.Event,
            Order = 100)]
        public virtual string Location { get; set; }

        [Display(
            Name = "Contact",
            Description = "Contact information",
            GroupName = Tabs.Event,
            Order = 200)]
        public virtual string Contact { get; set; }

        [Display(
            Name = "Event start",
            Description = "When is the event starting",
            GroupName = Tabs.Event,
            Order = 300)]
        public virtual DateTime EventStart { get; set; }

        [Display(
            Name = "Event end",
            Description = "When is the event finished",
            GroupName = Tabs.Event,
            Order = 400)]
        public virtual DateTime EventEnd { get; set; }

        [Display(
            Name = "Number of seats",
            Description = "Number of participants getting registration confirmation automatically",
            GroupName = Tabs.Event,
            Order = 500)]
        public virtual int NumberOfSeats { get; set; }

        [Display(
            Name = "Cancelled",
            Description = "Is the event cancelled?",
            GroupName = Tabs.Event,
            Order = 510)]
        public virtual bool Cancelled { get; set; }


        [Display(
            Name = "Private",
            Description = "Is the event private?",
            GroupName = Tabs.Event,
            Order = 520)]
        public virtual bool Private { get; set; }


        [Display(
            Name = "Registration open",
            Description = "When it should be possible to start registering",
            GroupName = Tabs.Event,
            Order = 600)]
        public virtual DateTime RegistrationOpen { get; set; }

        [Display(
            Name = "Registration closed",
            Description = "When is registration finished",
            GroupName = Tabs.Event,
            Order = 700)]
        public virtual DateTime RegistrationClose { get; set; }

        [UIHint(UIHint.MediaFile)]
        [Display(
            Name = "PDF template",
            Description = "PDF template for badge or diploma",
            GroupName = Tabs.Event,
            Order = 800)]
        public virtual ContentReference PdfTemplate { get; set; }

        [Display(
            Name = "Price",
            Description = "Ticket price",
            GroupName = Tabs.Event,
            Order = 900)]
        public virtual int Price { get; set; }
        

        

    }
}