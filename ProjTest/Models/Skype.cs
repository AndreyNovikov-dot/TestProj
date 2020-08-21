using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjTest.Models
{
    public class Skype
    {
        public int SkypeId { get; set; }
        public string SkypeLogin { get; set; }
        public Nullable<int> PersonId { get; set; }
        public Person Person { get; set; }
    }
}
