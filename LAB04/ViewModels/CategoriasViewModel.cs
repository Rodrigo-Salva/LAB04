using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LAB04.Data;
using LAB04.Models;

namespace LAB04.ViewModels
{
    public partial class CategoriasViewModel : ObservableObject
    {
        private readonly CategoriaRepository _repo = new();

        public ObservableCollection<Categoria> Categorias { get; } = new();

        [ObservableProperty]
        private Categoria? categoriaSeleccionada;

        [ObservableProperty]
        private string nombre = string.Empty;

        [ObservableProperty]
        private string? descripcion;

        [ObservableProperty]
        private string mensaje = string.Empty;

        [ObservableProperty]
        private bool esError;

        public CategoriasViewModel()
        {
            _ = CargarListaAsync();
        }

        partial void OnCategoriaSeleccionadaChanged(Categoria? value)
        {
            if (value == null) return;
            Nombre = value.NombreCategoria;
            Descripcion = value.Descripcion;
            Mensaje = string.Empty;
        }

        private async Task CargarListaAsync()
        {
            try
            {
                var categorias = await _repo.ListarAsync();
                Categorias.Clear();
                foreach (var c in categorias) Categorias.Add(c);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar categorías: " + ex.Message);
            }
        }

        [RelayCommand]
        private void Nuevo()
        {
            CategoriaSeleccionada = null;
            Nombre = string.Empty;
            Descripcion = string.Empty;
            Mensaje = string.Empty;
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                MostrarError("El nombre de la categoría es obligatorio.");
                return;
            }
            try
            {
                if (CategoriaSeleccionada == null)
                {
                    var nueva = new Categoria { NombreCategoria = Nombre.Trim(), Descripcion = Descripcion };
                    await _repo.InsertarAsync(nueva);
                }
                else
                {
                    CategoriaSeleccionada.NombreCategoria = Nombre.Trim();
                    CategoriaSeleccionada.Descripcion = Descripcion;
                    await _repo.ActualizarAsync(CategoriaSeleccionada);
                }
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
            if (CategoriaSeleccionada == null)
            {
                MostrarError("Selecciona una categoría de la lista.");
                return;
            }
            if (MessageBox.Show($"¿Eliminar la categoría '{CategoriaSeleccionada.NombreCategoria}'?", "Confirmar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                await _repo.EliminarAsync(CategoriaSeleccionada.IdCategoria);
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
