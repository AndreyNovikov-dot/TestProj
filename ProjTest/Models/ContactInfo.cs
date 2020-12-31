﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjTest.Models
{
    public enum ContactTypes
    {
        Skype,
        Email,
        Phone,
        Other
    }
    public class ContactInfo
    {
        [Key]
        public int ContactID { get; set; }
        
        public int PersonID { get; set; }
        public ContactTypes Type { get; set; }
        public Person Person { get; set; }
        public string Info { get; set; }
        public ContactInfo()
        {

        } 
        public ContactInfo(string s,ContactTypes t)
        {
            Info = s;
            Type = t;
        }
    }
}