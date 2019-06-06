
using System;
using System.Collections.Generic;

namespace St.BookStore.Core.Exceptions
{
    public class NotEnoughInventoryException : Exception
    {
        public IEnumerable<INameQuantity> Missing { get; }

        public NotEnoughInventoryException(IEnumerable<INameQuantity> missing) : base()
        {
            this.Missing = missing;
        }
    }
}