using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetData()
        {
            List<Contact> contacts = await db.Contacts.ToListAsync();

            return Json(new {data = contacts}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id==0)
            {
                return View(new Contact());
            }
            else
            {
                return View(db.Contacts.Where(x => x.Id == id).FirstOrDefault());
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddOrEdit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (contact.Id == 0)
                {
                    db.Contacts.Add(contact);
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = "Saved Successfuly" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(contact).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = "Updated Successfuly" }, JsonRequestBehavior.AllowGet);
                }
                
            }
            return Json(ModelState);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.Contacts.Remove(db.Contacts.Where(x => x.Id == id).FirstOrDefault());
            db.SaveChangesAsync();
            return Json(new { success = true, message = "Deleted Successfuly" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage(MessageViewModel model)
        {
            List<Contact> contacts = await db.Contacts.Where(x => model.Ids.Contains(x.Id)).ToListAsync();

            var contactList = new List<string>();
            foreach (var contact in contacts)
            {
                contactList.Add(contact.Number);
            }

            var numbers = string.Join(",", contactList.ToArray());

            String message = HttpUtility.UrlEncode(model.Message);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                    {"apikey" , "ANwxGF9BUcY-NsHB120pv3egucdvFlQhGGgTuFHJgL"},
                    {"numbers" , numbers},
                    {"message" , message},
                    {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return Json(new { success = true, message = result}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}