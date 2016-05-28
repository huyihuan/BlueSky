IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemLog]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemLog];

CREATE TABLE [SystemLog] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserId] int NOT NULL,
	[AccessTime] datetime NOT NULL,
	[AccessFunctionName] nvarchar(255) NULL,
	[AccessActionName] nvarchar(255) NULL,
	[AccessURL] nvarchar(1024) NULL,
	[Remark] text NULL
);

ALTER TABLE [SystemLog] ADD CONSTRAINT [PK_SystemLog_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemLog] ADD CONSTRAINT [FK_SystemLog_UserInfomation_Id] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);

EXEC sp_addextendedproperty 'MS_Description', '系统日志表,记录用户在系统的操作足迹', 'Schema', [dbo], 'table', [SystemLog];







