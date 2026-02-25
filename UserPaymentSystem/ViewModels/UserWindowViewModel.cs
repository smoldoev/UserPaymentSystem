using UserPaymentSystem.Models;

namespace UserPaymentSystem.ViewModels;

public class UserWindowViewModel : ViewModelBase
{
    private User _user;
    private string _windowTitle;
    private string _buttonText;

    public UserWindowViewModel(User user, bool isEdit = false)
    {
        _user = user;

        // Настройка заголовка и кнопки в зависимости от режима
        if (isEdit)
        {
            WindowTitle = "Edit User";
            ButtonText = "Save";
        }
        else
            WindowTitle = "Add New User";
        ButtonText = "Add";
    }
    

    // Свойства для привязки
    public string WindowTitle
    {
        get => _windowTitle;
        set
        {
            _windowTitle = value;
            OnPropertyChanged();
        }
    }

    public string ButtonText
    {
        get => _buttonText;
        set
        {
            _buttonText = value;
            OnPropertyChanged();
        }
    }

    // Свойства пользователя для редактирования
    public string FirstName
    {
        get => _user.FirstName;
        set
        {
            _user.FirstName = value;
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => _user.LastName;
        set
        {
            _user.LastName = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _user.Email;
        set
        {
            _user.Email = value;
            OnPropertyChanged();
        }
    }

    public string Phone
    {
        get => _user.Phone;
        set
        {
            _user.Phone = value;
            OnPropertyChanged();
        }
    }

    // Получаем готового пользователя
    public User GetUser() => _user;
}