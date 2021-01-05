using System;
using System.Collections.Generic;




namespace ProjTest.Models
{
    public class PersonRecord:Person
    {
              
        public int Id { get; set; }    
        public ICollection<ContactInfo> Contacts { get; set; }
        public PersonRecord(int id,string personName, string personSurname, string personPatrName, DateTime personBirthDay, string personOrganization, string personPosition,ICollection<ContactInfo> personContacts):base(personName, personSurname, personPatrName,personBirthDay,personOrganization,personPosition)
        {
            Contacts = personContacts;
            Id = id;
        }
        public PersonRecord()
        {

        }
    }
}
   