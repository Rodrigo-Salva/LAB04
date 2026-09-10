using System.Windows.Controls;

namespace LAB04.Views
{
    /// <summary>
    /// Code-behind mínimo: solo inicializa el componente. Toda la lógica vive en
    /// CategoriasViewModel (patrón MVVM); el DataContext lo asigna el DataTemplate
    /// de MainWindow al momento de navegar.
    /// </summary>
    public partial class CategoriasView : UserControl
    {
        public CategoriasView()
        {
            InitializeComponent();
        }
    }
}
