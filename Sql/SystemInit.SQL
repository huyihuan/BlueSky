--��ʼ��ģ���
delete from SystemAction;
delete from SystemFunction;
delete from SystemModule;

SET IDENTITY_INSERT [SystemModule] ON;
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(1,'ϵͳ����','SystemManage','WebWorld.SystemManage','ϵͳ��������ģ�飬�����ʼ��!');
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(2,'��ϵ�˹���','ContactsManage','WebWorld.Modules.ContactsManage','��ϵ�˹���');
SET IDENTITY_INSERT [SystemModule] OFF;

--��ʼ��ϵͳ���ܱ�
SET IDENTITY_INSERT [SystemFunction] ON;
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(1,'ϵͳ����','SystemManage',1,-1,'','',1);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(2,'ϵͳģ�����','SystemModuleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(3,'ϵͳ���ܹ���','SystemFunctionManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(4,'ϵͳ��ɫ����','SystemRoleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(5,'ϵͳ�û�����','SystemUserManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(6,'��ϵ�˹���','ContactsManage',2,-1,'','',1);
SET IDENTITY_INSERT [SystemFunction] OFF;

--��ʼ��Action��
SET IDENTITY_INSERT [SystemAction] ON;
--ģ�����Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(1,'ģ���б�','ModuleList',1,'','',1,2,'ModuleList',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(2,'����ģ��','ModuleAdd',1,'','',2,2,'ModuleAdd',1,'top',600,400,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(3,'�޸�ģ��','ModuleEdit',1,'','',2,2,'ModuleAdd',1,'top',600,400,1,1,2,1,1,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(4,'ɾ��','ModuleDelete',2,'moduleDelete','',2,2,'ModuleList',2,'top',600,400,1,1,2,1,1,-1,'delete.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(5,'����ģ��','ModuleSet',1,'','',2,2,'ModuleUpdate',1,'top',600,400,1,1,2,1,1,1,'refresh.png');


--�û�����Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(6,'�û��б�','UserList',1,'','',1,5,'UserList',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(7,'����','UserAdd',1,'','',2,5,'UserAdd',1,'top',500,600,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(8,'�޸��û�','UserEdit',1,'','',2,5,'UserAdd',1,'top',500,600,2,2,2,2,2,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(9,'�����û�Ȩ��','UserFunction',1,'','',2,5,'SetUserFunction',1,'top',500,600,2,2,2,2,2,1,'role_function.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(10,'ɾ��','UserDelete',1,'','',2,5,'UserList',2,'top',500,600,1,1,2,1,1,-1,'delete.gif');

--��ɫ����Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(11,'��ɫ�б�','RoleList',1,'','',1,4,'RoleList',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(12,'����','RoleAdd',1,'','',2,4,'RoleAdd',1,'top',500,600,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(13,'�޸Ľ�ɫ','RoleEdit',1,'','',2,4,'RoleAdd',1,'top',500,600,2,2,2,2,2,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(14,'����Ȩ��','RoleFunction',1,'','',2,4,'SetRoleFunction',1,'top',500,600,2,2,2,2,2,1,'role_function.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(15,'ɾ��','RoleDelete',1,'','',2,4,'RoleList',2,'top',500,600,1,1,2,1,1,-1,'delete.gif');

--���ܹ���Action
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(16,'������','FunctionList',1,'','',1,3,'FunctionsManage',2,'top',0,0,2,2,2,2,2,0,'list.png');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(17,'��������','FunctionAdd',1,'','',2,3,'FunctionAdd',1,'top',500,600,1,1,2,1,1,0,'add.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(18,'�޸Ĺ���','FunctionEdit',1,'','',2,3,'FunctionAdd',1,'top',500,600,2,2,2,2,2,1,'edit.gif');
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup,Target,Width,Height,IsResize,IsMove,IsIncludeMinBox,IsIncludeMaxBox,IsShowInTaskBar,EntityCount,IconName) VALUES(19,'ɾ��','FunctionDelete',1,'','',2,3,'FunctionDelete',2,'top',500,600,1,1,2,1,1,-1,'delete.gif');

SET IDENTITY_INSERT [SystemAction] OFF;