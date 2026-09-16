using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LAB04.Data;
using LAB04.Models;

namespace LAB04.ViewModels
{
    public partial class PedidosViewModel : ObservableObject
    {
        private readonly PedidoRepository _repo = new();
        private readonly ProductoRepository _repoProductos = new();

        public ObservableCollection<Pedido> Pedidos { get; } = new();
        public ObservableCollection<ItemCombo> Clientes { get; } = new();
        public ObservableCollection<ItemCombo> Empleados { get; } = new();
        public ObservableCollection<ItemCombo> Transportistas { get; } = new();
        public ObservableCollection<Producto> ProductosDisponibles { get; } = new();
        public ObservableCollection<DetallePedido> Detalle { get; } = new();
        public ObservableCollection<ReporteDetallePedido> Reporte { get; } = new();

        [ObservableProperty] private Pedido? pedidoSeleccionado;

        [ObservableProperty] private ItemCombo? clienteCombo;
        [ObservableProperty] private ItemCombo? empleadoCombo;
        [ObservableProperty] private DateTime? fechaPedido = DateTime.Today;
        [ObservableProperty] private DateTime? fechaRequerida;
        [ObservableProperty] private DateTime? fechaEnvio;
        [ObservableProperty] private ItemCombo? transportistaCombo;
        [ObservableProperty] private string? destinatario;
        [ObservableProperty] private string? ciudadDestino;
        [ObservableProperty] private string? paisDestino;

        [ObservableProperty] private Producto? productoDetalle;
        [ObservableProperty] private string precioDetalleTexto = "0";
        [ObservableProperty] private string cantidadDetalleTexto = "1";
        [ObservableProperty] private string descuentoDetalleTexto = "0";
        [ObservableProperty] private DetallePedido? lineaDetalleSeleccionada;

        [ObservableProperty] private DateTime? fechaDesde = DateTime.Today.AddMonths(-1);
        [ObservableProperty] private DateTime? fechaHasta = DateTime.Today;
        [ObservableProperty] private string totalReporte = string.Empty;

        [ObservableProperty] private string mensaje = string.Empty;
        [ObservableProperty] private bool esError;

        public PedidosViewModel()
        {
            CargarCombos();
            CargarLista();
        }

        partial void OnPedidoSeleccionadoChanged(Pedido? value)
        {
            Detalle.Clear();
            if (value == null) return;

            ClienteCombo = Clientes.FirstOrDefault(c => c.Id == value.IdCliente);
            EmpleadoCombo = Empleados.FirstOrDefault(e => e.Id == value.IdEmpleado);
            FechaPedido = value.FechaPedido;
            FechaRequerida = value.FechaRequerida;
            FechaEnvio = value.FechaEnvio;
            TransportistaCombo = Transportistas.FirstOrDefault(t => t.Id == value.IdTransportista);
            Destinatario = value.NombreDestinatario;
            CiudadDestino = value.CiudadDestino;
            PaisDestino = value.PaisDestino;
            Mensaje = string.Empty;

            CargarDetalle(value.IdPedido);
        }

        partial void OnProductoDetalleChanged(Producto? value)
        {
            if (value != null) PrecioDetalleTexto = value.PrecioUnidad.ToString(CultureInfo.InvariantCulture);
        }

        private void CargarCombos()
        {
            try
            {
                Clientes.Clear();
                foreach (var c in _repo.ListarClientes()) Clientes.Add(c);
                Empleados.Clear();
                foreach (var e in _repo.ListarEmpleados()) Empleados.Add(e);
                Transportistas.Clear();
                foreach (var t in _repo.ListarTransportistas()) Transportistas.Add(t);
                ProductosDisponibles.Clear();
                foreach (var p in _repoProductos.Listar()) ProductosDisponibles.Add(p);
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
                Pedidos.Clear();
                foreach (var p in _repo.Listar()) Pedidos.Add(p);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar pedidos: " + ex.Message);
            }
        }

        private void CargarDetalle(int idPedido)
        {
            try
            {
                Detalle.Clear();
                foreach (var d in _repo.ListarDetalles(idPedido)) Detalle.Add(d);
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar el detalle: " + ex.Message);
            }
        }

        [RelayCommand]
        private void NuevoPedido()
        {
            PedidoSeleccionado = null;
            ClienteCombo = null;
            EmpleadoCombo = null;
            FechaPedido = DateTime.Today;
            FechaRequerida = null;
            FechaEnvio = null;
            TransportistaCombo = null;
            Destinatario = string.Empty;
            CiudadDestino = string.Empty;
            PaisDestino = string.Empty;
            Detalle.Clear();
            Mensaje = string.Empty;
        }

