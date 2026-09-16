/* =========================================================================
   NeptunoDB - Semana 05: eliminación lógica
   ------------------------------------------------------------------------
   Agrega el campo Activo (BIT, por defecto 1) a Categorias, Proveedores,
   Productos y Pedidos, para reemplazar el DELETE físico por una baja
   lógica (UPDATE ... SET Activo = 0) en esas cuatro tablas.

   Ejecutar DESPUÉS de base.sql y ANTES de ProcedimientosAlmacenados.sql.
   Es seguro volver a ejecutarlo: cada ALTER está protegido con un IF que
   comprueba si la columna ya existe.
   ========================================================================= */
USE NeptunoDB;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Categorias') AND name = 'Activo')
BEGIN
    ALTER TABLE dbo.Categorias ADD Activo BIT NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Proveedores') AND name = 'Activo')
BEGIN
    ALTER TABLE dbo.Proveedores ADD Activo BIT NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Productos') AND name = 'Activo')
BEGIN
    ALTER TABLE dbo.Productos ADD Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT 1;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Pedidos') AND name = 'Activo')
BEGIN
    ALTER TABLE dbo.Pedidos ADD Activo BIT NOT NULL CONSTRAINT DF_Pedidos_Activo DEFAULT 1;
END
GO

-- Verificación
SELECT t.name AS Tabla, c.name AS Columna, c.is_nullable, dc.definition AS ValorPorDefecto
FROM sys.columns c
JOIN sys.tables t ON t.object_id = c.object_id
LEFT JOIN sys.default_constraints dc ON dc.object_id = c.default_object_id
WHERE c.name = 'Activo'
ORDER BY t.name;
GO
