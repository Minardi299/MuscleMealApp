using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using MuscleMealUI.ViewModels;

namespace MuscleMealUI.Views;

public partial class LoginView : UserControl
{
    private MyDbContext _context = new MyDbContext() ;
    public LoginView()
    {
        InitializeComponent();
        LoginViewModel loginViewModel = new LoginViewModel(this._context);
        this.DataContext = loginViewModel;
 


    }
}