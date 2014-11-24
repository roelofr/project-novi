using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Project_Novi.Tekst
{
    public partial class TextSaver : Form
    {
        public TextSaver()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Categorie = textBox1.Text;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("AvatarTekst.xml");

            if (Categorie == "Welkom")
            {         
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Welkom");
                string strTxt = "", strID = "";  
                Random random = new Random();
                int number = random.Next(4);
                foreach (XmlNode node in nodeList)
                {
                    strID = node.SelectSingleNode("String_ID").InnerText;
                    strTxt = node.SelectSingleNode("String_text").InnerText;
                    if (number == 0)
                    {
                        if (strID == "1")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 1)
                    {
                        if (strID == "2")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 2)
                    {
                        if (strID == "3")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 3)
                    {
                        if (strID == "4")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }

                }
                
            }
            else if(Categorie == "Poke")
            {
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Poke");
                string strTxt = "", strID = "";
                Random random = new Random();
                int number = random.Next(4);
                foreach (XmlNode node in nodeList)
                {
                    strID = node.SelectSingleNode("String_ID").InnerText;
                    strTxt = node.SelectSingleNode("String_text").InnerText;
                    if (number == 0)
                    {
                        if (strID == "1")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 1)
                    {
                        if (strID == "2")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 2)
                    {
                        if (strID == "3")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 3)
                    {
                        if (strID == "4")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                }
            }
            else if (Categorie == "Idle")
            {
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Idle");
                string strTxt = "", strID = "";
                Random random = new Random();
                int number = random.Next(4);
                foreach (XmlNode node in nodeList)
                {
                    strID = node.SelectSingleNode("String_ID").InnerText;
                    strTxt = node.SelectSingleNode("String_text").InnerText;
                    if (number == 0)
                    {
                        if (strID == "1")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 1)
                    {
                        if (strID == "2")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 2)
                    {
                        if (strID == "3")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 3)
                    {
                        if (strID == "4")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                }
            }
            else if (Categorie == "Kaart")
            {
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Kaart");
                string strTxt = "", strID = "";
                Random random = new Random();
                int number = random.Next(4);
                foreach (XmlNode node in nodeList)
                {
                    strID = node.SelectSingleNode("String_ID").InnerText;
                    strTxt = node.SelectSingleNode("String_text").InnerText;
                    if (number == 0)
                    {
                        if (strID == "1")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 1)
                    {
                        if (strID == "2")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 2)
                    {
                        if (strID == "3")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 3)
                    {
                        if (strID == "4")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                }
            }
            else if (Categorie == "Route")
            {
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Table/Route");
                string strTxt = "", strID = "";
                Random random = new Random();
                int number = random.Next(4);
                foreach (XmlNode node in nodeList)
                {
                    strID = node.SelectSingleNode("String_ID").InnerText;
                    strTxt = node.SelectSingleNode("String_text").InnerText;
                    if (number == 0)
                    {
                        if (strID == "1")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 1)
                    {
                        if (strID == "2")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 2)
                    {
                        if (strID == "3")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                    else if (number == 3)
                    {
                        if (strID == "4")
                        {
                            MessageBox.Show(strTxt);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Die ken ik niet!");
            }
        }          
    } 
}


