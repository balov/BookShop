using BookShop.BookStore;
using BookShop.BookStore.Constants;
using System;
using System.IO;
using System.Linq;

namespace BookShop
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var store = new Store();

            if (!File.Exists(StoreConstants.JsonFilePath))
            {
                throw new Exception(StoreConstants.FileNotFoundExceptionMessage);
            }

            using (var streamReader = new StreamReader(StoreConstants.JsonFilePath))
            {
                string bookStoreJson = streamReader.ReadToEnd();

                store.Import(bookStoreJson);
            }

            Console.WriteLine($"List of available books: {string.Join(" ", store.StoreDb.Catalog.Select(b => b.Name))} ");
            Console.WriteLine("List of available commands:");
            Console.WriteLine("1. Quantity book name");
            Console.WriteLine("2. Buy book names separated by space");

            while (true)
            {
                var input = Console.ReadLine().Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);

                switch (input[0])
                {
                    case "Buy":
                        store.Buy(input.Skip(1).ToArray());
                        break;
                    case "Quantity":
                        Console.WriteLine($"The remaining quantity of {input[1]} is {store.Quantity(input[1])}");
                        break;
                    default:
                        Console.WriteLine("Invalid command!");
                        break;
                }
            }

        }
    }
}
