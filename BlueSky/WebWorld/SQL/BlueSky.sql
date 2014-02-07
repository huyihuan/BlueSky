--���ܱ�
Drop table "FunctionItem";
create TABLE "FunctionItem"
(
	"Id" int PRIMARY key identity(1,1),
	"Name" varchar(255),
	"Value" text,
	"Tip" text,
	"ParentId" int,
	"IconName" nvarchar(255),
	"Description" text
)

insert into FunctionItem(Name,Value,Tip,ParentId) values('���Թ�����','FunctionControls/CodeTest.ascx','���Թ�����',-1);
insert into FunctionItem(Name,Value,Tip,ParentId) values('ϵͳ����','#','ϵͳ����',-1);
insert into FunctionItem(Name,Value,Tip,ParentId) values('���ܹ���','FunctionControls/SystemManage/FunctionsManage.ascx','���ܹ���',2);
insert into FunctionItem(Name,Value,Tip,ParentId) values('��ɫ����','#','��ɫ����',2);
insert into FunctionItem(Name,Value,Tip,ParentId) values('��ɫȨ��','FunctionControls/SystemManage/FunctionsManage.ascx','��ɫȨ��',4);
insert into FunctionItem(Name,Value,Tip,ParentId) values('�ٶ�','http://www.baidu.com','�ٶ�����',4);
insert into FunctionItem(Name,Value,Tip,ParentId) values('��������','http://www.baidu.com','��������',3);


alter table FunctionItem add Width int,Height int,IsResize int,IsToMove int,IsIncludeMinBox int,IsIncludeMaxBox int,IsShowInTaskBar int;
alter table FunctionItem add Target nvarchar(32);
alter table FunctionItem add Level int not null default 0;
SET IDENTITY_INSERT FunctionItem ON;

--�û���
Drop table "UserItem";
create TABLE "UserItem"
(
	"Id" int PRIMARY key identity(1,1),
	"UserName"	nvarchar(255),
	"Password"	nvarchar(255),
	"NickName"	nvarchar(255),
	"Gender"	nvarchar(1),
	"IDCard"	nvarchar(32),
	"Tel"		nvarchar(64),
	"Address"	text,
	"Email"		nvarchar(255),
	"Post"		nvarchar(255),
	"QQ"		nvarchar(32),
	"MSN"		nvarchar(64)
)

--���ñ�
Drop table "SettingItem";
create TABLE "SettingItem"
(
	"Id" int PRIMARY key identity(1,1),
	"Key"	text,
	"Value"	text,
)

--��ɫ��
create table RoleItem
(
	Id int PRIMARY key identity(1,1),
	RoleName nvarchar(255),
	Remark text
)
--�û���ɫ��
create table UserRoleItem
(
	Id int PRIMARY key identity(1,1),
	UserItemId int not null,
	RoleItemId int not null
)
--��ɫȨ�ޱ�
create table RoleFunctionItem
(
	Id int PRIMARY key identity(1,1),
	RoleItemId int not null,
	FunctionItemId int not null
)
--������
drop table ActionItem
create table ActionItem
(
	Id int PRIMARY key identity(1,1),
	FunctionItemId int not null,
	ActionName nvarchar(255) not null,
	ActionKey nvarchar(255) not null,
	ControlName nvarchar(255) not null,
	ActionIcon nvarchar(255)
)
--��ɫAction
create table RoleActionItem
(
	Id int PRIMARY key identity(1,1),
	RoleItemId int not null,
	FunctionItemId int not null,
	ActionItemId int not null
)



--�༶��
create table ClassItem
(
	Id int PRIMARY key identity(1,1),
	ClassName nvarchar(255) not null,
	StudentNumber int
)
--ѧ����
create table StudentItem
(
	Id int PRIMARY key identity(1,1),
	ClassItemId int not null,
	Name nvarchar(255) not null,
	Age int
)