using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using BlueSky.Interfaces;
using BlueSky.Attribute;
using BlueSky.EntityAccess;
using BlueSky.DataAccess;

namespace WebWorld.Server.SystemManage
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebPerformanceReceiver : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Stream content = context.Request.InputStream;
            string strJson = "", strType = context.Request.QueryString["type"];
            if (content.CanRead)
            {
                byte[] bContent = new byte[1024000];
                content.Read(bContent, 0, context.Request.ContentLength);
                strJson = context.Request.ContentEncoding.GetString(bContent);
            }
            if(!string.IsNullOrEmpty(strJson))
            {
                if (strType == "resource")
                {
                    JsonPerformanceResourceTiming[] resources = JsonConvert.DeserializeObject<JsonPerformanceResourceTiming[]>(strJson);
                }
                else if(strType == "page")
                {
                    string strIP = context.Request.UserHostAddress, strURL = context.Request.Url.AbsoluteUri, strLanguages = string.Join(";", context.Request.UserLanguages);
                    JsonPerformanceTiming jsonTiming = JsonConvert.DeserializeObject<JsonPerformanceTiming>(strJson);
                    
                    PerformanceTiming timing = new PerformanceTiming(jsonTiming);
                    timing.IP = strIP;
                    timing.URL = strURL;
                    timing.UserLanguages = strLanguages;
                    PerformanceTiming.Save(timing);
                    
                }
                
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public struct JsonPerformanceTiming
    {
        public long connectEnd;
        public long connectStart;
        public long domComplete;
        public long domContentLoadedEventEnd;
        public long domContentLoadedEventStart;
        public long domInteractive;
        public long domLoading;
        public long domainLookupEnd;
        public long domainLookupStart;
        public long fetchStart;
        public long loadEventEnd;
        public long loadEventStart;
        public long navigationStart;
        public long redirectEnd;
        public long redirectStart;
        public long requestStart;
        public long responseEnd;
        public long responseStart;
        public long secureConnectionStart;
        public long unloadEventEnd;
        public long unloadEventStart;
    }
    public struct JsonPerformanceResourceTiming
    {
        public float connectEnd;
        public float connectStart;
        public float domainLookupEnd;
        public float domainLookupStart;
        public float duration;
        public string entryType;
        public float fetchStart;
        public string initiatorType;
        public string name;
        public float redirectEnd;
        public float redirectStart;
        public float requestStart;
        public float responseEnd;
        public float responseStart;
        public float secureConnectionStart;
        public float startTime;
    }

    [EntityAttribue(EnableCache = false, ConectionName = "BlueSkyPerformance", DbType = DatabaseType.SqlServer)]
    public class PerformanceTiming : IEntity
    {
        [EntityField(FieldName = "Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        public string IP { get; set; }
        public string URL { get; set; }
        public string UserLanguages { get; set; }

        public long ConnectTime { get; set; }
        public long LoadEventTime { get; set; }
        public long DomainLookupTime { get; set; }
        public long RequestTime { get; set; }
        public long ResponseTime { get; set; }
        public long DomInitTime { get; set; }
        public long DomReadyTime { get; set; }
        public long TotalTime { get; set; }
        public DateTime CreateTime { get; set; }
        
        public PerformanceTiming() {
            this.CreateTime = DateTime.Now;
        }
        public PerformanceTiming(JsonPerformanceTiming timing)
        {
            this.ConnectTime = timing.connectEnd - timing.connectStart;
            this.LoadEventTime = timing.domContentLoadedEventEnd - timing.domContentLoadedEventStart;
            this.DomainLookupTime = timing.domainLookupEnd - timing.domainLookupStart;
            this.RequestTime = timing.responseEnd - timing.requestStart;
            this.ResponseTime = timing.responseEnd - timing.responseStart;
            this.DomInitTime = timing.domInteractive - timing.responseEnd;
            this.DomReadyTime = timing.domComplete - timing.domInteractive;
            this.TotalTime = timing.loadEventEnd - timing.navigationStart;
            this.CreateTime = DateTime.Now;
        }

        public static PerformanceTiming Get(int _nId)
        {
            if (_nId <= 0)
            {
                return null;
            }
            return EntityAccess<PerformanceTiming>.Access.Get(_nId);
        }
        public static int Save(PerformanceTiming _Entity)
        {
            if (null == _Entity)
                return -1;
            return EntityAccess<PerformanceTiming>.Access.Save(_Entity);
        }
    }

    public class PerformanceResourceTiming : IEntity
    {
        [EntityField(FieldName = "Id", IsPrimaryKey = true)]
        public int Id { get; set; }
        public int PerformanceTiming { get; set; }
        public float connectEnd { get; set; }
        public float connectStart { get; set; }
        public float domainLookupEnd { get; set; }
        public float domainLookupStart { get; set; }
        public float duration { get; set; }
        public string entryType { get; set; }
        public float fetchStart { get; set; }
        public string initiatorType { get; set; }
        public string name { get; set; }
        public float redirectEnd { get; set; }
        public float redirectStart { get; set; }
        public float requestStart { get; set; }
        public float responseEnd { get; set; }
        public float responseStart { get; set; }
        public float secureConnectionStart { get; set; }
        public float startTime { get; set; }

        public PerformanceResourceTiming() { }
        public PerformanceResourceTiming(JsonPerformanceResourceTiming timing)
        { 
            
        }

        public static PerformanceResourceTiming Get(int _nId)
        {
            if (_nId <= 0)
            {
                return null;
            }
            return EntityAccess<PerformanceResourceTiming>.Access.Get(_nId);
        }

        public static int Save(PerformanceResourceTiming _Entity)
        {
            if(null == _Entity)
                return -1;
            return EntityAccess<PerformanceResourceTiming>.Access.Save(_Entity);
        }
    }
}
