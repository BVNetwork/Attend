using EPiServer.Data;
using EPiServer.Data.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVNetwork.Attend.Business.Settings
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class DDSSetting : IDynamicData
    {
        public Identity Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }


        public DDSSetting()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
        }
    }
}