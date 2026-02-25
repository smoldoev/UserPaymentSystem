using System.Windows;
using UserPaymentSystem.ViewModels;

namespace UserPaymentSystem;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Создаем ViewModel и привязываем к окну
        DataContext = new MainViewModel();
    }
}