using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Restaurant.ApplicationLogic.DTO;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.Infrastructure.Requests;

namespace Restaurant.UI
{
    public partial class History : UserControl
    {
        private readonly IRequestHandler _requestHandler;

        public History(IRequestHandler requestHandler)
        {
            _requestHandler = requestHandler;
            InitializeComponent();
        }

        private void Details(object sender, EventArgs e)
        {
            Int32 selectedCellCount = showOrders.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount == 1)
            {
                showDetailsofOrder.Visible = true;                   
                int selectedIndex = showOrders.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = showOrders.Rows[selectedIndex];
                var orderId = (Guid)selectedRow.Cells["Id"].Value;
                var products = _requestHandler.Send<IProductSaleService, IEnumerable<ProductSaleDto>>(p => p.GetAllByOrderId(orderId));
                showDetailsofOrder.DataSource = products.ToList();
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

        private void DeleteOrderFromDB(object sender, EventArgs e)
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

                _requestHandler.Send<IOrderService>(o => o.DeleteOrders(orderIds));

                var results = _requestHandler.Send<IOrderService, IEnumerable<OrderDto>>(o => o.GetAll());
                LoadOrdersToDataGrid(results.ToList());
                showDetailsofOrder.Visible = false;
            }
        }

        private void LoadLeftHistory(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                var results = _requestHandler.Send<IOrderService, IEnumerable<OrderDto>>(o => o.GetAll());
                LoadOrdersToDataGrid(results.ToList());
            }
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
