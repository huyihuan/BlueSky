using System;
using System.Web;
using System.Web.Services;
using WebBase.Server.ServerProcess;
using BlueSky.Utilities;

namespace WebBase.Server.Server
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SystemFunctionService : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse oHR = context.Response;
            string strUserId = context.Request.QueryString["uid"] + "";
            oHR.ContentType = "text/plain";
            if (string.IsNullOrEmpty(strUserId))
            {
                oHR.Write("");
                oHR.End();
            }
            SystemFunctionServer oServer = new SystemFunctionServer();
            oHR.Write(oServer.GetUserFunction(TypeUtil.ParseInt(strUserId, -1)));
            oHR.End();
        }

        #endregion
    }
}
