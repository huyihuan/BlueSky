IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemOrganizationType]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemOrganizationType];

CREATE TABLE [SystemOrganizationType] ( 
	[Id] int identity(1,1)  NOT NULL,
	[ParentId] int NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	[Remark] text NULL
);

ALTER TABLE [SystemOrganizationType] ADD CONSTRAINT [PK_SystemOrganizationType_Id] 
	PRIMARY KEY CLUSTERED ([Id]);





