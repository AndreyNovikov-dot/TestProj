using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjTest.Models
{
    public class Phone 
    {
        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Person Person { get; set; }
        
    }
    
}