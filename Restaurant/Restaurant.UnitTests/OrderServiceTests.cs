using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Restaurant.ApplicationLogic.Implementation;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mail;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Repositories;
using Restaurant.Shared.DTO;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Restaurant.UnitTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService orderService;
        private IOrderRepository orderRepository;
        private IUnitOfWork unitOfWork;
        private IProductSaleRepository productSaleRepository;
        private IMailSender mailSender;

        [SetUp]
        public void Setup()
        {
            orderRepository = Substitute.For<IOrderRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            productSaleRepository = Substitute.For<IProductSaleRepository>();
            mailSender = Substitute.For<IMailSender>();
            orderService = new OrderService(orderRepository, unitOfWork, productSaleRepository, mailSender, NullLogger<OrderService>.Instance);
        }

        [Test]
        public async Task given_valid_order_when_add_is_called_then_order_is_added()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var orderDto = new OrderDetailsDto
            {
                Created = DateTime.UtcNow,
                Email = "email@email.com",
                Note = "Please deliver fast",
                Price = 100.0m,
                Products =
                [
                    new ProductSaleDto { ProductId = productId, EndPrice = 100m, Email = "email@email.com", Product = new ProductDto
                        {
                            Id = productId,
                            ProductName = "Pizza",
                            Price = 100m
                        }
                    }
                ]
            };
            orderRepository.AddAsync(Arg.Any<Order>())
                .Returns(callInfo =>
                {
                    var order = callInfo.Arg<Order>();
                    return Task.FromResult(order);
                });

            // Act
            var result = await orderService.AddAsync(orderDto);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            await orderRepository.Received(1).AddAsync(Arg.Is<Order>(o =>
                o.Email.Value == orderDto.Email &&
                o.Note == orderDto.Note &&
                o.Price == orderDto.Price
            ));
            unitOfWork.Received(1).Begin();
            unitOfWork.Received(1).Commit();
            await mailSender.Received(1).SendAsync(Arg.Is<ApplicationLogic.Mail.Email>(e => string.Equals(e.Value, orderDto.Email, StringComparison.OrdinalIgnoreCase)), Arg.Any<EmailMessage>());
        }
    }
}
