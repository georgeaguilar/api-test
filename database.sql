IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Clientes] (
    [ClienteId] int NOT NULL IDENTITY,
    [Nombre] nvarchar(150) NOT NULL,
    [Identidad] nvarchar(450) NOT NULL,
    [FechaNacimiento] datetime2 NOT NULL,
    [TipoCliente] nvarchar(20) NOT NULL,
    [Telefono] nvarchar(max) NULL,
    [CorreoElectronico] nvarchar(150) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([ClienteId])
);
GO

CREATE TABLE [TiposSeguro] (
    [TipoSeguroId] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Descripcion] nvarchar(max) NULL,
    CONSTRAINT [PK_TiposSeguro] PRIMARY KEY ([TipoSeguroId])
);
GO

CREATE TABLE [Cotizaciones] (
    [CotizacionId] int NOT NULL IDENTITY,
    [NumeroCotizacion] nvarchar(30) NOT NULL,
    [FechaCotizacion] datetime2 NOT NULL,
    [TipoSeguroId] int NOT NULL,
    [ClienteId] int NOT NULL,
    [Moneda] nvarchar(10) NOT NULL,
    [DescripcionBien] nvarchar(max) NULL,
    [SumaAsegurada] decimal(18,2) NOT NULL,
    [Tasa] decimal(5,2) NOT NULL,
    CONSTRAINT [PK_Cotizaciones] PRIMARY KEY ([CotizacionId]),
    CONSTRAINT [FK_Cotizaciones_Clientes_ClienteId] FOREIGN KEY ([ClienteId]) REFERENCES [Clientes] ([ClienteId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Cotizaciones_TiposSeguro_TipoSeguroId] FOREIGN KEY ([TipoSeguroId]) REFERENCES [TiposSeguro] ([TipoSeguroId]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TipoSeguroId', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[TiposSeguro]'))
    SET IDENTITY_INSERT [TiposSeguro] ON;
INSERT INTO [TiposSeguro] ([TipoSeguroId], [Descripcion], [Nombre])
VALUES (1, N'Seguro médico general', N'Médico'),
(2, N'Seguro para vehículos', N'Automóvil'),
(3, N'Seguro contra incendios', N'Incendio'),
(4, N'Seguro de fianzas', N'Fianzas');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TipoSeguroId', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[TiposSeguro]'))
    SET IDENTITY_INSERT [TiposSeguro] OFF;
GO

CREATE UNIQUE INDEX [IX_Clientes_Identidad] ON [Clientes] ([Identidad]);
GO

CREATE INDEX [IX_Cotizaciones_ClienteId] ON [Cotizaciones] ([ClienteId]);
GO

CREATE INDEX [IX_Cotizaciones_FechaCotizacion] ON [Cotizaciones] ([FechaCotizacion]);
GO

CREATE UNIQUE INDEX [IX_Cotizaciones_NumeroCotizacion] ON [Cotizaciones] ([NumeroCotizacion]);
GO

CREATE INDEX [IX_Cotizaciones_TipoSeguroId] ON [Cotizaciones] ([TipoSeguroId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251128171455_InitialCreate', N'8.0.5');
GO

COMMIT;
GO

