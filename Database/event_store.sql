/****** EventStore ******/
USE [master]
GO
CREATE DATABASE [EventStore]
GO
USE [EventStore]
GO
CREATE TABLE [dbo].[Events](
	[Id] [uniqueidentifier] PRIMARY KEY,
	[AggregateId] [uniqueidentifier] NOT NULL,
	[AggregateVersion] [int] NOT NULL,
	[AggregateName] [nvarchar](max) NOT NULL,
	[EventName] [nvarchar](max) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[OccuredOn] [datetimeoffset](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE INDEX IX_Events_AggregateId
   ON Events (AggregateId ASC);
GO