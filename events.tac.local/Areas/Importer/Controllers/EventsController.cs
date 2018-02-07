using events.tac.local.Areas.Importer.Models;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace events.tac.local.Areas.Importer.Controllers
{
    public class EventsController : Controller
    {
        // GET: Importer/Events
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, string parentPath)
        {
            IEnumerable<Event> events = null;
            string message = null;

            using (var reader = new System.IO.StreamReader(file.InputStream))
            {
                var contents = reader.ReadToEnd();

                try
                {
                    events = JsonConvert.DeserializeObject<IEnumerable<Event>>(contents);
                }
                catch(Exception ex)
                {
                    // to be added later
                    return null;
                }

                Database database = Sitecore.Configuration.Factory.GetDatabase("master");
                Item parentItem = database.GetItem(parentPath);

                // ID is the Sitecore ItemId for the Event Details template - this will differ...
                TemplateID templateID = new TemplateID(new ID("{216DB771-5D7E-4AE7-B686-A4D552C49B27}"));
                using (new SecurityDisabler())
                {
                    foreach (var ev in events)
                    {
                        string name = ItemUtil.ProposeValidItemName(ev.ContentHeading);
                        Item item = parentItem.Add(name, templateID);
                        item.Editing.BeginEdit();
                        //assign values for all the fields, for example for ContentHeading:
                        item["ContentHeading"] = ev.ContentHeading;
                        item["ContentIntro"] = ev.ContentIntro;
                        item["Difficulty"] = ev.Difficulty;
                        item["Duration"] = ev.Duration;
                        item["Highlights"] = ev.Highlights;
                        item["StartDate"] = Sitecore.DateUtil.ToIsoDate(ev.StartDate);
                        item.Editing.EndEdit();
                    }
                }

                return View();
            }
        }
    }
}