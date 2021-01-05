using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;


namespace ProjTest.Models
{
    public class ViewModel:Person
    {
        public int Id { get; set; }

        [ListEmail(ErrorMessage = "Неверный формат")]
        public List<string> Email { get; set; }

        [ListSkype(ErrorMessage = "Превышение макс длины")]
        public List<string> Skype { get; set; }

        [ListPhone(ErrorMessage = "Неверный формат")]
        public List<string> Phone { get; set; }

        [ListSkype(ErrorMessage = "Превышение макс длины")]
        public List<string> Other { get; set; }
        public ViewModel(string personName, string personSurname, string personPatrName, DateTime personBirthDay, string personOrganization, string personPosition, int personId) : base(personName, personSurname, personPatrName, personBirthDay, personOrganization, personPosition)
        {
            Id=personId;
        }
        public ViewModel (PersonRecord p)
        {
            Id = p.Id;
            Name = p.Name;
            Surname = p.Surname;
            PatrName = p.PatrName;
            Organization = p.Organization;
            Position=p.Position;
            BirthDay = p.BirthDay;

            Email = p.Contacts.Where(x => x.Type == ContactTypes.Email&&x.PersonID==p.Id).Select(x=>x.Info).ToList();
            Skype = p.Contacts.Where(x => x.Type == ContactTypes.Skype && x.PersonID == p.Id).Select(x => x.Info).ToList();
            Phone = p.Contacts.Where(x => x.Type == ContactTypes.Phone && x.PersonID == p.Id).Select(x => x.Info).ToList();
            Other = p.Contacts.Where(x => x.Type == ContactTypes.Other && x.PersonID == p.Id).Select(x => x.Info).ToList();
        }
        public ViewModel()
        {

        }
        private class ContactContainer
        {
            public List<string> Info { get; set; }
            public ContactTypes Type { get; set; }
            public ContactContainer(List<string> l, ContactTypes t)
            {
                Info = l;
                Type = t;
            }

        }
        private List<ContactInfo> ConvertToContacts(params ContactContainer[] convertParams)
        {
            List<ContactInfo> contacts = new List<ContactInfo>();
            foreach (var element in convertParams)
            {
                if (element.Info != null)
                {
                    foreach (var contact in element.Info)
                    {
                        if (contact != null)
                        {
                            ContactInfo c = new ContactInfo(contact, element.Type);
                            c.PersonID = Id;
                            contacts.Add(c);
                        }

                    }
                }
               
            }
            return contacts;
        }
        public PersonRecord ConvertToPerson()
        {
           
            List<ContactInfo> contacts =ConvertToContacts(new ContactContainer(Email, ContactTypes.Email), new ContactContainer(Skype, ContactTypes.Skype), new ContactContainer(Other, ContactTypes.Other), new ContactContainer(Phone, ContactTypes.Phone));
            return new PersonRecord(Id,Name, Surname, PatrName, BirthDay, Organization, Position, contacts);
            
            
        }
        public int[] CreateViewBag()
        {
            return new int[] {Phone is null ? 1:Phone.Count, Email is null?1:Email.Count ,Skype is null? 1:Skype.Count,Other is null? 1:Other.Count };
        }
    }
    class ListPhoneAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            var list = (IList<string>)value;
            string pattern = "(^8-[0-9]{3}-[0-9]{3}-[0-9]{2}-[0-9]{2}$)|(^[0-9]{2}-[0-9]{2}-[0-9]{2}$)";
            if (list != null)
            {
                foreach (var phone in list)
                {
                    if (phone != null)
                    {
                        if (!Regex.IsMatch(phone, pattern))
                        {
                            return false;

                        }

                    }

                }
            }
           
            return true;

        }
    }
    class ListEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = (IList<string>)value;
            string pattern = "^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$";
            if (list != null)
            {
                foreach (var email in list)
                {
                    if (email != null)
                    {
                        if (!Regex.IsMatch(email, pattern))
                        {
                            return false;
                        }
                    }
                }
            }
               

            return true;
        }

    }
    class ListSkypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = (IList<string>)value;
            int length = 30;
            if (list != null)
            {
                foreach (var skype in list)
                {
                    if (skype != null)
                    {
                        if (skype.Length > length)
                        {
                            return false;
                        }
                    }
                }
            }
          
            return true;
        }
    }
    class ListOtherAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = (IList<string>)value;
            int length = 100;
            if (list != null)
            {
                foreach (var info in list)
                {
                    if (info != null)
                    {
                        if (info.Length > length)
                        {
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
    }
} 
