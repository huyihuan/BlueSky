IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('[SystemNotice]') AND  OBJECTPROPERTY(id, 'IsUserTable') = 1)
DROP TABLE [SystemNotice];

CREATE TABLE [SystemNotice] ( 
	[Id] int identity(1,1)  NOT NULL,    /* 系统编号 */
	[Title] nvarchar(1024) NOT NULL,    /* 公告标题 */
	[Content] nvarchar(max) NULL,    /* 公告内容 */
	[RangeType] int NOT NULL,    /* 通知范围类型 */
	[TargetObject] int NOT NULL,    /* 通知对象 */
	[BeginTime] datetime NOT NULL,    /* 公告时间 */
	[EndTime] datetime NOT NULL    /* 公告结束时间 */
);

ALTER TABLE [SystemNotice] ADD CONSTRAINT [PK_SystemNotice_Id] 
	PRIMARY KEY CLUSTERED ([Id]);

EXEC sp_addextendedproperty 'MS_Description', '系统公告', 'Schema', [dbo], 'table', [SystemNotice];
EXEC sp_addextendedproperty 'MS_Description', '系统编号', 'Schema', [dbo], 'table', [SystemNotice], 'column', [Id];

EXEC sp_addextendedproperty 'MS_Description', '公告标题', 'Schema', [dbo], 'table', [SystemNotice], 'column', [Title];

EXEC sp_addextendedproperty 'MS_Description', '公告内容', 'Schema', [dbo], 'table', [SystemNotice], 'column', [Content];

EXEC sp_addextendedproperty 'MS_Description', '通知范围类型', 'Schema', [dbo], 'table', [SystemNotice], 'column', [RangeType];

EXEC sp_addextendedproperty 'MS_Description', '通知对象', 'Schema', [dbo], 'table', [SystemNotice], 'column', [TargetObject];

EXEC sp_addextendedproperty 'MS_Description', '公告时间', 'Schema', [dbo], 'table', [SystemNotice], 'column', [BeginTime];

EXEC sp_addextendedproperty 'MS_Description', '公告结束时间', 'Schema', [dbo], 'table', [SystemNotice], 'column', [EndTime];

