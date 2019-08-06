using BookShop.BookStore;
using BookShop.BookStore.Constants;
using System;
using System.IO;

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


        }
    }
}
