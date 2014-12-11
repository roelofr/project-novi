using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Modules.Home
{
    class TileReference
    {
        public String Text { get; private set; }
        public String Target { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public TileReference(String text, String target, Rectangle rectangle)
        {
            Text = text;
            Target = target;
            Rectangle = rectangle;
        }
    }
}
