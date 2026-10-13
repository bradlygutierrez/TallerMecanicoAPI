/*
  SISTEMA DE GESTION DE TALLER - SQL SERVER 2022
  Ejecutar en una instancia donde se permita crear bases de datos.
  Los esquemas agrupan responsabilidades, NO crean bases de datos separadas.
  No volver a ejecutar sobre una base ya inicializada: es un script de instalacion inicial.
*/
IF DB_ID(N'TallerMecanicoDB') IS NULL
    CREATE DATABASE TallerMecanicoDB;
GO
USE TallerMecanicoDB;
GO

/* 1. ESQUEMAS */
IF SCHEMA_ID(N'seguridad') IS NULL EXEC(N'CREATE SCHEMA seguridad');
IF SCHEMA_ID(N'catalogos') IS NULL EXEC(N'CREATE SCHEMA catalogos');
IF SCHEMA_ID(N'clientes') IS NULL EXEC(N'CREATE SCHEMA clientes');
IF SCHEMA_ID(N'taller') IS NULL EXEC(N'CREATE SCHEMA taller');
GO

/* 2. TABLAS BASE. Tres entidades admiten creador NULL para el arranque inicial:
      EstadoExistencia, Rol y Usuario. Se completan al crear el usuario sistema. */
CREATE TABLE catalogos.EstadoExistencia (
    EstadoExistenciaId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_EstadoExistencia PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_EstadoExistencia_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_EstadoExistencia_Nombre UNIQUE (Nombre)
);
GO
CREATE TABLE seguridad.Rol (
    RolId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Rol PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Rol_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_Rol_Nombre UNIQUE (Nombre)
);
GO
CREATE TABLE seguridad.Usuario (
    UsuarioId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuario PRIMARY KEY,
    RolId INT NOT NULL,
    NombreUsuario NVARCHAR(80) NOT NULL,
    Telefono NVARCHAR(25) NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Usuario_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NULL, -- permite registrar el usuario inicial del sistema
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (RolId) REFERENCES seguridad.Rol(RolId),
    CONSTRAINT FK_Usuario_EstadoExistencia FOREIGN KEY (EstadoExistenciaId)
        REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId)
);
GO

/* 3. CATALOGOS */
CREATE TABLE catalogos.Marca (
    MarcaId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Marca PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    PaisOrigen NVARCHAR(100) NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Marca_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_Marca_Nombre UNIQUE (Nombre)
);
GO
CREATE TABLE catalogos.Modelo (
    ModeloId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Modelo PRIMARY KEY,
    MarcaId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Modelo_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT FK_Modelo_Marca FOREIGN KEY (MarcaId) REFERENCES catalogos.Marca(MarcaId),
    CONSTRAINT UQ_Modelo_Marca_Nombre UNIQUE (MarcaId, Nombre),
    CONSTRAINT UQ_Modelo_MarcaId_ModeloId UNIQUE (MarcaId, ModeloId)
);
GO
CREATE TABLE catalogos.Servicio (
    ServicioId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Servicio PRIMARY KEY,
    Nombre NVARCHAR(120) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    PrecioBase DECIMAL(18,2) NOT NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Servicio_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_Servicio_Nombre UNIQUE (Nombre),
    CONSTRAINT CK_Servicio_PrecioBase CHECK (PrecioBase >= 0)
);
GO
CREATE TABLE catalogos.EstadoOrden (
    EstadoOrdenId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_EstadoOrden PRIMARY KEY,
    Nombre NVARCHAR(60) NOT NULL,
    Descripcion NVARCHAR(250) NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_EstadoOrden_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_EstadoOrden_Nombre UNIQUE (Nombre)
);
GO
CREATE TABLE catalogos.ProblemaReportado (
    ProblemaReportadoId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ProblemaReportado PRIMARY KEY,
    Nombre NVARCHAR(120) NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_ProblemaReportado_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_ProblemaReportado_Nombre UNIQUE (Nombre)
);
GO

/* 4. CLIENTES Y VEHICULOS */
CREATE TABLE clientes.Cliente (
    ClienteId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cliente PRIMARY KEY,
    NombreCompleto NVARCHAR(150) NOT NULL,
    Telefono NVARCHAR(25) NULL,
    CorreoElectronico NVARCHAR(254) NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Cliente_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL
);
GO
CREATE TABLE clientes.Vehiculo (
    VehiculoId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Vehiculo PRIMARY KEY,
    Placa NVARCHAR(20) NOT NULL,
    MarcaId INT NOT NULL,
    ModeloId INT NOT NULL,
    AnioFabricacion SMALLINT NOT NULL,
    ClienteId INT NOT NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_Vehiculo_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_Vehiculo_Placa UNIQUE (Placa),
    CONSTRAINT CK_Vehiculo_AnioFabricacion CHECK (AnioFabricacion BETWEEN 1886 AND 2100),
    CONSTRAINT FK_Vehiculo_Cliente FOREIGN KEY (ClienteId) REFERENCES clientes.Cliente(ClienteId),
    CONSTRAINT FK_Vehiculo_Marca FOREIGN KEY (MarcaId) REFERENCES catalogos.Marca(MarcaId),
    CONSTRAINT FK_Vehiculo_Modelo_Marca FOREIGN KEY (MarcaId, ModeloId)
        REFERENCES catalogos.Modelo(MarcaId, ModeloId)
);
GO

