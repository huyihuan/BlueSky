using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using WebBaseServer.Interface;
using System.Web.SessionState;
using WebBase.Utilities;
using ToolService.Interface;

namespace WebWorld.Server
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ServerRouting : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string strAction = context.Request.QueryString["action"];
            context.Response.ContentType = "text/plain";
            if (strAction == "GetFunctions")
            {
                context.Response.ContentType = "application/x-javascript";
                ChannelFactory<ISystemFunctionServer> channel = new ChannelFactory<ISystemFunctionServer>("SystemService");
                ISystemFunctionServer server = channel.CreateChannel();
                string strFuncs = server.GetUserFunction(SystemUtil.GetCurrentUserId());
                context.Response.Write(strFuncs);
                context.Response.End();
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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
