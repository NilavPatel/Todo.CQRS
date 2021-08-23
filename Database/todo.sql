/****** Todo ******/
USE [master]
Go
CREATE DATABASE [Todo];
GO
USE [Todo]
GO
CREATE TABLE [dbo].[TodoItems](
	[Id] [uniqueidentifier] PRIMARY KEY,
	[Version] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[IsComplete] [bit] NOT NULL
)
GO