using System;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace DR_RTM
{

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            Application.Run(new Form1());
        }
        static void OnProcessExit(object sender, EventArgs e)
        {
            Dead_Rising_Speedrunning_Skip_Tool.Properties.Settings.Default.Save();
        }
    }
}
