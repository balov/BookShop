using BookShop.BookStore.Interfaces;

namespace BookShop.BookStore.Models
{
    public class NameQuantity : INameQuantity
    {
        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
