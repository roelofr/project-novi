using System;
using System.Xml;

namespace Project_Novi.Text
{
    static class TextManager
    {
        public static String GetText(string category)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("AvatarTekst.xml");

            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/Strings/{0}/String", category));
            var random = new Random();
            return nodeList[random.Next(nodeList.Count)].InnerText;
        }
    }
}
