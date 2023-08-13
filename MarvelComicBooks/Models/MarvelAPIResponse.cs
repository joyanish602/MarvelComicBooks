using System.Collections.Generic;

namespace MarvelComicBooks.Models
{
    public class MarvelAPIResponse
    {
        public MarvelAPIData Data { get; set; }
    }

    public class MarvelAPIData
    {
        public List<MarvelAPIResult> Results { get; set; }
    }

    public class MarvelAPIResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<MarvelBookPrice> Prices { get; set; }
        public MarvelAPIThumbnail Thumbnail { get; set; }
    }

    public class MarvelBookPrice
    {
        public string Type { get; set; }
        public double Price { get; set; }
    }

    public class MarvelAPIThumbnail
    {
        public string Path { get; set; }
        public string Extension { get; set; }
    }
}