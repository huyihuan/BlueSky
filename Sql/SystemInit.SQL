--初始化模块表
delete from SystemAction;
delete from SystemFunction;
delete from SystemModule;

SET IDENTITY_INSERT [SystemModule] ON;
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(1,'系统管理','SystemManage','WebWorld.SystemManage','系统基础管理模块，必须初始化!');
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(2,'联系人管理','ContactsManage','WebWorld.Modules.ContactsManage','联系人管理');
SET IDENTITY_INSERT [SystemModule] OFF;

--初始化系统功能表
SET IDENTITY_INSERT [SystemFunction] ON;
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(1,'系统管理','SystemManage',1,-1,'','',1);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(2,'系统模块管理','SystemModuleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(3,'系统功能管理','SystemFunctionManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(4,'系统角色管理','SystemRoleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(5,'系统用户管理','SystemUserManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(6,'联系人管理','ContactsManage',2,-1,'','',1);
SET IDENTITY_INSERT [SystemFunction] OFF;

--初始化Action表
SET IDENTITY_INSERT [SystemAction] ON;
--模块管理Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(1,'模块列表','ModuleList',1,'','',1,2,'ModuleList',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(2,'新增模块','ModuleAdd',1,'','',2,2,'ModuleAdd',1,'top',600,400,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(3,'修改模块','ModuleEdit',1,'','',2,2,'ModuleAdd',1,'top',600,400,1,1,2,1,1,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(4,'删除','ModuleDelete',2,'moduleDelete','',2,2,'ModuleList',2,'top',600,400,1,1,2,1,1,-1,'delete.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(5,'配置模块','ModuleSet',1,'','',2,2,'ModuleUpdate',1,'top',600,400,1,1,2,1,1,1,'refresh.png');


--用户管理Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(6,'用户列表','UserList',1,'','',1,5,'UserList',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(7,'新增','UserAdd',1,'','',2,5,'UserAdd',1,'top',500,600,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(8,'修改用户','UserEdit',1,'','',2,5,'UserAdd',1,'top',500,600,2,2,2,2,2,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(9,'设置用户权限','UserFunction',1,'','',2,5,'SetUserFunction',1,'top',500,600,2,2,2,2,2,1,'role_function.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(10,'删除','UserDelete',1,'','',2,5,'UserList',2,'top',500,600,1,1,2,1,1,-1,'delete.gif');

--角色管理Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(11,'角色列表','RoleList',1,'','',1,4,'RoleList',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(12,'新增','RoleAdd',1,'','',2,4,'RoleAdd',1,'top',500,600,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(13,'修改角色','RoleEdit',1,'','',2,4,'RoleAdd',1,'top',500,600,2,2,2,2,2,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(14,'设置权限','RoleFunction',1,'','',2,4,'SetRoleFunction',1,'top',500,600,2,2,2,2,2,1,'role_function.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(15,'删除','RoleDelete',1,'','',2,4,'RoleList',2,'top',500,600,1,1,2,1,1,-1,'delete.gif');

--功能管理Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(16,'功能树','FunctionList',1,'','',1,3,'FunctionsManage',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(17,'新增功能','FunctionAdd',1,'','',2,3,'FunctionAdd',1,'top',500,600,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(18,'修改功能','FunctionEdit',1,'','',2,3,'FunctionAdd',1,'top',500,600,2,2,2,2,2,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(19,'删除','FunctionDelete',1,'','',2,3,'FunctionDelete',2,'top',500,600,1,1,2,1,1,-1,'delete.gif');

SET IDENTITY_INSERT [SystemAction] OFF;
