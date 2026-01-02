using Restaurant.ApplicationLogic.DTO;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mappings;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Implementation
{
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductSaleRepository _productSaleRepository;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IProductSaleRepository productSaleRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _productSaleRepository = productSaleRepository;
        }

        public Guid Add(OrderDto order)
        {
            order.Id = Guid.NewGuid();
            var currentDate = DateTime.UtcNow;
            var latestOrder = _orderRepository.GetLatestOrderOnDate(currentDate).AsDto();
            var orderNumber = CreateOrderNumber(latestOrder, currentDate);
            order.OrderNumber = orderNumber;
            order.Created = currentDate;

            var id = _orderRepository.Add(order.AsEntity());
            return id;
        }

        public Guid Add(OrderDetailsDto orderDetailsDto)
        {
            orderDetailsDto.Id = Guid.NewGuid();
            var currentDate = DateTime.UtcNow;
            var latestOrder = _orderRepository.GetLatestOrderOnDate(currentDate)?.AsDto();
            var orderNumber = CreateOrderNumber(latestOrder, currentDate);
            orderDetailsDto.OrderNumber = orderNumber;
            orderDetailsDto.Created = currentDate;

            _unitOfWork.Begin();

            try
            {
                var id = _orderRepository.Add(orderDetailsDto.AsEntity());

                foreach (var productSale in orderDetailsDto.Products)
                {
                    productSale.OrderId = id;
                    productSale.Id = Guid.NewGuid();
                    productSale.ProductSaleState = ProductSaleState.Ordered;
                    _productSaleRepository.Add(productSale.AsEntity());
                }

                _unitOfWork.Commit();
                return id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<Guid> AddAsync(OrderDetailsDto orderDetailsDto)
        {
            orderDetailsDto.Id = Guid.NewGuid();
            var currentDate = DateTime.UtcNow;
            var latestOrder = (await _orderRepository.GetLatestOrderOnDateAsync(currentDate))?.AsDto();
            var orderNumber = CreateOrderNumber(latestOrder, currentDate);
            orderDetailsDto.OrderNumber = orderNumber;
            orderDetailsDto.Created = currentDate;

            _unitOfWork.Begin();

            try
            {
                var id = await _orderRepository.AddAsync(orderDetailsDto.AsEntity());

                foreach (var productSale in orderDetailsDto.Products)
                {
                    productSale.OrderId = id;
                    productSale.Id = Guid.NewGuid();
                    productSale.ProductSaleState = ProductSaleState.Ordered;
                    _productSaleRepository.Add(productSale.AsEntity());
                }

                _unitOfWork.Commit();
                return id;
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Delete(Guid id)
        {
            _orderRepository.Delete(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public void DeleteOrders(IEnumerable<Guid> ids)
        {
            _orderRepository.DeleteOrders(ids);
        }

        public async Task DeleteOrdersAsync(IEnumerable<Guid> ids)
        {
            await _orderRepository.DeleteOrdersAsync(ids);
        }

        public OrderDetailsDto Get(Guid id)
        {
            var order = _orderRepository.Get(id);
            return order.AsDetailsDto();
        }

        public IEnumerable<OrderDto> GetAll()
        {
            var orders = _orderRepository.GetAll();
            return orders.Select(o => o.AsDto());
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => o.AsDto());
        }

        public async Task<OrderDetailsDto> GetAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(id);
            return order.AsDetailsDto();
        }

        public void Update(OrderDto order)
        {
            _orderRepository.Update(order.AsEntity());
        }

        private string CreateOrderNumber(OrderDto latestOrder, DateTime currentDate)
        {
            int number = 1;
            if (latestOrder != null)
            {
                var lastOrderNumberToday = latestOrder.OrderNumber;
                var stringNumber = lastOrderNumberToday.Substring(17);//18
                int.TryParse(stringNumber, out number);
                number++;
            }

            var orderNumber = new StringBuilder("ORDER/")
                .Append(currentDate.Year).Append('/').Append(currentDate.Month.ToString("d2"))
                .Append('/').Append(currentDate.Day.ToString("00")).Append('/').Append(number).ToString();
            return orderNumber;
        }
    }
}
