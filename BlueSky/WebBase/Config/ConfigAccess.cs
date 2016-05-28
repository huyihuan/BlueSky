using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
namespace WebBase.Config
{
	public class ConfigAccess
	{
		public const string APPSETTINGKEY_EnableDebuggingOn = "EnableDebuggingOn";
		public const string APPSETTINGKEY_DebuggingUserName = "DebuggingUserName";
		public const string APPSETTINGKEY_DebuggingPassword = "DebuggingPassword";
		public string ConfigPath
		{
			get;
			set;
		}
		public Configuration oConfig
		{
			get;
			set;
		}
		public ConfigAccess()
		{
			this.ConfigPath = HttpContext.Current.Request.ApplicationPath;
			this.oConfig = WebConfigurationManager.OpenWebConfiguration(this.ConfigPath);
		}
		public string GetAppSetting(string _strKey)
		{
			string result;
			if (null == this.oConfig)
			{
				result = "";
			}
			else
			{
				KeyValueConfigurationElement oElement = this.oConfig.AppSettings.Settings[_strKey];
				if (null == oElement)
				{
					result = "";
				}
				else
				{
					result = oElement.Value;
				}
			}
			return result;
		}
		public void SetAppSetting(string _strKey, string _strValue)
		{
			if (null != this.oConfig)
			{
				KeyValueConfigurationElement kvElement = this.oConfig.AppSettings.Settings[_strKey];
				if (null == kvElement)
				{
					kvElement = new KeyValueConfigurationElement(_strKey, _strValue);
					this.oConfig.AppSettings.Settings.Add(kvElement);
				}
				else
				{
					kvElement.Value = _strValue;
				}
				this.oConfig.Save();
			}
		}
	}
}
