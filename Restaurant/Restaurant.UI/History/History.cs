using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Restaurant.Shared.DTO;
using Restaurant.UI.Services;

namespace Restaurant.UI
{
    public partial class History : UserControl
    {
        private readonly IOrderService _orderService;

        public History(IOrderService orderService)
        {
            _orderService = orderService;
            InitializeComponent();
        }

        private async void Details(object sender, EventArgs e)
        {
            Int32 selectedCellCount = showOrders.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount == 1)
            {
                showDetailsofOrder.Visible = true;                   
                int selectedIndex = showOrders.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = showOrders.Rows[selectedIndex];
                var orderId = (Guid)selectedRow.Cells["Id"].Value;
                var result = await _orderService.GetOrderAsync(orderId);
                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Error.Message, result.Error.Context, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                showDetailsofOrder.DataSource = result.Data.Products;
                showDetailsofOrder.Columns["Id"].DisplayIndex = 0;
                showDetailsofOrder.Columns["EndPrice"].HeaderText = "Cena końcowa [zł]";
                showDetailsofOrder.Columns["EndPrice"].DisplayIndex = 1;
                showDetailsofOrder.Columns["Email"].HeaderText = "Email";
                showDetailsofOrder.Columns["Email"].DisplayIndex = 2;
                showDetailsofOrder.Columns["ProductSaleState"].HeaderText = "Status";
                showDetailsofOrder.Columns["ProductSaleState"].DisplayIndex = 3;
                showDetailsofOrder.Columns["Product"].HeaderText = "Produkt";
                showDetailsofOrder.Columns["Product"].DisplayIndex = 3;
                showDetailsofOrder.Columns["ProductId"].HeaderText = "Id produktu";
                showDetailsofOrder.Columns["ProductId"].DisplayIndex = 4;
                showDetailsofOrder.Columns["Addition"].HeaderText = "Dodatek";
                showDetailsofOrder.Columns["Addition"].DisplayIndex = 5;
                showDetailsofOrder.Columns["AdditionId"].HeaderText = "Id dodatku";
                showDetailsofOrder.Columns["AdditionId"].DisplayIndex = 6;
                showDetailsofOrder.Columns["OrderId"].HeaderText = "Id zamówienia";
                showDetailsofOrder.Columns["OrderId"].DisplayIndex = 7;
            }
        }

        private async void DeleteOrderFromDB(object sender, EventArgs e)
        {
            Int32 selectedCellCount = showOrders.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                var confirmDeleteData = MessageBox.Show("Czy chcesz usunąć zamówienie", "Usuń zamówienie",
                               MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

                if (confirmDeleteData == DialogResult.No)
                {
                    return;
                }

                List<Guid> orderIds = new List<Guid>();
                for (int i =0; i< selectedCellCount; i++)
                {
                    int selected_row = showOrders.SelectedCells[i].RowIndex;
                    DataGridViewRow selectedRow = showOrders.Rows[selected_row];
                    orderIds.Add((Guid)selectedRow.Cells["Id"].Value);
                }

                var result = await _orderService.DeleteOrders(orderIds);
                if (result.IsSuccess == false)
                {
                    MessageBox.Show(result.Error.Message, result.Error.Context, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var results = await _orderService.GetAllOrdersAsync();
                if (results.IsSuccess == false)
                {
                    MessageBox.Show(result.Error.Message, result.Error.Context, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LoadOrdersToDataGrid(results.Data);
                showDetailsofOrder.Visible = false;
            }
        }

        private async void LoadLeftHistory(object sender, EventArgs e)
        {
            if (!Visible)
            {
                return;
            }

            var result = await _orderService.GetAllOrdersAsync();
            if (result.IsSuccess == false)
            {
                MessageBox.Show(result.Error.Message, result.Error.Context, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadOrdersToDataGrid(new List<OrderDto>());
                return;
            }

            LoadOrdersToDataGrid(result.Data);
        }

        private void LoadOrdersToDataGrid(IList<OrderDto> orders)
        {
            showOrders.DataSource = orders;
            showOrders.Columns["Id"].DisplayIndex = 0;
            showOrders.Columns["OrderNumber"].HeaderText = "Numer zamówienia";
            showOrders.Columns["OrderNumber"].DisplayIndex = 1;
            showOrders.Columns["Created"].HeaderText = "Data utworzenia";
            showOrders.Columns["Created"].DisplayIndex = 2;
            showOrders.Columns["Price"].HeaderText = "Koszt [zł]";
            showOrders.Columns["Price"].DisplayIndex = 3;
            showOrders.Columns["Email"].HeaderText = "Email";
            showOrders.Columns["Email"].DisplayIndex = 4;
            showOrders.Columns["Note"].HeaderText = "Uwagi";
            showOrders.Columns["Note"].DisplayIndex = 5;

        }
    }
}
