namespace Restaurant.UI.ErrorHandling
{
    internal static class ErrorHandlerExtension
    {
        public static void MapToMessageBox(this Exception exception)
        {
            if (exception.GetType() == typeof(FileLoadException))
            {
                MessageBox.Show(exception.Message, "FileLoad",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (exception.GetType() == typeof(FileNotFoundException))
            {
                MessageBox.Show(exception.Message, "NotFoundFile",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else if (exception.GetType() == typeof(InvalidOperationException))
            {
                MessageBox.Show(exception.Message, "InvalidOperation",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Coś poszło nie tak", "Error",
                               MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}
