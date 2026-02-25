using System.Windows;
using UserPaymentSystem.ViewModels;

namespace UserPaymentSystem.Views;

public partial class UserWindow : Window
{
    public UserWindow(UserWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Проверяем, что поля заполнены
        var viewModel = (UserWindowViewModel)DataContext;

        if (string.IsNullOrWhiteSpace(viewModel.FirstName) ||
            string.IsNullOrWhiteSpace(viewModel.LastName) ||
            string.IsNullOrWhiteSpace(viewModel.Email))
        {
            MessageBox.Show("Please fill all required fields", "Validation Error",
                          MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Если всё ок - закрываем окно с успехом
        DialogResult = true;
        Close();
    }
}