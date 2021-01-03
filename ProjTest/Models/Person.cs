using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjTest.Models
{
    public class Person
    {
        [MaxLength(20)]
        [RegularExpression("^[А-Я]{1}[а-я]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Surname { get; set; }

        [MaxLength(20)]
        [Required]
        [RegularExpression("^[А-Я]{1}[а-я]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Name { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[А-Я]{1}[а-я]{0,20}$", ErrorMessage = "Неверный формат")]
        public string PatrName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[А-Я]{1}[а-яА-Я ]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Organization { get; set; }

        [MaxLength(20)]
        [RegularExpression("^[А-Я]{1}[а-я ]{0,20}$", ErrorMessage = "Неверный формат")]
        public string Position { get; set; }
        public Person()
        {

        }
        public Person(string personName,string personSurname,string personPatrName,DateTime personBirthDay,string personOrganization,string personPosition)
        {
            Surname = personSurname;
            Name = personName;
            PatrName = personPatrName;
            BirthDay = personBirthDay;
            Organization = personOrganization;
            Position = personPosition;
        }

    }
}
