namespace Restaurant.UI.Dialog
{
    internal static class DialogExtension
    {
        public static void ShowDialog(string text, string caption, EventHandler eventHandler)
        {
            Form prompt = new DialogWindow(500, 150, caption, text, eventHandler);
            prompt.ShowDialog();
        }
    }
}
