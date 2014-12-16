using System;
using System.Drawing;
using Project_Novi.Api;
using Project_Novi.Background;
using Project_Novi.Text;
using Project_Novi.Render.UI;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;

namespace Project_Novi.Modules.Home
{
    class HomeView : IView
    {
        private HomeModule _module;
        private IController _controller;

        private List<TileButton> buttons = new List<TileButton>();

        public Type ModuleType
        {
            get { return typeof(HomeModule); }
        }

        private static List<HomeTileLocation> HomeTileLocations = new List<HomeTileLocation>();
        private Rectangle _rectText;
        private Rectangle _rectAvatar;
        private DateTime _lastSpokenTime = DateTime.Now.AddMinutes(-16);

        public static void LoadTileLocations()
        {
            if (HomeTileLocations.Count > 0)
                return;

            var errors = new List<String>();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load("TileLocations.xml");

                var nodeList = xmlDoc.DocumentElement.SelectNodes("/tiles/tile");

                foreach (XmlNode node in nodeList)
                {
                    try
                    {
                        var x = Int32.Parse(node.SelectSingleNode("x").InnerText);
                        var y = Int32.Parse(node.SelectSingleNode("y").InnerText);
                        var w = Int32.Parse(node.SelectSingleNode("width").InnerText);
                        var h = Int32.Parse(node.SelectSingleNode("height").InnerText);
                        var rect = new Rectangle(x, y, w, h);

                        var homeTile = new HomeTileLocation(rect, null);
                        HomeTileLocations.Add(homeTile);
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
                var msg = String.Format("An error occurred: {0}", e.Message);
                errors.Add(msg);
            }

            if (errors.Count > 0)
            {

                var output = String.Format("Number of errors: {0}\r\n-----------------------------\r\n\r\n{1}", errors.Count, String.Join("\r\n", errors));
                Alert(output);
            }
        }
        private static void Alert(String message)
        {
            MessageBox.Show(message);
        }


        public IModule Module
        {
            get
            {
                return _module;
            }
        }

        public IBackgroundView BackgroundView { get; private set; }

        public void Initialize(IController controller)
        {
            _controller = controller;
            BackgroundView = new MainBackground();

            LoadTileLocations();
        }

        private void RegisterTilesToModules(IController controller)
        {
            if (HomeTileLocations.Count == 0)
                return;

            if (controller == null || controller.ModuleManager == null)
                return;


            var moduleNames = controller.ModuleManager.GetModuleNameList();
            moduleNames.Remove(_module.Name);

            foreach (var tile in HomeTileLocations)
            {
                if (moduleNames.Count > 0)
                {
                    var content = moduleNames[0];
                    moduleNames.RemoveAt(0);
                    tile.SetModuleName(content);
                }
                else
                {
                    tile.SetModuleName();
                }

            }
        }


        public void Attach(IModule module)
        {
            var homeModule = module as HomeModule;
            if (homeModule == null)
                throw new ArgumentException("A MapView can only render the interface for a MapModule");

            _module = homeModule;

            RegisterTilesToModules(_controller);
            
            if (DateTime.Now.Subtract(_lastSpokenTime).TotalMinutes > 10)
            {
                _controller.Avatar.Say(_module.AvatarText);
                _lastSpokenTime = DateTime.Now;
            }

            _controller.Touch += ControllerOnTouch;

            int count = 1;
            foreach (HomeTileLocation tile in HomeTileLocations)
            {
                var btn = new TileButton(_controller, tile.ModuleName, Properties.Resources.icon_maps);
                btn.Location = tile.Rectangle.Location;
                btn.Size = tile.Rectangle.Size;

                if (tile.ModuleName == null)
                    btn.IsReleased = false;
                else
                    btn.Click += btn_Click;
                buttons.Add(btn);
                count++;
            }
        }

        private void ControllerOnTouch(Point point)
        {
            if (!_rectAvatar.Contains(point))
                return;

            if (_controller.Avatar.Talking)
            {
                _controller.Avatar.Talking = false;
            }
            else
            {
                _controller.Avatar.Pinch();
                _controller.Avatar.Say(TextManager.GetText("Poke"));
            }

        }

        public void Detach()
        {
            _module = null;
            buttons.Clear();
        }

        private void btn_Click(object sender, System.EventArgs e)
        {
            if (sender is TileButton)
            {
                var button = sender as TileButton;

                var location = button.Location;

                foreach (HomeTileLocation tile in HomeTileLocations)
                {
                    if (tile.Rectangle.Location.Equals(location))
                        _controller.SelectModule(_controller.ModuleManager.GetModule(tile.ModuleName));
                }
            }
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            _rectText = new Rectangle(1, 295, 1920, 90);
            const int fontSize = 45;

            var strFont = TextUtils.GetFont(fontSize) ??
                          new Font(SystemFonts.DefaultFont.Name, fontSize, FontStyle.Regular);

            graphics.DrawString(_module.AvatarText, strFont, Brushes.White, _rectText, stringFormat);

            _rectAvatar = new Rectangle(_rectText.X + ((1920 / 2) - 250), 489, 500, 1080 - 489);
            _controller.Avatar.Render(graphics, _rectAvatar);

            foreach (var btn in buttons)
            {
                btn.Render(graphics);
            }
        }
    }
}
