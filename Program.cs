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

            var jsonFilePath = GetJsonFilePath();

            if (!File.Exists(jsonFilePath))
            {
                throw new Exception(StoreConstants.FileNotFoundExceptionMessage);
            }

            using (var streamReader = new StreamReader(jsonFilePath))
            {
                string bookStoreJson = streamReader.ReadToEnd();

                store.Import(bookStoreJson);
            }

            Console.WriteLine("List of available books:");
            Console.WriteLine(string.Join("\n", store.GetBookNames()));
            Console.WriteLine("List of available commands:");
            Console.WriteLine("1. Quantity : book name");
            Console.WriteLine("2. Buy book : names separated by ,");

            while (true)
            {
                var input = Console.ReadLine().Split(new[] { ":", "," }, StringSplitOptions.RemoveEmptyEntries);
                input = TrimArrayParams(input);

                if (input.Length < 2)
                {
                    Console.WriteLine("Invalid command!");
                    continue;
                }

                try
                {
                    switch (input[0])
                    {
                        case "Buy":
                            var totalPrice = store.Buy(input.Skip(1).ToArray());
                            Console.WriteLine($"Total price: {totalPrice}");
                            break;
                        case "Quantity":
                            Console.WriteLine($"The remaining quantity of {input[1]} is {store.Quantity(input[1])}");
                            break;
                        default:
                            Console.WriteLine("Invalid command!");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }

            }

        }

        private static string[] TrimArrayParams(string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = array[i].Trim();
            }

            return array;
        }

        private static string GetJsonFilePath()
        {
            string rootPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            return $"{rootPath}{StoreConstants.JsonFilePath}";
        }
    }
}
