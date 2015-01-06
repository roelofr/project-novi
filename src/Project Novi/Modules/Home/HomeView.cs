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

            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load("TileLocations.xml");
            }
            catch (Exception e)
            {
                var msg = String.Format("An error occurred: {0}", e.Message);
                Alert(msg);
                return;
            }

            var docBody = xmlDoc.DocumentElement;

            var nodeList = docBody != null ? docBody.SelectNodes("/tiles/tile") : null;

            if (nodeList == null)
                return;

            foreach (XmlNode node in nodeList)
            {
                var x = node.SelectSingleNode("x");
                var y = node.SelectSingleNode("y");
                var w = node.SelectSingleNode("width");
                var h = node.SelectSingleNode("height");

                var xOut = x != null ? Int32.Parse(x.InnerText) : 0;
                var yOut = y != null ? Int32.Parse(y.InnerText) : 0;
                var wOut = w != null ? Int32.Parse(w.InnerText) : 0;
                var hOut = h != null ? Int32.Parse(h.InnerText) : 0;
                var rect = new Rectangle(xOut, yOut, wOut, hOut);

                var homeTile = new HomeTileLocation(rect, null);
                HomeTileLocations.Add(homeTile);
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
                    tile.ModuleName = content;
                }
                else
                {
                    tile.ModuleName = null;
                }

            }
        }


        public void Attach(IModule module)
        {
            var homeModule = module as HomeModule;
            if (homeModule == null)
                throw new ArgumentException("A HomeView can only render the interface for a HomeModule");

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
                string tileModuleName = null;
                Bitmap tileModuleIcon = null;
                if (tile.ModuleName != null)
                {
                    var mod = _controller.ModuleManager.GetModule(tile.ModuleName);
                    tileModuleName = mod.DisplayName;
                    tileModuleIcon = mod.Icon ?? Properties.Resources.tileIcon;
                }
                var btn = new TileButton(_controller, tileModuleName, tileModuleIcon);

                if (tile.ModuleName == null)
                    btn.IsReleased = false;
                else
                    btn.Click += btn_Click;

                btn.Location = tile.Rectangle.Location;
                btn.Size = tile.Rectangle.Size;

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
