using System.Drawing;

namespace Project_Novi.Api
{
    /// <summary>
    /// IModule is the basic interface which all modules should implement.
    /// There will be one instance of every module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// The name of the module. Other modules will use this name when referring to this module.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The icon to be displayed for the module. Should be of size 64 x 64.
        /// </summary>
        Bitmap Icon { get; }

        /// <summary>
        /// The display name of the module. Will be shown on the front-end
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Called when the module is loaded into the application.
        /// Note that this does not mean the module will actually be used at this time.
        /// </summary>
        void Initialize(IController controller);

        /// <summary>
        /// Called when the module is selected as the currently active module.
        /// Should perform any operations required to provide a smooth experience.
        /// </summary>
        void Start();

        /// <summary>
        /// Called when the module stops being the active module.
        /// Should perform cleanup, if required.
        /// </summary>
        void Stop();
    }
}
