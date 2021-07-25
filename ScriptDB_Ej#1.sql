CREATE DATABASE ExamenIngreso
GO

CREATE TABLE GPS_DATA(
	[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[DateSystem] [datetime] NOT NULL,
	[DateEvent] [datetime] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[Battery] [int] NULL,
	[Source] [int] NULL,
	[Type] [int] NULL
)

GO

CREATE PROC Get_GPSData
	@Id		INT = NULL
AS
BEGIN
	SELECT * FROM GPS_DATA WITH(NOLOCK)
	WHERE ISNULL(@Id, 0) <= 0 OR (ISNULL(@Id, 0) = Id)
END

GO

CREATE PROC Insert_GPSData
	@DateSystem		DATETIME,
	@DateEvent		DATETIME = NULL,
	@Latitude		FLOAT	= NULL,
	@Longitude		FLOAT	= NULL,
	@Battery		INT		= NULL,
	@Source			INT		= NULL,
	@Type			INT		= NULL,
	@Id				INT	OUTPUT
AS
BEGIN

	BEGIN TRY
		INSERT INTO GPS_DATA (
			DateSystem,	
			DateEvent,	
			Latitude,	
			Longitude,	
			Battery,	
			Source,
			Type	
		) VALUES (
			@DateSystem	,
			@DateEvent,
			@Latitude,
			@Longitude,	
			@Battery,
			@Source,	
			@Type	
		)

		SET @Id = (SELECT SCOPE_IDENTITY())
		RETURN
	END TRY
	BEGIN CATCH
		RAISERROR('No se pudo insertar registro.',16, 1)
		RETURN
	END CATCH
END
GO

CREATE PROC Delete_GPSData
	@Id		INT
AS
BEGIN

	BEGIN TRY
		IF NOT EXISTS(SELECT TOP 1 1 FROM GPS_DATA
				WHERE Id = @Id)
				RAISERROR('No se pudo borrar el registro deseado.',16, 1)

		DELETE GPS_DATA
		WHERE Id = @Id
	END TRY
	BEGIN CATCH
		RAISERROR('No se pudo borrar el registro deseado.',16, 1)
		RETURN
	END CATCH

END
GO

CREATE PROC Update_GPSData
	@Id				INT,
	@DateSystem		DATETIME,
	@DateEvent		DATETIME	= NULL,
	@Latitude		FLOAT		= NULL,
	@Longitude		FLOAT		= NULL,
	@Battery		INT			= NULL,
	@Source			INT			= NULL,
	@Type			INT			= NULL,
	@Response		INT OUTPUT
AS
BEGIN

	BEGIN TRY
		IF NOT EXISTS(SELECT TOP 1 1 FROM GPS_DATA
				WHERE Id = @Id)
		BEGIN
			SET @Response = 0
			RETURN 
		END

		UPDATE GPS_DATA SET
			DateSystem	= @DateSystem,	
			DateEvent	= @DateEvent,	
			Latitude	= @Latitude,	
			Longitude	= @Longitude,	
			Battery		= @Battery,	
			Source		= @Source,
			Type		= @Type
		WHERE Id = @Id

		SET @Response = 1
		RETURN
	END TRY
	BEGIN CATCH
		RAISERROR('No se pudo actualizar el registro', 16, 1)
		RETURN
	END CATCH
END
