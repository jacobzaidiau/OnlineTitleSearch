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
            string url = $"https://www.google.com/search?q=online+title+search&num=100";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = $"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string data = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (String.IsNullOrWhiteSpace(response.CharacterSet))
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();
            }

            var x = data.Split(new string[] { "<div class=\"g\">" }, StringSplitOptions.None);

            Search search = new Search
            {
                SearchDate = DateTime.Now
            };

            db.Searches.Add(search);

            db.SaveChanges();

            for (int i = 1; i < x.Length; i++)
            {
                Console.WriteLine(i.ToString());
                //Console.WriteLine("\n\n\n");


                int startIndex = x[i].IndexOf("<a href=\"") + 9;
                int length = x[i].Substring(startIndex).IndexOf("\"");


                int headerStartIndex = x[i].IndexOf("<h3") + 3;
                int headerLength = x[i].Substring(headerStartIndex).IndexOf(">") + 1;

                headerStartIndex += headerLength;
                headerLength = x[i].Substring(headerStartIndex).IndexOf("</h3>");

                string domainUrl = x[i].Substring(startIndex, length);
                List<Domain> domains = (from d in db.Domains
                                        where d.DomainUrl == domainUrl
                                        select d).ToList();

                Domain domain;
                if (domains.Count == 0)
                {
                    domain = new Domain
                    {
                        DomainUrl = x[i].Substring(startIndex, length),
                        DomainTitle = x[i].Substring(headerStartIndex, headerLength)
                    };

                    db.Domains.Add(domain);
                    db.SaveChanges();
                }
                else
                {
                    domain = domains.First();
                }

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
            return View();
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
