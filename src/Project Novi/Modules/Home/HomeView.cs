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

        private static readonly List<TileReference> tileLocations = new List<TileReference>();

        public static void loadTileLocations()
        {
            if (tileLocations.Count > 0)
                return;

            List<String> errors = new List<String>();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load("TileLocations.xml");

                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/tiles/tile");

                foreach (XmlNode node in nodeList)
                {
                    try
                    {
                        var x = Int32.Parse(node.SelectSingleNode("x").InnerText);
                        var y = Int32.Parse(node.SelectSingleNode("y").InnerText);
                        var w = Int32.Parse(node.SelectSingleNode("width").InnerText);
                        var h = Int32.Parse(node.SelectSingleNode("height").InnerText);
                        var text = node.SelectSingleNode("text").InnerText;
                        var target = node.SelectSingleNode("target").InnerText;
                        var rect = new Rectangle(x, y, w, h);
                        tileLocations.Add(new TileReference(text, target, rect));
                    }
                    catch (Exception e)
                    {
                        errors.Add(String.Format("Error: {0}", e.Message));
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                var Msg = String.Format("An error occurred: {0}", e.Message);
                errors.Add(Msg);
            }

            if (errors.Count > 0)
            {

                var output = String.Format("Number of errors: {0}\r\n-----------------------------\r\n\r\n{1}", errors.Count, String.Join("\r\n", errors));
                alert(output);
            }
        }

        private static void alert(String message)
        {
            MessageBox.Show(message);
        }

        private List<TileButton> buttons = new List<TileButton>();

        public IModule Module
        {
            get
            {
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
            foreach (TileReference tile in tileLocations)
            {
                var lbl = String.Format(tile.Text, count);
                var btn = new TileButton(controller, lbl, Properties.Resources.icon_maps);
                btn.Location = tile.Rectangle.Location;
                btn.Size = tile.Rectangle.Size;
                if (tile.Target == "-")
                    btn.IsReleased = false;
                else
                    btn.Click += btn_Click;
                buttons.Add(btn);
                count++;
            }
        }

        void btn_Click(object sender, System.EventArgs e)
        {
            if (sender is TileButton == false)
                return;

            var button = sender as TileButton;

            var location = button.Location;

            foreach (TileReference tile in tileLocations)
            {
                if (tile.Rectangle.Location.Equals(location))
                {
                    if (tile.Target == "MapModule")
                    {
                        _controller.SelectModule(new Map.MapModule(_controller));
                    }
                    break;
                }
            }
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
