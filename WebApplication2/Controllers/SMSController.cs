using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class SMSController : Controller
    {
        // GET: SMS
        public ActionResult SendSms()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Send(SMSViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            String message = HttpUtility.UrlEncode(model.Message);
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                    {"apikey" , "ANwxGF9BUcY-NsHB120pv3egucdvFlQhGGgTuFHJgL"},
                    {"numbers" , model.Number},
                    {"message" , message},
                    {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return RedirectToAction("SendSms");
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to send message");
            return View(model);
        }
    }
}