/* 5. ORDENES Y DETALLES */
CREATE TABLE taller.OrdenTrabajo (
    OrdenTrabajoId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_OrdenTrabajo PRIMARY KEY,
    VehiculoId INT NOT NULL,
    FechaIngreso DATETIME2(0) NOT NULL CONSTRAINT DF_OrdenTrabajo_FechaIngreso DEFAULT SYSUTCDATETIME(),
    ProblemaReportadoId INT NOT NULL,
    EstadoOrdenId INT NOT NULL,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_OrdenTrabajo_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT FK_OrdenTrabajo_Vehiculo FOREIGN KEY (VehiculoId) REFERENCES clientes.Vehiculo(VehiculoId),
    CONSTRAINT FK_OrdenTrabajo_ProblemaReportado FOREIGN KEY (ProblemaReportadoId)
        REFERENCES catalogos.ProblemaReportado(ProblemaReportadoId),
    CONSTRAINT FK_OrdenTrabajo_EstadoOrden FOREIGN KEY (EstadoOrdenId)
        REFERENCES catalogos.EstadoOrden(EstadoOrdenId)
);
GO
CREATE TABLE taller.DetalleOrdenTrabajo (
    DetalleOrdenTrabajoId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_DetalleOrdenTrabajo PRIMARY KEY,
    OrdenTrabajoId INT NOT NULL,
    ServicioId INT NOT NULL,
    Cantidad DECIMAL(12,2) NOT NULL,
    PrecioUnitarioAplicado DECIMAL(18,2) NOT NULL,
    Subtotal AS (CONVERT(DECIMAL(30,4), Cantidad * PrecioUnitarioAplicado)) PERSISTED,
    EstadoExistenciaId INT NOT NULL,
    FechaCreacion DATETIME2(0) NOT NULL CONSTRAINT DF_DetalleOrdenTrabajo_FechaCreacion DEFAULT SYSUTCDATETIME(),
    CreadoPorUsuarioId INT NOT NULL,
    FechaActualizacion DATETIME2(0) NULL,
    ActualizadoPorUsuarioId INT NULL,
    CONSTRAINT UQ_DetalleOrdenTrabajo_Orden_Servicio UNIQUE (OrdenTrabajoId, ServicioId),
    CONSTRAINT CK_DetalleOrdenTrabajo_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_DetalleOrdenTrabajo_Precio CHECK (PrecioUnitarioAplicado >= 0),
    CONSTRAINT FK_DetalleOrdenTrabajo_OrdenTrabajo FOREIGN KEY (OrdenTrabajoId)
        REFERENCES taller.OrdenTrabajo(OrdenTrabajoId),
    CONSTRAINT FK_DetalleOrdenTrabajo_Servicio FOREIGN KEY (ServicioId)
        REFERENCES catalogos.Servicio(ServicioId)
);
GO

/* 6. FK ESTADO DE EXISTENCIA Y AUDITORIA: ambas apuntan a las tablas maestras. */
ALTER TABLE catalogos.Marca ADD CONSTRAINT FK_Marca_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
ALTER TABLE catalogos.Modelo ADD CONSTRAINT FK_Modelo_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
ALTER TABLE catalogos.Servicio ADD CONSTRAINT FK_Servicio_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
ALTER TABLE clientes.Cliente ADD CONSTRAINT FK_Cliente_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
ALTER TABLE clientes.Vehiculo ADD CONSTRAINT FK_Vehiculo_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
ALTER TABLE taller.OrdenTrabajo ADD CONSTRAINT FK_OrdenTrabajo_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
ALTER TABLE taller.DetalleOrdenTrabajo ADD CONSTRAINT FK_DetalleOrdenTrabajo_EstadoExistencia FOREIGN KEY (EstadoExistenciaId) REFERENCES catalogos.EstadoExistencia(EstadoExistenciaId);
GO

/* FK individuales de auditoria: incluye las tablas maestras, Rol y Usuario. */
DECLARE @Sql NVARCHAR(MAX) = N'';
SELECT @Sql = @Sql + N'ALTER TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) +
    N' ADD CONSTRAINT ' + QUOTENAME(N'FK_' + t.name + N'_CreadoPorUsuario') +
    N' FOREIGN KEY (CreadoPorUsuarioId) REFERENCES seguridad.Usuario(UsuarioId);' + CHAR(10) +
    N'ALTER TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) +
    N' ADD CONSTRAINT ' + QUOTENAME(N'FK_' + t.name + N'_ActualizadoPorUsuario') +
    N' FOREIGN KEY (ActualizadoPorUsuarioId) REFERENCES seguridad.Usuario(UsuarioId);' + CHAR(10)
