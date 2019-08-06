using System.Collections.Generic;

namespace BookShop.BookStore.Models
{
    public class StoreDb
    {
        public List<Category> Category { get; set; }

        public List<Book> Catalog { get; set; }
    }
}
