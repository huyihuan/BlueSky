using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using WebBase.Utilities;

namespace WebWorld.Include.html
{
    public partial class VCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)     
        {         
            string[] strVCode = new string[4];
            string strDisplayCode = "";
            string [] a=new string [61] {"1","2","3","4","5","6","7","8","9","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};//生成随机生成器
            Random random = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < 4; i++)
            {
                strVCode[i] = a[random.Next(60)];
                strDisplayCode += strVCode[i];
            }
            CreateVCodeImage(strVCode);
            SystemUtil.VCodeSaveCurrent(strDisplayCode);
        }
        public void CreateVCodeImage(string[] __alCodes)
        {
            int nWidth = (int)Math.Ceiling((__alCodes.Length * 30.1));
            int nHeight = 40;
            Bitmap image = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Random random = new Random(DateTime.Now.Millisecond);//清空图片背景色
                g.Clear(Color.FromArgb(239, 243, 247));//画图片的背景噪音线
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
                for (int k = 0; k <= __alCodes.Length - 1; k++)
                {
                    int cindex = random.Next(7);
                    Font drawFont = new Font(strFontFamily, 21, (FontStyle.Bold));
                    SolidBrush drawBrush = new SolidBrush(c[cindex]);
                    int sjx = random.Next(10);
                    int sjy = random.Next(image.Height - (int)height);
                    RectangleF drawRect = new RectangleF(x + sjx + (k * 25), y + sjy, width, height);
                    
                    g.DrawString(__alCodes[k], drawFont, drawBrush, drawRect, drawFormat);
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
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            { 
                g.Dispose();
                image.Dispose();
            }
        }
    }
}
