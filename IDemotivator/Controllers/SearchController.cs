using IDemotivator.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDemotivator.Controllers
{
    public class SearchController : Controller
    {
        private Entities db = new Entities();
        // GET: Search
        public ActionResult Index(string r)
        {
            var uri = new Uri("https://IlwEAOgvDkuHk3yiB74RhwSs1YC0KCUu:@aniknaemm.east-us.azr.facetflow.io");
            var settings = new ConnectionSettings(uri).SetDefaultIndex("indexdem");
            var client = new ElasticClient(settings);

            var result = client.Search<Demotivator>(q => q
            .Query(f => f
               .QueryString(t => t.Query(r + "*").OnFields(u => u.Name)) || f
               .QueryString(t => t.Query(r + "*").OnFields(u => u.Str1)))

            );
            SearchViewModel model = new SearchViewModel();
            List<Demotivator> tr = new List<Demotivator>();


            foreach (var t in result.Hits)
            {
                
                var sleep = (Demotivator)t.Source;
                int temp = new int();

                if (sleep != null)
                {
                   
                    tr.Add(sleep);
                }
                else
                {
                }
                
            }
            model.demotivators = tr;
            return View(model);
        }


        [HttpPost]
        public JsonResult Search(string term)
        {

            var uri = new Uri("https://IlwEAOgvDkuHk3yiB74RhwSs1YC0KCUu:@aniknaemm.east-us.azr.facetflow.io");
            var settings = new ConnectionSettings(uri).SetDefaultIndex("indexdem");
            var client = new ElasticClient(settings);

            var result = client.Search<Demotivator>(q => q
            .Query(f => f
               .QueryString(t => t.Query(term + "*").OnFields(u => u.Name)) || f
               .QueryString(t => t.Query(term + "*").OnFields(u => u.Str1)))

            );


            return Json(result.Hits.Select(t => t.Source), JsonRequestBehavior.AllowGet);
        }

    }
}