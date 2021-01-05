﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ProjTest.Models
{
    public class Context : DbContext
    {
        public DbSet<PersonRecord> Persons { get; set; }    
        public DbSet<ContactInfo> Contacts { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
       
    }
}
