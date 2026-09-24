using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LAB04.Data;
using LAB04.Models;

namespace LAB04.ViewModels
{
    public partial class ProveedoresViewModel : ObservableObject
    {
        private readonly ProveedorRepository _repo = new();

        public ObservableCollection<Proveedor> Proveedores { get; } = new();

        [ObservableProperty] private Proveedor? proveedorSeleccionado;

        [ObservableProperty] private string buscarContacto = string.Empty;
        [ObservableProperty] private string buscarCiudad = string.Empty;

        [ObservableProperty] private string nombreCompania = string.Empty;
        [ObservableProperty] private string? nombreContacto;
        [ObservableProperty] private string? cargo;
        [ObservableProperty] private string? direccion;
        [ObservableProperty] private string? ciudad;
        [ObservableProperty] private string? codigoPostal;
        [ObservableProperty] private string? pais;
        [ObservableProperty] private string? telefono;
        [ObservableProperty] private string? fax;

        [ObservableProperty] private string mensaje = string.Empty;
        [ObservableProperty] private bool esError;

        public ProveedoresViewModel()
        {
            _ = CargarListaAsync();
        }

        partial void OnProveedorSeleccionadoChanged(Proveedor? value)
        {
            if (value == null) return;
            NombreCompania = value.NombreCompania;
            NombreContacto = value.NombreContacto;
            Cargo = value.Cargo;
            Direccion = value.Direccion;
            Ciudad = value.Ciudad;
            CodigoPostal = value.CodigoPostal;
            Pais = value.Pais;
            Telefono = value.Telefono;
            Fax = value.Fax;
            Mensaje = string.Empty;
        }

        private async Task CargarListaAsync()
        {
            try
            {
                var proveedores = await _repo.ListarAsync();
                Proveedores.Clear();
                foreach (var p in proveedores) Proveedores.Add(p);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar proveedores: " + ex.Message);
            }
        }

        [RelayCommand]
        private async Task BuscarAsync()
        {
            try
            {
                var proveedores = await _repo.BuscarPorContactoCiudadAsync(BuscarContacto, BuscarCiudad);
                Proveedores.Clear();
                foreach (var p in proveedores) Proveedores.Add(p);
                Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                MostrarError("Error al buscar: " + ex.Message);
            }
        }

        [RelayCommand]
        private async Task LimpiarBusquedaAsync()
        {
            BuscarContacto = string.Empty;
            BuscarCiudad = string.Empty;
            await CargarListaAsync();
        }

        [RelayCommand]
        private void Nuevo()
        {
            ProveedorSeleccionado = null;
            NombreCompania = string.Empty;
            NombreContacto = string.Empty;
            Cargo = string.Empty;
            Direccion = string.Empty;
            Ciudad = string.Empty;
            CodigoPostal = string.Empty;
            Pais = string.Empty;
            Telefono = string.Empty;
            Fax = string.Empty;
            Mensaje = string.Empty;
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreCompania))
            {
                MostrarError("El nombre de la compañía es obligatorio.");
                return;
            }
            try
            {
                var p = ProveedorSeleccionado ?? new Proveedor();
                p.NombreCompania = NombreCompania.Trim();
                p.NombreContacto = NombreContacto;
                p.Cargo = Cargo;
                p.Direccion = Direccion;
                p.Ciudad = Ciudad;
                p.CodigoPostal = CodigoPostal;
                p.Pais = Pais;
                p.Telefono = Telefono;
                p.Fax = Fax;

                if (ProveedorSeleccionado == null)
                    await _repo.InsertarAsync(p);
                else
                    await _repo.ActualizarAsync(p);

                await CargarListaAsync();
                Nuevo();
                MostrarExito("Guardado correctamente.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al guardar: " + ex.Message);
            }
        }

        [RelayCommand]
        private async Task EliminarAsync()
        {
            if (ProveedorSeleccionado == null)
            {
                MostrarError("Selecciona un proveedor de la lista.");
                return;
            }
            if (MessageBox.Show($"¿Eliminar el proveedor '{ProveedorSeleccionado.NombreCompania}'?", "Confirmar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                await _repo.EliminarAsync(ProveedorSeleccionado.IdProveedor);
                await CargarListaAsync();
                Nuevo();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo eliminar: " + ex.Message);
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
