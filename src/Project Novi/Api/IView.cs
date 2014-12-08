using System;
using System.Drawing;

namespace Project_Novi.Api
{
    /// <summary>
    /// IView is the basic interface every view should implement.
    /// </summary>
    interface IView
    {
        /// <summary>
        /// The type of the module this view is designed to display.
        /// </summary>
        Type ModuleType { get; }

        /// <summary>
        /// The background view for this view.
        /// This backgroundview will be rendered before calling Render on this view itself.
        /// </summary>
        IBackgroundView BackgroundView { get; }

        /// <summary>
        /// Called when the view is loaded into the application.
        /// Note that this does not mean the view will actually be used at this time.
        /// </summary>
        /// <param name="controller"></param>
        void Initialize(IController controller);

        /// <summary>
        /// A module of the type designated by ModuleType becomes the active module.
        /// </summary>
        /// <param name="module"></param>
        void Attach(IModule module);

        /// <summary>
        /// The previously Attached module stops being the active module.
        /// </summary>
        void Detach();

        /// <summary>
        /// Render the contents of the module to the screen.
        /// </summary>
        /// <param name="graphics">The Graphics to use for drawing.</param>
        /// <param name="rectangle">The Rectangle in which the rendered contents should be drawn.</param>
        void Render(Graphics graphics, Rectangle rectangle);
    }
}
