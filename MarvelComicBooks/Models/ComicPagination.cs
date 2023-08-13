
namespace MarvelComicBooks.Models
{
    public class ComicPagination
    {
        public string SearchQuery { get; set; }
        public int SortBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}