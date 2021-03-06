using System;
using System.Collections.Generic;
using DddCqrsExample.Framework;

namespace DddCqrsExample.Domain.Orders
{
    public class SalesOrder : Aggregate
    {
        public SalesOrder()
        {
            Lines = new List<SalesOrderLine>();
        }

        public Money OrderValue { get; private set; }
        public Money MaxCustomerOrderValue { get; private set; }
        public IList<SalesOrderLine> Lines { get; private set; }

        public void Create(string id, Money maxCustomerOrderValue)
        {
            Id = id;
            OrderValue = new Money(0, maxCustomerOrderValue.Currency);
            MaxCustomerOrderValue = maxCustomerOrderValue;

            Record(new SalesOrderCreatedEvent(id, maxCustomerOrderValue, DateTimeOffset.Now));
        }

        public void AddItem(Sku sku, uint quantity, Money unitPrice)
        {
            if (sku == null) throw new ArgumentNullException("sku");
            if (unitPrice == null) throw new ArgumentNullException("unitPrice");

            if (OrderValue != null && unitPrice.Currency != OrderValue.Currency)
            {
                throw new ArgumentException(string.Format("Unable to mix currencies on an order (SalesOrder value: {0}, supplied unit price: {1}", OrderValue, unitPrice));
            }

            Money itemsValue = quantity * unitPrice;
            if ((OrderValue + itemsValue) > MaxCustomerOrderValue)
            {
                throw new InvalidOperationException(string.Format("Adding items with value of {0} would take the current order value of {1} over the customer allowed maximum of {2}", itemsValue, OrderValue, MaxCustomerOrderValue));
            }

            OrderValue += quantity * unitPrice;

            Lines.Add(new SalesOrderLine(sku, quantity, unitPrice));

            Record(new ItemsAddedToSalesOrderEvent(Id, sku, quantity, unitPrice, DateTimeOffset.Now));
        }
    }
}