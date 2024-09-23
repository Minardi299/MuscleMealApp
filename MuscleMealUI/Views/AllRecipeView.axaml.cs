using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MuscleMealUI.ViewModels;
using System.Diagnostics;

namespace MuscleMealUI.Views
{
    public partial class AllRecipeView : UserControl
    {
        public AllRecipeView()
        {
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
