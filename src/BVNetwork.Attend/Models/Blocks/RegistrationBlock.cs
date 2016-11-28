using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace BVNetwork.Attend.Models.Blocks
{
    [ImageUrl("~/Modules/BVNetwork.Attend/Static/attend_blue.png")]
    [ContentType(DisplayName = "Registration", GUID = "57482cfd-9943-49a1-b301-cc39efc90fc2", Description = "")]
    public class RegistrationBlock : BlockData
    {
        [Ignore]
        public ContentArea MainContentArea { get; set; }

    }
}