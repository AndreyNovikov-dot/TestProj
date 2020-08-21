using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjTest.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;

namespace ProjTest.Controllers
{
    public class HomeController : Controller
    {

        Context db;

        public HomeController(Context context)
        {
            db = context;
            db.Phones.Include(p => p.Person).ToList();
            db.Emails.Include(p => p.Person).ToList();
            db.Skypes.Include(p => p.Person).ToList();
            db.AdditionalInfs.Include(p => p.Person).ToList();
        }

        public IActionResult Index()
        {

            return View(db.Persons.ToList());

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                db.Persons.Remove(db.Persons.First(x => x.Id == id));
                db.SaveChanges();

                CleanNulls();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Change(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Person person = db.Persons.First(x => x.Id == id);

                ViewBag.numberOfElements = CreateViewBag(person);

                return View(person);
            }

        }

        [HttpPost]
        public IActionResult Change(Person p)
        {
            
           
            AddModelError(p);
            
            if (ModelState.IsValid)
            {
                Person person = db.Persons.First(x => x.Id == p.Id);

                person.Name = p.Name;
                person.Organization = p.Organization;
                person.PatrName = p.PatrName;
                person.Position = p.Position;
                person.Surname = p.Surname;
                person.Phone = p.Phone;
                person.Skype = p.Skype;
                person.Addinf = p.Addinf;
                person.BirthDay = p.BirthDay;
                person.Email = p.Email;
                db.SaveChanges();

                CleanNulls();

                return RedirectToAction("Index");
            }

            ViewBag.numberOfElements = CreateViewBag(p);


            return View(p);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Person p)
        {
            AddModelError(p);

            if (ModelState.IsValid)
            {
                db.Persons.Add(p);
                db.SaveChanges();
                CleanNulls();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        public IActionResult Find(String find)
        {
            if (find != null)
            {
                string[] split = find.Split(' ');
                List<Person> persons = new List<Person>();
                foreach (var str in split)
                {
                    persons.AddRange(db.Persons.FromSqlInterpolated($"SELECT * FROM Persons INNER JOIN Skypes ON Skypes.PersonId=Persons.Id  WHERE SkypeLogin LIKE {"%" + str + "%"} UNION ALL  SELECT * FROM Persons INNER JOIN Phones ON Phones.PersonId=Persons.Id  WHERE Name LIKE {"%" + str + "%"} OR Surname LIKE {"%" + str + "%"} OR PatrName LIKE {"%" + str + "%"} OR BirthDay LIKE {"%" + str + "%"} OR Organization LIKE {"%" + str + "%"} OR Position LIKE {"%" + str + "%"} OR Phones.PhoneNumber LIKE {"%" + str + "%"} UNION ALL SELECT * FROM Persons INNER JOIN Emails ON Emails.PersonId=Persons.Id  WHERE EmailAddres LIKE {"%" + str + "%"} UNION ALL  SELECT * FROM Persons INNER JOIN AdditionalInfs ON AdditionalInfs.PersonId=Persons.Id  WHERE AdditionalInfo LIKE {"%" + str + "%"}").ToList());
                }

                return View(persons.Distinct());
            }
            else
            {
                return View(db.Persons.ToList());
            }

        }

        [HttpGet]
        public IActionResult ViewPerson(int? id)
        {
            Person person = db.Persons.First(x => x.Id == id);

            return View(person);

        }

        //Удаление строк таблиц со значением nulll
        private void CleanNulls()
        {
            db.Phones.RemoveRange(db.Phones.Where(x => x.PersonId == null || x.PhoneNumber == null));

            db.Skypes.RemoveRange(db.Skypes.Where(x => x.PersonId == null || x.SkypeLogin == null));

            db.Emails.RemoveRange(db.Emails.Where(x => x.PersonId == null || x.EmailAddres == null));

            db.AdditionalInfs.RemoveRange(db.AdditionalInfs.Where(x => x.PersonId == null || x.AdditionalInfo == null));



            db.SaveChanges();
        }

        //Проверка листа на null
        private int IsNull<T>(IList<T> list)
        {
                        
            if (list== null)
            {
                return 1;
            }
            else
            {
                return list.Count;
            }
        }

        //Метод для создания ViewBag
        private int[] CreateViewBag(Person p)
        {
            int[] numberOfElements = new int[] { IsNull(p.Phone), IsNull(p.Email), IsNull(p.Skype), IsNull(p.Addinf) };
            return numberOfElements;
        }

        //Проверка на уникальность Имени Фамилии и Отчества
        [HttpGet]
        [HttpPost]
        public ActionResult IsPersonExists(string Name, string PatrName, string Surname,int Id)
        {
            //Если вызван и Add
            if (Id == 0)
            {

                return Json(!db.Persons.Any(x => x.Name == Name && x.PatrName == PatrName && x.Surname == Surname));

            }
            //Если вызван из Change
            else
            {
                return Json(!db.Persons.Any(x => x.Name == Name && x.PatrName == PatrName && x.Surname == Surname && x.Id != Id));
            }
            

        }

        //Проверка на уникальность телефона
        private bool IsUniqPhone(Person p)
        {
            //Если вызван и Add
            if (p.Id == 0)
            {
                foreach(var phone in p.Phone)
                {
                    if (db.Phones.Any(x => x.PhoneNumber == phone.PhoneNumber))
                    {
                        return false;
                    }
                }
            }
            //Если вызван из Change
            else
            {
                foreach(var phone in p.Phone)
                {

                    if (db.Phones.Any(x => x.PhoneNumber == phone.PhoneNumber&&x.PersonId!=p.Id))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //Проверяем на уникальность Email
        private bool IsUniqEmail(Person p)
        {
            //Если вызван и Add
            if (p.Id == 0)
            {
                foreach (var email in p.Email)
                {
                    if (db.Emails.Any(x => x.EmailAddres == email.EmailAddres))
                    {
                        return false;
                    }
                }
            }
            //Если вызван из Change
            else
            {
                foreach (var email in p.Email)
                {

                    if (db.Emails.Any(x => x.EmailAddres == email.EmailAddres && x.PersonId != p.Id))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        //Проверяем на уникальность Skype
        private bool IsUniqSkype(Person p)
        {
            //Если вызван и Add
            if (p.Id == 0)
            {
                foreach (var skype in p.Skype)
                {
                    if (db.Skypes.Any(x => x.SkypeLogin == skype.SkypeLogin))
                    {
                        return false;
                    }
                }
            }
            //Если вызван из Change
            else
            {
                foreach (var skype in p.Skype)
                {

                    if (db.Skypes.Any(x => x.SkypeLogin == skype.SkypeLogin && x.PersonId != p.Id))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //Добавляем ошибка валидации
        private void AddModelError(Person p)
        {
            if (!IsUniqPhone(p))
            {
                ModelState.AddModelError("Phone", "Такой телефон уже существует");
            }
            if (!IsUniqEmail(p))
            {
                ModelState.AddModelError("Email", "Такой Email уже существует");
            }
            if (!IsUniqSkype(p))
            {
                ModelState.AddModelError("Skype", "Такой Skype уже существует");
            }
        }
    }
}
