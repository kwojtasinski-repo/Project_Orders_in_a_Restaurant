using Restaurant.UI.DTO;
using Restaurant.UI.Services;
using System;
using System.Windows.Forms;

namespace Restaurant.UI
{
    public partial class Settings : UserControl
    {
        private readonly ISettingsService _settingsService;
        private string field;

        public Settings(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            InitializeComponent();
        }

        private void SetValues(object sender, EventArgs e)
        {
            var dto = new SettingsDto();
            try
            {
                field = "Email";
                dto.Email = textBoxEmail.Text;
                field = "SmtpClient";
                dto.SmtpClient = textBoxSMTPClient.Text;
                field = "SmtpPort";
                dto.SmtpPort = Convert.ToInt32(textBoxSTMPPort.Text);
                field = "Login";
                dto.Login = textBoxLogin.Text;
                field = "Password";
                dto.Password = textBoxPass.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Niepoprawne dane " + ex.Message, field,
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            SaveSettings(dto);
        }

        private async void SaveSettings(SettingsDto dto)
        {
            var result = await _settingsService.SaveSettings(dto);
            if (!result.IsSuccess)
            {
                MessageBox.Show("Wystąpił błąd podczas zapisu", "Save",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }

            MessageBox.Show("Pomyślnie ustawiono dane", "Ustawienia",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
        }

        private async void LoadSettings(object sender, EventArgs e)
        {
            if (!Visible)
            {
                return;
            }

            var result = await _settingsService.GetSettings();
            if (!result.IsSuccess)
            {
                MessageBox.Show("Wystąpił błąd podczas wczytywania", "Load",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                return;
            }

            textBoxEmail.Text = result.Data?.Email;
            textBoxSMTPClient.Text = result.Data?.SmtpClient;
            textBoxSTMPPort.Text = (result.Data?.SmtpPort ?? 0).ToString();
            textBoxLogin.Text = result.Data?.Login;
            textBoxPass.Text = result.Data?.Password;
        }
    }
}
