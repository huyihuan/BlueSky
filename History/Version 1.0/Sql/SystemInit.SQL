--��ʼ��ģ���
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(1,'ϵͳ����','SystemManage','WebWorld.SystemManage','ϵͳ��������ģ�飬�����ʼ��!');
insert into SystemModule(Id,Name,[Key],Controller,Description) VALUES(2,'��ϵ�˹���','ContactsManage','WebWorld.Modules.ContactsManage','��ϵ�˹���');


--��ʼ��ϵͳ���ܱ�
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(1,'ϵͳ����','SystemManage',1,-1,'','',1);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(2,'ϵͳģ�����','SystemModuleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(3,'ϵͳ���ܹ���','SystemFunctionManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(4,'ϵͳ��ɫ����','SystemRoleManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(5,'ϵͳ�û�����','SystemUserManage',1,1,'','',2);
insert into SystemFunction(Id,Name,[Key],ModuleId,ParentId,IconName,Description,Level) VALUES(6,'��ϵ�˹���','ContactsManage',2,-1,'','',1);


--��ʼ��Action��
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup) VALUES(1,'�û��б�','UserList',1,'','',1,5,'UserList',0);
insert into SystemAction(Id,Name,[Key],ActionType,ActionValue,Description,IsDefault,FunctionId,ControlName,IsPopup) VALUES(2,'����','UserAdd',0,'','',1,5,'UserAdd',0);