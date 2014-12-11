using System.Drawing;
using Project_Novi.Render;
using Project_Novi.Render.UI;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using System;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private readonly HomeModule _module;
        private readonly IController _controller;
        public Avatar Avatar;

        private static readonly List<Rectangle> tileLocations = new List<Rectangle>();

        public static void loadTileLocations()
        {
            if (tileLocations.Count > 0)
                return;

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load("TileLocations.xml");

                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Tile/Tile");

                foreach (XmlNode node in nodeList)
                {
                    try
                    {
                        var x = Int32.Parse(node.SelectSingleNode("/x").InnerText);
                        var y = Int32.Parse(node.SelectSingleNode("/y").InnerText);
                        var w = Int32.Parse(node.SelectSingleNode("/width").InnerText);
                        var h = Int32.Parse(node.SelectSingleNode("/height").InnerText);
                        var rect = new Rectangle(x, y, w, h);
                        tileLocations.Add(rect);
                    }
                    catch(Exception e)
                    {
                        Console.Error.WriteLine("Error: {0}", e.Message);
                        continue;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error!");
                Console.Error.WriteLine("Tile locations file damaged or of wrong format.");
                return;
            }
        }

        private List<TileButton> buttons = new List<TileButton>();

        public IModule Module
        {
            get {
                return _module;
            }
        }

        public HomeView(HomeModule module, IController controller)
        {
            this._module = module;
            this._controller = controller;
            Avatar = new Avatar(controller);
            Avatar.Say(module.AvatarText);

            loadTileLocations();

            int count = 1;
            foreach (Rectangle location in tileLocations)
            {
                var lbl = String.Format("Button {0}", count);
                var btn = new TileButton(controller, lbl, Properties.Resources.icon_maps);
                btn.Location = location.Location;
                btn.Size = location.Size;
                btn.Click += btn_Click;
                buttons.Add(btn);
                count++;
            }
        }

        void btn_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Got some bacon!");
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            var rectText = new Rectangle(1, 295, 1920, 90);
            const int fontSize = 45;

            var strFont = TextUtils.GetFont(fontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(_module.AvatarText, strFont, Brushes.White, rectText, stringFormat);

            var rectAvatar = new Rectangle(rectText.X, 489, 1920, 1080 - 489);
            Avatar.Render(graphics, rectAvatar);

            foreach (var btn in buttons)
            {
                btn.Render(graphics);
            }
        }
    }
}
