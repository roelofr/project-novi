using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Novi
{
    class NoviLauncher
    {
        static void Main(string[] args)
        {
                // Open main app
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Tekst.TextSaver());
                //Application.Run(new Novi());
            
        }

    }
}
