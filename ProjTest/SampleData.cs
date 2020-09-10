using System.Linq;
using ProjTest.Models;
using System;

namespace ProjTest
{
    public class SampleData
    {
        public static void Initialize(Context context)
        {
            //Инициализация начальных данных
            if (!context.Persons.Any())
            {
                context.Persons.AddRange(
                    new Person
                    {
                        
                        Surname = "Новиков",
                        Name = "Андрей",
                        PatrName = "Андреевич",
                        BirthDay ="16.04.1997",
                        Organization = "ИжГТУ",
                        Position = "Студент",
                       
                    },
                    new Person
                    {

                        Surname = "Иванов",
                        Name = "Иван",
                        PatrName = "Иванович",
                        BirthDay = "16.04.1997",
                        Organization = "УдГУ",
                        Position = "Студент",
                       
                    }
                );
                context.SaveChanges();
            }
            if (!context.Phones.Any())
            {
                context.Phones.AddRange(
                    new Phone
                    {
                        PhoneNumber = "11-89-89",
                        PersonId = 1
                    },
                    new Phone
                    {
                        PhoneNumber = "21-89-89",
                        PersonId = 2
                    },
                    new Phone
                    {
                        PhoneNumber = "12-89-89",
                        PersonId = 1
                    },
                     new Phone
                     {
                         PhoneNumber = "22-89-89",
                         PersonId = 2
                     }
                    );
                context.SaveChanges();
            }
            if (!context.Emails.Any())
            {
                context.Emails.AddRange(
                    new Email
                    {
                        EmailAddres = "oneone@mail.ru",
                        PersonId = 1
                    },
                    new Email
                    {
                        EmailAddres = "onetwo@mail.ru",
                        PersonId = 1
                    },
                    new Email
                    {
                        EmailAddres = "twoone@mail.ru",
                        PersonId = 2
                    },
                     new Email
                     {
                         EmailAddres = "twotwo@mail.ru",
                         PersonId = 2
                     }
                    );
                context.SaveChanges();
            }
            if (!context.Skypes.Any())
            {
                context.Skypes.AddRange(
                    new Skype
                    {
                        SkypeLogin = "AndrOneOne",
                        PersonId = 1
                    }, 
                    new Skype
                    {
                        SkypeLogin = "AndrOneTwo",
                        PersonId = 1
                    },
                    new Skype
                    {
                        SkypeLogin = "AndrTwoOne",
                        PersonId = 2
                    },
                    new Skype
                    {
                        SkypeLogin = "AndrTwoTwo",
                        PersonId = 2
                    }
                    );
                context.SaveChanges();
            }
            if (!context.AdditionalInfs.Any())
            {
                context.AdditionalInfs.AddRange(
                    new AdditionalInf
                    {
                        AdditionalInfo = "OneOne",
                        PersonId = 1
                    },
                    new AdditionalInf
                    {
                        AdditionalInfo = "OneTwo",
                        PersonId = 1
                    },
                    new AdditionalInf
                    {
                        AdditionalInfo = "TwoOne",
                        PersonId = 2
                    },
                    new AdditionalInf
                    {
                        AdditionalInfo = "TwoTwo",
                        PersonId = 2
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}
