using System;
using System.Windows.Forms;

namespace Restaurant.UI
{
    public partial class MainPanel : Form
    {
        public MainPanel(Menu menu, Settings settings, History history)
        {
            menu1 = menu;
            settings1 = settings;
            history1 = history;
            InitializeComponent();
            panel3.Controls.Add(menu1);
            panel3.Controls.Add(settings1);
            panel3.Controls.Add(history1);
            menu1.Visible = true;
            settings1.Visible = false;
            history1.Visible = false;
        }

        private void ShowMenu(object sender, EventArgs e)
        {
            settings1.Visible = false;
            history1.Visible = false;
            menu1.Visible = true;
            menu1.BringToFront();
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            menu1.Visible = false;
            history1.Visible = false;
            settings1.Visible = true;
            settings1.BringToFront();
        }

        private void ShowHistory(object sender, EventArgs e)
        {
            menu1.Visible = false;
            settings1.Visible = false;
            history1.Visible = true;
            history1.BringToFront();
        }

        private void Close(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
