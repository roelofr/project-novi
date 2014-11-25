using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Project_Novi.Text
{
    static class TextManager
    {
        public static String GetText(string category)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("AvatarTekst.xml");

            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/" + category);
            Random random = new Random();
            int number = random.Next(3);
            return nodeList[random.Next(nodeList.Count)].SelectSingleNode("String_text").InnerText;          
        }
    }
}
