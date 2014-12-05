using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Novi.Render.UI
{
    interface IButton {
    
        event EventHandler Click;
        void Render(Graphics graphics);
    }
}
