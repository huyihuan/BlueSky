using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.Interfaces;
using System;
namespace WebBase.SystemClass
{
	public class SystemNotice : IEntity
	{
		[EntityField(FieldName = "Id", IsPrimaryKey = true)]
		public int Id
		{
			get;
			set;
		}
		public string Title
		{
			get;
			set;
		}
		public string Content
		{
			get;
			set;
		}
		public int RangeType
		{
			get;
			set;
		}
		public int TargetObject
		{
			get;
			set;
		}
		public DateTime BeginTime
		{
			get;
			set;
		}
		public DateTime EndTime
		{
			get;
			set;
		}
		public static SystemNotice Get(int _nId)
		{
			SystemNotice result;
			if (_nId <= 0)
			{
				result = null;
			}
			else
			{
				result = EntityAccess<SystemNotice>.Access.Get(_nId);
			}
			return result;
		}
		public static void Delete(int _nId)
		{
			SystemNotice oDel = SystemNotice.Get(_nId);
			if (null != oDel)
			{
				EntityAccess<SystemNotice>.Access.Delete(oDel);
			}
		}
		public static SystemNotice[] List()
		{
			return EntityAccess<SystemNotice>.Access.List();
		}
		public static SystemNotice[] List(string __strFilter, string __strSort, int __nPageIndex, int __nPageSize)
		{
            return EntityAccess<SystemNotice>.Access.List(__strFilter, __strSort, __nPageIndex, __nPageSize);
		}
		public static int Save(SystemNotice _Entity)
		{
            if (null != _Entity)
            {
                return EntityAccess<SystemNotice>.Access.Save(_Entity);
            }
            return -1;
		}
	}
}
