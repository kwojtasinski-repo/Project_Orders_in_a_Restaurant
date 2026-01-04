using Restaurant.Shared.DTO;
using Restaurant.UI.Dialog;
using Restaurant.UI.ErrorHandling;
using Restaurant.UI.Services;

namespace Restaurant.UI
{
    public partial class Menu : UserControl
    {
        private readonly List<ProductSaleDto> productSalesList = new List<ProductSaleDto>();
        decimal amountToPay = decimal.Zero;
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;
        private IEnumerable<ProductDto> _products = new List<ProductDto>();
        private IEnumerable<AdditionDto> _additions = new List<AdditionDto>();
        private ProductDto currentProduct;
        private AdditionDto? currentAddition;
        private string email;
        private string notes;

        public Menu(IOrderService orderService, IMenuService menuService)
        {
            _orderService = orderService;
            _menuService = menuService;
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
                DialogExtension.ShowDialog("Wprowadź email", "Email", ValidEmail!);
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

        private async void OrderRealization(object sender, EventArgs e)
        {
            if (!productSalesList.Any())
            {
                MessageBox.Show("Dodaj produkty", "Zamówienie",
                                   MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                  return;
            }

            DialogExtension.ShowDialog("Wprowadź uwagi do zamówienia", "Uwagi", AddNotes!);

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

            try
            {
                var result = await _orderService.AddOrderAsync(order);
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error.Message, result.Error.Context, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                MessageBox.Show("Utworzono zamówienie", "Zamówienie",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ex.MapToMessageBox();
                return;
            }
        }

        private async void OnLoad(object sender, EventArgs e)
        {
            labelCostOfOrder.Text = amountToPay > 0 ? "Koszt: " + amountToPay + "zł" : "";
            var response = await _menuService.GetMenuAsync();
            if (!response.IsSuccess)
            {
                MessageBox.Show(response.Error.Message, response.Error.Context, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _products = response.Data?.Products ?? new List<ProductDto>();
            _additions = response.Data?.Additions ?? new List<AdditionDto>();
            comboBoxMainDishes1.Items.AddRange(_products.Select(p => p.ProductName).ToArray());
        }

        private void ChangedAddition(object sender, EventArgs e)
        {
            string? currentAdditionName = comboBoxAdditions.SelectedItem as string;

            if (currentAdditionName is null)
            {
                return;
            }

            currentAddition = _additions.Where(a => string.Equals(a.AdditionName, currentAdditionName)).SingleOrDefault();
        }
    }
}
