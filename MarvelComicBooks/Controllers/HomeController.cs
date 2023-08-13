using MarvelComicBooks.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarvelComicBooks.Controllers
{
    public class HomeController : Controller
    {
        private const string BaseUrl = "https://gateway.marvel.com/v1/public/comics";   // Marvel base URL
        private const string publicKey = "f33ffc425056f1ec0db06cc9a68ea5aa";            // Marvel public key
        private const string privateKey = "e676148e5685a4aa3679ffbaafdcfa35b71eb71f";   // Marvel private key

        public async Task<ActionResult> Index(ComicPagination pageModel)
        {
            if (!(pageModel.PageNumber > 0))
            {
                pageModel.PageNumber = 1;
            }

            if (!(pageModel.PageSize > 0))
            {
                pageModel.PageSize = 5;
            }


            var client = new HttpClient();
            var timestamp = DateTime.Now.Ticks.ToString();

            var hashString = GenerateHash(timestamp);

            var stringURLQuery = $"?apikey={publicKey}&ts={timestamp}{(pageModel.SortBy == 1 ? "&orderBy=title" : "")}&hash={hashString}{(pageModel.SearchQuery != null & pageModel.SearchQuery != "" ? "&titleStartsWith=" + pageModel.SearchQuery : "")}";

            var apiUrl = $"{BaseUrl}{stringURLQuery}";

            var response = await client.GetAsync(apiUrl);   // Get Comic books
            var responseData = await response.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<MarvelAPIResponse>(responseData);

            var comics = deserializedResponse.Data.Results.Select(c => new ComicBook
            {
                Title = c.Title,
                Description = c.Description,
                Price = c.Prices[0].Price,
                ThumbnailUrl = $"{c.Thumbnail.Path}.{c.Thumbnail.Extension}"
            }).ToList();

            ViewBag.Pagination = pageModel;

            return View(comics.ToPagedList(pageModel.PageNumber, pageModel.PageSize));  // PagedList - For pagination
        }

        public string GenerateHash(string timestamp)
        {
            var input = timestamp + privateKey + publicKey;
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hash).ToLower().Replace("-", "");
        }
    }
}