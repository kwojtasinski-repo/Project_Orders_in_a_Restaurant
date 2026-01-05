using NUnit.Framework;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using Shouldly;
using System;

namespace Restaurant.UnitTests
{
    [TestFixture]
    public class OrdersTests
    {
        [Test]
        public void given_valid_order_should_create()
        {
            var orderNumber = "ORDER/1/2";
            var price = 100M;
            var email = Email.Of("email@test.email.com");

            var order = new Order(Guid.NewGuid(), orderNumber, DateTime.UtcNow, price, email);

            order.ShouldNotBeNull();
            order.OrderNumber.ShouldBe(orderNumber);
            order.Price.ShouldBe(price);
            order.Email.ShouldBe(email);
        }

        [Test]
        public void given_invalid_order_number_should_throw_an_exception()
        {
            var firstOrderNumber = "ORDER/1/2";
            var secondOrderNumber = "";
            var order = new Order(Guid.NewGuid(), firstOrderNumber, DateTime.UtcNow, 100M, Email.Of("email@test.email.com"));

            var exception = Should.Throw<Exception>(() => order.ChangeOrderNumber(secondOrderNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("cannot be empty");
        }

        [Test]
        public void given_too_short_order_number_should_throw_an_exception()
        {
            var firstOrderNumber = "ORDER/1/2";
            var secondOrderNumber = "O";
            var order = new Order(Guid.NewGuid(), firstOrderNumber, DateTime.UtcNow, 100M, Email.Of("email@test.email.com"));

            var exception = Should.Throw<Exception>(() => order.ChangeOrderNumber(secondOrderNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("at least 3 characters");
        }

        [Test]
        public void given_valid_order_number_should_change()
        {
            var firstOrderNumber = "ORDER/1/2";
            var secondOrderNumber = "ORDER/1/2/3";
            var order = new Order(Guid.NewGuid(), firstOrderNumber, DateTime.UtcNow, 100M, Email.Of("email@test.email.com"));

            order.ChangeOrderNumber(secondOrderNumber);

            order.OrderNumber.ShouldNotBe(firstOrderNumber);
            order.OrderNumber.ShouldBe(secondOrderNumber);
        }

        [Test]
        public void given_negative_price_should_throw_an_exception()
        {
            var firstPrice = 100M;
            var secondPrice = -200M;
            var order = new Order(Guid.NewGuid(), "ORDER/1/2", DateTime.UtcNow, firstPrice, Email.Of("email@test.email.com"));

            var exception = Should.Throw<Exception>(() => order.ChangePrice(secondPrice));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("cannot be negative");
        }

        [Test]
        public void given_valid_price_should_change()
        {
            var firstPrice = 100M;
            var secondPrice = 200M;
            var order = new Order(Guid.NewGuid(), "ORDER/1/2", DateTime.UtcNow, firstPrice, Email.Of("email@test.email.com"));

            order.ChangePrice(secondPrice);

            order.Price.ShouldNotBe(firstPrice);
            order.Price.ShouldBe(secondPrice);
        }

        [Test]
        public void given_empty_email_should_throw_an_exception()
        {
            var secondEmail = "";
            
            var exception = Should.Throw<Exception>(() => Email.Of(secondEmail));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("Email cannot be empty");
        }

        [Test]
        public void given_invalid_email_should_throw_an_exception()
        {
            var secondEmail = "admin.com";
         
            var exception = Should.Throw<Exception>(() => Email.Of(secondEmail));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<RestaurantException>();
            exception.Message.ShouldContain("Invalid Email");
        }

        [Test]
        public void given_valid_email_should_change()
        {
            var firstEmail = Email.Of("email@email.com");
            var secondEmail = Email.Of("admin@gmail.com");
            var order = new Order(Guid.NewGuid(), "ORDER/1/2", DateTime.UtcNow, 100M, firstEmail);

            order.ChangeEmail(secondEmail);

            order.Email.ShouldNotBe(firstEmail);
            order.Email.ShouldBe(secondEmail);
        }
    }
}
