IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[FK_SystemOrganization_SystemOrganizationType_TypeId]') AND OBJECTPROPERTY(id, 'IsForeignKey') = 1)
ALTER TABLE [SystemOrganization] DROP CONSTRAINT [FK_SystemOrganization_SystemOrganizationType_TypeId];



IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemOrganizationType]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemOrganizationType];

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemOrganization]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemOrganization];


CREATE TABLE [SystemOrganizationType] ( 
	[Id] int identity(1,1)  NOT NULL,
	[ParentId] int NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Remark] text NULL
);

CREATE TABLE [SystemOrganization] ( 
	[Id] int identity(1,1)  NOT NULL,
	[ParentId] int NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[TypeId] int NOT NULL,
	[Remark] ntext NULL
);


ALTER TABLE [SystemOrganizationType] ADD CONSTRAINT [PK_SystemOrganizationType_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

ALTER TABLE [SystemOrganization] ADD CONSTRAINT [PK_SystemOrganization_Id] 
	PRIMARY KEY CLUSTERED ([Id]);



ALTER TABLE [SystemOrganization] ADD CONSTRAINT [FK_SystemOrganization_SystemOrganizationType_TypeId] 
	FOREIGN KEY ([TypeId]) REFERENCES [SystemOrganizationType] ([Id]);













EXEC sp_addextendedproperty 'MS_Description', '组织机构表', 'Schema', [dbo], 'table', [SystemOrganization];
