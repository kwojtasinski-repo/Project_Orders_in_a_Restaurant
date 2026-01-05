using NUnit.Framework;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.IntegrationTests.Common;
using Shouldly;
using System;

namespace Restaurant.IntegrationTests
{
    [TestFixture]
    public class OrderRepositoryTests : BaseTest
    {
        private IOrderRepository repository;

        [SetUp]
        public void Setup()
        {
            repository = GetService<IOrderRepository>();
        }

        [Test]
        public void given_valid_order_should_add_to_db()
        {
            var order = new Order(id: Guid.NewGuid(), price: 105.50M, orderNumber: "ORD/5", email: Email.Of("email@email.com"), created: DateTime.UtcNow);

            repository.Add(order);

            var orderFromDb = repository.Get(order.Id);
            orderFromDb.ShouldNotBeNull();
            orderFromDb.OrderNumber.ShouldBe(order.OrderNumber);
        }

        [Test]
        public void given_valid_id_should_delete_order()
        {
            var order = new Order(id: Guid.NewGuid(), price: 105.50M, orderNumber: "ORD/502", email: Email.Of("email2@email.com"), created: DateTime.UtcNow);
            repository.Add(order);

            repository.Delete(order.Id);

            var orderFromDb = repository.Get(order.Id);
            orderFromDb.ShouldBeNull();
        }

        [Test]
        public void should_return_products()
        {
            var orders = repository.GetAll();

            orders.ShouldNotBeNull();
            orders.ShouldNotBeEmpty();
        }

        [Test]
        public void given_valid_product_should_update()
        {
            var order = new Order(id: Guid.NewGuid(), price: 105.50M, orderNumber: "ORD/25", email: Email.Of("email@email.com"), created: DateTime.UtcNow);
            repository.Add(order);
            var orderModified = new Order(id: order.Id, price: 125.55M, orderNumber: "ORD/524", email: order.Email, created: order.Created);

            repository.Update(orderModified);

            var prderFromDb = repository.Get(order.Id);
            prderFromDb.ShouldNotBeNull();
            prderFromDb.Price.ShouldBe(orderModified.Price);
            prderFromDb.OrderNumber.ShouldBe(orderModified.OrderNumber);
        }

        [Test]
        public void given_valid_product_id_should_return_from_db()
        {
            var orderId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd");

            var order = repository.Get(orderId);

            order.ShouldNotBeNull();
            order.Price.ShouldBe(1000M);
        }
    }
}
