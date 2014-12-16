using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;

namespace Project_Novi.Modules.Home
{
    class HomeTileLocation
    {
        public String ModuleName { get; private set; }
        public Rectangle Rectangle { get; private set; }

        public HomeTileLocation(Rectangle rectangle, String moduleName)
        {
            ModuleName = moduleName;
            Rectangle = rectangle;
        }

        public void SetModuleName(String moduleName)
        {
            ModuleName = moduleName;
        }
        public void SetModuleName()
        {
            SetModuleName(null);
        }
    }
}
