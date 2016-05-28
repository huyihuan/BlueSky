--初始化模块表
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(1,'系统管理','SystemManage','WebWorld.SystemManage','系统基础管理模块，必须初始化!');
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(2,'联系人管理','ContactsManage','WebWorld.Modules.ContactsManage','联系人管理');


--初始化系统功能表
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(1,'系统管理','SystemManage',1,-1,'','',1);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(2,'系统模块管理','SystemModuleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(3,'系统功能管理','SystemFunctionManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(4,'系统角色管理','SystemRoleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(5,'系统用户管理','SystemUserManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(6,'联系人管理','ContactsManage',2,-1,'','',1);


--初始化Action表
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup) VALUES(1,'用户列表','UserList',1,'','',1,5,'UserList',0);
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup) VALUES(2,'新增','UserAdd',0,'','',1,5,'UserAdd',0);