using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using BVNetwork.Attend.Business.Text;

namespace BVNetwork.Attend.Models.Blocks
{
    [ImageUrl("~/Modules/BVNetwork.Attend/Static/attend_blue.png")]
    [ContentType( DisplayName = "Attend My Page", GUID = "93a8c9fa-5030-420b-b961-ff8d1a187ecd", Description = "", GroupName = Groups.Attend)]
    public class MyPageBlock : BlockData
    {
        [Ignore]
        public ContentArea MainContentArea { get; set; }

    }
}