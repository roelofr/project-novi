using System.Drawing;

namespace Project_Novi.Api
{
    /// <summary>
    /// IBackgroundView is the basic interface every background view should implement.
    /// A background view is a view that is not directly tied to any module,
    /// which should render any background information that is not specific to
    /// the currently active module and view.
    /// </summary>
    public interface IBackgroundView
    {
        /// <summary>
        /// Get the Rectangle reserved for the module's view to draw in,
        /// taking into account the full available space.
        /// </summary>
        /// <param name="fullRectangle">The available space.</param>
        /// <returns>The rectangle reserved for the module's view to draw in.</returns>
        Rectangle GetModuleRectangle(Rectangle fullRectangle);

        /// <summary>
        /// Render the background.
        /// </summary>
        /// <param name="graphics">The Graphics to use for drawing.</param>
        /// <param name="rectangle">The Rectangle within which the background should be drawn.</param>
        void Render(Graphics graphics, Rectangle rectangle);
    }
}
