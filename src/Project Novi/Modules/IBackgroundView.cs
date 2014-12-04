using System.Drawing;

namespace Project_Novi.Modules
{
    interface IBackgroundView {
    
        void Render(Graphics graphics, Rectangle rectangle);
    }
}
