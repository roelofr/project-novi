using System.Drawing;

namespace Project_Novi.Modules
{
    interface IView
    {
        IModule Module { get; }
        void Render(Graphics graphics, Rectangle rectangle);
    }
}
