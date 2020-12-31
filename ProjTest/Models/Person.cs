using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;



namespace ProjTest.Models
{
    public class Person
    {
        [Required]        
        public int Id { get; set; }

        [Remote("IsPersonExists", "Home", ErrorMessage = "Уже есть", AdditionalFields = "Name,PatrName,Id")]        
        [RegularExpression("^[А-Я]{1}[а-я]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Surname { get; set; }

        [Remote("IsPersonExists", "Home",ErrorMessage ="Уже есть",AdditionalFields ="PatrName,Surname,Id")]
        [Required(ErrorMessage = "Укажите имя")]
        [RegularExpression("^[А-Я]{1}[а-я]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Name { get; set; }

        [Remote("IsPersonExists", "Home", ErrorMessage = "Уже есть", AdditionalFields = "Name,Surname,Id")]        
        [RegularExpression("^[А-Я]{1}[а-я]{0,20}$", ErrorMessage = "Неверный формат")]
        public string PatrName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime BirthDay { get; set; }

        
        [RegularExpression("^[А-Я]{1}[а-яА-Я ]{0,20}$", ErrorMessage = "Неверный формат")]        
        public string Organization { get; set; }

        
        [RegularExpression("^[А-Я]{1}[а-я ]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Position { get; set; }
        
        [ListPhone(ErrorMessage = "Неверный формат или повторяющийся телефон")]
        public IList<Phone> Phone { get; set; }

        [ListEmail(ErrorMessage = "Неверный формат или повторяющийся Email")]
        public IList<Email> Email { get; set; }

        [ListSkype(ErrorMessage = "Превышение макс длины или повторяющйся Skype")]
        public IList<Skype> Skype { get; set; }

        [ListAddinfo(ErrorMessage = "Превышение макс длины или повторение доп информации")]
        public IList<AdditionalInf> Addinf { get; set; }
    }
   

    //Проверка каждого члена листа Phone на соответсвие регулярному выражению
    class ListPhoneAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            var list = (IList<Phone>)value;
            string pattern = "(^8-[0-9]{3}-[0-9]{3}-[0-9]{2}-[0-9]{2}$)|(^[0-9]{2}-[0-9]{2}-[0-9]{2}$)";
            foreach (var phone in list)
            {
                if (phone.PhoneNumber != null)
                {
                    if (!Regex.IsMatch(phone.PhoneNumber, pattern)||(list.Where(x=>x.PhoneNumber==phone.PhoneNumber).Count()>1))
                    {
                        return false;

                    }                   

                }
                
            }
            return true;

        } 
    }

    //Проверка каждого члена листа Email на соответсвие регулярному выражению
    class ListEmailAttribute : ValidationAttribute 
    {
        public override bool IsValid(object value)
        {
            var list = (IList<Email>)value;
            string pattern = "^[a-z0-9][-a-z0-9._]+@([-a-z0-9]+.)+[a-z]{2,5}$";
            foreach (var email in list)
            {
                if (email.EmailAddres != null)
                {
                    if (!Regex.IsMatch(email.EmailAddres, pattern)||(list.Where(x=>x.EmailAddres==email.EmailAddres).Count()>1))
                    {
                        return false;
                    }
                }
               

            }
            
            return true;
        }

    }

    //Проверка каждого члена листа Skype на непревышение макс длины 
    class ListSkypeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = (IList<Skype>)value;
            int length = 30;
            foreach (var skype in list)
            {
                if (skype.SkypeLogin != null)
                {
                    if (skype.SkypeLogin.Length > length||(list.Where(x=>x.SkypeLogin==skype.SkypeLogin).Count()>1))
                    {
                        return false;
                    }
                }
                

            }
            return true;
        }
    }

    //Проверка каждого члена листа Addinf на непревышение макс длины 
    class ListAddinfoAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = (IList<AdditionalInf>)value;
            int length = 100;
            foreach (var info in list)
            {
                if (info.AdditionalInfo != null)
                {
                    if (info.AdditionalInfo.Length > length || (list.Where(x => x.AdditionalInfo == info.AdditionalInfo).Count() > 1))
                    {
                        return false;
                    }
                }
               

            }
            return true;
        }
    }
    
   
}
   