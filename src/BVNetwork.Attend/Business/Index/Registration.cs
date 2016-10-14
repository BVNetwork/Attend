using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using BVNetwork.Attend.Business.Participant;
using EPiServer.Core;
using EPiServer.XForms;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using System.Text;
using EPiServer.Data.Dynamic;
using EPiServer.Data;
using BVNetwork.Attend.Models.Blocks;
using EPiServer.ServiceLocation;
using BVNetwork.Attend.Business.API;

namespace BVNetwork.Attend.Data
{


    [Serializable()]
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class Registration
    {
        public static DynamicDataStore RegistrationDataStore { get { return typeof(Registration).GetStore(); } }
        
        public EPiServer.Data.Identity Id { get; set; }
        public string RegistrationCode {get; set;}
        public string Email { get; set; }
        public int ParticipantBlock { get; set; }

        protected void Initialize()
        {
            Id = Identity.NewIdentity(Guid.NewGuid());
        }

        public Registration()
        {
            Initialize();
        }

        public void Save()
        {
            RegistrationDataStore.Save(this);
        }

        public static List<IParticipant> GetRegistrations(string email)
        {
            List<IParticipant> participations = new List<IParticipant>();
            var registrations = from r in RegistrationDataStore.Items<Registration>()
                where r.Email == email
                select r;
            foreach (Registration r in registrations) {
                IParticipant p = AttendRegistrationEngine.GetParticipant(r.ParticipantBlock);
                if (p != null)
                    participations.Add(p);
            }
            return participations;
        }


    }
}
