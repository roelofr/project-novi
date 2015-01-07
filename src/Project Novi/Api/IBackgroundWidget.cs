using System.Drawing;

namespace Project_Novi.Api
{
    public interface IBackgroundWidget
    {
        string ModuleName { get; }
        void Render(Graphics graphics, Rectangle rectangle);
        void Initialize(IController controller, IModule module);
    }
}
