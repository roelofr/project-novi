using System;
using System.Windows.Forms;
using System.Xml;

namespace Project_Novi
{
    public partial class Controlpanel : Form
    {
        public Controlpanel()
        {
            InitializeComponent();
            GetUsernamesTwitter();
        }

        private void Controlpanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SaveUsernames();
                    Close();
                    break;
                case Keys.Escape:
                    Close();
                    break;
            }
        }

        public void GetUsernamesTwitter()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

            XmlNode nodeUsername1 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username1/String"));
            XmlNode nodeUsername2 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username2/String"));
            XmlNode nodeUsername3 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username3/String"));

            textboxUsernameTwitter1.Text = nodeUsername1.InnerText;
            textboxUsernameTwitter2.Text = nodeUsername2.InnerText;
            textboxUsernameTwitter3.Text = nodeUsername3.InnerText;
        }

        public static string GetUsernameTwitter(string usernameNumber)
        {
            //username1
            //username2
            //username3
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

            var nodeList = xmlDoc.DocumentElement.SelectNodes(String.Format("/Strings/{0}/String", usernameNumber));
            
            return nodeList[0].InnerText;  
        }

        public void SaveUsernames()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("TwitterSettings.xml");
            XmlNode nodeUsername1 = doc.SelectSingleNode(String.Format("/Strings/username1/String"));
            XmlNode nodeUsername2 = doc.SelectSingleNode(String.Format("/Strings/username2/String"));
            XmlNode nodeUsername3 = doc.SelectSingleNode(String.Format("/Strings/username3/String"));

            nodeUsername1.InnerText = textboxUsernameTwitter1.Text;
            nodeUsername2.InnerText = textboxUsernameTwitter2.Text;
            nodeUsername3.InnerText = textboxUsernameTwitter3.Text;

            doc.Save("TwitterSettings.xml");
        }
    }
}
