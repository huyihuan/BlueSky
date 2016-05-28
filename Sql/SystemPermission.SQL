IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserActionPermission_UserInformation]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserActionPermission] DROP CONSTRAINT [FK_SystemUserActionPermission_UserInformation];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserActionPermission_SystemAction]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserActionPermission] DROP CONSTRAINT [FK_SystemUserActionPermission_SystemAction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserFunctionPermission_UserInformation]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserFunctionPermission] DROP CONSTRAINT [FK_SystemUserFunctionPermission_UserInformation];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserFunctionPermission_SystemFunction]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserFunctionPermission] DROP CONSTRAINT [FK_SystemUserFunctionPermission_SystemFunction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemRoleActionPermission_SystemRole]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemRoleActionPermission] DROP CONSTRAINT [FK_SystemRoleActionPermission_SystemRole];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemRoleActionPermission_SystemAction]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemRoleActionPermission] DROP CONSTRAINT [FK_SystemRoleActionPermission_SystemAction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemRoleFunctionPermission_SystemRole]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemRoleFunctionPermission] DROP CONSTRAINT [FK_SystemRoleFunctionPermission_SystemRole];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemRoleFunctionPermission_SystemFunction]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemRoleFunctionPermission] DROP CONSTRAINT [FK_SystemRoleFunctionPermission_SystemFunction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserModulePermission_UserInformation]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserModulePermission] DROP CONSTRAINT [FK_SystemUserModulePermission_UserInformation];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserModulePermission_SystemModule]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserModulePermission] DROP CONSTRAINT [FK_SystemUserModulePermission_SystemModule];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemRoleModulePermission_SystemRole]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemRoleModulePermission] DROP CONSTRAINT [FK_SystemRoleModulePermission_SystemRole];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemRoleModulePermission_SystemModule]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemRoleModulePermission] DROP CONSTRAINT [FK_SystemRoleModulePermission_SystemModule];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserRole_SystemRole]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserRole] DROP CONSTRAINT [FK_SystemUserRole_SystemRole];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemUserRole_UserInformation]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemUserRole] DROP CONSTRAINT [FK_SystemUserRole_UserInformation];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemAction_SystemFunction]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemAction] DROP CONSTRAINT [FK_SystemAction_SystemFunction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemFunction_SystemModule]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemFunction] DROP CONSTRAINT [FK_SystemFunction_SystemModule];



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemUserActionPermission]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemUserActionPermission];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemUserFunctionPermission]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemUserFunctionPermission];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemRoleActionPermission]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemRoleActionPermission];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemRoleFunctionPermission]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemRoleFunctionPermission];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemUserModulePermission]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemUserModulePermission];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemRoleModulePermission]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemRoleModulePermission];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemUserRole]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemUserRole];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemRole]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemRole];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[UserInformation]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [UserInformation];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemAction]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemAction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemFunction]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemFunction];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemModule]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemModule];


CREATE TABLE [SystemUserActionPermission] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserId] int NOT NULL,
	[ActionId] int NOT NULL,
	[FunctionId] int NOT NULL
);

CREATE TABLE [SystemUserFunctionPermission] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserId] int NOT NULL,
	[FunctionId] int NOT NULL
);

CREATE TABLE [SystemRoleActionPermission] ( 
	[Id] int identity(1,1)  NOT NULL,
	[RoleId] int NOT NULL,
	[ActionId] int NOT NULL,
	[FunctionId] int NOT NULL
);

CREATE TABLE [SystemRoleFunctionPermission] ( 
	[Id] int identity(1,1)  NOT NULL,
	[RoleId] int NOT NULL,
	[FunctionId] int NOT NULL
);

CREATE TABLE [SystemUserModulePermission] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserId] int NOT NULL,
	[ModuleId] int NOT NULL
);

CREATE TABLE [SystemRoleModulePermission] ( 
	[Id] int identity(1,1)  NOT NULL,
	[RoleId] int NOT NULL,
	[ModuleId] int NOT NULL
);

