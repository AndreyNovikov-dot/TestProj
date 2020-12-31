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
using System.Collections;
using System.Globalization;

namespace ProjTest.Controllers
{
    //Класс для представления записи БД
 
    public class HomeController : Controller
    {

        Context db;
        
        public HomeController(Context context)
        {
            db = context;           
        }

        public IActionResult Index()
        {
            List<ViewModel> model = new List<ViewModel>();
            foreach (var person in db.Persons.Include(x => x.Contacts))
            {
                ViewModel p = new ViewModel(person);
                model.Add(p);
            }

            return View(model);

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

               //CleanNulls();

                return RedirectToAction("Index");
            }
            
        }

        [HttpGet]
        public IActionResult Change(int? id)
        {

            //if (id == null)
            //{
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    Person person = db.Persons.First(x => x.Id == id);

            //    ViewBag.numberOfElements = CreateViewBag(person);

            //    return View(person);
            //}
            return View();
        }

        [HttpPost]
        public IActionResult Change(Person p)
        {

            //AddModelError(p);
           
            //if (ModelState.IsValid)
            //{
               
            //    Person person = db.Persons.First(x => x.Id == p.Id);
               
            //    person.Name = p.Name;
            //    person.Organization = p.Organization;
            //    person.PatrName = p.PatrName;
            //    person.Position = p.Position;
            //    person.Surname = p.Surname;
            //    person.Phone = p.Phone;
            //    person.Skype = p.Skype;
            //    person.Addinf = p.Addinf;
            //    person.BirthDay = p.BirthDay;
            //    person.Email = p.Email;
            //    db.SaveChanges();

            //    CleanNulls();

            //    return RedirectToAction("Index");
            //}

            

            //ViewBag.numberOfElements = CreateViewBag(p);


            return View(p);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ViewModel m)
        {
            //AddModelError(p);
            Person person = m.ConvertToPerson();
            

           // if (ModelState.IsValid)
         //   {
                db.Persons.Add(m.Person);
                db.SaveChanges();
                //CleanNulls();

                return RedirectToAction("Index");
           // }
          //  else
         //   {
            //    return View();
          //  }
            
        }

        public IActionResult Find(String find)
        {
            if (find != null)
            {
                string[] split = find.Split(' ');
                List<Person> p=db.Persons.Include(x => x.Contacts).Where(p => p.Name.Contains(find) || p.Organization.Contains(find) || p.PatrName.Contains(find) || p.Position.Contains(find) || p.Surname.Contains(find) || p.Contacts.Where(i => i.PersonID == p.Id).Select(c => c.Info).Contains(find)).ToList();
                List<ViewModel> model = new List<ViewModel>();
                foreach (var person in p)
                {
                    ViewModel m = new ViewModel(person);
                    model.Add(m);
                }

                return View(model);
               // return View(db.Persons.Where(p => p.Name.Contains(find) || p.Organization.Contains(find) || p.PatrName.Contains(find) || p.Position.Contains(find) || p.Surname.Contains(find)||p.Contacts.Where(i=>i.PersonID==p.Id).Select(c=>c.Info).Contains(find)));
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public IActionResult ViewPerson(int? id)
        {
            Person person = db.Persons.Include(x=>x.Contacts).FirstOrDefault(x => x.Id == id);
            ViewModel model = new ViewModel(person);
            return View(model);

        }

        //Удаление строк таблиц со значением null
        //private void CleanNulls()
        //{
        //    db.Phones.RemoveRange(db.Phones.Where(x => x.PersonId == null || x.PhoneNumber == null));

        //    db.Skypes.RemoveRange(db.Skypes.Where(x => x.PersonId == null || x.SkypeLogin == null));

        //    db.Emails.RemoveRange(db.Emails.Where(x => x.PersonId == null || x.EmailAddres == null));

        //    db.AdditionalInfs.RemoveRange(db.AdditionalInfs.Where(x => x.PersonId == null || x.AdditionalInfo == null));



        //    db.SaveChanges();
        //}
       
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
        //private int[] CreateViewBag(Person p)
        //{
        //    int[] numberOfElements = new int[] { IsNull(p.Phone), IsNull(p.Email), IsNull(p.Skype), IsNull(p.Addinf) };
        //    return numberOfElements;
        //}

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
        //private void AddModelError(Person p)
        //{
        //    if (!IsExistsInDB(db.Phones.ToList(),p.Phone,p.Id,"PhoneNumber"))
        //    {
        //        ModelState.AddModelError("Phone", "Такой телефон уже существует");
        //    }
        //    if (!IsExistsInDB(db.Emails.ToList(), p.Email, p.Id, "EmailAddres"))
        //    {
        //        ModelState.AddModelError("Email", "Такой Email уже существует");
        //    }
        //    if (!IsExistsInDB(db.Skypes.ToList(), p.Skype, p.Id, "SkypeLogin"))
        //    {
        //        ModelState.AddModelError("Skype", "Такой Skype уже существует");
        //    }
            
        //}
        //Создание листа из элементов свойств Phone/Email/Skype сущности Person
        //private List<string> MakePersonList<T>(IList<T> listB, string listfield)
        //{

        //    List<string> valueList = new List<string>();
        //    string value = "";
        //    PropertyInfo prop = typeof(T).GetProperty(listfield);
        //    foreach (var item in listB)
        //    {

        //        value = (string)prop.GetValue(item);
        //        valueList.Add(value);

        //    }
        //    return valueList;
        //}
        //Создание листа из элементов таблицы Phone/Email/Skype 
        //private List<TableData> MakeTableList<T>(IList<T> listB, string listfield)
        //{

        //    List<TableData> valueList = new List<TableData>();

        //    string value = "";
        //    int id = 0;
        //    PropertyInfo propId = typeof(T).GetProperty("PersonId");
        //    PropertyInfo propvalue = typeof(T).GetProperty(listfield);
        //    foreach (var item in listB)
        //    {
        //        id = (int)propId.GetValue(item);
        //        value = (string)propvalue.GetValue(item);
        //        TableData data = new TableData(id, value);
        //        valueList.Add(data);

        //    }
        //    return valueList;
        //}

        //Проверка на существования записи в БД
        //private bool IsExistsInDB<T>(IList<T> db,IList<T> person,int id,string propertyName)
        //{
        //    List<string> personlist = MakePersonList(person, propertyName);
        //    List<TableData> dblist = MakeTableList(db, propertyName);
        //    //Если вызван из Add
        //    if (id == 0)
        //    {
        //        foreach (var item in personlist)
        //        {
        //            if (dblist.Any(x=>x.StringField==item))
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    //Если вызван из Change
        //    else
        //    {
        //        foreach (var item in personlist)
        //        {

        //            if (dblist.Any(x => x.StringField == item&&x.Id!=id))
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
       // }
    }
}
