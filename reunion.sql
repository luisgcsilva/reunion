CREATE DATABASE reunion

USE [reunion]

CREATE TABLE [dbo].[Categorias]
(
	[CategoriaId] INT IDENTITY (1, 1) NOT NULL, 
    [Descricao] NVARCHAR(MAX) NOT NULL,
	PRIMARY KEY CLUSTERED ([CategoriaId] ASC)
)

CREATE TABLE [dbo].[AdminGroups]
(
	[AdminGroupId] INT IDENTITY(1, 1) NOT NULL,
	[Grupo] NVARCHAR(MAX) NOT NULL,
	[SecurityGroup] NVARCHAR(MAX) NOT NULL,
	PRIMARY KEY CLUSTERED ([AdminGroupId] ASC),
)

CREATE TABLE [dbo].[Locais]
(
	[LocalId] INT IDENTITY (1, 1) NOT NULL, 
    [Descricao] NVARCHAR(MAX) NOT NULL,
	[IsActive] BIT NOT NULL,
	[AdminGroupId] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([LocalId] ASC),
	CONSTRAINT [FK_Locais_AdminGroup] FOREIGN KEY ([AdminGroupId]) REFERENCES [dbo].[AdminGroups] ([AdminGroupId]) ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE [dbo].[Materiais] (
    [MaterialId] INT IDENTITY (1, 1) NOT NULL,
    [Descricao] NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([MaterialId] ASC)
)

CREATE TABLE [dbo].[Salas] (
    [SalaId] INT IDENTITY (1, 1) NOT NULL,
	[LocalId] INT NOT NULL,
    [Descricao] NVARCHAR(MAX) NOT NULL,
    [Capacidade] INT NOT NULL,
    [Localizacao] NVARCHAR(MAX) NOT NULL,
    [IsActive] BIT NOT NULL,
    [Cor] NVARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([SalaId] ASC),
	CONSTRAINT [FK_Salas_Locais] FOREIGN KEY ([LocalId]) REFERENCES [dbo].[Locais] ([LocalId]) ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE [dbo].[SalaMateriais] (
    [SalaMateriaisId] INT IDENTITY (1, 1) NOT NULL,
    [SalaId] INT NOT NULL,
    [MaterialId] INT NOT NULL,
    [Quantidade] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([SalaMateriaisId] ASC),
    CONSTRAINT [FK_SalaMateriais_Salas] FOREIGN KEY ([SalaId]) REFERENCES [dbo].[Salas] ([SalaId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_SalaMateriais_Materiais] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Materiais] ([MaterialId]) ON DELETE CASCADE ON UPDATE CASCADE
)

CREATE TABLE [dbo].[Marcacoes] (
    [MarcacaoId] INT IDENTITY (1, 1) NOT NULL,
    [SalaId] INT NULL,
    [NumPessoas] INT NOT NULL,
	[LocalId] INT NOT NULL,
	[CategoriaId] INT NOT NULL,
    [Utilizador] NVARCHAR(MAX) NOT NULL,
    [Dia] DATE NOT NULL,
    [Hora_Inicio] TIME (7) NOT NULL,
    [Hora_Fim] TIME (7) NOT NULL,
    [Estado] NVARCHAR (10)  NOT NULL,
    [Observacoes] NVARCHAR(MAX) NULL,
    [DataRegisto] DATETIME NOT NULL,
    [ModificadoPor] NVARCHAR(MAX) NULL,
    [ModificadoEm]  DATETIME NULL,
    [Motivo] NVARCHAR(MAX) NULL,
    PRIMARY KEY CLUSTERED ([MarcacaoId] ASC),
    CONSTRAINT [FK_Marcacoes_Sala] FOREIGN KEY ([SalaId]) REFERENCES [dbo].[Salas] ([SalaId]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Marcacoes_Locais] FOREIGN KEY ([LocalId]) REFERENCES [dbo].[Locais] ([LocalId]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Marcacoes_Categoria] FOREIGN KEY ([CategoriaId]) REFERENCES [dbo].[Categorias] ([CategoriaId]) ON DELETE NO ACTION ON UPDATE NO ACTION
)

CREATE TABLE [dbo].[MarcacaoMateriais] (
    [MarcacaoMateriaisId] INT IDENTITY (1, 1) NOT NULL,
    [MarcacaoId] INT NOT NULL,
    [MaterialId] INT NOT NULL,
    [Quantidade] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([MarcacaoMateriaisId] ASC),
    CONSTRAINT [FK_MarcacaoMateriais_Marcacao] FOREIGN KEY ([MarcacaoId]) REFERENCES [dbo].[Marcacoes] ([MarcacaoId]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_MarcacaosMateriais_Material] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Materiais] ([MaterialId]) ON DELETE CASCADE ON UPDATE CASCADE
)

INSERT INTO AdminGroups (Grupo, SecurityGroup)
VALUES ('AdminHospital','AGENDAFORMACAO_perfilAdmin_hospital'),
		('AdminCA', 'AGENDAFORMACAO_perfilAdmin_CA'),
		('AdminSecCA', 'AGENDAFORMACAO_perfilAdmin_CA_secretariado'),
		('AdminAlcacer', 'AGENDAFORMACAO_perfilAdmin_alcacer'),
		('AdminGrandola', 'AGENDAFORMACAO_perfilAdmin_grandola'),
		('AdminOdemira', 'AGENDAFORMACAO_perfilAdmin_odemira'),
		('AdminSantiago', 'AGENDAFORMACAO_perfilAdmin_santiago'),
		('AdminSines', 'AGENDAFORMACAO_perfilAdmin_sines');


INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Hospital', 1, 1)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('CA', 0, 3)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Alcácer do Sal', 1, 4)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Grândola', 1, 5)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Santiago do Cacém', 1, 7)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Sines', 1, 8)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Odemira', 1, 6)
INSERT INTO [dbo].[Locais] ([Descricao], [IsActive], [AdminGroupId]) VALUES ('Vila Nova de Santo André', 1, 7)

INSERT INTO Salas (LocalId, Descricao, Capacidade, Localizacao, IsActive, Cor)
VALUES (1, 'Pendente', 1, 'N/A', 'False', '#000000'),
		(2, 'Sala de Reuniões do CA', 1, 'CA', 'False', '#0062ff');

INSERT INTO Categorias (Descricao)
VALUES ('Reunião'),
		('Formação');