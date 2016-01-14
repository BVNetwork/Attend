using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Models.Blocks
{
    [ContentType(DisplayName = "AvailableSeatsBlock", GUID = "1c6f1295-3635-4ed9-bdba-a7813a8293fb", Description = "", AvailableInEditMode = false, GroupName = Groups.Attend)]
    public class AvailableSeatsBlock : BlockData
    {
        [Display(
            Name = "One seat",
            Description = "Text to show when there is one available seat",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual string OneSeat { get; set; }
        [Display(
            Name = "Two seats",
            Description = "Text to show when there are two available seats",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual string TwoSeats { get; set; }
        [Display(
            Name = "Three seats",
            Description = "Text to show when there are three available seats",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual string ThreeSeats { get; set; }
        [Display(
            Name = "More seats",
            Description = "Text to show when there are more available seats ({0} will add number of seats. E.g. There are {0} available seats.)",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual string ManyAvailableSeats { get; set; }
        [Display(
            Name = "Show availability",
            Description = "Show texts about seat availability",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual bool ShowAvailableSeats { get; set; }

        /*
                [CultureSpecific]
                [Editable(true)]
                [Display(
                    Name = "Name",
                    Description = "Name field's description",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual String Name { get; set; }
         */
    }
}