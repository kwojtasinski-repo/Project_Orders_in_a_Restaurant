﻿using Restaurant.Domain.Exceptions;
using System;
using System.Text.RegularExpressions;

namespace Restaurant.Domain.Entities
{
    public class ProductSale : BaseEntity<Guid>
    {
        private ProductSale()
        {
        }

        public ProductSale(Guid id, Product product, ProductSaleState productSaleState, Email email, Addition addition = null, Guid? orderId = null, Order order = null)
            : base(id)
        {
            ChangeProduct(product);

            if (addition != null)
            {
                ChangeAddition(addition);
            }

            OrderId = orderId;
            Order = order;
            ProductSaleState = productSaleState;
            Email = email;
        }

        public Guid ProductId { get; private set; }
        public Product Product { get; private set; }
        public Guid? AdditionId { get; private set; } = null;
        public Addition Addition { get; private set; } = null;
        public decimal EndPrice { get; private set; } = decimal.Zero;
        public Guid? OrderId { get; private set; }
        public Order Order { get; private set; } = null;
        public ProductSaleState ProductSaleState { get; private set; } = ProductSaleState.New;
        public Email Email { get; private set; }

        public void ChangeProduct(Product product)
        {
            if (product is null)
            {
                throw new RestaurantException("Cannot add null product", typeof(ProductSale).FullName, "Product");
            }

            if (Product != null)
            {
                EndPrice -= Product.Price;
            }

            Product = product;
            ProductId = product.Id;
            EndPrice += product.Price;
        }

        public void ChangeAddition(Addition addition)
        {
            if (addition is null)
            {
                throw new RestaurantException("Cannot add null addition", typeof(ProductSale).FullName, "Addition");
            }

            if (Addition != null)
            {
                EndPrice -= Addition.Price;
            }

            Addition = addition;
            AdditionId = addition.Id;
            EndPrice += addition.Price;
        }

        public void RemoveAddition()
        {
            if (Addition is null)
            {
                throw new RestaurantException("There is no addition in product", typeof(ProductSale).FullName, "Additon");
            }

            EndPrice -= Addition.Price;
            Addition = null;
            AdditionId = null;
        }

        public void AddOrder(Order order)
        {
            if (order is null)
            {
                throw new RestaurantException("Cannot assign null order", typeof(ProductSale).FullName, "AddOrder");
            }

            if (Order != null)
            {
                throw new RestaurantException("Cannot override existing order", typeof(ProductSale).FullName, "AddOrder");
            }

            Order = order;
            OrderId = order.Id;
            ProductSaleState = ProductSaleState.Ordered;
        }

        public void RemoveOrder()
        {
            if (Order is null)
            {
                throw new RestaurantException("Cannot remove product, because there is no ordered", typeof(ProductSale).FullName, "RemoveOrder");
            }

            Order = null;
            OrderId = null;
            ProductSaleState = ProductSaleState.New;
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }
    }
}
