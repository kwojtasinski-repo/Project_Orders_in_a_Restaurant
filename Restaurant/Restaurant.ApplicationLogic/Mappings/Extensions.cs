using Restaurant.ApplicationLogic.DTO;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Exceptions;
using System;
using System.Linq;
using System.Text;

namespace Restaurant.ApplicationLogic.Mappings
{
    internal static class Extensions
    {
        public static Product AsEntity(this ProductDto productDto)
        {
            var product = new Product(productDto.Id, productDto.ProductName, productDto.Price, (Domain.Entities.ProductKind) productDto.ProductKind);
            return product;
        }

        public static ProductDto AsDto(this Product product)
        {
            var productDto = new ProductDto()
            {
                Id = product.Id,
                Price = product.Price,
                ProductName = product.ProductName,
                ProductKind = (DTO.ProductKind) product.ProductKind
            };

            return productDto;
        }

        public static ProductDetailsDto AsDetailsDto(this Product product)
        {
            var productDto = new ProductDetailsDto()
            {
                Id = product.Id,
                Price = product.Price,
                ProductName = product.ProductName,
                ProductKind = (DTO.ProductKind) product.ProductKind,
                Orders = product.Orders.Select(o => o.AsDto())
            };

            return productDto;
        }

        public static Order AsEntity(this OrderDto orderDto)
        {
            var order = new Order(orderDto.Id, orderDto.OrderNumber, orderDto.Created, orderDto.Price, Email.Of(orderDto.Email), orderDto.Note);
            return order;
        }

        public static OrderDto AsDto(this Order order)
        {
            var orderDto = new OrderDto()
            {
                Id = order.Id,
                Email = order.Email.Value,
                OrderNumber = order.OrderNumber,
                Created = order.Created,
                Price = order.Price,
                Note = order.Note
            };

            return orderDto;
        }

        public static OrderDetailsDto AsDetailsDto(this Order order)
        {
            var orderDto = new OrderDetailsDto()
            {
                Id = order.Id,
                Email = order.Email.Value,
                OrderNumber = order.OrderNumber,
                Created = order.Created,
                Price = order.Price,
                Note = order.Note,
                Products = order.Products.Select(p => p.AsDto()).ToList()
            };

            return orderDto;
        }

        public static ProductSaleDto AsDto(this ProductSale productSale)
        {
            var productSaleDto = new ProductSaleDto()
            {
                Id = productSale.Id,
                Addition = productSale.Addition?.AsDto(),
                AdditionId = productSale.Addition?.Id,
                Email = productSale.Email.Value,
                EndPrice = productSale.EndPrice,
                OrderId = productSale.OrderId,
                Product = productSale.Product.AsDto(),
                ProductId = productSale.ProductId,
                ProductSaleState = (DTO.ProductSaleState) productSale.ProductSaleState
            };

            return productSaleDto;
        }

        public static ProductSale AsEntity(this ProductSaleDto productSale)
        {
            var productSaleDto = new ProductSale(productSale.Id, productSale.Product.AsEntity(),
                        (Domain.Entities.ProductSaleState) productSale.ProductSaleState, Email.Of(productSale.Email),
                        productSale.Addition?.AsEntity(), productSale.OrderId);

            return productSaleDto;
        }

        public static ProductSaleDetailsDto AsDetailsDto(this ProductSale productSale)
        {
            var productSaleDto = new ProductSaleDetailsDto()
            {
                Id = productSale.Id,
                Addition = productSale.Addition.AsDto(),
                AdditionId = productSale.Addition?.Id,
                Email = productSale.Email.Value,
                EndPrice = productSale.EndPrice,
                OrderId = productSale.OrderId,
                Product = productSale.Product.AsDto(),
                ProductId = productSale.ProductId,
                ProductSaleState = (DTO.ProductSaleState) productSale.ProductSaleState,
                Order = productSale.Order.AsDto()
            };

            return productSaleDto;
        }

        public static AdditionDto AsDto(this Addition addition)
        {
            var additionDto = new AdditionDto()
            {
                Id = addition.Id,
                AdditionName = addition.AdditionName,
                Price = addition.Price,
                ProductKind = (DTO.ProductKind) addition.ProductKind
            };

            return additionDto;
        }

        public static Addition AsEntity(this AdditionDto addition)
        {
            var additionDto = new Addition(addition.Id, addition.AdditionName, addition.Price, (Domain.Entities.ProductKind) addition.ProductKind);
            return additionDto;
        }

        public static string ContentEmail(this OrderDetailsDto order)
        {
            StringBuilder messageBody = new StringBuilder();

            if (order == null)
            {
                throw new RestaurantException("There is no order to sent", typeof(Extensions).FullName, "CreateEmail");
            }

            messageBody.Append("<font>Nr zamówienia: " + order.OrderNumber + ", data zamówienia: "
                + order.Created + "</font><br><br>");

            string startTable = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string endTable = "</table>";
            string startHeaderRow = "<tr style=\"background-color:#6FA1D2; color:#ffffff;\">";
            string endHeaderRow = "</tr>";
            string startTr = "<tr style=\"color:#555555;\">";
            string endTr = "</tr>";
            string startTd = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
            string endTd = "</td>";
            messageBody.Append(startTable);
            messageBody.Append(startHeaderRow);
            messageBody.Append(startTd + "Nazwa Dania" + endTd);
            messageBody.Append(startTd + "Koszt" + endTd);
            messageBody.Append(endHeaderRow);

            foreach (var productSale in order.Products)
            {
                messageBody.Append(startTr);
                messageBody.Append(startTd + productSale.Product.ProductName + endTd);
                messageBody.Append(startTd + productSale.Product.Price + " zł" + endTd);
                messageBody.Append(endTr);

                if (productSale.Addition != null)
                {
                    messageBody.Append(startTr);
                    messageBody.Append(startTd + productSale.Addition.AdditionName + " zł" + endTd);
                    messageBody.Append(startTd + productSale.Addition.Price + " zł" + endTd);
                    messageBody.Append(endTr);
                }

                messageBody.Append(endTr);
            }
            messageBody.Append(endTable);
            messageBody.Append("<br/>");
            messageBody.Append("<h5>");
            messageBody.Append("Uwagi:");
            messageBody.Append("</h5>");
            messageBody.Append("<br/>");
            messageBody.Append("<h5>");
            messageBody.Append(order.Note);
            messageBody.Append("</h5>");
            messageBody.Append("<br/>");
            messageBody.AppendLine("\n<font>Koszt : " + order.Price + " zł" + "</font>");
            return messageBody.ToString();
        }
    }
}
