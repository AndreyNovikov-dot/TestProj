using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProjTest.Models
{
    public class AdditionalInf
    {
        public int AdditionalInfId { get; set; }
        
        public string AdditionalInfo{ get; set; }
        
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
   
}
