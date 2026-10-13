/* Ejecutar UNA vez sobre TallerMecanicoDB, después de Taller_SQLServer2022_Schemas.sql.
   La API necesita esta columna para autenticar usuarios (login con JWT). */
USE TallerMecanicoDB;
GO
IF COL_LENGTH(N'seguridad.Usuario', N'PasswordHash') IS NULL
    ALTER TABLE seguridad.Usuario ADD PasswordHash NVARCHAR(500) NULL;
GO
