IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemNotice]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemNotice];

CREATE TABLE [SystemNotice] ( 
	[Id] int identity(1,1)  NOT NULL,    /* ϵͳ��� */
	[Title] nvarchar(1024) NOT NULL,    /* ������� */
	[Content] nvarchar(max) NULL,    /* �������� */
	[RangeType] int NOT NULL,    /* ֪ͨ��Χ���� */
	[TargetObject] int NOT NULL,    /* ֪ͨ���� */
	[BeginTime] datetime NOT NULL,    /* ����ʱ�� */
	[EndTime] datetime NOT NULL    /* �������ʱ�� */
);

ALTER TABLE [SystemNotice] ADD CONSTRAINT [PK_SystemNotice_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

EXEC sp_addextendedproperty 'MS_Description', 'ϵͳ����', 'Schema', [dbo], 'table', [SystemNotice];
EXEC sp_addextendedproperty 'MS_Description', 'ϵͳ���', 'Schema', [dbo], 'table', [SystemNotice], 'column', [Id];

EXEC sp_addextendedproperty 'MS_Description', '�������', 'Schema', [dbo], 'table', [SystemNotice], 'column', [Title];

EXEC sp_addextendedproperty 'MS_Description', '��������', 'Schema', [dbo], 'table', [SystemNotice], 'column', [Content];

EXEC sp_addextendedproperty 'MS_Description', '֪ͨ��Χ����', 'Schema', [dbo], 'table', [SystemNotice], 'column', [RangeType];

EXEC sp_addextendedproperty 'MS_Description', '֪ͨ����', 'Schema', [dbo], 'table', [SystemNotice], 'column', [TargetObject];

EXEC sp_addextendedproperty 'MS_Description', '����ʱ��', 'Schema', [dbo], 'table', [SystemNotice], 'column', [BeginTime];

EXEC sp_addextendedproperty 'MS_Description', '�������ʱ��', 'Schema', [dbo], 'table', [SystemNotice], 'column', [EndTime];

