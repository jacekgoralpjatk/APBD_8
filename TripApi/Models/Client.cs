﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace TripApi.Models
{
    public partial class Client
    {
        public Client()
        {
            ClientTrip = new HashSet<ClientTrip>();
        }

        public int IdClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Pesel { get; set; }

        public virtual ICollection<ClientTrip> ClientTrip { get; set; }
    }
}