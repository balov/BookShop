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
        public StoreDb StoreDb { get; set; }

        private Dictionary<string, int> Order = new Dictionary<string, int>();

        public double Buy(params string[] basketByNames)
        {
            double price = 0;

            this.FillOrders(basketByNames);

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

        private void FillOrders(params string[] basketByNames)
        {
            foreach (var bookName in basketByNames)
            {
                if (this.Order.ContainsKey(bookName))
                {
                    this.Order[bookName]++;
                }
                else
                {
                    this.Order.Add(bookName, 1);
                }
            }
        }

        private void CheckAvailabilityOfBooks()
        {
            var nonValidBooks = new List<NameQuantity>();

            foreach (var kvp in this.Order)
            {
                if (this.StoreDb.Catalog.FirstOrDefault(b => b.Name == kvp.Key).Quantity < kvp.Value)
                {
                    var nonValidBook = new NameQuantity
                    {
                        Name = kvp.Key,
                        Quantity = kvp.Value
                    };

                    nonValidBooks.Add(nonValidBook);
                }
            }

            if (nonValidBooks.Any())
            {
                throw new NotEnoughInventoryException(nonValidBooks);
            }
        }
    }
}
