using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Shouldly;
using System;

namespace Restaurant.IntegrationTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private IProductRepository repository;

        [SetUp]
        public void Setup()
        {
            repository = Config.Container.GetRequiredService<IProductRepository>();
        }

        [Test]
        public void given_valid_product_should_add_to_db()
        {
            var product = new Product(id: Guid.NewGuid(), price: 105.50M, productName: "Product #105", productKind: ProductKind.Pizza);
            
            repository.Add(product);

            var productFromDb = repository.Get(product.Id);
            productFromDb.ShouldNotBeNull();
            productFromDb.ProductName.ShouldBe(product.ProductName);
        }

        [Test]
        public void given_valid_id_should_delete_product()
        {
            var product = new Product(id: Guid.NewGuid(), price: 105.50M, productName: "Product #107", productKind: ProductKind.Drink);
            repository.Add(product);

            repository.Delete(product.Id);

            var productFromDb = repository.Get(product.Id);
            productFromDb.ShouldBeNull();
        }

        [Test]
        public void should_return_products()
        {
            var products = repository.GetAll();

            products.ShouldNotBeNull();
            products.ShouldNotBeEmpty();
        }

        [Test]
        public void given_valid_product_should_update()
        {
            var product = new Product(id: Guid.NewGuid(), price: 105.50M, productName: "Product #107", productKind: ProductKind.MainDish);
            repository.Add(product);
            var productModified = new Product(id: product.Id, price: 125.55M, productName: "Product #1", productKind: ProductKind.MainDish);

            repository.Update(productModified);

            var productFromDb = repository.Get(product.Id);
            productFromDb.ShouldNotBeNull();
            productFromDb.Price.ShouldBe(productModified.Price);
            productFromDb.ProductName.ShouldBe(productModified.ProductName);
        }

        [Test]
        public void given_valid_product_id_should_return_from_db()
        {
            var productId = new Guid("6f542d82-4f0d-4bd6-b90b-6d2b7b79efdd");

            var product = repository.Get(productId);

            product.ShouldNotBeNull();
            product.Price.ShouldBe(500M);
        }
    }
}
