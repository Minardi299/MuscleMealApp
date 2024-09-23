using Avalonia.Controls;
using MuscleMealUI.ViewModels;

namespace MuscleMealUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}