using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjTest.Models;
using System.Reflection;
using System.Collections;

namespace ProjTest.Controllers
{
    //Класс для представления записи БД
    public class TableData
    {
        public TableData(int id,string str) 
        {
            this.StringField = str;
            this.Id = id;
            
        }
        public int Id { get; set; }
        public string StringField { get; set; }
    }

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
                Person person = db.Persons.First(x=>x.Id==p.Id);

                //person.Name = p.Name;
                //person.Organization = p.Organization;
                //person.PatrName = p.PatrName;
                //person.Position = p.Position;
                //person.Surname = p.Surname;
                //person.Phone = p.Phone;
                //person.Skype = p.Skype;
                //person.Addinf = p.Addinf;
                //person.BirthDay = p.BirthDay;
                //person.Email = p.Email;
                //db.Persons.Update(person);
                //db.Phones.UpdateRange(person.Phone);
                //db.Emails.UpdateRange(person.Email);
                //db.Skypes.UpdateRange(person.Skype);
                //db.AdditionalInfs.UpdateRange(person.Addinf);
                db.Entry(db.Persons.First(x => x.Id == p.Id)).CurrentValues.SetValues(p);
                db.SaveChanges();
                

               // CleanNulls();

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

        //Удаление строк таблиц со значением null
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

      

        //Добавляем ошибки валидации
        private void AddModelError(Person p)
        {
            if (!IsExistsInDB(db.Phones.ToList(),p.Phone,p.Id,"PhoneNumber"))
            {
                ModelState.AddModelError("Phone", "Такой телефон уже существует");
            }
            if (!IsExistsInDB(db.Emails.ToList(), p.Email, p.Id, "EmailAddres"))
            {
                ModelState.AddModelError("Email", "Такой Email уже существует");
            }
            if (!IsExistsInDB(db.Skypes.ToList(), p.Skype, p.Id, "SkypeLogin"))
            {
                ModelState.AddModelError("Skype", "Такой Skype уже существует");
            }
            
        }
        //Создание листа из эелементов свойств Phone/Email/Skype сущности Person
        private List<string> MakePersonList<T>(IList<T> listB,string listfield)
        {

            List<string> valueList = new List<string>();            
            string value = "";
            PropertyInfo prop = typeof(T).GetProperty(listfield);
            foreach (var item in listB)
            {
                 
               value=(string)prop.GetValue(item);
               valueList.Add(value); 
                
            }
            return valueList;
        }
        //Создание листа из эелементов таблицы Phone/Email/Skype 
        private List<TableData> MakeTableList<T>(IList<T> listB, string listfield)
        {

            List<TableData> valueList = new List<TableData>();
            
            string value = "";
            int id = 0;
            PropertyInfo propId = typeof(T).GetProperty("PersonId");
            PropertyInfo propvalue = typeof(T).GetProperty(listfield);
            foreach (var item in listB)
            {                
                id = (int)propId.GetValue(item);                    
                value = (string)propvalue.GetValue(item);
                TableData data = new TableData(id, value);
                valueList.Add(data);

            }
            return valueList;
        }

        //Проверка на существования записи в БД
        private bool IsExistsInDB<T>(IList<T> db,IList<T> person,int id,string propertyName)
        {
            List<string> personlist = MakePersonList(person, propertyName);
            List<TableData> dblist = MakeTableList(db, propertyName);
            //Если вызван из Add
            if (id == 0)
            {
                foreach (var item in personlist)
                {
                    if (dblist.Any(x=>x.StringField==item))
                    {
                        return false;
                    }
                }
            }
            //Если вызван из Change
            else
            {
                foreach (var item in personlist)
                {

                    if (dblist.Any(x => x.StringField == item&&x.Id!=id))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
