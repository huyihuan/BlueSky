using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WebBase.Utilities;
using System.Web.SessionState;
using WebBase.SystemClass;
using System.Threading;
using System.Drawing;

namespace WebWorld.Server.SystemManage
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Login : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string strAction = context.Request.QueryString["action"];
            context.Response.ContentType = "text/plain";
            if (strAction == "Login")
            {
                context.Response.ContentType = "application/x-javascript";
                string strVCode = context.Request.QueryString["vcode"];
                if (!strVCode.ToLower().Equals(SystemUtil.VCodeGetCurrent().ToLower()))
                {
                    context.Response.Write("{ \"success\" : false, \"text\" : \"验证码错误！\" }");
                    context.Response.End();
                }
                string strUserName = context.Request.QueryString["uid"];
                string strPassword = context.Request.QueryString["pwd"];
                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    context.Response.Write("{ \"success\" : false, \"text\" : \"用户名或密码错误！\"  }");
                    context.Response.End();
                }
                
                if (UserInformation.ExistUser(strUserName, strPassword))
                {
                    UserInformation oLogin = UserInformation.Get(strUserName);
                    SystemUtil.LoginUser(oLogin);
                    context.Response.Write("{ \"success\" : true }");
                    //开启线程存储用户登陆日志
                    LogParams para = new LogParams(oLogin.Id, context.Request.Url.AbsoluteUri, "系统登陆", "登陆");
                    Thread tLog = new Thread(new ParameterizedThreadStart(SaveLog));
                    tLog.Start(para);
                }
                else
                {
                    context.Response.Write("{ \"success\" : false, \"text\" : \"用户名或密码错误！\" }");
                }
            }
            else if (strAction == "Logout")
            {
                LogParams para = new LogParams(SystemUtil.GetCurrentUserId(), context.Request.Url.AbsoluteUri, "系统退出", "退出");
                Thread tLog = new Thread(new ParameterizedThreadStart(SaveLog));
                tLog.Start(para);
                SystemUtil.LogoutUser();
                context.Response.Write("true");
            }
            else if (strAction == "VCode")
            {
                CreateVCodeImage(context);
            }
            else if (strAction == "TreeCreate")
            {
                context.Response.ContentType = "application/x-javascript";
                string strTree = "[{ \"text\":\"节点1\", \"value\":\"1\"},{ \"text\":\"节点2\", \"value\":\"2\"}]";
                context.Response.Write(strTree);
            }
            context.Response.End();
        }

        public void SaveLog(object _para)
        {
            LogParams para = (LogParams)_para;
            //记录登陆日志
            SystemLog oLog = new SystemLog();
            oLog.UserId = para.UserId;
            oLog.AccessFunctionName = para.FunctionName;
            oLog.AccessActionName = para.ActionName;
            oLog.AccessTime = DateTime.Now;
            oLog.AccessURL = para.RequestURI;
            oLog.Remark = "";
            SystemLog.Save(oLog);
        }

        public void CreateVCodeImage(HttpContext context)
        {
            string[] strVCode = new string[4];
            string strDisplayCode = "";
            string[] a = new string[61] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };//生成随机生成器
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 4; i++)
            {
                strVCode[i] = a[random.Next(60)];
                strDisplayCode += strVCode[i];
            }
            SystemUtil.VCodeSaveCurrent(strDisplayCode);
            int nWidth = (int)Math.Ceiling((strVCode.Length * 30.1));
            int nHeight = 40;
            Bitmap image = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //清空图片背景色
                g.Clear(Color.FromArgb(239, 243, 247));
                //画图片的背景噪音线
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255))), x1, y1, x2, y2);
                }
                //定义颜色
                Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
                //定义字体
                //string[] f = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
                string strFontFamily = "宋体";
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Center;
                float x = 5.0F;
                float y = 0.0F;
                float width = 41.5F;
                float height = 40.0F;
                for (int k = 0; k <= strVCode.Length - 1; k++)
                {
                    int cindex = random.Next(7);
                    Font drawFont = new Font(strFontFamily, 21, (FontStyle.Bold));
                    SolidBrush drawBrush = new SolidBrush(c[cindex]);
                    int sjx = random.Next(10);
                    int sjy = random.Next(image.Height - (int)height);
                    RectangleF drawRect = new RectangleF(x + sjx + (k * 25), y + sjy, width, height);

                    g.DrawString(strVCode[k], drawFont, drawBrush, drawRect, drawFormat);
                }
                //画图片的前景噪音点
                for (int i = 0; i < 20; i++)
                {
                    image.SetPixel(random.Next(image.Width), random.Next(image.Height), Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                context.Response.BinaryWrite(ms.ToArray());
                context.Response.End();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void TreeCreate(HttpContext context)
        { 
            
        }
    }

    public class LogParams
    {
        public int UserId;
        public string RequestURI;
        public string FunctionName;
        public string ActionName;
        public LogParams(int _nUserId, string _strRequestURI, string _strFunctionName, string _strActionName)
        {
            this.UserId = _nUserId;
            this.RequestURI = _strRequestURI;
            this.FunctionName = _strFunctionName;
            this.ActionName = _strActionName;
        }
    }
}
