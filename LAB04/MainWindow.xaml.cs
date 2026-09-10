using System.Windows;

namespace LAB04
{
    /// <summary>
    /// Ventana raíz. El DataContext (MainViewModel) se asigna en el XAML;
    /// la navegación entre mantenimientos se resuelve por DataTemplates (MVVM),
    /// sin lógica de UI en este code-behind.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
