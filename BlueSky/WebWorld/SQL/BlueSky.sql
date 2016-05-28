--功能表
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

insert into FunctionItem(Name,Value,Tip,ParentId) values('测试功能树','FunctionControls/CodeTest.ascx','测试功能树',-1);
insert into FunctionItem(Name,Value,Tip,ParentId) values('系统管理','#','系统管理',-1);
insert into FunctionItem(Name,Value,Tip,ParentId) values('功能管理','FunctionControls/SystemManage/FunctionsManage.ascx','功能管理',2);
insert into FunctionItem(Name,Value,Tip,ParentId) values('角色管理','#','角色管理',2);
insert into FunctionItem(Name,Value,Tip,ParentId) values('角色权限','FunctionControls/SystemManage/FunctionsManage.ascx','角色权限',4);
insert into FunctionItem(Name,Value,Tip,ParentId) values('百度','http://www.baidu.com','百度搜索',4);
insert into FunctionItem(Name,Value,Tip,ParentId) values('测试链接','http://www.baidu.com','测试链接',3);


alter table FunctionItem add Width int,Height int,IsResize int,IsToMove int,IsIncludeMinBox int,IsIncludeMaxBox int,IsShowInTaskBar int;
alter table FunctionItem add Target nvarchar(32);
alter table FunctionItem add Level int not null default 0;
SET IDENTITY_INSERT FunctionItem ON;

--用户表
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

--设置表
Drop table "SettingItem";
create TABLE "SettingItem"
(
	"Id" int PRIMARY key identity(1,1),
	"Key"	text,
	"Value"	text,
)

--角色表
create table RoleItem
(
	Id int PRIMARY key identity(1,1),
	RoleName nvarchar(255),
	Remark text
)
--用户角色表
create table UserRoleItem
(
	Id int PRIMARY key identity(1,1),
	UserItemId int not null,
	RoleItemId int not null
)
--角色权限表
create table RoleFunctionItem
(
	Id int PRIMARY key identity(1,1),
	RoleItemId int not null,
	FunctionItemId int not null
)
--操作表
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
--角色Action
create table RoleActionItem
(
	Id int PRIMARY key identity(1,1),
	RoleItemId int not null,
	FunctionItemId int not null,
	ActionItemId int not null
)



--班级表
create table ClassItem
(
	Id int PRIMARY key identity(1,1),
	ClassName nvarchar(255) not null,
	StudentNumber int
)
--学生表
create table StudentItem
(
	Id int PRIMARY key identity(1,1),
	ClassItemId int not null,
	Name nvarchar(255) not null,
	Age int
)