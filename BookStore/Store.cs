using BookShop.BookStore.Exceptions;
using BookShop.BookStore.Interfaces;
using BookShop.BookStore.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.BookStore
{
    public class Store : IStore
    {
        private StoreDb StoreDb { get; set; }

        private List<NameQuantity> Order = new List<NameQuantity>();

        public double Buy(params string[] basketByNames)
        {
            this.FillOrders(basketByNames);

            this.CheckAvailabilityOfBooks();

            var price = this.CalculateBookPrice();

            this.UpdateStoreAfterOrder();

            this.Order.Clear();

            return price;
        }

        public void Import(string catalogAsJson)
        {
            this.StoreDb = JsonConvert.DeserializeObject<StoreDb>(catalogAsJson);
        }

        public int Quantity(string name)
        {
            return this.StoreDb.Catalog.FirstOrDefault(b => b.Name == name).Quantity;
        }

        public IEnumerable<string> GetBookNames()
        {
            return this.StoreDb.Catalog.Select(b => b.Name);
        }

        private void FillOrders(params string[] basketByNames)
        {
            foreach (var bookName in basketByNames)
            {
                var book = new NameQuantity
                {
                    Name = bookName,
                    Quantity = 1
                };

                if (this.Order.Any(b => b.Name == bookName))
                {
                    this.Order.FirstOrDefault(b => b.Name == bookName).Quantity++;
                }
                else
                {
                    this.Order.Add(book);
                }
            }
        }

        private void CheckAvailabilityOfBooks()
        {
            var nonValidBooks = new List<NameQuantity>();

            foreach (var book in this.Order)
            {
                if (this.StoreDb.Catalog.FirstOrDefault(b => b.Name == book.Name).Quantity < book.Quantity)
                {
                    nonValidBooks.Add(book);
                }
            }

            if (nonValidBooks.Any())
            {
                this.Order.Clear();

                throw new NotEnoughInventoryException(nonValidBooks);
            }
        }

        private void UpdateStoreAfterOrder()
        {
            foreach (var book in this.Order)
            {
                this.StoreDb.Catalog.FirstOrDefault(b => b.Name == book.Name).Quantity -= book.Quantity;
            }
        }

        private double CalculateBookPrice()
        {
            double totalBookPrice = 0;

            var orderedBooks = this.StoreDb.Catalog.Where(b => this.Order.Select(o => o.Name).Contains(b.Name));

            foreach (var book in orderedBooks)
            {
                var quantity = this.Order.FirstOrDefault(b => b.Name == book.Name).Quantity;

                if (orderedBooks.Where(b => b.Category == book.Category).Count() > 1)
                {
                    var discount = this.StoreDb.Category.FirstOrDefault(c => c.Name == book.Category).Discount;

                    if (quantity > 1)
                    {
                        totalBookPrice += (book.Price * (1 - discount) + book.Price * (quantity - 1));
                    }
                    else
                    {
                        totalBookPrice += (book.Price * (1 - discount));
                    }
                }
                else
                {
                    totalBookPrice += (book.Price * quantity);
                }
            }

            return totalBookPrice;
        }
    }
}