CREATE TABLE [SystemUserRole] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserId] int NOT NULL,
	[RoleId] int NOT NULL
);

CREATE TABLE [SystemRole] ( 
	[Id] int identity(1,1)  NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Description] text NULL
);

CREATE TABLE [UserInformation] ( 
	[Id] int identity(1,1)  NOT NULL,
	[UserName] nvarchar(255) NOT NULL,
	[Password] nvarchar(255) NOT NULL,
	[Nickname] nvarchar(255) NOT NULL,
	[Gender] int NULL,
	[CardID] nvarchar(255) NULL,
	[Age] int NULL,
	[Email] nvarchar(255) NULL,
	[PostCode] varchar(6) NULL,
	[MSN] nvarchar(255) NULL,
	[Country] nvarchar(255) NULL,
	[QQ] nvarchar(255) NULL
);

CREATE TABLE [SystemAction] ( 
	[Id] int identity(1,1)  NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Key] nvarchar(255) NOT NULL,
	[ActionType] nvarchar(255) NOT NULL,
	[ActionValue] text NULL,
	[Description] text NULL,
	[IsDefault] int NOT NULL,
	[FunctionId] int NOT NULL,
	[ControlName] nvarchar(255) NULL,
	[Tip] text NULL,
	[IconName] nvarchar(255) NULL,
	[Target] nvarchar(255) NULL,
	[IsPopup] int NOT NULL,
	[Width] int NULL,
	[Height] int NULL,
	[IsResize] int NULL,
	[IsMove] int NULL,
	[IsIncludeMinBox] int NULL,
	[IsIncludeMaxBox] int NULL,
	[IsShowInTaskBar] int NULL,
	[EntityCount] int NULL,
	[OrderId] int NULL
);

CREATE TABLE [SystemFunction] ( 
	[Id] int identity(1,1)  NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Key] nvarchar(255) NOT NULL,
	[ModuleId] int NOT NULL,
	[ParentId] int NOT NULL,
	[IconName] nvarchar(255) NULL,
	[Description] text NULL,
	[Level] int NOT NULL,
	[OrderId] int NULL
);

CREATE TABLE [SystemModule] ( 
	[Id] int identity(1,1)  NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Key] nvarchar(255) NOT NULL,
	[Controller] nvarchar(255) NOT NULL,
	[Description] text NULL,
	[OrderId] int NULL,
	[IconName] nvarchar(255) NULL
);


ALTER TABLE [SystemUserActionPermission] ADD CONSTRAINT [PK_SystemUserActionPermission_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemUserFunctionPermission] ADD CONSTRAINT [PK_SystemUserFunctionPermission_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemRoleActionPermission] ADD CONSTRAINT [PK_SystemRoleActionPermission_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemRoleFunctionPermission] ADD CONSTRAINT [PK_SystemRoleFunctionPermission_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemUserModulePermission] ADD CONSTRAINT [PK_SystemUserModulePermission_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemRoleModulePermission] ADD CONSTRAINT [PK_SystemRoleModulePermission_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemUserRole] ADD CONSTRAINT [PK_SystemUserRole_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemRole] ADD CONSTRAINT [PK_SystemRole_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [UserInformation] ADD CONSTRAINT [PK_UserInformation_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemAction] ADD CONSTRAINT [PK_SystemAction_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemFunction] ADD CONSTRAINT [PK_SystemFunction_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemModule] ADD CONSTRAINT [PK_SystemModule_Id] 
	PRIMARY KEY CLUSTERED ([Id]);


ALTER TABLE [SystemRole]
	ADD CONSTRAINT [UQ_SystemRole_Name] UNIQUE ([Name]);

ALTER TABLE [UserInformation]
	ADD CONSTRAINT [UQ_UserInformation_UserName] UNIQUE ([UserName]);

ALTER TABLE [SystemAction]
	ADD CONSTRAINT [UQ_SystemAction_Key] UNIQUE ([Key]);

ALTER TABLE [SystemFunction]
	ADD CONSTRAINT [UQ_SystemFunction_Key] UNIQUE ([Key]);

ALTER TABLE [SystemModule]
	ADD CONSTRAINT [UQ_SystemModule_Key] UNIQUE ([Key]);


ALTER TABLE [SystemUserActionPermission] ADD CONSTRAINT [FK_SystemUserActionPermission_UserInformation] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);

