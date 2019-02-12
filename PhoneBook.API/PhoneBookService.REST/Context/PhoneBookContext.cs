using PhoneBookService.REST.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhoneBookService.REST.Context
{
    public class PhoneBookContext : DbContext
    {
        public PhoneBookContext() : base("ConnectionString") { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
    }
}