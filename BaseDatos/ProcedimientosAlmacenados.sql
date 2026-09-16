/* =========================================================================
   NeptunoDB - Procedimientos almacenados
   CRUD de Productos, Categorias, Proveedores, Pedidos (+ DetallePedidos)
   y procedimientos de búsqueda / reportes.

   Semana 05: eliminación lógica. Categorias, Proveedores, Productos y
   Pedidos tienen un campo Activo (BIT). Sus procedimientos "Eliminar"
   hacen UPDATE ... SET Activo = 0 en vez de DELETE, y todos los listados
   (y el reporte) solo devuelven registros con Activo = 1.
   DetallePedidos NO tiene columna Activo: sus líneas se siguen
   agregando/editando/quitando con INSERT/UPDATE/DELETE físico normal,
   porque no forma parte del requisito de baja lógica.

   Orden de ejecución: base.sql -> 02_AgregarColumnaActivo.sql -> este script.
   ========================================================================= */
USE NeptunoDB;
GO

-- =========================================================================
-- CATEGORIAS
-- =========================================================================
IF OBJECT_ID('dbo.sp_Categorias_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Categorias_Listar;
GO
CREATE PROCEDURE dbo.sp_Categorias_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID AS IdCategoria, NombreCategoria, Descripcion
    FROM dbo.Categorias
    WHERE Activo = 1
    ORDER BY NombreCategoria;
END
GO

IF OBJECT_ID('dbo.sp_Categorias_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Categorias_ObtenerPorId;
GO
CREATE PROCEDURE dbo.sp_Categorias_ObtenerPorId
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID AS IdCategoria, NombreCategoria, Descripcion
    FROM dbo.Categorias
    WHERE CategoriaID = @IdCategoria AND Activo = 1;
END
GO

IF OBJECT_ID('dbo.sp_Categorias_Insertar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Categorias_Insertar;
GO
CREATE PROCEDURE dbo.sp_Categorias_Insertar
    @NombreCategoria NVARCHAR(30),
    @Descripcion NVARCHAR(200) = NULL,
    @IdCategoria INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Categorias (NombreCategoria, Descripcion)
    VALUES (@NombreCategoria, @Descripcion);
    SET @IdCategoria = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID('dbo.sp_Categorias_Actualizar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Categorias_Actualizar;
GO
CREATE PROCEDURE dbo.sp_Categorias_Actualizar
    @IdCategoria INT,
    @NombreCategoria NVARCHAR(30),
    @Descripcion NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Categorias
    SET NombreCategoria = @NombreCategoria,
        Descripcion = @Descripcion
    WHERE CategoriaID = @IdCategoria;
END
GO

IF OBJECT_ID('dbo.sp_Categorias_Eliminar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Categorias_Eliminar;
GO
CREATE PROCEDURE dbo.sp_Categorias_Eliminar
    @IdCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Baja lógica: nunca se borra físicamente el registro.
    UPDATE dbo.Categorias SET Activo = 0 WHERE CategoriaID = @IdCategoria;
END
GO

-- =========================================================================
-- PROVEEDORES
-- =========================================================================
IF OBJECT_ID('dbo.sp_Proveedores_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Proveedores_Listar;
GO
CREATE PROCEDURE dbo.sp_Proveedores_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID AS IdProveedor, CompaniaNombre AS NombreCompania, NombreContacto,
           CargoContacto AS Cargo, Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE Activo = 1
    ORDER BY CompaniaNombre;
END
GO

IF OBJECT_ID('dbo.sp_Proveedores_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Proveedores_ObtenerPorId;
GO
CREATE PROCEDURE dbo.sp_Proveedores_ObtenerPorId
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID AS IdProveedor, CompaniaNombre AS NombreCompania, NombreContacto,
           CargoContacto AS Cargo, Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE ProveedorID = @IdProveedor AND Activo = 1;
END
GO

IF OBJECT_ID('dbo.sp_Proveedores_Insertar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Proveedores_Insertar;
GO
CREATE PROCEDURE dbo.sp_Proveedores_Insertar
    @NombreCompania NVARCHAR(60),
    @NombreContacto NVARCHAR(40) = NULL,
    @Cargo NVARCHAR(40) = NULL,
    @Direccion NVARCHAR(80) = NULL,
    @Ciudad NVARCHAR(30) = NULL,
    @CodigoPostal NVARCHAR(10) = NULL,
    @Pais NVARCHAR(30) = NULL,
    @Telefono NVARCHAR(24) = NULL,
    @Fax NVARCHAR(24) = NULL,
    @IdProveedor INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Proveedores
        (CompaniaNombre, NombreContacto, CargoContacto, Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax)
    VALUES
        (@NombreCompania, @NombreContacto, @Cargo, @Direccion, @Ciudad, @CodigoPostal, @Pais, @Telefono, @Fax);
    SET @IdProveedor = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID('dbo.sp_Proveedores_Actualizar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Proveedores_Actualizar;
GO
CREATE PROCEDURE dbo.sp_Proveedores_Actualizar
    @IdProveedor INT,
    @NombreCompania NVARCHAR(60),
    @NombreContacto NVARCHAR(40) = NULL,
    @Cargo NVARCHAR(40) = NULL,
    @Direccion NVARCHAR(80) = NULL,
    @Ciudad NVARCHAR(30) = NULL,
    @CodigoPostal NVARCHAR(10) = NULL,
    @Pais NVARCHAR(30) = NULL,
    @Telefono NVARCHAR(24) = NULL,
    @Fax NVARCHAR(24) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Proveedores
    SET CompaniaNombre = @NombreCompania,
        NombreContacto = @NombreContacto,
        CargoContacto = @Cargo,
        Direccion = @Direccion,
        Ciudad = @Ciudad,
        CodigoPostal = @CodigoPostal,
        Pais = @Pais,
        Telefono = @Telefono,
        Fax = @Fax
    WHERE ProveedorID = @IdProveedor;
END
GO

IF OBJECT_ID('dbo.sp_Proveedores_Eliminar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Proveedores_Eliminar;
GO
CREATE PROCEDURE dbo.sp_Proveedores_Eliminar
    @IdProveedor INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Baja lógica: nunca se borra físicamente el registro.
    UPDATE dbo.Proveedores SET Activo = 0 WHERE ProveedorID = @IdProveedor;
END
GO

-- Búsqueda de proveedores por NombreContacto y Ciudad (parámetros opcionales)
IF OBJECT_ID('dbo.sp_Proveedores_BuscarPorContactoCiudad', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Proveedores_BuscarPorContactoCiudad;
GO
CREATE PROCEDURE dbo.sp_Proveedores_BuscarPorContactoCiudad
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID AS IdProveedor, CompaniaNombre AS NombreCompania, NombreContacto,
           CargoContacto AS Cargo, Direccion, Ciudad, CodigoPostal, Pais, Telefono, Fax
    FROM dbo.Proveedores
    WHERE Activo = 1
      AND (@NombreContacto IS NULL OR @NombreContacto = '' OR NombreContacto LIKE '%' + @NombreContacto + '%')
      AND (@Ciudad IS NULL OR @Ciudad = '' OR Ciudad LIKE '%' + @Ciudad + '%')
    ORDER BY CompaniaNombre;
END
GO

-- =========================================================================
-- PRODUCTOS
-- =========================================================================
IF OBJECT_ID('dbo.sp_Productos_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Productos_Listar;
GO
CREATE PROCEDURE dbo.sp_Productos_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductoID AS IdProducto, p.NombreProducto, p.ProveedorID AS IdProveedor,
           pr.CompaniaNombre AS NombreProveedor, p.CategoriaID AS IdCategoria, c.NombreCategoria,
           p.CantidadPorUnidad, p.PrecioUnidad, p.UnidadesEnExistencia, p.UnidadesEnPedido,
           p.NivelDeReorden AS NivelReorden, p.Descontinuado
    FROM dbo.Productos p
    LEFT JOIN dbo.Proveedores pr ON pr.ProveedorID = p.ProveedorID
    LEFT JOIN dbo.Categorias c ON c.CategoriaID = p.CategoriaID
    WHERE p.Activo = 1
    ORDER BY p.NombreProducto;
END
GO

IF OBJECT_ID('dbo.sp_Productos_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Productos_ObtenerPorId;
GO
CREATE PROCEDURE dbo.sp_Productos_ObtenerPorId
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProductoID AS IdProducto, NombreProducto, ProveedorID AS IdProveedor, CategoriaID AS IdCategoria,
           CantidadPorUnidad, PrecioUnidad, UnidadesEnExistencia, UnidadesEnPedido,
           NivelDeReorden AS NivelReorden, Descontinuado
    FROM dbo.Productos
    WHERE ProductoID = @IdProducto AND Activo = 1;
END
GO

IF OBJECT_ID('dbo.sp_Productos_Insertar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Productos_Insertar;
GO
CREATE PROCEDURE dbo.sp_Productos_Insertar
    @NombreProducto NVARCHAR(60),
    @IdProveedor INT = NULL,
    @IdCategoria INT = NULL,
    @CantidadPorUnidad NVARCHAR(30) = NULL,
    @PrecioUnidad DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT = 0,
    @UnidadesEnPedido SMALLINT = 0,
    @NivelReorden SMALLINT = 0,
    @Descontinuado BIT = 0,
    @IdProducto INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Productos
        (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad,
         UnidadesEnExistencia, UnidadesEnPedido, NivelDeReorden, Descontinuado)
    VALUES
        (@NombreProducto, @IdProveedor, @IdCategoria, @CantidadPorUnidad, @PrecioUnidad,
         @UnidadesEnExistencia, @UnidadesEnPedido, @NivelReorden, @Descontinuado);
    SET @IdProducto = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID('dbo.sp_Productos_Actualizar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Productos_Actualizar;
GO
CREATE PROCEDURE dbo.sp_Productos_Actualizar
    @IdProducto INT,
    @NombreProducto NVARCHAR(60),
    @IdProveedor INT = NULL,
    @IdCategoria INT = NULL,
    @CantidadPorUnidad NVARCHAR(30) = NULL,
    @PrecioUnidad DECIMAL(10,2) = 0,
    @UnidadesEnExistencia SMALLINT = 0,
    @UnidadesEnPedido SMALLINT = 0,
    @NivelReorden SMALLINT = 0,
    @Descontinuado BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos
    SET NombreProducto = @NombreProducto,
        ProveedorID = @IdProveedor,
        CategoriaID = @IdCategoria,
        CantidadPorUnidad = @CantidadPorUnidad,
        PrecioUnidad = @PrecioUnidad,
        UnidadesEnExistencia = @UnidadesEnExistencia,
        UnidadesEnPedido = @UnidadesEnPedido,
        NivelDeReorden = @NivelReorden,
        Descontinuado = @Descontinuado
    WHERE ProductoID = @IdProducto;
END
GO

IF OBJECT_ID('dbo.sp_Productos_Eliminar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Productos_Eliminar;
GO
CREATE PROCEDURE dbo.sp_Productos_Eliminar
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Baja lógica: nunca se borra físicamente el registro.
    UPDATE dbo.Productos SET Activo = 0 WHERE ProductoID = @IdProducto;
END
GO

-- =========================================================================
-- LISTAS DE APOYO (para combos en Pedidos)
-- =========================================================================
IF OBJECT_ID('dbo.sp_Clientes_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Clientes_Listar;
GO
CREATE PROCEDURE dbo.sp_Clientes_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ClienteID AS Id, Empresa AS Texto FROM dbo.Clientes ORDER BY Empresa;
END
GO

IF OBJECT_ID('dbo.sp_Empleados_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Empleados_Listar;
GO
CREATE PROCEDURE dbo.sp_Empleados_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmpleadoID AS Id, Nombre + ' ' + Apellidos AS Texto FROM dbo.Empleados ORDER BY Apellidos;
END
GO

IF OBJECT_ID('dbo.sp_Transportistas_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Transportistas_Listar;
GO
CREATE PROCEDURE dbo.sp_Transportistas_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TransportistaID AS Id, CompaniaNombre AS Texto FROM dbo.Transportistas ORDER BY CompaniaNombre;
END
GO

-- =========================================================================
-- PEDIDOS
-- =========================================================================
IF OBJECT_ID('dbo.sp_Pedidos_Listar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Pedidos_Listar;
GO
CREATE PROCEDURE dbo.sp_Pedidos_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT pe.PedidoID AS IdPedido, pe.ClienteID AS IdCliente, c.Empresa AS NombreCliente,
           pe.EmpleadoID AS IdEmpleado, e.Nombre + ' ' + e.Apellidos AS NombreEmpleado,
           pe.FechaPedido, pe.FechaRequerida, pe.FechaEnvio,
           pe.TransportistaID AS IdTransportista, t.CompaniaNombre AS NombreTransportista,
           pe.Destinatario AS NombreDestinatario, pe.CiudadDestino, pe.PaisDestino
    FROM dbo.Pedidos pe
    LEFT JOIN dbo.Clientes c ON c.ClienteID = pe.ClienteID
    LEFT JOIN dbo.Empleados e ON e.EmpleadoID = pe.EmpleadoID
    LEFT JOIN dbo.Transportistas t ON t.TransportistaID = pe.TransportistaID
    WHERE pe.Activo = 1
    ORDER BY pe.FechaPedido DESC;
END
GO

IF OBJECT_ID('dbo.sp_Pedidos_ObtenerPorId', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Pedidos_ObtenerPorId;
GO
CREATE PROCEDURE dbo.sp_Pedidos_ObtenerPorId
    @IdPedido INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PedidoID AS IdPedido, ClienteID AS IdCliente, EmpleadoID AS IdEmpleado,
           FechaPedido, FechaRequerida, FechaEnvio, TransportistaID AS IdTransportista,
           Destinatario AS NombreDestinatario, CiudadDestino, PaisDestino
    FROM dbo.Pedidos
    WHERE PedidoID = @IdPedido AND Activo = 1;
END
GO

IF OBJECT_ID('dbo.sp_Pedidos_Insertar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Pedidos_Insertar;
GO
CREATE PROCEDURE dbo.sp_Pedidos_Insertar
    @IdCliente INT = NULL,
    @IdEmpleado INT = NULL,
    @FechaPedido DATE,
    @FechaRequerida DATE = NULL,
    @FechaEnvio DATE = NULL,
    @IdTransportista INT = NULL,
    @NombreDestinatario NVARCHAR(60) = NULL,
    @CiudadDestino NVARCHAR(30) = NULL,
    @PaisDestino NVARCHAR(30) = NULL,
    @IdPedido INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Pedidos
        (ClienteID, EmpleadoID, FechaPedido, FechaRequerida, FechaEnvio, TransportistaID,
         Destinatario, CiudadDestino, PaisDestino)
    VALUES
        (@IdCliente, @IdEmpleado, @FechaPedido, @FechaRequerida, @FechaEnvio, @IdTransportista,
         @NombreDestinatario, @CiudadDestino, @PaisDestino);
    SET @IdPedido = SCOPE_IDENTITY();
END
GO

IF OBJECT_ID('dbo.sp_Pedidos_Actualizar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Pedidos_Actualizar;
GO
CREATE PROCEDURE dbo.sp_Pedidos_Actualizar
    @IdPedido INT,
    @IdCliente INT = NULL,
    @IdEmpleado INT = NULL,
    @FechaPedido DATE,
    @FechaRequerida DATE = NULL,
    @FechaEnvio DATE = NULL,
    @IdTransportista INT = NULL,
    @NombreDestinatario NVARCHAR(60) = NULL,
    @CiudadDestino NVARCHAR(30) = NULL,
    @PaisDestino NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Pedidos
    SET ClienteID = @IdCliente,
        EmpleadoID = @IdEmpleado,
        FechaPedido = @FechaPedido,
        FechaRequerida = @FechaRequerida,
        FechaEnvio = @FechaEnvio,
        TransportistaID = @IdTransportista,
        Destinatario = @NombreDestinatario,
        CiudadDestino = @CiudadDestino,
        PaisDestino = @PaisDestino
    WHERE PedidoID = @IdPedido;
END
GO

IF OBJECT_ID('dbo.sp_Pedidos_Eliminar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_Pedidos_Eliminar;
GO
CREATE PROCEDURE dbo.sp_Pedidos_Eliminar
    @IdPedido INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Baja lógica: nunca se borra físicamente el pedido ni su detalle.
    -- Al quedar Activo = 0, el pedido deja de aparecer en sp_Pedidos_Listar
    -- y sus líneas quedan automáticamente excluidas del reporte por fechas
    -- (que hace INNER JOIN con Pedidos filtrando Activo = 1).
    UPDATE dbo.Pedidos SET Activo = 0 WHERE PedidoID = @IdPedido;
END
GO

-- =========================================================================
-- DETALLE DE PEDIDOS (CRUD, dependiente de Pedidos)
-- =========================================================================
IF OBJECT_ID('dbo.sp_DetallesPedidos_ListarPorPedido', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_DetallesPedidos_ListarPorPedido;
GO
CREATE PROCEDURE dbo.sp_DetallesPedidos_ListarPorPedido
    @IdPedido INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dp.PedidoID AS IdPedido, dp.ProductoID AS IdProducto, p.NombreProducto,
           dp.PrecioUnidad, dp.Cantidad, dp.Descuento,
           CAST((dp.PrecioUnidad * dp.Cantidad * (1 - dp.Descuento)) AS DECIMAL(10,2)) AS Subtotal
    FROM dbo.DetallePedidos dp
    INNER JOIN dbo.Productos p ON p.ProductoID = dp.ProductoID
    WHERE dp.PedidoID = @IdPedido
    ORDER BY p.NombreProducto;
END
GO

IF OBJECT_ID('dbo.sp_DetallesPedidos_Insertar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_DetallesPedidos_Insertar;
GO
CREATE PROCEDURE dbo.sp_DetallesPedidos_Insertar
    @IdPedido INT,
    @IdProducto INT,
    @PrecioUnidad DECIMAL(10,2),
    @Cantidad SMALLINT,
    @Descuento DECIMAL(4,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.DetallePedidos (PedidoID, ProductoID, PrecioUnidad, Cantidad, Descuento)
    VALUES (@IdPedido, @IdProducto, @PrecioUnidad, @Cantidad, @Descuento);
END
GO

IF OBJECT_ID('dbo.sp_DetallesPedidos_Actualizar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_DetallesPedidos_Actualizar;
GO
CREATE PROCEDURE dbo.sp_DetallesPedidos_Actualizar
    @IdPedido INT,
    @IdProducto INT,
    @PrecioUnidad DECIMAL(10,2),
    @Cantidad SMALLINT,
    @Descuento DECIMAL(4,2) = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.DetallePedidos
    SET PrecioUnidad = @PrecioUnidad,
        Cantidad = @Cantidad,
        Descuento = @Descuento
    WHERE PedidoID = @IdPedido AND ProductoID = @IdProducto;
END
GO

IF OBJECT_ID('dbo.sp_DetallesPedidos_Eliminar', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_DetallesPedidos_Eliminar;
GO
CREATE PROCEDURE dbo.sp_DetallesPedidos_Eliminar
    @IdPedido INT,
    @IdProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.DetallePedidos WHERE PedidoID = @IdPedido AND ProductoID = @IdProducto;
END
GO

-- Reporte: detalle de pedidos + INNER JOIN con Pedidos, filtrando por intervalo de fechas.
-- Excluye pedidos dados de baja lógicamente (Activo = 0).
IF OBJECT_ID('dbo.sp_DetallesPedidos_ReportePorFechas', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_DetallesPedidos_ReportePorFechas;
GO
CREATE PROCEDURE dbo.sp_DetallesPedidos_ReportePorFechas
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT pe.PedidoID AS IdPedido, pe.FechaPedido, c.Empresa AS NombreCliente,
           p.NombreProducto, dp.PrecioUnidad, dp.Cantidad, dp.Descuento,
           CAST((dp.PrecioUnidad * dp.Cantidad * (1 - dp.Descuento)) AS DECIMAL(10,2)) AS Subtotal
    FROM dbo.Pedidos pe
    INNER JOIN dbo.DetallePedidos dp ON dp.PedidoID = pe.PedidoID
    INNER JOIN dbo.Productos p ON p.ProductoID = dp.ProductoID
    LEFT JOIN dbo.Clientes c ON c.ClienteID = pe.ClienteID
    WHERE pe.FechaPedido BETWEEN @FechaInicio AND @FechaFin
      AND pe.Activo = 1
    ORDER BY pe.FechaPedido, pe.PedidoID;
END
GO
