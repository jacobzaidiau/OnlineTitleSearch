using OnlineTitleSearch.Context;
using OnlineTitleSearch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineTitleSearch.Controllers
{
    public class ResultController : Controller
    {
        SearchDbContext db = new SearchDbContext();
        // GET: Result
        public ActionResult Index()
        {
            string searchText = "Online title search";

            // if we were to pass the search string as an input, we would need to
            // handle the special characters using UrlEncode.

            searchText = HttpUtility.UrlEncode(searchText).ToLower().Replace(' ', '+');
            string url = $"https://www.google.com/search?q={searchText}&num=100";

            // providing the necessary information to the HttpWebRequest in order to generate a friendly response
            // If an employee knew the CEO's computer, choice of browser, etc, this would be the right place to add
            // this information in.

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Retrieve the data and insert it into the data variable.
            // And return nothing if unable to retrieve a response from Google

            string data = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }
            else { return View(); }


            // with the User Agent chosen, all results are seperated by <div class="g">
            var x = data.Split(new string[] { "<div class=\"g\">" }, StringSplitOptions.None);

            // Add the unique search to the table
            // Do not check if it exists already

            Search search = new Search { SearchDate = DateTime.Now };
            db.Searches.Add(search);
            db.SaveChanges();

            for (int i = 1; i < x.Length; i++)
            {
                // parse the URL between <a href=" and "\>
                int startIndex = x[i].IndexOf("<a href=\"") + 9;
                int length = x[i].Substring(startIndex).IndexOf("\"");

                // parse the domain between <h3 ...> and </h3>
                int headerStartIndex = x[i].IndexOf("<h3") + 3;
                int headerLength = x[i].Substring(headerStartIndex).IndexOf(">") + 1;
                headerStartIndex += headerLength;

                headerLength = x[i].Substring(headerStartIndex).IndexOf("</h3>");

                // Check if the domain is already in the database, and if not add it.
                Domain domain;
                string domainTitle = x[i].Substring(headerStartIndex, headerLength);
                domainTitle = HttpUtility.HtmlDecode(domainTitle);

                string domainUrl = x[i].Substring(startIndex, length);
                List<Domain> domains = (from d in db.Domains
                                        where d.DomainUrl == domainUrl
                                        select d).ToList();

                if (domains.Count == 0)
                {
                    domain = new Domain
                    {
                        DomainUrl = x[i].Substring(startIndex, length),
                        DomainTitle = domainTitle
                    };

                    db.Domains.Add(domain);
                    db.SaveChanges();
                }
                else
                {
                    domain = domains.First();
                }

                // Add the result bridge table to the database
                // we could limit the domains to InfoTrack here
                db.Results.Add(new Result
                {
                    SearchId = search.SearchId,
                    DomainId = domain.DomainId,
                    ResultIndex = i
                });

                db.SaveChanges();
            }

            // Only select the result of the current search, 
            // and only select InfoTrack domains 
            return View(db.Results.Where(
                y => y.SearchId == search.SearchId
                && y.Domain.DomainUrl.StartsWith("https://www.infotrack.com.au/"))
                .OrderBy(z => z.ResultIndex).ToList());
        }

        // GET: Result/Details/5
        public ActionResult Details(int id)
        {
            List<Result> results = (from x in db.Results
                           join s in db.Searches on x.SearchId equals s.SearchId
                           join d in db.Domains on x.DomainId equals d.DomainId
                           where x.SearchId == id && d.DomainUrl.StartsWith("https://www.infotrack.com.au")
                           select new
                           {
                               SearchId = x.SearchId,
                               Search = s,
                               DomainId = x.DomainId,
                               Domain = d,
                               ResultIndex = x.ResultIndex
                           }).ToList().Select(x => new Result {
                               SearchId = x.SearchId,
                               Search = x.Search,
                               DomainId = x.DomainId,
                               Domain = x.Domain,
                               ResultIndex = x.ResultIndex
                           }).ToList();


            return View("Index", results);
        }

        // GET: Result/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Result/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Result/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Result/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Result/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Result/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
