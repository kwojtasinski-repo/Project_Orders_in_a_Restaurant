using Restaurant.Shared.DTO;
using Restaurant.UI.Exceptions;
using System.Text;

namespace Restaurant.UI
{
    public static class MailExtensions
    {
        public static string ContentEmail(this OrderDetailsDto order)
        {
            StringBuilder messageBody = new StringBuilder();

            if (order == null)
            {
                throw new RestaurantClientException("There is no order to sent", typeof(MailExtensions).FullName, "CreateEmail");
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

            foreach(var productSale in order.Products)
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
            messageBody.AppendLine("\n<font>Koszt : " + order.Price + " zł" +  "</font>");
            return messageBody.ToString();
        }
    }
}
