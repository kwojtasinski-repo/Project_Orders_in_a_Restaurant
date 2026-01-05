using NUnit.Framework;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.UnitTests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void given_valid_product_should_create()
        {
            var price = 100M;
            var productName = "Product #1";
            
            Product product = new Product(Guid.NewGuid(), productName, price, ProductKind.Soup);

            product.ShouldNotBeNull();
            product.Price.ShouldBe(price);
            product.ProductName.ShouldBe(productName);
        }

        [Test]
        public void given_empty_product_name_should_throw_an_exception()
        {
            var price = 100M;
            var product = new Product(Guid.NewGuid(), "Product #1", price, ProductKind.Pizza);
            var productName = "";

            var exception = Should.Throw<Exception>(() => product.ChangeProductName(productName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("ProductName cannot be empty");
        }
        

        [Test]
        public void given_invalid_product_name_should_throw_an_exception()
        {
            var price = 100M;
            var product = new Product(Guid.NewGuid(), "Product #1", price, ProductKind.Drink);
            var productName = "#1";

            var exception = Should.Throw<Exception>(() => product.ChangeProductName(productName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("at least 3 characters");
        }

        [Test]
        public void given_valid_product_name_should_change()
        {
            var price = 100M;
            var firstProductName = "Product #1";
            var secondProductName = "Product";
            var product = new Product(Guid.NewGuid(), firstProductName, price, ProductKind.Soup);

            product.ChangeProductName(secondProductName);

            product.ProductName.ShouldNotBe(firstProductName);
            product.ProductName.ShouldBe(secondProductName);
        }

        [Test]
        public void given_negative_product_price_should_throw_an_exception()
        {
            var price = -100M;
            var product = new Product(Guid.NewGuid(), "Product #1", 100M, ProductKind.MainDish);

            var exception = Should.Throw<Exception>(() => product.ChangePrice(price));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("cannot be negative");
        }

        [Test]
        public void given_valid_product_price_should_throw_an_exception()
        {
            var firstPrice = 100M;
            var secondPrice = 200M;
            var product = new Product(Guid.NewGuid(), "Product #1", firstPrice, ProductKind.Pizza);

            product.ChangePrice(secondPrice);

            product.Price.ShouldNotBe(firstPrice);
            product.Price.ShouldBe(secondPrice);
        }

        [Test]
        public void given_valid_orders_should_add()
        {
            var product = new Product(Guid.NewGuid(), "Product #1", 100M, ProductKind.MainDish);
            var orders = new List<Order>() { new Order(Guid.NewGuid(), "ORDER", DateTime.UtcNow, 100M, Email.Of("email@email.com")) };

            product.AddOrders(orders);

            product.Orders.ShouldNotBeEmpty();
            product.Orders.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void given_null_orders_should_throw_an_exception()
        {
            var product = new Product(Guid.NewGuid(), "Product #1", 100M, ProductKind.Drink);

            var exception = Should.Throw<Exception>(() => product.AddOrders(null));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("Cannot add empty orders");
        }
    }
}
