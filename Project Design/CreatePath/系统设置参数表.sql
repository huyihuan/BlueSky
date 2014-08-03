IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemSetting]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemSetting];

CREATE TABLE [SystemSetting] ( 
	[Id] int NOT NULL,
	[Key] nvarchar(255) NOT NULL,
	[Value] nvarchar(1024) NULL,
	[UserId] int NULL,
	[Remark] text NULL
);

ALTER TABLE [SystemSetting] ADD CONSTRAINT [PK_SystemSetting_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemSetting] ADD CONSTRAINT [FK_SystemSetting_UserInformation_Id] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);

EXEC sp_addextendedproperty 'MS_Description', '系统设置参数表', 'Schema', [dbo], 'table', [SystemSetting];





