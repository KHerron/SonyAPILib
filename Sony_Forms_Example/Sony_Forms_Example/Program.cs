using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SonyAPILib;

namespace Sony_Forms_Example
{
    static class Program
    {
        public static SonyAPILib.SonyAPILib mySonyLib = new SonyAPILib.SonyAPILib();
        public static List<SonyAPILib.SonyAPILib.SonyDevice> fDev = new List<SonyAPILib.SonyAPILib.SonyDevice>();
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
