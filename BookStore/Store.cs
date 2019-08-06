using BookShop.BookStore.Interfaces;
using BookShop.BookStore.Models;
using Newtonsoft.Json;
using System;

namespace BookShop.BookStore
{
    public class Store : IStore
    {
        public StoreDb StoreDb { get; set; }

        public double Buy(params string[] basketByNames)
        {
            throw new NotImplementedException();
        }

        public void Import(string catalogAsJson)
        {
            this.StoreDb = JsonConvert.DeserializeObject<StoreDb>(catalogAsJson);
        }

        public int Quantity(string name)
        {
            throw new NotImplementedException();
        }
    }
}
