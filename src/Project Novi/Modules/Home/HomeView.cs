using System;
using System.Drawing;
using System.Xml;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
        public IModule Module
        {
            get { return _module; }
        }

        public HomeView(HomeModule module)
        {
            _module = module;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.Clear(Color.FromArgb(255, 32, 103, 178));

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("AvatarTekst.xml");

            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Welkom");
            string strTxt = "", strID = "";
           // Random random = new Random();
            int number = 1;
            foreach (XmlNode node in nodeList)
            {
                strID = node.SelectSingleNode("String_ID").InnerText;
                strTxt = node.SelectSingleNode("String_text").InnerText;
                Font strFont = new Font("Sergoe UI", 50);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                Rectangle rect1 = new Rectangle(1, 1, 1920, 1080);


                if (strID == "1")
                {
                    if (strID == "1")
                    {
                        SizeF stringSize = new SizeF();
                        stringSize = graphics.MeasureString(strTxt, strFont);
                        graphics.DrawString(strTxt, strFont, Brushes.Blue, rect1, stringFormat);
                        break;
                    }
                }
                else if (number == 1)
                {
                    if (strID == "2")
                    {
                        graphics.DrawString(strTxt, new Font("Segoe UI", 50), Brushes.White, 200, 100); 
                        break;
                    }
                }
                else if (number == 2)
                {
                    if (strID == "3")
                    {
                        graphics.DrawString(strTxt, new Font("Segoe UI", 50), Brushes.White, 100, 100); 
                        break;
                    }
                }
            }

            
            // TODO: Don't simulate a BSOD
            
            graphics.DrawString(":(", new Font("Segoe UI", 200), Brushes.White, 200, 100);
            
        }
    }
}
