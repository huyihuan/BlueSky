IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[Music]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [Music];

CREATE TABLE [Music] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserId] int NOT NULL,
	[MusicName] nvarchar(255) NOT NULL,
	[MusicURL] nvarchar(1024) NOT NULL,
	[MusicType] nvarchar(32) NOT NULL
);

ALTER TABLE [Music] ADD CONSTRAINT [PK_Music_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [Music] ADD CONSTRAINT [FK_Music_UserInformation_Id] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);






