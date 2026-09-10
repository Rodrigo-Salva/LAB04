using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LAB04.ViewModels
{
    /// <summary>ViewModel raíz: controla qué mantenimiento se muestra en la ventana principal.</summary>
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject currentViewModel;

        /// <summary>Nombre de la sección visible; se usa para resaltar el botón activo en la barra lateral.</summary>
        [ObservableProperty]
        private string seccionActual = "Productos";

        public MainViewModel()
        {
            currentViewModel = new ProductosViewModel();
        }

        [RelayCommand]
        private void MostrarProductos()
        {
            CurrentViewModel = new ProductosViewModel();
            SeccionActual = "Productos";
        }

        [RelayCommand]
        private void MostrarCategorias()
        {
            CurrentViewModel = new CategoriasViewModel();
            SeccionActual = "Categorias";
        }

        [RelayCommand]
        private void MostrarProveedores()
        {
            CurrentViewModel = new ProveedoresViewModel();
            SeccionActual = "Proveedores";
        }

        [RelayCommand]
        private void MostrarPedidos()
        {
            CurrentViewModel = new PedidosViewModel();
            SeccionActual = "Pedidos";
        }
    }
}
