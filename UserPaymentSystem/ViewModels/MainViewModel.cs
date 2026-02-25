using System.Windows;
using UserPaymentSystem.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UserPaymentSystem.Data;
using UserPaymentSystem.Models;

namespace UserPaymentSystem.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly AppDbContext _context;

    // Коллекции для отображения в DataGrid
    private ObservableCollection<User> _users = new();
    private ObservableCollection<Payment> _payments = new();

    // Выбранные элементы
    private User? _selectedUser;
    private Payment? _selectedPayment;

    // Текст статуса
    private string _statusText = "Ready";

    public MainViewModel()
    {
        _context = new AppDbContext();
        _context.Database.EnsureCreated(); // Создаст БД если нет

        // Загружаем данные
        LoadUsers();
        LoadPayments();

        // Инициализация команд
        AddUserCommand = new RelayCommand(AddUser);
        EditUserCommand = new RelayCommand(EditUser, CanEditOrDeleteUser);
        DeleteUserCommand = new RelayCommand(DeleteUser, CanEditOrDeleteUser);
        RefreshCommand = new RelayCommand(Refresh);

        AddPaymentCommand = new RelayCommand(AddPayment, CanAddPayment);
        DeletePaymentCommand = new RelayCommand(DeletePayment, CanEditOrDeletePayment);
    }

    // Свойства для привязки
    public ObservableCollection<User> Users
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Payment> Payments
    {
        get => _payments;
        set
        {
            _payments = value;
            OnPropertyChanged();
        }
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();

            // Когда меняется выбранный пользователь, обновляем список платежей
            LoadPayments();

            // Обновляем состояние команд
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public Payment? SelectedPayment
    {
        get => _selectedPayment;
        set
        {
            _selectedPayment = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public string StatusText
    {
        get => _statusText;
        set
        {
            _statusText = value;
            OnPropertyChanged();
        }
    }

    // Команды
    public ICommand AddUserCommand { get; }
    public ICommand EditUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand RefreshCommand { get; }
    public ICommand AddPaymentCommand { get; }
    public ICommand DeletePaymentCommand { get; }

    // Методы загрузки данных
    private void LoadUsers()
    {
        Users = new ObservableCollection<User>(_context.Users.ToList());
        StatusText = $"Loaded {Users.Count} users";
    }

    private void LoadPayments()
    {
        if (SelectedUser != null)
        {
            var payments = _context.Payments
                .Include(p => p.User)
                .Where(p => p.UserId == SelectedUser.Id)
                .ToList();
            Payments = new ObservableCollection<Payment>(payments);
        }
        else
            Payments.Clear();
    }

    // Методы для команд
    private void AddUser(object? parameter)
    {
        var userWindow = new Views.UserWindow(new UserWindowViewModel(new User()));
        userWindow.Owner = Application.Current.MainWindow;

        if (userWindow.ShowDialog() == true)
        {
            var viewModel = (UserWindowViewModel)userWindow.DataContext;
            var newUser = viewModel.GetUser();
            newUser.CreatedAt = DateTime.Now;

            _context.Users.Add(newUser);
            _context.SaveChanges();

            LoadUsers();
            StatusText = $"User {newUser.FullName} added successfully";
        }
    }


    private void EditUser(object? parameter)
    {
        if (SelectedUser == null) return;

        // Создаем копию пользователя для редактирования
        var userToEdit = new User
        {
            Id = SelectedUser.Id,
            FirstName = SelectedUser.FirstName,
            LastName = SelectedUser.LastName,
            Email = SelectedUser.Email,
            Phone = SelectedUser.Phone,
            CreatedAt = SelectedUser.CreatedAt
        };

        var userWindow = new Views.UserWindow(new UserWindowViewModel(userToEdit, true));
        userWindow.Owner = Application.Current.MainWindow;

        if (userWindow.ShowDialog() == true)
        {
            var viewModel = (UserWindowViewModel)userWindow.DataContext;
            var editedUser = viewModel.GetUser();

            // Обновляем данные в базе
            var userInDb = _context.Users.Find(editedUser.Id);
            if (userInDb != null)
            {
                userInDb.FirstName = editedUser.FirstName;
                userInDb.LastName = editedUser.LastName;
                userInDb.Email = editedUser.Email;
                userInDb.Phone = editedUser.Phone;

                _context.SaveChanges();
                LoadUsers();
                StatusText = $"User {editedUser.FullName} updated successfully";
            }
        }
    }

    private void DeleteUser(object? parameter)
    {
        if (SelectedUser == null) return;

        _context.Users.Remove(SelectedUser);
        _context.SaveChanges();

        LoadUsers();
        SelectedUser = null;
        StatusText = "User deleted successfully";
    }

    private bool CanEditOrDeleteUser(object? parameter)
    {
        return SelectedUser != null;
    }

    private void Refresh(object? parameter)
    {
        LoadUsers();
        LoadPayments();
        StatusText = "Refreshed";
    }

    private void AddPayment(object? parameter)
    {
        if (SelectedUser == null) return;

        var newPayment = new Payment
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "New payment",
            UserId = SelectedUser.Id,
            User = SelectedUser
        };

        _context.Payments.Add(newPayment);
        _context.SaveChanges();

        LoadPayments();
        StatusText = "Payment added successfully";
    }

    private bool CanAddPayment(object? parameter)
    {
        return SelectedUser != null;
    }

    private void DeletePayment(object? parameter)
    {
        if (SelectedPayment == null) return;

        _context.Payments.Remove(SelectedPayment);
        _context.SaveChanges();

        LoadPayments();
        SelectedPayment = null;
        StatusText = "Payment deleted successfully";
    }

    private bool CanEditOrDeletePayment(object? parameter)
    {
        return SelectedPayment != null;
    }
}