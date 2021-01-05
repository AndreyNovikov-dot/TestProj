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
        public PersonRecord Person { get; set; }

        [MaxLength(100)]
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
