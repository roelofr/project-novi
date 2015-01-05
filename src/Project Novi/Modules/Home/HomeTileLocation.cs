using System;
using System.Drawing;

namespace Project_Novi.Modules.Home
{
    class HomeTileLocation
    {
        public String ModuleName { get; set; }
        public Rectangle Rectangle { get; private set; }

        public HomeTileLocation(Rectangle rectangle, String moduleName)
        {
            ModuleName = moduleName;
            Rectangle = rectangle;
        }
    }
}