ALTER TABLE [SystemUserActionPermission] ADD CONSTRAINT [FK_SystemUserActionPermission_SystemAction] 
	FOREIGN KEY ([ActionId]) REFERENCES [SystemAction] ([Id]);

ALTER TABLE [SystemUserFunctionPermission] ADD CONSTRAINT [FK_SystemUserFunctionPermission_UserInformation] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);

ALTER TABLE [SystemUserFunctionPermission] ADD CONSTRAINT [FK_SystemUserFunctionPermission_SystemFunction] 
	FOREIGN KEY ([FunctionId]) REFERENCES [SystemFunction] ([Id]);

ALTER TABLE [SystemRoleActionPermission] ADD CONSTRAINT [FK_SystemRoleActionPermission_SystemRole] 
	FOREIGN KEY ([RoleId]) REFERENCES [SystemRole] ([Id]);

ALTER TABLE [SystemRoleActionPermission] ADD CONSTRAINT [FK_SystemRoleActionPermission_SystemAction] 
	FOREIGN KEY ([ActionId]) REFERENCES [SystemAction] ([Id]);

ALTER TABLE [SystemRoleFunctionPermission] ADD CONSTRAINT [FK_SystemRoleFunctionPermission_SystemRole] 
	FOREIGN KEY ([RoleId]) REFERENCES [SystemRole] ([Id]);

ALTER TABLE [SystemRoleFunctionPermission] ADD CONSTRAINT [FK_SystemRoleFunctionPermission_SystemFunction] 
	FOREIGN KEY ([FunctionId]) REFERENCES [SystemFunction] ([Id]);

ALTER TABLE [SystemUserModulePermission] ADD CONSTRAINT [FK_SystemUserModulePermission_UserInformation] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);

ALTER TABLE [SystemUserModulePermission] ADD CONSTRAINT [FK_SystemUserModulePermission_SystemModule] 
	FOREIGN KEY ([ModuleId]) REFERENCES [SystemModule] ([Id]);

ALTER TABLE [SystemRoleModulePermission] ADD CONSTRAINT [FK_SystemRoleModulePermission_SystemRole] 
	FOREIGN KEY ([RoleId]) REFERENCES [SystemRole] ([Id]);

ALTER TABLE [SystemRoleModulePermission] ADD CONSTRAINT [FK_SystemRoleModulePermission_SystemModule] 
	FOREIGN KEY ([ModuleId]) REFERENCES [SystemModule] ([Id]);

ALTER TABLE [SystemUserRole] ADD CONSTRAINT [FK_SystemUserRole_SystemRole] 
	FOREIGN KEY ([RoleId]) REFERENCES [SystemRole] ([Id]);

ALTER TABLE [SystemUserRole] ADD CONSTRAINT [FK_SystemUserRole_UserInformation] 
	FOREIGN KEY ([UserId]) REFERENCES [UserInformation] ([Id]);

ALTER TABLE [SystemAction] ADD CONSTRAINT [FK_SystemAction_SystemFunction] 
	FOREIGN KEY ([FunctionId]) REFERENCES [SystemFunction] ([Id]);

ALTER TABLE [SystemFunction] ADD CONSTRAINT [FK_SystemFunction_SystemModule] 
	FOREIGN KEY ([ModuleId]) REFERENCES [SystemModule] ([Id]);
