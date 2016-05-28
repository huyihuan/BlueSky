using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebWorld.FunctionControls.AjaxApplication
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FunctionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            FunctionItem funItem = FunctionItem.Get(1);
            if (null == funItem)
            {
                context.Response.Write("");
                context.Response.End();
            }
            string strJosn = JsonConvert.SerializeObject(funItem);
            context.Response.Write(strJosn);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
