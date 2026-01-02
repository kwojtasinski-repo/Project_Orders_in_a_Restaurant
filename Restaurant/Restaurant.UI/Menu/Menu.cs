using Newtonsoft.Json;
using Restaurant.ApplicationLogic.DTO;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mail;
using Restaurant.Infrastructure.Requests;
using Restaurant.UI.Async;
using Restaurant.UI.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant.UI
{
    public partial class Menu : UserControl
    {
        private readonly List<ProductSaleDto> productSalesList = new List<ProductSaleDto>();
        decimal amountToPay = decimal.Zero;
        private readonly IRequestHandler _requestHandler;
        private readonly HttpClient _httpClient;
        private IEnumerable<ProductDto> _products = new List<ProductDto>();
        private IEnumerable<AdditionDto> _additions = new List<AdditionDto>();
        private ProductDto currentProduct;
        private AdditionDto currentAddition;
        private string email;
        private string notes;

        public Menu(IRequestHandler requestHandler, HttpClient httpClient)
        {
            _requestHandler = requestHandler;
            _httpClient = httpClient;
            InitializeComponent();
        }

        private void ChangedItem(object sender, EventArgs e)
        {
            var currentProductName = (string) comboBoxMainDishes1.SelectedItem;

            if (currentProductName is null)
            {
                return;
            }

            currentProduct = _products.Where(p => p.ProductName == currentProductName).SingleOrDefault();

            if (currentProduct is null)
            {
                return;
            }

            currentAddition = null;
            var additions = _additions.Where(a => a.ProductKind == currentProduct.ProductKind).ToList();

            if (additions.Any())
            {
                comboBoxAdditions.Text = "";
                label3.Visible = true;
                comboBoxAdditions.Items.Clear();
                comboBoxAdditions.Items.AddRange(additions.Select(a => a.AdditionName).ToArray());
                comboBoxAdditions.Visible = true;
            }
            else
            {
                label3.Visible = false;
                comboBoxAdditions.Visible = false;
                comboBoxAdditions.SelectedItem = null;
            }
        }

        private void AddToOrder(object sender, EventArgs e)
        {
            var product = _products.Where(p => p.ProductName == (string) comboBoxMainDishes1.SelectedItem).SingleOrDefault();

            var productSale = new ProductSaleDto() { Id = Guid.NewGuid(), Email = email, EndPrice = decimal.Zero };
            if (product != null)
            {
                productSale.Product = product;
                productSale.ProductSaleState = ProductSaleState.New;
                productSale.EndPrice += product.Price;
                listViewOrderedProducts.Items.Add(productSale.Id.ToString(), product.ToString(), -1);
                productSalesList.Add(productSale);
            }

            var addition = _additions.Where(a => a.AdditionName == (string)comboBoxAdditions.SelectedItem).SingleOrDefault();

            if (addition != null)
            {
                productSale.Addition = addition;
                productSale.Addition.Id = addition.Id;
                productSale.EndPrice += addition.Price;
                listViewOrderedProducts.Items.Add(productSale.Id.ToString(), addition.ToString(), -1);
                comboBoxAdditions.SelectedIndex = comboBoxAdditions.Items.IndexOf("test1");
            }
        }

        private void DeleteFromOrder(object sender, EventArgs e)
        {
            if (listViewOrderedProducts.SelectedItems == null)
            {
                return;
            }

            if (listViewOrderedProducts.SelectedIndices.Count <= 0)
            {
                return;
            }

            var result = MessageBox.Show("Czy chcesz usunąć danie", "Usuń danie",
                        MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
            {
                return;
            }
             
            for (int i = listViewOrderedProducts.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int selectedindex = listViewOrderedProducts.SelectedIndices[i];
                var item = listViewOrderedProducts.Items[selectedindex].Text;
                var key = listViewOrderedProducts.Items[selectedindex].Name;
                var id = new Guid(key);
                listViewOrderedProducts.Items.RemoveAt(selectedindex);
                            
                var product = _products.Where(p => p.ProductName == item).FirstOrDefault();
                var productSale = productSalesList.Where(p => p.Id == id).SingleOrDefault();
                if (product != null)
                {
                    var itemToRemove = listViewOrderedProducts.Items[key];
                    listViewOrderedProducts.Items.Remove(itemToRemove);
                    productSalesList.Remove(productSale);
                    continue;
                }

                var addition = _additions.Where(a => a.AdditionName == item).FirstOrDefault();
                
                if (addition == null)
                {
                    continue;
                }

                productSale.Addition = null;
                productSale.AdditionId = null;
                productSale.EndPrice -= addition.Price;
            }
        }

        private void RefreshCost(object sender, EventArgs e)
        {
            if (currentProduct != null)
            {
                var additionPrice = currentAddition != null ? currentAddition.Price : decimal.Zero;
                var amountPrice = currentProduct.Price + additionPrice;
                PriceProduct.Text = $"{amountPrice.WithTwoDecimalPoints()} zł";
                PriceProduct.Visible = true;
                PriceProductLabel.Visible = true;
            }
            else
            {
                PriceProduct.Visible = false;
                PriceProductLabel.Visible = false;
            }

            amountToPay = decimal.Zero;

            foreach (ProductSaleDto productSale in productSalesList)
            {
                amountToPay += productSale.EndPrice;
            }

            labelCostOfOrder.Text = "Koszt: " + amountToPay.WithTwoDecimalPoints() + "zł";
        }

        private void LoadLeftMenu(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                Extensions.ShowDialog("Wprowadź email", "Email", ValidEmail);
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                productSalesList.Clear();
                listViewOrderedProducts.Clear();

                if (currentProduct != null)
                {
                    currentProduct = null;
                    PriceProduct.Visible = false;
                    PriceProductLabel.Visible = false;
                }

                if (comboBoxMainDishes1.Items.Count > 0)
                {
                    comboBoxMainDishes1.SelectedItem = null;
                }

                if (comboBoxAdditions.Items.Count > 0)
                {
                    comboBoxAdditions.SelectedItem = null;
                }
            }
        }

        private void AddNotes(object sender, EventArgs e)
        {
            DialogWindow form = (DialogWindow) sender;
            notes = form.InputBox.Text;
            form.Close();
        }

        private void ValidEmail(object sender, EventArgs e)
        {
            DialogWindow form = (DialogWindow) sender;
            var emailToValid = form.InputBox.Text;
            var isValid = emailToValid.ValidEmail();

            if (!isValid)
            {
                MessageBox.Show("Niepoprawny adres email", "Email",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            email = emailToValid;
            form.Close();
        }

        private void OrderRealization(object sender, EventArgs e)  // funkcja realizująca zamówienie, która przesyła zawartość zamówienia na adres email i wstawia wartości do tabeli SQL
        {
            if (!productSalesList.Any())
            {
                MessageBox.Show("Dodaj produkty", "Zamówienie",
                                   MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                  return;
            }

            Extensions.ShowDialog("Wprowadź uwagi do zamówienia", "Uwagi", AddNotes);

            var order = new OrderDetailsDto()
            {
                Price = amountToPay,
                Email = email,
                Note = notes
            };

            foreach (var product in productSalesList)
            {
                product.Email = email;
                order.Products.Add(product);
            }

            var id = _requestHandler.Send<IOrderService, Guid>(o => o.Add(order));
            var orderFromDb = _requestHandler.Send<IOrderService, OrderDetailsDto>(o => o.Get(id));
            var subject = $"Zamówienie nr {orderFromDb.OrderNumber}";
            var content = orderFromDb.ContentEmail();

            Task.Run(() => _requestHandler.Send<IMailSender, Task>((s) =>
                    s.SendAsync(Email.Of(email),
                        new EmailMessage(subject, content))));
                
            MessageBox.Show("Zamówienie wysłano na maila", "Email",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
        }

        private async void OnLoad(object sender, EventArgs e)
        {
            labelCostOfOrder.Text = amountToPay > 0 ? "Koszt: " + amountToPay + "zł" : "";
            try
            {
                var response = await _httpClient.GetAsync("/api/menu");
                var result = await response.Content.ReadAsStringAsync();
                var menu = JsonConvert.DeserializeObject<MenuDto>(result);
                _products = menu?.Products ?? new List<ProductDto>();
                _additions = menu?.Additions ?? new List<AdditionDto>();
                comboBoxMainDishes1.Items.AddRange(_products.Select(p => p.ProductName).ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd pobierania menu: {ex.Message}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangedAddition(object sender, EventArgs e)
        {
            var currentAdditionName = (string) comboBoxAdditions.SelectedItem;

            if (currentAdditionName is null)
            {
                return;
            }

            currentAddition = _additions.Where(a => a.AdditionName == currentAdditionName).SingleOrDefault();
        }

        private class MenuDto
        {
            public IEnumerable<ProductDto> Products { get; set; }
            public IEnumerable<AdditionDto> Additions { get; set; }
        }
    }

}
