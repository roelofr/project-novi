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
                Application.Run(new NoviController().Start());
        }
    }
}
