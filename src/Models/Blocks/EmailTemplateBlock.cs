using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using BVNetwork.Attend.Business.API;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Models.Blocks
{
    [ImageUrl("~/Modules/BVNetwork.Attend/Static/attend_blue.png")]
    [ContentType(DisplayName = "EmailTemplateBlock", GUID = "060ea4db-0a9e-4ed0-9a9d-a22443c0231b", Description = "", AvailableInEditMode=true, GroupName=Groups.Attend)]
    public class EmailTemplateBlock : BlockData
    {
        [Display(
            Name = "From",
            Description = "E-mail sender",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string From { get; set; }

        [Display(
            Name = "To",
            Description = "E-mail reciepient",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual string To { get; set; }

        [Display(
            Name = "CC",
            Description = "E-mail reciepient CC",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual string CC { get; set; }

        [Display(
            Name = "BCC",
            Description = "E-mail reciepient BCC",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        public virtual string BCC { get; set; }

        [Display(
            Name = "Subject",
            Description = "E-mail subject",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        public virtual string Subject { get; set; }

        [CultureSpecific]
        [Editable(true)]
        [Display(
            Name = "Main body",
            Description = "XHTML body of email",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Main text body",
            Description = "Text body of email",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        [UIHint(UIHint.Textarea)]
        public virtual string MainTextBody { get; set; }


        [Display(
            Name = "SMS",
            Description = "Send as SMS",
            GroupName = SystemTabNames.Content,
            Order = 600)]
        public virtual bool SendAsSms { get; set; }

        


    }
}