using BookShop.BookStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookShop.BookStore.Exceptions
{
    public class NotEnoughInventoryException : Exception
    {
        public NotEnoughInventoryException(IEnumerable<INameQuantity> missing)
            : base($"Not Enough Inventory For Books: {string.Join(" ", missing.Select(nq => nq.Name))}")
        {
            this.Missing = missing;
        }

        public IEnumerable<INameQuantity> Missing { get; }
    }
}
