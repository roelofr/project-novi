using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_Novi.Modules.Map
{
    class MapView : IView
    {
        private enum Floor
        {
            T0, T1, T2, T3, T4, T5
        }

        private readonly MapModule _module;
        private readonly IController _controller;
        private Floor _activeFloor;
        Point[] driehoek = new Point[3];
        SolidBrush brushActiveFloor = new SolidBrush(Color.Orange);
        SolidBrush brushFloors = new SolidBrush(Color.Blue);
      
        public IModule Module
        {
            get { return _module; }
        }

        public MapView(MapModule module, IController controller)
        {
            _module = module;
            _controller = controller;
            _controller.Touch += ControllerOnTouch;
        }

        private void ControllerOnTouch(Point p)
        {
            if (p.X > 1550 && p.X < 1800 && p.Y > 80 && p.Y < 220)
                _activeFloor = Floor.T5;
            else if (p.X > 1550 && p.X < 1800 && p.Y > 230 && p.Y < 370)
                _activeFloor = Floor.T4;
            else if (p.X > 1550 && p.X < 1800 && p.Y > 380 && p.Y < 520)
                _activeFloor = Floor.T3;
            else if (p.X > 1550 && p.X < 1800 && p.Y > 530 && p.Y < 670)
                _activeFloor = Floor.T2;
            else if (p.X > 1550 && p.X < 1800 && p.Y > 680 && p.Y < 820)
                _activeFloor = Floor.T1;
            else if (p.X > 1550 && p.X < 1800 && p.Y > 830 && p.Y < 970)
                _activeFloor = Floor.T0;
        }

        public void Render(Graphics graphics, Rectangle rectangle)
        {
            graphics.Clear(Color.Beige);
            //button T5
            graphics.FillRectangle(brushFloors, 1550, 80, 250, 140);
            //button T4
            graphics.FillRectangle(brushFloors, 1550, 230, 250, 140);
            //button T3
            graphics.FillRectangle(brushFloors, 1550, 380, 250, 140);
            //button T2
            graphics.FillRectangle(brushFloors, 1550, 530, 250, 140);
            //button T1
            graphics.FillRectangle(brushFloors, 1550, 680, 250, 140);
            //button T0
            graphics.FillRectangle(brushFloors, 1550, 830, 250, 140);
            
            switch (_activeFloor)
            {
                case Floor.T5:
                    graphics.DrawImage(Properties.Resources.T5x, 100, 200);
                    driehoek[0].X = 1540;
                    driehoek[0].Y = 80;
                    driehoek[1].X = 1540;
                    driehoek[1].Y = 220;
                    driehoek[2].X = 1490;
                    driehoek[2].Y = 150;
                    break; 
                case Floor.T4:
                    graphics.DrawImage(Properties.Resources.T4x, 100, 200);
                    driehoek[0].X = 1540;
                    driehoek[0].Y = 230;
                    driehoek[1].X = 1540;
                    driehoek[1].Y = 370;
                    driehoek[2].X = 1490;
                    driehoek[2].Y = 300;
                    break;
                case Floor.T3:
                    graphics.DrawImage(Properties.Resources.T3x, 100, 200);
                    driehoek[0].X = 1540;
                    driehoek[0].Y = 380;
                    driehoek[1].X = 1540;
                    driehoek[1].Y = 520;
                    driehoek[2].X = 1490;
                    driehoek[2].Y = 450;
                    break;
                case Floor.T2:
                    graphics.DrawImage(Properties.Resources.T2x, 100, 200);
                    driehoek[0].X = 1540;
                    driehoek[0].Y = 530;
                    driehoek[1].X = 1540;
                    driehoek[1].Y = 670;
                    driehoek[2].X = 1490;
                    driehoek[2].Y = 600;
                    break;
                case Floor.T1:
                    graphics.DrawImage(Properties.Resources.T1x, 100, 200);
                    driehoek[0].X = 1540;
                    driehoek[0].Y = 680;
                    driehoek[1].X = 1540;
                    driehoek[1].Y = 820;
                    driehoek[2].X = 1490;
                    driehoek[2].Y = 750;
                    break;
                case Floor.T0:
                    graphics.DrawImage(Properties.Resources.T0x, 100, 200);
                    driehoek[0].X = 1540;
                    driehoek[0].Y = 830;
                    driehoek[1].X = 1540;
                    driehoek[1].Y = 970;
                    driehoek[2].X = 1490;
                    driehoek[2].Y = 900;
                    break;
            }
            graphics.FillPolygon(brushActiveFloor, driehoek);
            var strFont = new Font("Helvetica Neue", 50);
            var rectT5 = new Rectangle(1520, 130, 300, 60);
            var rectT4 = new Rectangle(1520, 280, 300, 60);
            var rectT3 = new Rectangle(1520, 430, 300, 60);
            var rectT2 = new Rectangle(1520, 580, 300, 60);
            var rectT1 = new Rectangle(1520, 730, 300, 60);
            var rectT0 = new Rectangle(1520, 880, 300, 60);
            var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.DrawString("T5", strFont, Brushes.White, rectT5, stringFormat);
            graphics.DrawString("T4", strFont, Brushes.White, rectT4, stringFormat);
            graphics.DrawString("T3", strFont, Brushes.White, rectT3, stringFormat);
            graphics.DrawString("T2", strFont, Brushes.White, rectT2, stringFormat);
            graphics.DrawString("T1", strFont, Brushes.White, rectT1, stringFormat);
            graphics.DrawString("T0", strFont, Brushes.White, rectT0, stringFormat);
        }
    }
}
