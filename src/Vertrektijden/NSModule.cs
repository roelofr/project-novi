using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_Novi.Api;

namespace Vertrektijden
{
    public class NSModule : IModule
    {
        public string Name
        {
            get { return "NS"; }
        }

        public Bitmap Icon
        {
            get { return Properties.Resources.train_icon; }
        }

        public string DisplayName
        {
            get { return "Actuele vertrektijd"; }
        }

        public bool Rotatable
        {
            get { return true; }
        }

        public void Initialize(IController controller)
        {
            // Not doing anything on init
        }

        public void Start()
        {
            // Not doing anything on start
        }

        public void Stop()
        {
            // Not doing anything on stop
        }
    }
}
