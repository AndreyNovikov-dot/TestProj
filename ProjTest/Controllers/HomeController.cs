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
            foreach (var person in db.Persons)
            {
                
                
                model.Add(new ViewModel(person.Name,person.Surname,person.PatrName,person.BirthDay,person.Organization,person.Position,person.Id));
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
            
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                PersonRecord p = db.Persons.Include(c => c.Contacts).FirstOrDefault(x => x.Id == id);
                ViewModel model = new ViewModel(p);
                
                ViewBag.numberOfElements = model.CreateViewBag();

                return View(model);
            }
            
        }

        [HttpPost]
        public IActionResult Change(ViewModel p)
        {
            if (ModelState.IsValid)
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
                return RedirectToAction("Index");               
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
                db.Persons.Add(person);
                db.SaveChanges();
                

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
                
                DateTime data;
                DateTime.TryParse(find, out data);
                List<PersonRecord> p=db.Persons.Include(x => x.Contacts).Where(p => DateTime.Equals(p.BirthDay, data)||p.Surname.Contains(find) || p.Name.Contains(find) || p.PatrName.Contains(find) || p.Organization.Contains(find) ||  p.Position.Contains(find)  || p.Contacts.Where(i => i.PersonID == p.Id).Select(c => c.Info).Contains(find)).ToList();
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
            PersonRecord person = db.Persons.Include(x=>x.Contacts).FirstOrDefault(x => x.Id == id);
            ViewModel model = new ViewModel(person);
            return View(model);

        } 
       
       
    }
}
