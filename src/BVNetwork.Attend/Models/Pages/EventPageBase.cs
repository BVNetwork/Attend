using System;
using System.ComponentModel.DataAnnotations;
using BVNetwork.Attend.Business.Text;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.XForms;

namespace BVNetwork.Attend.Models.Pages
{
    public class EventPageBase : PageData
    {
        [Display(
            Name = "Registration form",
            Description = "Form for participant details in event registration",
            GroupName = Tabs.Event,
            Order = 50)]
        public virtual XForm RegistrationForm { get; set; }

        [Display(
            Name = "Registration form",
            Description = "Form for participant details in event registration",
            GroupName = Tabs.Event,
            Order = 55)]
        public virtual ContentArea RegistrationFormContainer { get; set; }


        [Display(
            Name = "EventName",
            Description = "Event name",
            GroupName = Tabs.Event,
            Order = 90)]
        public virtual string EventName { get; set; }

        [Display(
            Name = "Short name",
            Description = "Short event name",
            GroupName = Tabs.Event,
            Order = 100)]
        public virtual string ShortName { get; set; }

        [Display(
            Name = "Sessions",
            Description = "Event sessions",
            GroupName = Tabs.Event,
            Order = 10000)]
        [AllowedTypes(typeof(SessionBlock))]
        public virtual ContentArea Sessions { get; set; }


        [Display(
            Name = "Intro body",
            Description = "Introduction text for event",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual XhtmlString IntroBody { get; set; }


        [Display(
            Name = "Details content",
            Description = "Content area for event details",
            GroupName = SystemTabNames.Content,
            Order = 310)]
        [UIHint("DialogOnly")]
        public virtual ContentArea DetailsContent { get; set; }

        [Display(
            Name = "No seats content",
            Description = "Content area when no available seats",
            GroupName = SystemTabNames.Content,
            Order = 320)]
        [UIHint("DialogOnly")]
        public virtual ContentArea NoSeatsContent { get; set; }

        [Display(
            Name = "Complete content",
            Description = "Content area when finished",
            GroupName = SystemTabNames.Content,
            Order = 510)]
        [UIHint("DialogOnly")]
        public virtual ContentArea CompleteContent { get; set; }

        [Display(
            Name = "Complete content",
            Description = "XHTML when finished (For use with Episerver Forms)",
            GroupName = SystemTabNames.Content,
            Order = 511)]
        public virtual XhtmlString CompleteContentXhtml { get; set; }

        [Display(
            Name = "Closed content",
            Description = "Content area when event is closed",
            GroupName = SystemTabNames.Content,
            Order = 610)]
        [UIHint("DialogOnly")]
        public virtual ContentArea ClosedContent { get; set; }



        [Display(
            Name = "Submitted content",
            Description = "Content area when status is submitted",
            GroupName = SystemTabNames.Content,
            Order = 610)]
        [UIHint("DialogOnly")]
        public virtual ContentArea SubmittedContent { get; set; }

        [Display(
            Name = "Submitted content",
            Description = "XHTML when status is submitted (For use with Episerver Forms)",
            GroupName = SystemTabNames.Content,
            Order = 611)]
        public virtual XhtmlString SubmittedContentXhtml { get; set; }



        [Display(
            Name = "Event details",
            Description = "Details for current event",
            GroupName = Tabs.Event,
            Order = 1000)]
        public virtual EventDetailsBlock EventDetails { get; set; }

        [Display(
            Name = "Fields to hide in View Mode",
            Description = "",
            GroupName = SystemTabNames.Content)]
        [UIHint(EventRenderTags.AttendXFormFields)]
        public virtual string RemoveFieldsFromView { get; set; }

        public virtual AvailableSeatsBlock AvailableSeatsText { get; set; }

    }
}