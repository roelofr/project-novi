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
            GetHashtagsTwitter();

        }

        private void Controlpanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SaveUsernames();
                    SaveHashtags();
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

            XmlNode nodeUsername1 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username1/Username"));
            XmlNode nodeUsername2 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username2/Username"));
            XmlNode nodeUsername3 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username3/Username"));

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
            XmlNode nodeUsername1 = doc.SelectSingleNode(String.Format("/Strings/username1/Username"));
            XmlNode nodeUsername2 = doc.SelectSingleNode(String.Format("/Strings/username2/Username"));
            XmlNode nodeUsername3 = doc.SelectSingleNode(String.Format("/Strings/username3/Username"));
            XmlNode nodeTag1 = doc.SelectSingleNode(String.Format("/Strings/username1/tag"));
            XmlNode nodeTag2 = doc.SelectSingleNode(String.Format("/Strings/username2/tag"));
            XmlNode nodeTag3 = doc.SelectSingleNode(String.Format("/Strings/username3/tag"));

            nodeUsername1.InnerText = textboxUsernameTwitter1.Text;
            nodeUsername2.InnerText = textboxUsernameTwitter2.Text;
            nodeUsername3.InnerText = textboxUsernameTwitter3.Text;
            nodeTag1.InnerText = textboxUsernameTwitter1.Text;
            nodeTag2.InnerText = textboxUsernameTwitter2.Text;
            nodeTag3.InnerText = textboxUsernameTwitter3.Text;

            doc.Save("TwitterSettings.xml");
        }

        public void GetHashtagsTwitter()
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load("TwitterSettings.xml");

           
            XmlNode nodeHashtags1 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username4/Username"));
            XmlNode nodeHashtags2 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username5/Username"));
            XmlNode nodeHashtags3 = xmlDoc.DocumentElement.SelectSingleNode(String.Format("/Strings/username6/Username"));
            
            textboxHashtag1.Text = nodeHashtags1.InnerText;
            textboxHashtag2.Text = nodeHashtags2.InnerText;
            textboxHashtag3.Text = nodeHashtags3.InnerText;
        }


        public void SaveHashtags()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("TwitterSettings.xml");
            XmlNode nodeHashtags1 = doc.DocumentElement.SelectSingleNode(String.Format("/Strings/username4/Username"));
            XmlNode nodeHashtags2 = doc.DocumentElement.SelectSingleNode(String.Format("/Strings/username5/Username"));
            XmlNode nodeHashtags3 = doc.DocumentElement.SelectSingleNode(String.Format("/Strings/username6/Username"));
            XmlNode nodeType1 = doc.DocumentElement.SelectSingleNode(String.Format("/Strings/username4/type"));
            XmlNode nodeType2 = doc.DocumentElement.SelectSingleNode(String.Format("/Strings/username5/type"));
            XmlNode nodeType3 = doc.DocumentElement.SelectSingleNode(String.Format("/Strings/username6/type"));

            nodeHashtags1.InnerText = textboxHashtag1.Text;
            nodeHashtags2.InnerText = textboxHashtag2.Text;
            nodeHashtags3.InnerText = textboxHashtag3.Text;
            nodeType1.InnerText = textboxHashtag1.Text;
            nodeType2.InnerText = textboxHashtag2.Text;
            nodeType3.InnerText = textboxHashtag3.Text;

            doc.Save("TwitterSettings.xml");
        }
    }
}