FROM sys.tables t
INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
WHERE s.name IN (N'seguridad', N'catalogos', N'clientes', N'taller')
  AND EXISTS (SELECT 1 FROM sys.columns c WHERE c.object_id = t.object_id AND c.name = N'CreadoPorUsuarioId')
  AND EXISTS (SELECT 1 FROM sys.columns c WHERE c.object_id = t.object_id AND c.name = N'ActualizadoPorUsuarioId');
EXEC sys.sp_executesql @Sql;
GO

/* 7. INDICES PARA FK Y CONSULTAS HABITUALES */
CREATE INDEX IX_Usuario_RolId ON seguridad.Usuario(RolId);
CREATE INDEX IX_Usuario_EstadoExistenciaId ON seguridad.Usuario(EstadoExistenciaId);
CREATE INDEX IX_Modelo_MarcaId ON catalogos.Modelo(MarcaId);
CREATE INDEX IX_Cliente_EstadoExistenciaId ON clientes.Cliente(EstadoExistenciaId);
CREATE INDEX IX_Vehiculo_ClienteId ON clientes.Vehiculo(ClienteId);
CREATE INDEX IX_OrdenTrabajo_Vehiculo_FechaIngreso ON taller.OrdenTrabajo(VehiculoId, FechaIngreso DESC);
CREATE INDEX IX_OrdenTrabajo_EstadoOrdenId ON taller.OrdenTrabajo(EstadoOrdenId);
CREATE INDEX IX_DetalleOrdenTrabajo_ServicioId ON taller.DetalleOrdenTrabajo(ServicioId);
GO

/* 8. DATOS MINIMOS DE ARRANQUE.
   El usuario 'sistema' es solamente un actor de auditoria: no tiene password,
   hash ni login habilitado por este script. La autenticacion va en otra capa. */
SET XACT_ABORT ON;
BEGIN TRANSACTION;
DECLARE @ActivoId INT, @RolAdministradorId INT, @UsuarioSistemaId INT;
INSERT INTO catalogos.EstadoExistencia (Nombre, Descripcion)
VALUES (N'Activo', N'Registro disponible para su uso');
SET @ActivoId = CONVERT(INT, SCOPE_IDENTITY());
INSERT INTO catalogos.EstadoExistencia (Nombre, Descripcion)
VALUES (N'Inactivo', N'Registro desactivado de forma logica');
INSERT INTO seguridad.Rol (Nombre, Descripcion)
VALUES (N'Administrador', N'Rol administrativo del sistema');
SET @RolAdministradorId = CONVERT(INT, SCOPE_IDENTITY());
INSERT INTO seguridad.Usuario (RolId, NombreUsuario, EstadoExistenciaId)
VALUES (@RolAdministradorId, N'sistema', @ActivoId);
SET @UsuarioSistemaId = CONVERT(INT, SCOPE_IDENTITY());
UPDATE catalogos.EstadoExistencia SET CreadoPorUsuarioId = @UsuarioSistemaId WHERE CreadoPorUsuarioId IS NULL;
UPDATE seguridad.Rol SET CreadoPorUsuarioId = @UsuarioSistemaId WHERE CreadoPorUsuarioId IS NULL;
UPDATE seguridad.Usuario SET CreadoPorUsuarioId = @UsuarioSistemaId WHERE UsuarioId = @UsuarioSistemaId;
INSERT INTO catalogos.EstadoOrden (Nombre, Descripcion, CreadoPorUsuarioId)
VALUES (N'Pendiente', N'Orden en cola', @UsuarioSistemaId),
       (N'EnProceso', N'Orden en ejecución', @UsuarioSistemaId),
       (N'Completada', N'Trabajo terminado', @UsuarioSistemaId),
       (N'Cancelada', N'Orden cancelada', @UsuarioSistemaId);
COMMIT TRANSACTION;
GO

/* 9. VISTA DE TOTALES: evita almacenar un total desactualizado.
      Se incluye solo el detalle Activo y la existencia Activo se resuelve por nombre. */
CREATE VIEW taller.vw_TotalOrdenTrabajo
AS
SELECT o.OrdenTrabajoId,
       COALESCE(SUM(CASE WHEN ee.Nombre = N'Activo' THEN d.Subtotal ELSE 0 END), 0) AS Total
FROM taller.OrdenTrabajo AS o
LEFT JOIN taller.DetalleOrdenTrabajo AS d ON d.OrdenTrabajoId = o.OrdenTrabajoId
LEFT JOIN catalogos.EstadoExistencia AS ee ON ee.EstadoExistenciaId = d.EstadoExistenciaId
GROUP BY o.OrdenTrabajoId;
GO

/* Verificacion basica */
SELECT s.name AS Esquema, t.name AS Tabla
FROM sys.tables AS t JOIN sys.schemas AS s ON s.schema_id = t.schema_id
WHERE s.name IN (N'seguridad', N'catalogos', N'clientes', N'taller')
ORDER BY s.name, t.name;
GO
