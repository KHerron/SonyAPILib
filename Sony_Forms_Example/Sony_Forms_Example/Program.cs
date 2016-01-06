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
        public static SonyAPI_Lib mySonyLib = new SonyAPI_Lib();
        public static List<SonyAPI_Lib.SonyDevice> fDev = new List<SonyAPI_Lib.SonyDevice>();
        
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
