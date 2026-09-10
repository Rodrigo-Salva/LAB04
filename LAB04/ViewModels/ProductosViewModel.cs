using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LAB04.Data;
using LAB04.Models;

namespace LAB04.ViewModels
{
    /// <summary>ViewModel del mantenimiento de Productos.</summary>
    public partial class ProductosViewModel : ObservableObject
    {
        private readonly ProductoRepository _repo = new();
        private readonly CategoriaRepository _repoCategorias = new();
        private readonly ProveedorRepository _repoProveedores = new();

        public ObservableCollection<Producto> Productos { get; } = new();
        public ObservableCollection<Categoria> Categorias { get; } = new();
        public ObservableCollection<Proveedor> Proveedores { get; } = new();

        [ObservableProperty] private Producto? productoSeleccionado;

        [ObservableProperty] private string nombre = string.Empty;
        [ObservableProperty] private Categoria? categoriaCombo;
        [ObservableProperty] private Proveedor? proveedorCombo;
        [ObservableProperty] private string cantidadPorUnidad = string.Empty;
        [ObservableProperty] private string precioTexto = "0";
        [ObservableProperty] private string existenciaTexto = "0";
        [ObservableProperty] private string enPedidoTexto = "0";
        [ObservableProperty] private string nivelReordenTexto = "0";
        [ObservableProperty] private bool descontinuado;

        [ObservableProperty] private string mensaje = string.Empty;
        [ObservableProperty] private bool esError;

        public ProductosViewModel()
        {
            CargarCombos();
            CargarLista();
        }

        partial void OnProductoSeleccionadoChanged(Producto? value)
        {
            if (value == null) return;
            Nombre = value.NombreProducto;
            CategoriaCombo = Categorias.FirstOrDefault(c => c.IdCategoria == value.IdCategoria);
            ProveedorCombo = Proveedores.FirstOrDefault(p => p.IdProveedor == value.IdProveedor);
            CantidadPorUnidad = value.CantidadPorUnidad ?? string.Empty;
            PrecioTexto = value.PrecioUnidad.ToString(CultureInfo.InvariantCulture);
            ExistenciaTexto = value.UnidadesEnExistencia.ToString();
            EnPedidoTexto = value.UnidadesEnPedido.ToString();
            NivelReordenTexto = value.NivelReorden.ToString();
            Descontinuado = value.Descontinuado;
            Mensaje = string.Empty;
        }

        private void CargarCombos()
        {
            try
            {
                Categorias.Clear();
                foreach (var c in _repoCategorias.Listar()) Categorias.Add(c);
                Proveedores.Clear();
                foreach (var p in _repoProveedores.Listar()) Proveedores.Add(p);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar combos: " + ex.Message);
            }
        }

        private void CargarLista()
        {
            try
            {
                Productos.Clear();
                foreach (var p in _repo.Listar()) Productos.Add(p);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar productos: " + ex.Message);
            }
        }

        [RelayCommand]
        private void Nuevo()
        {
            ProductoSeleccionado = null;
            Nombre = string.Empty;
            CategoriaCombo = null;
            ProveedorCombo = null;
            CantidadPorUnidad = string.Empty;
            PrecioTexto = "0";
            ExistenciaTexto = "0";
            EnPedidoTexto = "0";
            NivelReordenTexto = "0";
            Descontinuado = false;
            Mensaje = string.Empty;
        }

        [RelayCommand]
        private void Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MostrarError("El nombre del producto es obligatorio.");
                return;
            }
            if (!decimal.TryParse(PrecioTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio))
            {
                MostrarError("El precio unitario no es válido.");
                return;
            }
            if (!short.TryParse(ExistenciaTexto, out var existencia) ||
                !short.TryParse(EnPedidoTexto, out var enPedido) ||
                !short.TryParse(NivelReordenTexto, out var nivelReorden))
            {
                MostrarError("Existencia, unidades en pedido y nivel de reorden deben ser números enteros.");
                return;
            }

            try
            {
                var p = ProductoSeleccionado ?? new Producto();
                p.NombreProducto = Nombre.Trim();
                p.IdCategoria = CategoriaCombo?.IdCategoria;
                p.IdProveedor = ProveedorCombo?.IdProveedor;
                p.CantidadPorUnidad = CantidadPorUnidad;
                p.PrecioUnidad = precio;
                p.UnidadesEnExistencia = existencia;
                p.UnidadesEnPedido = enPedido;
                p.NivelReorden = nivelReorden;
                p.Descontinuado = Descontinuado;

                if (ProductoSeleccionado == null)
                    _repo.Insertar(p);
                else
                    _repo.Actualizar(p);

                CargarLista();
                Nuevo();
                MostrarExito("Guardado correctamente.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al guardar: " + ex.Message);
            }
        }

        [RelayCommand]
        private void Eliminar()
        {
            if (ProductoSeleccionado == null)
            {
                MostrarError("Selecciona un producto de la lista.");
                return;
            }
            if (MessageBox.Show($"¿Eliminar el producto '{ProductoSeleccionado.NombreProducto}'?", "Confirmar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                _repo.Eliminar(ProductoSeleccionado.IdProducto);
                CargarLista();
                Nuevo();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo eliminar (¿tiene pedidos asociados?): " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            EsError = true;
            Mensaje = mensaje;
        }

        private void MostrarExito(string mensaje)
        {
            EsError = false;
            Mensaje = mensaje;
        }
    }
}
