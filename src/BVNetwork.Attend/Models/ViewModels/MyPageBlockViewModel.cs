using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using BVNetwork.Attend.Models.Blocks;
using BVNetwork.Attend.Models.Pages;

namespace BVNetwork.Attend.Models.ViewModels
{
    public class MyPageBlockViewModel<T> where T : BlockData
    {

        public List<ParticipantBlock> AllEntries { get; set; }
        public List<ParticipantBlock> PastEntries { get; set; }
        public List<ParticipantBlock> UpcomingEntries { get; set; }
        public string Email { get; set; }
        public string RegistrationCode { get; set; }
        public string StatusCssClass { get; set; }
        public string ParticipantName { get; set; }
        public MyPageBlock CurrentBlock { get; set; }
        public PageData CurrentPage { get; set; }
        public EventPageBase CurrentEvent { get; set; }
        public string PredefinedValues { get; set; }

        public MyPageBlockViewModel(T currentBlock) : base()
        {
            AllEntries = new List<ParticipantBlock>();
            PastEntries = new List<ParticipantBlock>();
            UpcomingEntries = new List<ParticipantBlock>();
        }


    }
}