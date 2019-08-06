using BookShop.BookStore.Interfaces;
using System;
using System.Collections.Generic;

namespace BookShop.BookStore.Exceptions
{
    public class NotEnoughInventoryException : Exception
    {
        public IEnumerable<INameQuantity> Missing { get; }
    }
}
