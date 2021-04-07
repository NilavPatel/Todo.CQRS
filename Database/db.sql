/****** EventStore ******/
GO
CREATE DATABASE EventStore;
USE [EventStore]
GO
CREATE TABLE [dbo].[Events](
	[Id] [uniqueidentifier] NOT NULL,
	[AggregateName] [nvarchar](max) NOT NULL,
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateVersion] [int] NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[EventName] [nvarchar](max) NOT NULL,
	[EventFullName] [varbinary](max) NOT NULL,
	[TimeStamp] [datetimeoffset](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Todo ******/
CREATE DATABASE Todo;
GO
USE [Todo]
GO
CREATE TABLE [dbo].[TodoItems](
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [int] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[IsComplete] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

