using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjTest.Models
{
    public class ViewModel
    {
        public Person Person { get; set; }
        public List<string> Email { get; set; }
        public List<string> Skype { get; set; }
        public List<string> Phone { get; set; }
        public List<string> Other { get; set; }
        public ViewModel (Person p)
        {
            Person = p;
            Email = p.Contacts.Where(x => x.Type == ContactTypes.Email&&x.PersonID==p.Id).Select(x=>x.Info).ToList();
            Skype = p.Contacts.Where(x => x.Type == ContactTypes.Skype && x.PersonID == p.Id).Select(x => x.Info).ToList();
            Phone = p.Contacts.Where(x => x.Type == ContactTypes.Phone && x.PersonID == p.Id).Select(x => x.Info).ToList();
            Other = p.Contacts.Where(x => x.Type == ContactTypes.Other && x.PersonID == p.Id).Select(x => x.Info).ToList();
        }
        public ViewModel()
        {

        }
        class ListsToContacts
        {
            public IList<string> Info { get; set; }
            public ContactTypes Type { get; set; }
            public ListsToContacts(IList<string> l, ContactTypes t)
            {
                Info = l;
                Type = t;
            }

        }
        private List<ContactInfo> ConvertToContacts(params ListsToContacts[] coverts)
        {
            List<ContactInfo> contacts = new List<ContactInfo>();
            foreach (var element in coverts)
            {
                foreach (var contact in element.Info)
                {
                    if (contact != null)
                    {
                        ContactInfo c = new ContactInfo(contact, element.Type);
                        contacts.Add(c);
                    }
                    
                }
            }
            return contacts;
        }
        public Person ConvertToPerson()
        {
            Person person = new Person();
            List<ContactInfo> contacts =ConvertToContacts(new ListsToContacts(Email, ContactTypes.Email), new ListsToContacts(Skype, ContactTypes.Skype), new ListsToContacts(Other, ContactTypes.Other), new ListsToContacts(Phone, ContactTypes.Phone));
            person = Person;
            person.Contacts = contacts;
            return person;
        }
    }
}