        [RelayCommand]
        private void GuardarPedido()
        {
            if (FechaPedido == null)
            {
                MostrarError("La fecha del pedido es obligatoria.");
                return;
            }
            try
            {
                var p = PedidoSeleccionado ?? new Pedido();
                p.IdCliente = ClienteCombo?.Id;
                p.IdEmpleado = EmpleadoCombo?.Id;
                p.FechaPedido = FechaPedido.Value;
                p.FechaRequerida = FechaRequerida;
                p.FechaEnvio = FechaEnvio;
                p.IdTransportista = TransportistaCombo?.Id;
                p.NombreDestinatario = Destinatario;
                p.CiudadDestino = CiudadDestino;
                p.PaisDestino = PaisDestino;

                bool esNuevo = PedidoSeleccionado == null;
                if (esNuevo)
                    _repo.Insertar(p);
                else
                    _repo.Actualizar(p);

                CargarLista();
                PedidoSeleccionado = Pedidos.FirstOrDefault(x => x.IdPedido == p.IdPedido);
                MostrarExito(esNuevo
                    ? $"Pedido creado (Id {p.IdPedido}). Ahora puedes agregar líneas de detalle."
                    : "Pedido actualizado correctamente.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al guardar: " + ex.Message);
            }
        }

        [RelayCommand]
        private void EliminarPedido()
        {
            if (PedidoSeleccionado == null)
            {
                MostrarError("Selecciona un pedido de la lista.");
                return;
            }
            if (MessageBox.Show($"¿Eliminar el pedido #{PedidoSeleccionado.IdPedido} y sus detalles?", "Confirmar",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            try
            {
                _repo.Eliminar(PedidoSeleccionado.IdPedido);
                CargarLista();
                NuevoPedido();
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo eliminar: " + ex.Message);
            }
        }

        [RelayCommand]
        private void AgregarDetalle()
        {
            if (PedidoSeleccionado == null)
            {
                MostrarError("Primero guarda el pedido antes de agregar líneas de detalle.");
                return;
            }
            if (ProductoDetalle == null)
            {
                MostrarError("Selecciona un producto.");
                return;
            }
            if (!decimal.TryParse(PrecioDetalleTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out var precio) ||
                !short.TryParse(CantidadDetalleTexto, out var cantidad) ||
                !decimal.TryParse(DescuentoDetalleTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out var descuento))
            {
                MostrarError("Precio, cantidad y descuento deben ser numéricos válidos.");
                return;
            }

            try
            {
                _repo.InsertarDetalle(new DetallePedido
                {
                    IdPedido = PedidoSeleccionado.IdPedido,
                    IdProducto = ProductoDetalle.IdProducto,
                    PrecioUnidad = precio,
                    Cantidad = cantidad,
                    Descuento = descuento
                });
                CargarDetalle(PedidoSeleccionado.IdPedido);
                MostrarExito("Línea agregada.");
            }
            catch (Exception ex)
            {
                MostrarError("Error al agregar la línea (¿el producto ya está en el pedido?): " + ex.Message);
            }
        }

        [RelayCommand]
        private void EliminarDetalle()
        {
            if (PedidoSeleccionado == null || LineaDetalleSeleccionada == null)
            {
                MostrarError("Selecciona una línea del detalle.");
                return;
            }
            try
            {
                _repo.EliminarDetalle(LineaDetalleSeleccionada.IdPedido, LineaDetalleSeleccionada.IdProducto);
                CargarDetalle(PedidoSeleccionado.IdPedido);
            }
            catch (Exception ex)
            {
                MostrarError("Error al quitar la línea: " + ex.Message);
            }
        }

        [RelayCommand]
        private void GenerarReporte()
        {
            if (FechaDesde == null || FechaHasta == null)
            {
                TotalReporte = "Selecciona ambas fechas.";
                return;
            }
            try
            {
                var datos = _repo.ReportePorFechas(FechaDesde.Value, FechaHasta.Value);
                Reporte.Clear();
                foreach (var d in datos) Reporte.Add(d);
                var total = datos.Sum(d => d.Subtotal);
                TotalReporte = $"Líneas: {datos.Count}   Total: {total:C}";
            }
            catch (Exception ex)
            {
                TotalReporte = "Error al generar el reporte: " + ex.Message;
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
