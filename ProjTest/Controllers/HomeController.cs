using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjTest.Models;
using Microsoft.Extensions.Logging;

namespace ProjTest.Controllers
{
    
 
    public class HomeController : Controller
    {

        Context db;
        private readonly ILogger<HomeController> logger;

        public HomeController(Context context, ILogger<HomeController> log)
        {
            db = context;
            logger = log;
        }

        public IActionResult Index()
        {
            
            List<ViewModel> model = new List<ViewModel>();
            foreach (var person in db.Persons)
            {          
                model.Add(new ViewModel(person));
            }
            logger.LogInformation("Вход на главную страницу");
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
                try
                {
                    db.Persons.Remove(db.Persons.First(x => x.Id == id));
                    db.SaveChanges();
                    logger.LogInformation("Удален пользователь с ID:{0}", id);

                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    logger.LogInformation(" Ошибка удаления пользователя с ID:{0} Ошибка:{1}", id,ex.Message);
                    return RedirectToAction("Index");
                }
               
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
                               
                PersonRecord p = db.Persons.Include(c => c.Contacts).FirstOrDefault(x => x.Id == id);
                if (p!=null)
                {
                    ViewModel model = new ViewModel(p);

                    ViewBag.numberOfElements = model.CreateViewBag();

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            
        }

        [HttpPost]
        public IActionResult Change(ViewModel p)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    PersonRecord person = db.Persons.Include(c => c.Contacts).First(x => x.Id == p.Id);
                    PersonRecord updatedPerson = p.ConvertToPerson();

                    if (person != null)
                    {

                        db.Entry(person).CurrentValues.SetValues(updatedPerson);
                    }

                    foreach (var contact in person.Contacts.ToList())
                    {
                        if (updatedPerson.Contacts.FirstOrDefault(c => c.Info == contact.Info && c.Type == contact.Type) == null || updatedPerson.Contacts.Where(c => c.Info == contact.Info && c.Type == contact.Type).Count() < person.Contacts.Where(c => c.Info == contact.Info && c.Type == contact.Type).Count())
                        {
                            person.Contacts.Remove(contact);
                            db.Contacts.Remove(contact);
                        }
                    }
                    foreach (var contact in updatedPerson.Contacts.ToList())
                    {

                        if (person.Contacts.FirstOrDefault(c => c.Info == contact.Info && c.Type == contact.Type) == null || updatedPerson.Contacts.Where(c => c.Info == contact.Info && c.Type == contact.Type).Count() > person.Contacts.Where(c => c.Info == contact.Info && c.Type == contact.Type).Count())
                        {

                            var personContact = new ContactInfo()
                            {
                                Info = contact.Info,
                                Type = contact.Type,
                                PersonID = person.Id,
                                Person = person
                            };
                            person.Contacts.Add(personContact);
                            db.Contacts.Add(personContact);

                        }
                    }
                    db.SaveChanges();
                    logger.LogInformation("Изменен пользователь ID:{0}", p.Id);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    logger.LogInformation(" Ошибка изменения пользователя с ID:{0} Ошибка:{1}", p.Id, ex.Message);
                    ModelState.AddModelError("Name", "Ошибка при изменении пользователя. Повторите попытку");
                    ViewBag.numberOfElements = p.CreateViewBag();
                    return View(p);
                }
            }
            else
            {
                ViewBag.numberOfElements = p.CreateViewBag();
                return View(p);
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ViewModel m)
        {
            
            PersonRecord person = m.ConvertToPerson();
            

            if (ModelState.IsValid)
            {
                
                try
                {
                    db.Persons.Add(person);
                    db.SaveChanges();
                    logger.LogInformation("Добавлен пользователь {0}", m.Name);
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    logger.LogInformation(" Ошибка изменения пользователя с Именем:{0} Ошибка:{1}", m.Name, ex.Message);
                    ModelState.AddModelError("Name", "Ошибка при добавлении пользователя. Повторите попытку");
                    return View();
                }
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
                
                DateTime data;
                DateTime.TryParse(find, out data);
                List<PersonRecord> p=db.Persons.Include(x => x.Contacts).Where(p => DateTime.Equals(p.BirthDay, data)||p.Surname.Contains(find) || p.Name.Contains(find) || p.PatrName.Contains(find) || p.Organization.Contains(find) ||  p.Position.Contains(find)  || p.Contacts.Where(i => i.PersonID == p.Id).Select(c => c.Info).Contains(find)).ToList();
                if (p != null)
                {
                    List<ViewModel> model = new List<ViewModel>();
                    foreach (var person in p)
                    {
                        ViewModel m = new ViewModel(person);
                        logger.LogInformation("Найден пользователь с ID:{0}", person.Id);
                        model.Add(m);
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
                             
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult ViewPerson(int? id)
        {
            if (id != null)
            {
                PersonRecord person = db.Persons.Include(x => x.Contacts).FirstOrDefault(x => x.Id == id);
                if (person != null)
                {
                    ViewModel model = new ViewModel(person);
                    logger.LogInformation("Просмотрен пользователь с ID:{0}", person.Id);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            

        }  
    }
}
