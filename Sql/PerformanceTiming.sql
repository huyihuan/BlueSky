USE [BlueSkyPerformance]
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[PerformanceTiming]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [PerformanceTiming];
GO
CREATE table [PerformanceTiming]
(
	[Id] int identity(1,1) primary key,
	[ConnectTime] int,
	[LoadEventTime] int,
	[DomainLookupTime] int,
	[RequestTime] int,
	[ResponseTime] int,
	[DomInitTime] int,
	[DomReadyTime] int,
	[TotalTime] int,
	[IP] nvarchar(128) not null,
	[URL] nvarchar(1024) not null,
	[UserLanguages] nvarchar(64) not null,
	[CreateTime] datetime default getdate()
)