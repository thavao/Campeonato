CREATE DATABASE Campeonato
GO
USE Campeonato
GO

CREATE TABLE Time(
	Id int IDENTITY(1,1),
	Nome VARCHAR (40) not NULL,
	Apelido VARCHAR (30),
	DataCriacao date,
	Pontuacao int,
	MaxGolPartida int,
	TotalGol int,
	GolRecebido int,
	CONSTRAINT PK_Time PRIMARY KEY (Id)
);
GO

CREATE TABLE Partida(
	Id INT IDENTITY(1,1),
	TimeCasa INT NOT NULL,
	TimeVisitante INT NOT NULL,
	"Data" Date,
	GolCasa INT,
	GolVisitante INT,
	TotalGol INT,
	Vencedor INT,
	CONSTRAINT PK_Partida PRIMARY KEY (Id),
	CONSTRAINT FK_Partida_TimeCasa FOREIGN KEY (TimeCasa) REFERENCES Time (Id),
	CONSTRAINT FK_Partida_TimeVisitante FOREIGN KEY (TimeVisitante) REFERENCES Time (Id)
);
GO

CREATE PROCEDURE Inserir_Time
	@Nome VARCHAR (40),
	@Apelido VARCHAR(30),
	@DataCriacao DATE
AS
	BEGIN
		INSERT INTO Time
		VALUES(@Nome, @Apelido, @DataCriacao, 0, 0, 0, 0)
	END
GO

CREATE PROC Atualizar_Time
	@Id INT,
	@GolsFeitos INT,
	@GolsRecebidos INT,
	@NovosPontos INT
	AS
	BEGIN
		DECLARE
		@Pontos INT,
		@MaxGolPartida INT,
		@TotalGolsFeitos INT,
		@TotalGolsRecebidos INT

		SELECT 
			@Pontos = Pontuacao,
			@MaxGolPartida = MaxGolPartida,
			@TotalGolsFeitos = TotalGol,
			@TotalGolsRecebidos = GolRecebido
		 FROM [Time] WHERE Id = @Id

		SET @Pontos += @NovosPontos
		SET @TotalGolsFeitos += @GolsFeitos
		SET @TotalGolsRecebidos += @GolsRecebidos
		
		IF (@GolsFeitos > @MaxGolPartida)
		SET	@MaxGolPartida = @GolsFeitos
		
		UPDATE Time SET 
		Pontuacao = @Pontos,
		MaxGolPartida = @MaxGolPartida,
		TotalGol = @TotalGolsFeitos,
		GolRecebido = @TotalGolsRecebidos
		WHERE Id = @Id;
	END
GO

CREATE PROCEDURE Inserir_Partida
	@TimeCasa int,
	@TimeVisitante int,
	@Data date,
	@GolCasa int,
	@GolVisitante int
	AS
	BEGIN
	DECLARE 
		@Vencedor int,
		@TotalGol int,
		@PontuacaoTimeCasa INT,
		@PontuacaoTimeVisitante INT
	
	SET @TotalGol = @GolCasa + @GolVisitante
	SET @PontuacaoTimeCasa = 0
	SET @PontuacaoTimeVisitante = 0

	IF (@GolCasa > @GolVisitante)
		SET @Vencedor = @TimeCasa
	ELSE IF (@GolVisitante > @GolCasa)
		SET @Vencedor = @TimeVisitante
	ELSE
		SET @Vencedor = 0
	
	IF(@Vencedor = 0)
	BEGIN
		SET @PontuacaoTimeCasa = 1
		SET @PontuacaoTimeVisitante = 1
	END
	ELSE IF(@Vencedor = @TimeCasa)
		SET @PontuacaoTimeCasa = 3
	ELSE 
		SET @PontuacaoTimeVisitante = 5

	INSERT INTO Partida VALUES (@TimeCasa, @TimeVisitante, @Data, @GolCasa, @GolVisitante, @TotalGol, @Vencedor)
	
	EXEC Atualizar_Time @TimeCasa, @GolCasa, @GolVisitante, @PontuacaoTimeCasa
	EXEC Atualizar_Time @TimeVisitante, @GolVisitante, @GolCasa, @PontuacaoTimeVisitante
	END
GO

CREATE PROC Time_Mais_Goleou
AS
BEGIN
	DECLARE
		@TopQtdGols INT

	SELECT TOP 1 @TopQtdGols = TotalGol FROM [Time] ORDER BY TotalGol DESC;

	SELECT Nome, TotalGol 
	FROM [Time] 
	WHERE TotalGol = @TopQtdGols
	RETURN
END
GO

CREATE PROC Time_Mais_Goleado
AS
BEGIN
	DECLARE
		@TopQtdGolsRecebidos INT

	SELECT TOP 1 @TopQtdGolsRecebidos = GolRecebido FROM [Time] ORDER BY GolRecebido DESC;

	SELECT Nome, GolRecebido 
	FROM [Time] 
	WHERE GolRecebido = @TopQtdGolsRecebidos
	RETURN
END
GO

CREATE PROC Partida_Com_Mais_Gols
AS
BEGIN
	DECLARE @RecordeGolPartida INT

	SELECT TOP 1 @RecordeGolPartida = TotalGol FROM Partida ORDER by TotalGol DESC;

SELECT
	t1.Nome as Casa,
	GolCasa, 
	t2.Nome as Visitante,
	GolVisitante,
	p.TotalGol as 'Total de gols',
	[Data],
	v.Nome as Vencedor
	FROM Partida p JOIN Time t1 ON p.TimeCasa = t1.Id
	JOIN [Time] t2 on p.TimeVisitante = t2.Id
	JOIN [Time] v on p.Vencedor = v.Id
	WHERE p.TotalGol = @RecordeGolPartida
	ORDER BY p.TotalGol DESC
	RETURN
END
GO