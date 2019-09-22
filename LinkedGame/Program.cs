using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LinkedGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new InitializeForm());
            Application.Run(new GameForm());
            //Application.Run(new AnimationTest1());
            
        }
    }
}
